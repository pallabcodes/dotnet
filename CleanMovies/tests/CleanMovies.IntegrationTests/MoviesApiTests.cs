using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using CleanMovies.Api;
using CleanMovies.Application.Commands.CreateMovie;
using CleanMovies.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Xunit;

namespace CleanMovies.IntegrationTests;

public class MoviesApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public MoviesApiTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<MovieDbContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<MovieDbContext>(options =>
                {
                    options.UseInMemoryDatabase("IntegrationTests");
                });
            });
        });
    }

    [Fact]
    public async Task Create_Then_GetMovie_ShouldReturnCreatedMovie()
    {
        var client = _factory.CreateClient();

        client.DefaultRequestHeaders.Authorization = new("Bearer", CreateEditorToken());

        var create = new CreateMovieCommand("Blade Runner", 1982, "", new[] { "Sci-Fi" });
        var response = await client.PostAsJsonAsync("/movies", create);
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var created = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        created.Should().NotBeNull();
        Guid id = Guid.Parse(created!["id"]);

        var get = await client.GetAsync($"/movies/{id}");
        get.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Create_WithInvalidPayload_ShouldReturnBadRequest()
    {
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new("Bearer", CreateEditorToken());

        var create = new CreateMovieCommand("", 1800, "", Array.Empty<string>());

        var response = await client.PostAsJsonAsync("/movies", create);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task RateMovie_InvalidRating_ShouldReturnBadRequest()
    {
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new("Bearer", CreateEditorToken());

        var create = new CreateMovieCommand("Inception", 2010, "", new[] { "Sci-Fi" });
        var response = await client.PostAsJsonAsync("/movies", create);
        response.EnsureSuccessStatusCode();
        var created = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        created.Should().NotBeNull();
        Guid id = Guid.Parse(created!["id"]);

        var rateResponse = await client.PostAsJsonAsync($"/movies/{id}/ratings", new RateMovieRequest(Guid.NewGuid(), 0));
        rateResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Get_MissingMovie_ShouldReturnNotFound()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync($"/movies/{Guid.NewGuid()}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Swagger_ShouldExposeMoviesEndpoint()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/swagger/v1/swagger.json");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var json = await response.Content.ReadAsStringAsync();
        json.Should().Contain("/movies");
    }

    private static string CreateEditorToken()
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("replace-this-with-a-secure-long-secret-key"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: "cleanmovies",
            audience: "cleanmovies",
            claims: new[] { new Claim(ClaimTypes.Role, "editor") },
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

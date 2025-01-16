using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Movies.Api.Sdk;
using Movies.Api.Sdk.Consumer;
using Movies.Contracts.Requests;
using Refit;

var services = new ServiceCollection();

// Register services
services
    .AddHttpClient()
    .AddSingleton<AuthTokenProvider>()
    .AddSingleton<IMoviesApi>(provider =>
    {
        // Create a Refit client manually and pass HttpClient
        var httpClient = provider.GetRequiredService<IHttpClientFactory>().CreateClient();
        httpClient.BaseAddress = new Uri("https://localhost:7280");

        return RestService.For<IMoviesApi>(httpClient);
    });

services.AddHttpClient();

var provider = services.BuildServiceProvider();

var moviesApi = provider.GetRequiredService<IMoviesApi>();

// Example operations
var movie = await moviesApi.GetMovieAsync("ASPNET-v8-WEB-API-2024");

var newMovie = await moviesApi.CreateMovieAsync(new CreateMovieRequest
{
    Title = "Spiderman 2",
    YearOfRelease = 2002,
    Genres = new[] { "Action" }
});

await moviesApi.UpdateMovieAsync(newMovie.Id, new UpdateMovieRequest
{
    Title = "Spiderman 2",
    YearOfRelease = 2002,
    Genres = new[] { "Action", "Adventure" }
});

await moviesApi.DeleteMovieAsync(newMovie.Id);

var request = new GetAllMoviesRequest
{
    Title = null,
    Year = null,
    SortBy = null,
    Page = 1,
    PageSize = 3
};

var movies = await moviesApi.GetMoviesAsync(request);

foreach (var movieResponse in movies.Items) Console.WriteLine(JsonSerializer.Serialize(movieResponse));
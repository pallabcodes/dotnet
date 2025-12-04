using EventDrivenEcommerce.Api.Auth;
using EventDrivenEcommerce.Api.Middleware;
using EventDrivenEcommerce.Application;
using EventDrivenEcommerce.Application.Commands.PlaceOrder;
using EventDrivenEcommerce.Application.Common;
using EventDrivenEcommerce.Domain.ValueObjects;
using EventDrivenEcommerce.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Serilog configuration
builder.Host.UseSerilog((context, config) =>
{
    config.ReadFrom.Configuration(context.Configuration)
          .Enrich.FromLogContext()
          .Enrich.WithProperty("Application", "EventDrivenEcommerce")
          .WriteTo.Console()
          .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day);
});

// Add services
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// ASP.NET Core
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Authentication (skip in testing environment)
if (!builder.Environment.IsEnvironment("Testing") && builder.Configuration["ASPNETCORE_ENVIRONMENT"] != "Testing")
{
    var jwtSettings = builder.Configuration.GetSection("JWT").Get<JwtSettings>() ?? new JwtSettings();
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(2),
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
            };
        });

    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("Customer", policy => policy.RequireRole("customer"));
        options.AddPolicy("Admin", policy => policy.RequireRole("admin"));
    });
}

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<ValidationExceptionMiddleware>();

if (!app.Environment.IsEnvironment("Testing") && app.Configuration["ASPNETCORE_ENVIRONMENT"] != "Testing")
{
    app.UseAuthentication();
    app.UseAuthorization();
}

// API Endpoints
var ordersEndpoint = app.MapPost("/orders", async (PlaceOrderRequest request, IMediator mediator, CancellationToken ct) =>
{
    var command = new PlaceOrderCommand(
        CustomerId: new CustomerId(request.CustomerId),
        ShippingAddress: new Address(request.ShippingAddress.Street, request.ShippingAddress.City,
                                    request.ShippingAddress.State, request.ShippingAddress.ZipCode,
                                    request.ShippingAddress.Country),
            Items: request.Items.Select(i => new OrderItem(
                new ProductId(i.ProductId),
                i.ProductName,
                new Money(i.UnitPrice, "USD"),
                i.Quantity)).ToList());

    var result = await mediator.Send(command, ct);
    return result.Succeeded
        ? Results.Created($"/orders/{result.Value}", new { orderId = result.Value })
        : Results.BadRequest(result.Error);
});

if (!app.Environment.IsEnvironment("Testing") && app.Configuration["ASPNETCORE_ENVIRONMENT"] != "Testing")
{
    ordersEndpoint.RequireAuthorization("Customer");
}

// Health check
app.MapGet("/health", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }));

app.Run();

// Request/Response DTOs
public record PlaceOrderRequest(
    Guid CustomerId,
    AddressDto ShippingAddress,
    IReadOnlyCollection<OrderItemDto> Items);

public record AddressDto(
    string Street,
    string City,
    string State,
    string ZipCode,
    string Country);

public record OrderItemDto(
    Guid ProductId,
    string ProductName,
    decimal UnitPrice,
    int Quantity);

// Make Program class visible to integration tests
public partial class Program { }
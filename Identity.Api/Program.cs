var builder = WebApplication.CreateBuilder(args);

// Add controllers to the DI container like below
builder.Services.AddControllers();

var app = builder.Build();

app.UseAuthorization();

// Map controllers so that they can handle incoming requests
app.MapControllers();

app.Run();
using Ocelot.DependencyInjection;
using Ocelot.Middleware;


var builder = WebApplication.CreateBuilder(args);

// Configure logging
builder.Host.ConfigureLogging(logging =>
{
    logging.AddConsole();
});

// Configure app configuration
var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
builder.Configuration.AddJsonFile($"ocelot.{env}.json");

// Add Ocelot services
builder.Services.AddOcelot();

// Build the app
var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/", async context =>
    {
        await context.Response.WriteAsync("Welcome to my service");
    });
});

// Use Ocelot middleware
app.UseOcelot().Wait();

// Run the app
app.Run();

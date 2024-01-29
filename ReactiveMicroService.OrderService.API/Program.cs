using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Plain.RabbitMQ;
using RabbitMQ.Client;
using ReactiveMicroService.OrderService.API.Models;
using ReactiveMicroService.OrderService.API.Repository;
using ReactiveMicroService.OrderService.API.Service;

var builder = WebApplication.CreateBuilder(args);

// MongoDB configuration
var rabbitMqConnection = builder.Configuration["RabbitMQConfiguration:ConnectionString"]; 

// Add services to the container.
builder.Services.AddDbContext<DBContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddSingleton<IConnectionProvider>(new ConnectionProvider(rabbitMqConnection));
builder.Services.AddScoped<IPublisher>(x=> new Publisher(x.GetService<IConnectionProvider>(),
    "report-exchange",
    ExchangeType.Topic));
builder.Services.AddScoped(typeof(IGenericRepository<Orders>), typeof(GenericRepository<DBContext, Orders>));
builder.Services.AddScoped(typeof(IGenericRepository<OrderDetails>), typeof(GenericRepository<DBContext, OrderDetails>));
builder.Services.AddScoped<UtilityService>();
builder.Services.AddScoped<OrdersService>();
builder.Services.AddScoped<OrderDetailsService>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c=> {
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { 
        Title = "Reactive Microservice - Order Service",
        Version = "v1",
        Description = "Reactive Microservice - Order Service"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Order Service API");
});
app.UseAuthorization();

app.MapControllers();

app.Run();

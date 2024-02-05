using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Plain.RabbitMQ;
using RabbitMQ.Client;
using ReactiveMicroService.CustomerService.API.Configuration;
using ReactiveMicroService.CustomerService.API.Models;
using ReactiveMicroService.CustomerService.API.Repository;
using ReactiveMicroService.CustomerService.API.Service;
using System.Text;

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
builder.Services.AddScoped<IPublisher>(x => new Publisher(x.GetService<IConnectionProvider>(),"report-exchange",  ExchangeType.Topic));
builder.Services.AddScoped(typeof(IGenericRepository<Customers>), typeof(GenericRepository<DBContext, Customers>));
builder.Services.AddScoped(typeof(IGenericRepository<CustomerDevices>), typeof(GenericRepository<DBContext, CustomerDevices>));
builder.Services.AddScoped(typeof(IGenericRepository<CustomerAddresses>), typeof(GenericRepository<DBContext, CustomerAddresses>));
builder.Services.AddScoped<UtilityService>();
builder.Services.AddScoped<CustomersService>();
builder.Services.AddScoped<CustomerDevicesService>();
builder.Services.AddScoped<CustomerAddressesService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<AuthMiddleware>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
    };
});
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Reactive Microservice - Customer Service",
        Version = "v1",
        Description = "Reactive Microservice - Customer Service"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Customer Service API");
});
app.UseMiddleware<AuthMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

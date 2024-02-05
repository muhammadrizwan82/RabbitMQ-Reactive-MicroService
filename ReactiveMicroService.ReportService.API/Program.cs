using Microsoft.EntityFrameworkCore;
using Plain.RabbitMQ;
using RabbitMQ.Client;
using ReactiveMicroService.ReportService.API.Service;
using ReactiveMicroService.ReportService.API.Models;
using ReactiveMicroService.ReportService.API.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ReactiveMicroService.ReportService.API.Configuration;


var builder = WebApplication.CreateBuilder(args);

// MongoDB configuration
var rabbitMqConnection = builder.Configuration["RabbitMQConfiguration:ConnectionString"];

// Add services to the container.
builder.Services.AddDbContext<DBContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
}, ServiceLifetime.Scoped);

builder.Services.AddSingleton<IConnectionProvider>(new ConnectionProvider(rabbitMqConnection));
builder.Services.AddSingleton<ISubscriber>(x => new Subscriber(x.GetService<IConnectionProvider>(), 
    "report-exchange", 
    "report-queue",
    "report.*"
    ,ExchangeType.Topic));

builder.Services.AddScoped(typeof(IGenericRepository<Customers>), typeof(GenericRepository<DBContext, Customers>));
builder.Services.AddScoped(typeof(IGenericRepository<CustomerDevices>), typeof(GenericRepository<DBContext, CustomerDevices>));
builder.Services.AddScoped(typeof(IGenericRepository<CustomerAddresses>), typeof(GenericRepository<DBContext, CustomerAddresses>));
builder.Services.AddScoped(typeof(IGenericRepository<Orders>), typeof(GenericRepository<DBContext, Orders>));
builder.Services.AddScoped(typeof(IGenericRepository<OrderDetails>), typeof(GenericRepository<DBContext, OrderDetails>));

builder.Services.AddScoped<UtilityService>();
builder.Services.AddScoped<CustomersService>();
builder.Services.AddScoped<CustomerDevicesService>();
builder.Services.AddScoped<CustomerAddressesService>();
builder.Services.AddScoped<OrdersService>();
builder.Services.AddScoped<OrderDetailsService>();
builder.Services.AddScoped<AuthMiddleware>();

builder.Services.AddHostedService<ReportDataCollector>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

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
        Title = "Reactive Microservice - Report Service",
        Version = "v1",
        Description = "Reactive Microservice - Report Service"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Report Service API");
});
app.UseMiddleware<AuthMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();

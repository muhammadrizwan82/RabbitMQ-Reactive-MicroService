
using Microsoft.Extensions.Options;
using Plain.RabbitMQ;
using ReactiveMicroService.ReportService.API.Models;
using ReactiveMicroService.ReportService.API.Service;
using System.Text;
using System.Text.Json;

namespace ReactiveMicroService.ReportService.API.Configuration
{
    public class ReportDataCollector : IHostedService
    {
        private readonly ISubscriber _iSubscriber;
        private readonly IServiceProvider _serviceProvider;

        public ReportDataCollector(ISubscriber iSubscriber, IServiceProvider serviceProvider)
        {
            _iSubscriber = iSubscriber;
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _iSubscriber.Subscribe(ProcessMessage);
            return Task.CompletedTask;
        }


        private bool ProcessMessage(string message, IDictionary<string, object> headers)
        {            
            var header = headers.FirstOrDefault();
            string headerValue = "no header value";

            if (header.Key != null)
            {
                if (header.Value is byte[] byteArray)
                {
                    // Assuming the byte array contains UTF-8 encoded string
                    headerValue = Encoding.UTF8.GetString(byteArray);
                }
                else
                {
                    // For non-byte array values, use ToString()
                    headerValue = header.Value.ToString();
                }
                Console.WriteLine($"Header Key: {header.Key.ToLower()}, Value:{headerValue.ToLower()}");
                if (headerValue != null)
                {
                    var scope = _serviceProvider.CreateScope();

                    if (header.Key.ToLower() == "customer")
                    {
                        Console.WriteLine("Received message: " + message);
                        var customer = JsonSerializer.Deserialize<Customers>(message);
                        if (customer != null)
                        {
                            var scopeService = scope.ServiceProvider.GetRequiredService<CustomersService>();
                            if (headerValue.ToLower() == "new")
                            {
                                scopeService.CreateAsync(new Customers
                                {
                                    Id = customer.Id,
                                    DialCode = customer.DialCode,

                                    EmailAddress = customer.EmailAddress,
                                    FirstName = customer.FirstName,
                                    LastName = customer.LastName,
                                    FullName = customer.FullName,
                                    Password = customer.Password,
                                    PhoneNumber = customer.PhoneNumber,
                                    CreatedBy = customer.CreatedBy,
                                    CreatedIP = customer.CreatedIP,
                                    CreatedAt = customer.CreatedAt,
                                    IsActive = customer.IsActive,
                                    IsDeleted = customer.IsDeleted,
                                    UpdatedAt = customer.UpdatedAt,
                                    UpdatedBy = customer.UpdatedBy,
                                    UpdatedIP = customer.UpdatedIP
                                });
                            }
                            else if (headerValue.ToLower() == "update")
                            {
                                scopeService.UpdateAsync(customer.Id, customer);
                            }
                        }                      
                    }

                    if (header.Key.ToLower() == "customerdevice")
                    {
                        Console.WriteLine("Received message: " + message);
                        var customerDevices = JsonSerializer.Deserialize<CustomerDevices>(message);
                        if (customerDevices != null)
                        {
                            var scopeService = scope.ServiceProvider.GetRequiredService<CustomerDevicesService>();
                            if (headerValue.ToLower() == "new")
                            {
                                scopeService.CreateAsync(new CustomerDevices
                                {
                                    Id = customerDevices.Id,
                                    CustomerId = customerDevices.CustomerId,
                                    DeviceId = customerDevices.DeviceId,
                                    DeviceToken = customerDevices.DeviceToken,
                                    UserAgent = customerDevices.UserAgent,
                                    CreatedBy = customerDevices.CreatedBy,
                                    CreatedIP = customerDevices.CreatedIP,
                                    CreatedAt = customerDevices.CreatedAt,
                                    IsActive = customerDevices.IsActive,
                                    IsDeleted = customerDevices.IsDeleted,
                                    UpdatedAt = customerDevices.UpdatedAt,
                                    UpdatedBy = customerDevices.UpdatedBy,
                                    UpdatedIP = customerDevices.UpdatedIP
                                });
                            }
                            else if (headerValue.ToLower() == "update")
                            {
                                scopeService.UpdateAsync(customerDevices.Id, customerDevices);
                            }
                        }
                    }
                }
            }

            return true;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}

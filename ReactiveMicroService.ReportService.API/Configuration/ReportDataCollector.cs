
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
            Console.WriteLine("Received message: " + message);
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
                        var customersService = scope.ServiceProvider.GetRequiredService<CustomersService>();
                        if (headerValue.ToLower() == "new")
                        {
                            var customer = JsonSerializer.Deserialize<Customers>(message);
                            if (customer != null)
                            {
                                var insertedCustomer = customersService.CreateAsync(new Customers
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
                                if (insertedCustomer != null)
                                {
                                    var customerDevicesService = scope.ServiceProvider.GetRequiredService<CustomerDevicesService>();
                                    foreach (var customerDevice in customer.CustomerDevices)
                                    {
                                        customerDevicesService.CreateAsync(new CustomerDevices
                                        {
                                            CreatedBy = customerDevice.CreatedBy,
                                            CreatedIP = customerDevice.CreatedIP,
                                            CreatedAt = customerDevice.CreatedAt,
                                            IsActive = customerDevice.IsActive,
                                            IsDeleted = customerDevice.IsDeleted,
                                            UpdatedAt = customerDevice.UpdatedAt,
                                            UpdatedBy = customerDevice.UpdatedBy,
                                            UpdatedIP = customerDevice.UpdatedIP,
                                            CustomerId = customerDevice.CustomerId,
                                            DeviceId = customerDevice.DeviceId,
                                            DeviceToken = customerDevice.DeviceToken,
                                            UserAgent = customerDevice.UserAgent,
                                            Id = customerDevice.Id
                                        });
                                    }
                                }
                            }
                        }
                        else if (headerValue.ToLower() == "update")
                        {
                            var customer = JsonSerializer.Deserialize<Customers>(message);
                            if (customer != null)
                            {
                                var updatedCustomer = customersService.UpdateAsync(customer.Id, customer);
                                if (updatedCustomer != null)
                                {
                                    var customerDevicesService = scope.ServiceProvider.GetRequiredService<CustomerDevicesService>();
                                    foreach (var customerDevice in customer.CustomerDevices)
                                    {
                                        customerDevicesService.UpdateAsync(customerDevice.Id, customerDevice);
                                    }
                                }
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

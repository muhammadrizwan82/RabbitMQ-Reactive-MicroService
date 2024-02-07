
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

                    #region Customer Synching
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
                    #endregion Customer Synching

                    #region Customer Device Synching 
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
                    #endregion

                    #region Customer Address Synching 
                    if (header.Key.ToLower() == "customeraddress")
                    {
                        Console.WriteLine("Received message: " + message);
                        var customerAddresses = JsonSerializer.Deserialize<CustomerAddresses>(message);
                        if (customerAddresses != null)
                        {
                            var scopeService = scope.ServiceProvider.GetRequiredService<CustomerAddressesService>();
                            if (headerValue.ToLower() == "new")
                            {
                                scopeService.CreateAsync(new CustomerAddresses
                                {
                                    Id = customerAddresses.Id,
                                    CustomerId = customerAddresses.CustomerId,
                                    Area = customerAddresses.Area,
                                    City = customerAddresses.City,
                                    Country = customerAddresses.Country,
                                    CountryIso2Code = customerAddresses.CountryIso2Code,
                                    HouseNumber = customerAddresses.HouseNumber,
                                    Lane = customerAddresses.Lane,
                                    Sector = customerAddresses.Sector,
                                    State = customerAddresses.State,
                                    Town = customerAddresses.Town,
                                    CreatedBy = customerAddresses.CreatedBy,
                                    CreatedIP = customerAddresses.CreatedIP,
                                    CreatedAt = customerAddresses.CreatedAt,
                                    IsActive = customerAddresses.IsActive,
                                    IsDeleted = customerAddresses.IsDeleted,
                                    UpdatedAt = customerAddresses.UpdatedAt,
                                    UpdatedBy = customerAddresses.UpdatedBy,
                                    UpdatedIP = customerAddresses.UpdatedIP
                                });
                            }
                            else if (headerValue.ToLower() == "update")
                            {
                                scopeService.UpdateAsync(customerAddresses.Id, customerAddresses);
                            }
                        }
                    }
                    #endregion

                    #region Customer Order Synching 
                    if (header.Key.ToLower() == "order")
                    {
                        Console.WriteLine("Received message: " + message);
                        var order = JsonSerializer.Deserialize<Orders>(message);
                        if (order != null)
                        {
                            var scopeService = scope.ServiceProvider.GetRequiredService<OrdersService>();
                            if (headerValue.ToLower() == "new")
                            {
                                scopeService.CreateAsync(new Orders
                                {
                                    Id = order.Id,
                                    CustomerId = order.CustomerId,                                   
                                    CreatedBy = order.CreatedBy,
                                    CreatedIP = order.CreatedIP,
                                    CreatedAt = order.CreatedAt,
                                    IsActive = order.IsActive,
                                    IsDeleted = order.IsDeleted,
                                    UpdatedAt = order.UpdatedAt,
                                    UpdatedBy = order.UpdatedBy,
                                    UpdatedIP = order.UpdatedIP,
                                    CustomerAddressId = order.CustomerAddressId,
                                    DiscountPercentage = order.DiscountPercentage,
                                    OrderDescription = order.OrderDescription,
                                    OrderDisplayId = order.OrderDisplayId,
                                    
                                });
                            }
                            else if (headerValue.ToLower() == "update")
                            {
                                scopeService.UpdateAsync(order.Id, order);
                            }
                        }
                    }
                    #endregion

                    #region Customer OrderDetail Synching 
                    if (header.Key.ToLower() == "orderdetail")
                    {
                        Console.WriteLine("Received message: " + message);
                        var orderDetail = JsonSerializer.Deserialize<OrderDetails>(message);
                        if (orderDetail != null)
                        {
                            var scopeService = scope.ServiceProvider.GetRequiredService<OrderDetailsService>();
                            if (headerValue.ToLower() == "new")
                            {
                                scopeService.CreateAsync(new OrderDetails
                                {
                                    Id = orderDetail.Id,
                                    OrderId = orderDetail.OrderId,
                                    CreatedBy = orderDetail.CreatedBy,
                                    CreatedIP = orderDetail.CreatedIP,
                                    CreatedAt = orderDetail.CreatedAt,
                                    IsActive = orderDetail.IsActive,
                                    IsDeleted = orderDetail.IsDeleted,
                                    UpdatedAt = orderDetail.UpdatedAt,
                                    UpdatedBy = orderDetail.UpdatedBy,
                                    UpdatedIP = orderDetail.UpdatedIP,
                                    ProductAmount = orderDetail.ProductAmount,
                                    ProductDiscount = orderDetail.ProductDiscount,
                                    ProductDiscountedAmount = orderDetail.ProductDiscountedAmount,
                                    ProductId = orderDetail.ProductId,
                                    ProductQuantity = orderDetail.ProductQuantity
                                  
                                });
                            }
                            else if (headerValue.ToLower() == "update")
                            {
                                scopeService.UpdateAsync(orderDetail.Id, orderDetail);
                            }
                        }
                    }
                    #endregion
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

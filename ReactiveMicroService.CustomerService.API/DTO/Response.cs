namespace ReactiveMicroService.CustomerService.API.DTO
{
    public class Response
    {
        public int StatusCode { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }
    }
}

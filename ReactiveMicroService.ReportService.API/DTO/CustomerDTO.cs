using System.ComponentModel.DataAnnotations;

namespace ReactiveMicroService.ReportService.API.DTO
{
     
    public class LoginDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string? EmailAddress { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(10, ErrorMessage = "Must be between 5 and 10 characters", MinimumLength = 5)]
        [RegularExpression("^(?=.*[A-Z])(?=.*[a-z])(?=.*\\d)(?=.*[^\\w\\d\\s]).{5,10}$"
            , ErrorMessage = "Password must contain atleast 5 character with one capital letter,one special character and one number")]
        public string? Password { get; set; }
       
        [Required(ErrorMessage = "DeviceId is required")]
        [StringLength(500, ErrorMessage = "DeviceId must be between 1 and 500 digits", MinimumLength = 1)]
        public string? DeviceId { get; set; }

    }
    public class LoginToken
    {        
        public string? EmailAddress { get; set; }
         
        public string? Token { get; set; }

        public string? Message { get; set; }
    }
     
}

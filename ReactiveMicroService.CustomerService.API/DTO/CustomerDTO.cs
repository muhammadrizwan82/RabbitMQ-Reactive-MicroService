﻿using System.ComponentModel.DataAnnotations;

namespace ReactiveMicroService.CustomerService.API.DTO
{
    public class CustomerSignupDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string? EmailAddress { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(10, ErrorMessage = "Password be between 5 and 10 characters", MinimumLength = 5)]
        [RegularExpression("^(?=.*[A-Z])(?=.*[!@#$%^&*()-+])(?=.*\\d.*\\d)[a-zA-Z\\d!@#$%^&*()-+]{5,10}$"
            , ErrorMessage = "Password must contain atleast 5 character with one capital letter,one special character and one number")]        
        public string? Password { get; set; }

        [Required(ErrorMessage = "FirstName is required")]
        [StringLength(50, ErrorMessage = "FirstName must be between 1 and 50 characters", MinimumLength = 1)]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "FirstName must contain alphabets only")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required")]
        [StringLength(50, ErrorMessage = "LastName must be between 1 and 50 characters", MinimumLength = 1)]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "LastName must contain alphabets only")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "DialCode is required")]
        //[Range(minimum:1,maximum:4, ErrorMessage = "DialCode must be between 1 to 4 digits")]
        [RegularExpression("\\b\\d{1,4}\\b", ErrorMessage = "DialCode must contain numbers only between 1 and 4")]
        public int DialCode { get; set; }

        [Required(ErrorMessage = "PhoneNumber is required")]
        [StringLength(12, ErrorMessage = "PhoneNumber must be between 1 and 12 digits", MinimumLength = 1)]
        [RegularExpression("\\b\\d{1,12}\\b", ErrorMessage = "PhoneNumber must contain numbers only between 1 and 12")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "DeviceId is required")]
        [StringLength(500, ErrorMessage = "DeviceId must be between 1 and 500 digits", MinimumLength = 1)]
        public string? DeviceId { get; set; }

    }
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
    public class CustomerAddressDTO
    {
        [Required(ErrorMessage = "Country is required")]
        [StringLength(100, ErrorMessage = "Country must be between 1 and 50 characters", MinimumLength = 1)]
        [RegularExpression("^[a-zA-Z]{1,100}$", ErrorMessage = "Country must contain alphabets only")]
        public string? Country { get; set; }

        [Required(ErrorMessage = "CountryIso2Code is required")]
        [StringLength(2, ErrorMessage = "CountryIso2Code must be between 1 and 50 characters", MinimumLength = 1)]
        [RegularExpression("^[A-Za-z]{2}$", ErrorMessage = "CountryIso2Code must contain alphabets only")]
        public string? CountryIso2Code { get; set; }

        [Required(ErrorMessage = "State is required")]
        [StringLength(100, ErrorMessage = "State must be between 1 and 50 characters", MinimumLength = 1)]
        [RegularExpression("^[a-zA-Z0-9!@#$%^&*()-_+=\\[\\]{}|\\\\;:'\",.<>?\\/ ]{1,100}$", ErrorMessage = "State only allow alphabets, numbers and characters")]
        public string? State { get; set; }

        [Required(ErrorMessage = "City is required")]
        [StringLength(100, ErrorMessage = "City must be between 1 and 50 characters", MinimumLength = 1)]
        [RegularExpression("^[a-zA-Z0-9!@#$%^&*()-_+=\\[\\]{}|\\\\;:'\",.<>?\\/ ]{1,100}$", ErrorMessage = "City only allow alphabets, numbers and characters")]
        public string? City { get; set; }

        [Required(ErrorMessage = "State is required")]
        [StringLength(100, ErrorMessage = "State must be between 1 and 50 characters", MinimumLength = 1)]
        [RegularExpression("^[a-zA-Z0-9!@#$%^&*()-_+=\\[\\]{}|\\\\;:'\",.<>?\\/ ]{1,100}$", ErrorMessage = "State only allow alphabets, numbers and characters")]
        public string? Town { get; set; }

        [Required(ErrorMessage = "State is required")]
        [StringLength(100, ErrorMessage = "State must be between 1 and 50 characters", MinimumLength = 1)]
        [RegularExpression("^[a-zA-Z0-9!@#$%^&*()-_+=\\[\\]{}|\\\\;:'\",.<>?\\/ ]{1,100}$", ErrorMessage = "State only allow alphabets, numbers and characters")]
        public string? Area { get; set; }

        [Required(ErrorMessage = "State is required")]
        [StringLength(100, ErrorMessage = "State must be between 1 and 50 characters", MinimumLength = 1)]
        [RegularExpression("^[a-zA-Z0-9!@#$%^&*()-_+=\\[\\]{}|\\\\;:'\",.<>?\\/ ]{1,100}$", ErrorMessage = "State only allow alphabets, numbers and characters")]
        public string? Sector { get; set; }

        [Required(ErrorMessage = "State is required")]
        [StringLength(100, ErrorMessage = "State must be between 1 and 50 characters", MinimumLength = 1)]
        [RegularExpression("^[a-zA-Z0-9!@#$%^&*()-_+=\\[\\]{}|\\\\;:'\",.<>?\\/ ]{1,100}$", ErrorMessage = "State only allow alphabets, numbers and characters")]
        public string? Lane { get; set; }

        [Required(ErrorMessage = "State is required")]
        [StringLength(100, ErrorMessage = "State must be between 1 and 50 characters", MinimumLength = 1)]
        [RegularExpression("^[a-zA-Z0-9!@#$%^&*()-_+=\\[\\]{}|\\\\;:'\",.<>?\\/ ]{1,100}$", ErrorMessage = "State only allow alphabets, numbers and characters")]
        public string? HouseNumber { get; set; }
    }
}

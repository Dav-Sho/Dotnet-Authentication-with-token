using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_authentication.Models
{
    public class UserRegisterRequest
    {
        [Required]
        [EmailAddress(ErrorMessage="Please use a valid Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage="Please use a valid Email")]
        public string UserName { get; set; } = string.Empty;

        [Required, MinLength(6,  ErrorMessage = "Password must be at least 6 or more characters")]
        public string Password { get; set; } = string.Empty;

        [Required, Compare("Password")]
        public string ConfrimPassword { get; set; } = string.Empty;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_authentication.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; } = new byte[0];
        public byte[] PasswordSalt { get; set; } = new byte[0];
        public DateTime Createdat { get; set; }
        public string? VerificationToken { get; set; }
        public string? PasswordRestToken { get; set; }
        public DateTime ResetTokenExpires { get; set; }

    }
}
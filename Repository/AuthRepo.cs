using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_authentication.Repository
{
    public interface AuthRepo
    {
        Task<ServiceResponse<string>> Register(User user, string password);
        Task<ServiceResponse<string>> Login(string emai, string password);
        Task<bool> UserExist(string emai);

    }
}
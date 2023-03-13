using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;

namespace dotnet_authentication.utils
{
    public class ServiceResponse<T>
    {
        public T? Data { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
    }
}
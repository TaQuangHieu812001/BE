using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunitureApp.Models
{
    public class ApiResponse
    {
        public ApiResponse()
        {
            
        }
        public ApiResponse(bool success,string msg, dynamic data)
        {
            Success = success;
            Message = msg;
            Data = data;
        }
        public bool Success { get; set; }
        public string Message { get; set; }
        public dynamic Data { get; set; }
    }
}

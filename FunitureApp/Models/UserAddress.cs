using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunitureApp.Models
{
    public class UserAddress
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string ZipCode { get; set; }
        public string Name { get; set; }
        public string Active { get; set; }
        public int UserId;
    }
}

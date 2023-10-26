using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunitureApp.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string NameProduct { get; set; }
        public int Category_id { get; set; }
        public string Image { get; set; }
        public string Desc { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public int Quantity { get; set; }
        public DateTime? Create_at { get; set; }
    }

}

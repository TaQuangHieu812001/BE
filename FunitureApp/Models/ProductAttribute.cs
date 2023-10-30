using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunitureApp.Models
{
    public class ProductAttribute
    {
        public int Id { get; set; }
        public float Price { get; set; }
        public int Product_id { get; set; }
        public DateTime? Create_at { get; set; }
        public string Color { get; set; }
        public string HexColor { get; set; }
    }
}

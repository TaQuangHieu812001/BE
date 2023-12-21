using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunitureApp.Models
{
    public class ProductImport
    {
        public int Id { get; set; }
        public int? Product_id { get; set; }
        public DateTime? Create_at { get; set; }
        public int? Quantity { get; set; }
        public int? IsImported { get; set; }
       
    }
}

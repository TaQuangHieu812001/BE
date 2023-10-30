using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunitureApp.Models.ResponeModel
{
    public class ProductResponse
    {
        public Product Product { get; set; }
        public Decimal AvgStar { get; set; }
        public Decimal TotalComment { get; set; }
        public List<ProductAttribute> ProductAttribute { get; set; }
    }
}

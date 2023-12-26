using FunitureApp.Data;
using FunitureApp.Models;
using FunitureApp.Models.ResponeModel;
using FunitureApp.untils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunitureApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly DbFunitureContext _productDbContext;
        private readonly IConfiguration _configuration;


        public ProductController(IConfiguration configuration)
        {
            _productDbContext = new DbFunitureContext();
            _configuration = configuration;
        }
        // GET: api/products
        [HttpGet]
        public IActionResult GetProducts(bool? isHome, int? cateId, string status,int? priceFrom,int? priceTo,string name)
        {
            try
            {
                 var productsQuery = _productDbContext.Products.Where(e=>e.Quantity>0);
                if (isHome==true)
                {
                    productsQuery = productsQuery.OrderByDescending(e => e.Id).Take(10);
                }
                if (cateId != null)
                {
                    productsQuery = productsQuery.Where(e => e.Category_id == cateId);
                }
                if(!String.IsNullOrEmpty(status))
                {
                    productsQuery = productsQuery.Where(e => e.Status == status);
                }
                if (!String.IsNullOrEmpty(name))
                {
                    productsQuery = productsQuery.Where(e => e.NameProduct.Contains(name));
                }
                var products = productsQuery.ToList();
                 var productRespone = new List<ProductResponse>();
                int productCount = products.Count;
                for(int j = 0; j < productCount; j++)
                {
                    var product = products[j];
                    string baseUrl = StringHelper.BaseUrl;
                    if (!string.IsNullOrEmpty(product.Image))
                    {
                        
                        product.Image = baseUrl + product.Image;
                    }
                    if (!string.IsNullOrEmpty(product.ImageList))
                    {
                        var imgs = product.ImageList.Split(",");
                        for(int i= 0; i < imgs.Count(); i++)
                        {
                            imgs[i] = baseUrl + imgs[i];
                        }
                        product.ImageList = string.Join(",", imgs);
                    }
                }
                for (int i = 0; i < products.Count; i++)
                {
                    var p = new ProductResponse();
                    var id =  products[i].Id;
                    var productAttribute = _productDbContext.ProductAttributes.Where(u => u.Product_id == id);
                    if (priceFrom != null)
                    {
                        productAttribute = productAttribute.Where(w => w.Price >= priceFrom && w.Price <= priceTo);
                    }
                    p.Product = products[i];
                    p.ProductAttribute = productAttribute.ToList();
                    productRespone.Add(p);
                }   
            return Ok(
                           new ApiResponse
                           {
                               Success = true,
                               Message = "",
                               Data = productRespone,
                           }
                    );
            }
            catch (Exception err)
            {
                return StatusCode(500, "Lỗi máy chủ: " + err.Message);
            }
            }
        [HttpGet("{id}")]
         public async Task<IActionResult> GetProduct(int id)
        {
            try
            {
                var product = _productDbContext.Products.FirstOrDefault(p => p.Id == id);

                if (product == null)
                {
                    return NotFound("Không tìm thấy sản phẩm");
                }
                string baseUrl = StringHelper.BaseUrl;
                if (!string.IsNullOrEmpty(product.Image))
                {

                    product.Image = baseUrl + product.Image;
                }
                if (!string.IsNullOrEmpty(product.ImageList))
                {
                    var imgs = product.ImageList.Split(",");
                    for (int i = 0; i < imgs.Count(); i++)
                    {
                        imgs[i] = baseUrl + imgs[i];
                    }
                    product.ImageList = string.Join(",", imgs);
                }
                var p = new ProductResponse();
                var productAttribute = _productDbContext.ProductAttributes.Where(u => u.Product_id == id);
               
                p.Product = product;
                p.ProductAttribute = productAttribute.ToList();
                //if (_productDbContext.Comments.Where(c => c.Product_id == id).Count() > 0)
               // {
               //     p.AvgStar = (decimal)_productDbContext.Comments.Where(c => c.Product_id == id).Average(a => a.Star_rate);
               //     p.TotalComment = _productDbContext.Comments.Where(c => c.Product_id == id).Count();
              ///  }
//else { p.AvgStar = 0; p.TotalComment = 0; }
                return Ok(
                        new ApiResponse
                        {
                            Success = true,
                             Message = "",
                             Data = p,
                        }
                    );
            }catch(Exception err)
            {
                return StatusCode(500, "Lỗi máy chủ: " + err.Message);
            }
         

        }
        //
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveProduct(int id)
        {
            try
            {
                var existingProduct = await _productDbContext.Products.FindAsync(id);
                if(existingProduct == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Message = "Sản phẩm ko tồn tại",
                    });
                   
                }
                _productDbContext.Remove(existingProduct);
                await _productDbContext.SaveChangesAsync();
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Xóa sản phẩm thành công",

                });
            }catch(Exception err)
            {
                return StatusCode(500, "Lỗi máy chủ: " + err.Message);
            }
        }
    }
}

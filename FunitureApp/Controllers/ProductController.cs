using FunitureApp.Data;
using FunitureApp.Models;
using FunitureApp.Models.ResponeModel;
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
        public IActionResult GetProducts()
        {
            try
            {
                 var products = _productDbContext.Products.ToList();
                 var productRespone = new List<ProductResponse>();
                int productCount = products.Count;
                for(int j = 0; j < productCount; j++)
                {
                    var product = products[j];
                    if (!string.IsNullOrEmpty(product.Image))
                    {
                        string baseUrl = "http://192.168.1.115:5000";
                        product.Image = baseUrl + product.Image;
                    }
                }
                for (int i = 0; i < products.Count; i++)
                {
                        var p = new ProductResponse();
                    var id =  products[i].Id;
               var productAttribute = _productDbContext.ProductAttributes.Where(u => u.Product_id == id).FirstOrDefault();
                    p.Product = products[i];
                    p.ProductAttribute = productAttribute;
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
                return Ok(
                        new ApiResponse
                        {
                            Success = true,
                             Message = "",
                             Data = product,
                        }
                    );
            }catch(Exception err)
            {
                return StatusCode(500, "Lỗi máy chủ: " + err.Message);
            }
         

        }
        //
        [HttpGet("search-by-type")]
        public IActionResult SearchProductsByType([FromQuery] int categoryId)
        {
            try
            {
                var products = _productDbContext.Products
                    .Where(p => p.Category_id == categoryId)
                    .ToList();

                var productResponse = products.Select(product => new ProductResponse
                {
                    Product = product,
                    ProductAttribute = _productDbContext.ProductAttributes.FirstOrDefault(pa => pa.Product_id == product.Id)
                }).ToList();

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "",
                    Data = productResponse,
                });
            }
            catch (Exception err)
            {
                return StatusCode(500, "Lỗi máy chủ: " + err.Message);
            }
        }
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

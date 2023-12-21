using FunitureApp.Data;
using FunitureApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunitureApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductImportController : Controller
    {
        private readonly DbFunitureContext _productImportDbContext;
        private readonly IHttpContextAccessor _contextAccessor;
        public ProductImportController(IHttpContextAccessor httpContext)
        {
            _contextAccessor = httpContext;
            _productImportDbContext = new DbFunitureContext();
        }
        [HttpGet]
        public async Task<IActionResult> Get(int? productId)
        {
            var existingProduct = await _productImportDbContext.ProductImports.Where(e=>e.Product_id==productId).OrderByDescending(e=>e.Id).ToListAsync();
            return Ok(new ApiResponse
            {
                Success = true,
                Data = existingProduct,
            });
        }
        [HttpGet("import")]
        public async Task<IActionResult> Import(int? id)
        {
            var pImport = await _productImportDbContext.ProductImports.FindAsync(id);
            pImport.IsImported = 1;
            var existingProduct = await _productImportDbContext.Products.FindAsync(pImport.Product_id);
            existingProduct.Quantity += pImport.Quantity??0;
           await _productImportDbContext.SaveChangesAsync();
            return Ok(new ApiResponse
            {
                Success = true,
                Data = null,
            });
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromForm(Name = "product_id")] int? product_id,
            [FromForm(Name = "quantity")] int? quantity)
        {
            try
            {
                var existingProduct = await _productImportDbContext.Products.FindAsync(product_id);
                if (existingProduct == null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Sản phẩm không tồn tại",
                    }
                        );
                }
                //
                _productImportDbContext.ProductImports.Add(new ProductImport()
                {
                    Product_id = product_id ?? 0,
                    Quantity = quantity ?? 0,
                    Create_at = DateTime.Now,
                    IsImported=0
                    
                });
              
                await _productImportDbContext.SaveChangesAsync();
                return Ok(new ApiResponse
                {
                    Success = true,
                    Data = null,
                }
                    );
            }
            catch (Exception err)
            {
                return StatusCode(500, "Lỗi máy chủ: " + err.Message);
            }
        }
    
    }
}

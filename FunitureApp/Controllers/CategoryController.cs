using FunitureApp.Data;
using FunitureApp.Models;
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
    public class CategoryController : Controller
    {
        private readonly DbFunitureContext _categoryDbContext;
        private readonly IConfiguration _configuration;

        public CategoryController(IConfiguration configuration)
        {
            _categoryDbContext = new DbFunitureContext();
            configuration = _configuration;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoies()
        {
            try
            {
                var categories = _categoryDbContext.Categories.ToList();
                int categoriesCount = categories.Count;
                for(int i = 0; i< categoriesCount; i++)
                {
                    var category = categories[i];
                    if (!string.IsNullOrEmpty(category.Image))
                    {
                        string baseUrl = StringHelper.BaseUrl;

                        category.Image = baseUrl + category.Image;
                    }
                }
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "",
                    Data = categories,
                });
            }
            catch (Exception err)
            {
                return StatusCode(500, "Lỗi máy chủ: " + err.Message);  
            }

        }
    }
}

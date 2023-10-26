using FunitureApp.Data;
using FunitureApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunitureApp.Controllers
{
    [ApiController]
    [Route("api/favorites")]
    public class FavoritesController : Controller
    {
        private readonly DbFunitureContext _favoritesDbContext;
        private readonly IConfiguration _configuration;

        public FavoritesController(IConfiguration configuration)
        {
            _favoritesDbContext = new DbFunitureContext();
            _configuration = configuration;
        }
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetFavorites(int userId) {

            try
            {
                var favorites = _favoritesDbContext.Favorites.Where(f => f.UserId == userId).ToList();
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "",
                    Data = favorites,
                });
            } catch (Exception err)
            {
                return StatusCode(500, "Lỗi trong quá trình lấy danh sách yêu thích: " + err.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddFavorites(Favorites favorites)
        {
            try
            {
                var existingFavorite = _favoritesDbContext.Favorites
                    .Where
                    (f => f.UserId == favorites.UserId &&
                    f.ProductId == favorites.ProductId);
                
                _favoritesDbContext.Favorites.Add(favorites);
                await _favoritesDbContext.SaveChangesAsync();
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Sản phẩm đã được thêm vào yêu thích",
                });
            } catch (Exception err)
            {
                return StatusCode(500, "Lỗi trong quá trình thêm sản phẩm vào yêu thích: " + err.Message);
            }
        }
        [HttpDelete("{userId}/{productId}")]
        public async Task<IActionResult>RemoveFavorites(int userId,int productId)
        {
            try
            {
                var favorites = _favoritesDbContext.Favorites.Where(f => f.UserId == userId && f.ProductId == productId);
                if(favorites == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Message = "Không tìm thấy sản phẩm yêu thích.",
                    });
                }
                _favoritesDbContext.Remove(favorites);
                await _favoritesDbContext.SaveChangesAsync();
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Sản phẩm đã bị xóa khỏi yêu thích",
                });
            }catch(Exception err)
            {
                return StatusCode(500, "Lỗi trong quá trình xóa sản phẩm vào yêu thích: " + err.Message);
            }
        }
    }
}

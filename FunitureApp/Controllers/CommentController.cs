using FunitureApp.Data;
using FunitureApp.Models;
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

    public class CommentController : Controller
    {
        private readonly DbFunitureContext _commentDbContext;
        private readonly IConfiguration _configuration;

        public CommentController(IConfiguration configuration)
        {
            _commentDbContext = new DbFunitureContext();
            configuration = _configuration;
        }
        [HttpGet]
        public async Task<IActionResult> GetComment()
        {
            try
            {
                var  comments = _commentDbContext.Comments.ToList();
                return Ok(new ApiResponse {
                    Success = true,
                    Data = comments,
                    Message = "",
                });
            }catch(Exception err)
            {
                return StatusCode(500, "Lỗi máy chủ: " + err.Message);
            }
        }
            //
        [HttpPost]
        public async Task<IActionResult> AddComment(Comment comment)
        {
            try
            {
                if(comment == null)
                {
                    return BadRequest("Dữ liệu bình luận không hợp lệ");
                }
                _commentDbContext.Comments.Add(comment);
                _commentDbContext.SaveChanges();
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Bình luận đã được cập nhật",
                });
            }catch(Exception err)
            {
                return StatusCode(500, "Lỗi máy chủ: " + err.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(int id,Comment updateComment)
        {
            try
            {
                var existingComment = _commentDbContext.Comments.FirstOrDefault(c => c.Id == id);
                if(existingComment == null)
                {
                    return NotFound("Không tìm thấy bình luận");
                }
                existingComment.CommentUser = updateComment.CommentUser;
                existingComment.Star_rate = updateComment.Star_rate;
                _commentDbContext.SaveChanges();
            
            return Ok(new ApiResponse 
            { 
                Success= true,
                Message = "Bình luận đã được cập nhật thành công",
            }
            );
            }catch(Exception err)
            {
                return StatusCode(500, "Lỗi máy chủ: " + err.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            try
            {
                var existingComment = _commentDbContext.Comments.FirstOrDefault(c => c.Id == id);
                if (existingComment == null)
                {
                    return NotFound("Không tìm thấy bình luận");
                }
                _commentDbContext.Comments.Remove(existingComment);
                _commentDbContext.SaveChanges();

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Bình luận đã được xóa thành công",
                });
            }catch(Exception err)
            {
                return StatusCode(500, "Lỗi máy chủ: " + err.Message);
            }
        }

    }
}

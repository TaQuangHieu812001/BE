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
    [Route("api/[controller]")]
    public class UserOrderController : Controller
    {
        private readonly DbFunitureContext _userOrderDbContext;
        private readonly IConfiguration _configuration;


        public UserOrderController(IConfiguration configuration)
        {
            _userOrderDbContext = new DbFunitureContext();
            _configuration = configuration;
        }
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserOrders(int userId)
        {
            try
            {
                var userOrder = _userOrderDbContext.UserOrders.Where(uo => uo.User_id == userId).ToList();
                var userExists = _userOrderDbContext.UserOrders.SingleOrDefault(u => u.Id == userId);
                if (userExists == null)
                {
                    return NotFound("Người dùng không tồn tại.");
                }
                if (userOrder.Count == 0)
                {
                    return NotFound("Không tìm thấy đơn hàng cho người dùng này.");
                }


                return Ok(new ApiResponse
                {
                    Success = true,
                    Data = userOrder,

                });
            } catch (Exception err)
            {
                return StatusCode(500, "Lỗi trong quá trình lấy đơn hàng: " + err.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserOrders(UserOrder userOrder)
        {
            try
            {
                userOrder.Create_at = DateTime.Now; // Gán thời gian tạo đơn hàng
                _userOrderDbContext.UserOrders.Add(userOrder);
                await _userOrderDbContext.SaveChangesAsync();

                return Ok("Đơn hàng đã được tạo thành công.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi trong quá trình tạo đơn hàng: {ex.Message}");
            }
        }

        [HttpPut("{orderId}")]
        public async Task<IActionResult>UpdateUserOrder(int orderId, UserOrder updatedOrder)
        {
          try
            {
                var existingOrder = await _userOrderDbContext.UserOrders
              .FirstOrDefaultAsync(uo => uo.Id == orderId);
                if (existingOrder == null)
                {
                    return NotFound("Không tìm thấy đơn hàng");
                }
                // Cập nhật thông tin đơn hàng 
                existingOrder.Total = updatedOrder.Total;
                existingOrder.Status = updatedOrder.Status;
                existingOrder.Order_no = updatedOrder.Order_no;

                _userOrderDbContext.UserOrders.Update(existingOrder);
                await _userOrderDbContext.SaveChangesAsync();
                return Ok( 
                    new ApiResponse 
                {
                    Success =true,
                    Message = "Đơn hàng đã được cập nhật thành công",
                    }
                );

            }catch(Exception err)
            {
                return StatusCode(500, "Lỗi trong quá trình cập nhật đơn hàng "+err.Message);
               
            }

        }


        [HttpDelete("{orderId}")]
        public async Task<IActionResult>RemoveUserOrder(int orderId)
        {
            try
            {
                var orderToDelete = await _userOrderDbContext.UserOrders.FindAsync(orderId);
                 if(orderToDelete == null)
                {
                    return NotFound("Không tìm thấy đơn hàng.");
                }
                _userOrderDbContext.UserOrders.Remove(orderToDelete);
                await _userOrderDbContext.SaveChangesAsync();
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Xóa đơn hàng thành công",
                });
            }
            catch(Exception err){
                return StatusCode(500, "Lỗi trong quá trình xóa đơn hàng " + err.Message);
            }
        }
    }

}
  

using FunitureApp.Data;
using FunitureApp.Models;
using FunitureApp.Models.RequestModel;
using FunitureApp.Models.ResponeModel;
using FunitureApp.untils;
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
    [JwtAuthorize]
    public class UserOrderController : Controller
    {
        private readonly DbFunitureContext _userOrderDbContext;
        private readonly IConfiguration _configuration;


        public UserOrderController(IConfiguration configuration)
        {
            _userOrderDbContext = new DbFunitureContext();
            _configuration = configuration;
        }
        [HttpGet]
        public async Task<IActionResult> GetUserOrders(string status)
        {
            try
            {
                var authUserId = Int32.Parse(HttpContext.User.Claims.Where(u => u.Type == "Id").FirstOrDefault().Value);
                var userOrder = _userOrderDbContext.UserOrders.Where(uo => uo.User_id == authUserId && uo.Status==status).ToList();
                var response = new List<OrderResponse>();
                
               foreach(var o in userOrder)
                {
                    var orderRes = new OrderResponse()
                    {
                        order = o,
                        orderItems = new List<OrderItemResponse>()
                    };
                    var orderItem = _userOrderDbContext.UserOrderItems.AsNoTracking().Where(i => i.User_order_id == o.Id).ToList();
                    foreach(var item in orderItem)
                    {
                        var productAttr = _userOrderDbContext.ProductAttributes.AsNoTracking().Where(p => p.Id == item.Product_attr_id).FirstOrDefault();
                        var product = _userOrderDbContext.Products.AsNoTracking().Where(p => p.Id == productAttr.Product_id).FirstOrDefault();
                        product.Image = StringHelper.BaseUrl + product.Image;
                        var productImages = new List<string>();
                        if (!string.IsNullOrEmpty(product.ImageList))
                        {
                            foreach (var img in product.ImageList.Split(","))
                            {
                                productImages.Add(StringHelper.BaseUrl + img);
                            }
                            product.ImageList = string.Join(",", productImages);
                        }
                        orderRes.orderItems.Add(new OrderItemResponse()

                        {
                            orderItem = item,
                            product = product,
                            productAttribute = productAttr
                        });
                    }
                    response.Add(orderRes);
                }

                return Ok(new ApiResponse
                {
                    Success = true,
                    Data = response,
                });
            } catch (Exception err)
            {
                return StatusCode(500, "Lỗi trong quá trình lấy đơn hàng: " + err.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserOrders(OrderRequest userOrder)
        {
            try
            {
                var authUserId = Int32.Parse(HttpContext.User.Claims.Where(u => u.Type == "Id").FirstOrDefault().Value);

                foreach (var o in userOrder.orderItem)
                {
                    var product = _userOrderDbContext.Products.Where(p => p.Id == o.product.Id).FirstOrDefault();
                    if (product != null)
                    {
                        if (product.Quantity < o.count)
                        {
                            return Ok(new ApiResponse
                            {
                                Success = false,
                                Message = "Sản phẩm đã hết!"
                            });
                        }
                        else {
                            product.Quantity -= o.count;
                            _userOrderDbContext.SaveChanges();
                        }
                    }

                }


                var newOrder = new UserOrder()
                {
                    Delivery_free = (int)userOrder.deliveryFee,
                    PaymentStatus = 0,
                    Delivery_method_id = 0,
                    PaymentType = userOrder.paymentType,
                    ShipId = userOrder.shipId,
                    Status = "processing",
                    Total = userOrder.total,
                    User_id = authUserId,Create_at=DateTime.Now,
                    Order_no=DateTime.Now.Ticks.ToString()
                };
                _userOrderDbContext.UserOrders.Add(newOrder);
                _userOrderDbContext.SaveChanges();
                foreach(var o in userOrder.orderItem)
                {
                    _userOrderDbContext.UserOrderItems.Add(new UserOrderItem
                    {
                        Product_attr_id = o.productAttribute.Id,
                         Quantity=o.count,
                          Total=(decimal)o.productAttribute.Price*o.count,
                           User_order_id=newOrder.Id
                           
                    });

                }
                _userOrderDbContext.SaveChanges();
                return Ok(new ApiResponse
                {
                    Success = true,
                    Data = null,
                });
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
  

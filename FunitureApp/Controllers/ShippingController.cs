using FunitureApp.Data;
using FunitureApp.Models;
using FunitureApp.untils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FunitureApp.Controllers
{
    [ApiController]
    [JwtAuthorize]
    [Route("api/[controller]")]
    public class ShippingController : Controller
    {
        private readonly DbFunitureContext _userOrderDbContext;
        public ShippingController()
		{
            _userOrderDbContext = new DbFunitureContext();
        }
        [HttpGet]
        public async Task<IActionResult> GetAddress()
        {
            try
            {
                var authUserId = Int32.Parse(HttpContext.User.Claims.Where(u => u.Type == "Id").FirstOrDefault().Value);
                return Ok(new ApiResponse(true, "",await _userOrderDbContext.UserAddresses.Where(u => u.UserId == authUserId).ToListAsync()));
            }
            catch(Exception e)
            {
                return  Ok(new ApiResponse(false, "Internal", null));
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddAddress(UserAddress body)
        {
            try
            {
                var authUserId = Int32.Parse(HttpContext.User.Claims.Where(u => u.Type == "Id").FirstOrDefault().Value);
                var addresses= await _userOrderDbContext.UserAddresses.Where(u => u.UserId == authUserId).ToListAsync();
                foreach(var add in addresses)
                {
                    add.Active = 0;

                }

                _userOrderDbContext.UserAddresses.Add(new UserAddress()
                {
                    Active = 1,
                    PhoneNumber= body.PhoneNumber,
                    Name=body.Name,
                    ZipCode=body.ZipCode,
                    Address=body.Address,
                    UserId=authUserId
                });
                _userOrderDbContext.SaveChanges();
               
                return Ok(new ApiResponse(true, "",null ));
            }
            catch (Exception e)
            {
                return Ok(new ApiResponse(false, "Internal", null));
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAddressActive([FromQuery]int? id)
        {
            try
            {
                var authUserId = Int32.Parse(HttpContext.User.Claims.Where(u => u.Type == "Id").FirstOrDefault().Value);
                var addresses = await _userOrderDbContext.UserAddresses.Where(u =>  u.UserId == authUserId).ToListAsync();
                foreach (var add in addresses)
                {
                    if (add.Id == id)
                    {
                        add.Active = 1;
                    }
                    else
                    add.Active = 0;
                    
                }

                _userOrderDbContext.SaveChanges();

                return Ok(new ApiResponse(true, "", null));
            }
            catch (Exception e)
            {
                return Ok(new ApiResponse(false, "Internal", null));
            }
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateAddress(UserAddress body)
        {
            try
            {
                var authUserId = Int32.Parse(HttpContext.User.Claims.Where(u => u.Type == "Id").FirstOrDefault().Value);
                var add = await _userOrderDbContext.UserAddresses.Where(u => u.Id == body.Id && u.UserId==authUserId).FirstOrDefaultAsync();

                add.Address = body.Address;
                add.Active = body.Active;
                add.Name = body.Name;
                add.PhoneNumber = body.PhoneNumber;
                add.ZipCode = body.ZipCode;

                _userOrderDbContext.SaveChanges();

                return Ok(new ApiResponse(true, "", null));
            }
            catch (Exception e)
            {
                return Ok(new ApiResponse(false, "Internal", null));
            }
        }
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            try
            {
                var authUserId = Int32.Parse(HttpContext.User.Claims.Where(u => u.Type == "Id").FirstOrDefault().Value);
                var add = await _userOrderDbContext.UserAddresses.Where(u => u.Id == id && u.UserId == authUserId).FirstOrDefaultAsync();
                if (add != null) { 
                    _userOrderDbContext.Remove(add);
                    _userOrderDbContext.SaveChanges();
                    return Ok(new ApiResponse(true, "", null));
                }
                return Ok(new ApiResponse(false, "Đã Xoá", null));
            }
            catch (Exception e)
            {
                return Ok(new ApiResponse(false, "Internal", null));
            }
        }
    }
}


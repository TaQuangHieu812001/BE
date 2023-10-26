using FunitureApp.Data;
using FunitureApp.Models;
using FunitureApp.Models.RequestModels;
using FunitureApp.untils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunitureApp.Controllers
{   
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly DbFunitureContext _userDbContext;
        private readonly GeneratorToken _generatorToken;
        private readonly IConfiguration _configuration;
        public LoginController(IOptionsMonitor<AppSetting> optionsMonitor, IConfiguration configuration)
        {
            _userDbContext = new DbFunitureContext();
            _configuration = configuration;
            _generatorToken = new GeneratorToken(optionsMonitor);
       

        }
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginRequest userLoginRequest)
        {
           
            try
            {
                var isValidateEmail = new Validate();
                var user = _userDbContext.Users.Where(u => 
                u.Email == userLoginRequest.Email 
                &&
                u.Password == userLoginRequest.Password).FirstOrDefault();
                if (!isValidateEmail.IsValidEmail(userLoginRequest.Email))
                {
                    return BadRequest("Địa chỉ Email không hợp lệ");
                }
     
                if (user == null)//không đun
                {
                    return Ok(
                               new ApiResponse
                               {
                                   Success = false,
                                   Message="Tài khoản hoặc mật khẩu không đúng,xin vui lòng thử lại",
                               }
                        );
                }
               
                //
                if (user.Password != userLoginRequest.Password)
                {
                    return Unauthorized("Mật khẩu không đúng");
                }
                return Ok(new ApiResponse { 
                    Success=true,
                    Message = "Login success",
                    Data = _generatorToken.GenerateToken(user),

                });
            }
            catch (Exception err)
            {
                return StatusCode(500, "Lỗi máy chủ: " + err.Message);
            }
        }
    }
}

using FunitureApp.Data;
using FunitureApp.Models;
using FunitureApp.Models.RequestModel;
using FunitureApp.Models.RequestModels;
using FunitureApp.Models.ResponeModel;
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
        [HttpPost("")]
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
                    return Ok(
                               new ApiResponse
                               {
                                   Success = false,
                                   Message = "Địa chỉ Email không hợp lệ",
                               }
                        );
                }

                if (user == null)//không đun
                {
                    return Ok(
                               new ApiResponse
                               {
                                   Success = false,
                                   Message = "Tài khoản hoặc mật khẩu không đúng,xin vui lòng thử lại",
                               }
                        );
                }

                //
                if (user.Password != userLoginRequest.Password)
                {
                    return Ok(
                               new ApiResponse
                               {
                                   Success = false,
                                   Message = "Mật khẩu không đúng",
                               }
                        );

                }
                var userResult = new LoginResponse()
                {
                    CreateBy = user.CreateBy,
                    CreateOn = user.CreateOn,
                    Email = user.Email,
                    Id = user.Id,
                    ModifiedOn = user.ModifiedOn,
                    ModifiedBy = user.ModifiedBy,
                    token = _generatorToken.GenerateToken(user),
                    UserName = user.UserName
                };
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Login success",
                    Data = userResult,

                });
            }
            catch (Exception err)
            {
                return StatusCode(500, "Lỗi máy chủ: " + err.Message);
            }
        }

        [HttpPut("updateprofile")]
        [JwtAuthorize]
        public async Task<IActionResult> UpdateProfile([FromBody] User userProfile)
        {
            try
            {
                var authUserId = Int32.Parse(HttpContext.User.Claims.Where(u => u.Type == "Id").FirstOrDefault().Value);
                var uDb = await _userDbContext.Users.Where(u => u.Id == authUserId).FirstOrDefaultAsync();
                if (uDb != null)
                {
                    uDb.ModifiedOn = DateTime.Now;
                    uDb.UserName = userProfile.UserName;
                    await _userDbContext.SaveChangesAsync();
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "ok",
                        Data = null
                    });
                }
                return Ok(new ApiResponse
                {
                    Success = false,
                    Message = "Not found",
                    Data = null
                });

            }
            catch (Exception err)
            {
                return StatusCode(500, "Lỗi máy chủ: " + err.Message);
            }

        }
        [HttpPut("updatepass")]
        [JwtAuthorize]
        public async Task<IActionResult> UpdatePass([FromBody] ChangePassReq userProfile)
        {
            try
            {
                var authUserId = Int32.Parse(HttpContext.User.Claims.Where(u => u.Type == "Id").FirstOrDefault().Value);
                var uDb = await _userDbContext.Users.Where(u => u.Id == authUserId).FirstOrDefaultAsync();
                var md5Pass = new MD5Hash();
                if (uDb != null)
                {
                    if (uDb.Password != md5Pass.Hash(userProfile.OldPass))
                    {
                        return Ok(new ApiResponse
                        {
                            Success = false,
                            Message = "Sai mật khẩu cũ",
                            Data = null
                        });
                    }
                    uDb.Password = md5Pass.Hash(userProfile.NewPass);
                    await _userDbContext.SaveChangesAsync();
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "ok",
                        Data = null
                    });
                }
                return Ok(new ApiResponse
                {
                    Success = false,
                    Message = "Not found",
                    Data = null
                });

            }
            catch (Exception err)
            {
                return StatusCode(500, "Lỗi máy chủ: " + err.Message);
            }
        }
    }
}

using FunitureApp.Data;
using FunitureApp.Models;
using FunitureApp.Models.RequestModels;
using FunitureApp.untils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;

namespace FunitureApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegistrationController : Controller
    {
        private readonly DbFunitureContext _userDbContext;
        //private readonly AppSetting _appSettings;
        private readonly GeneratorToken _generatorToken;
        private readonly IConfiguration _configuration;
        public RegistrationController(IOptionsMonitor<AppSetting> optionsMonitor, IConfiguration configuration)
        {
            _userDbContext = new DbFunitureContext();
            //_appSettings = optionsMonitor.CurrentValue;
            _configuration = configuration;
            _generatorToken = new GeneratorToken(optionsMonitor);
        }

        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationRequest registrationRequest)
        {
            var isValidateEmail = new Validate();
            if (!isValidateEmail.IsValidEmail(registrationRequest.Email))
            {
                return BadRequest("Địa chỉ email không hợp lệ.");
            }
            try
            {   //check user co ton tai
                var existingUser = _userDbContext.Users.Where(u => u.Email == registrationRequest.Email);

                if (existingUser.Count() > 0)
                {
                    return BadRequest("Người dùng đã tồn tại");
                }
                var newUser = new User()
                {
                    UserName = registrationRequest.UserName,
                    Email = registrationRequest.Email,
                    Password = registrationRequest.Password,
                };
                await _userDbContext.Users.AddAsync(newUser);
                await _userDbContext.SaveChangesAsync();
                return Ok(
                           new ApiResponse
                           {
                               Success = true,
                               Message = "Đăng kí thành công",
                               Data = _generatorToken.GenerateToken(newUser),
                           }
                    ); ;

            }
            catch (Exception err)
            {
                return StatusCode(500, "Server error" + err.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRegistration(int id, [FromBody] RegistrationRequest updatedRegistration)
        {
            var isValidateEmail = new Validate();
            if (!isValidateEmail.IsValidEmail(updatedRegistration.Email))
            {
                return BadRequest("Địa chỉ email không hợp lệ.");
            }
            //
            try
            {
                var existingUser = await _userDbContext.Users.FindAsync(id);
                if (existingUser != null)
                {
                    //update information user
                    existingUser.UserName = updatedRegistration.UserName;
                    existingUser.Email = updatedRegistration.Email;
                    existingUser.Password = updatedRegistration.Password;
                    _userDbContext.Update(existingUser);
                    await _userDbContext.SaveChangesAsync();
                    return Ok(
                            new ApiResponse
                            {
                                Success = true,
                                Message = "Cập nhật thông tin người dùng thành công",
                                Data = _generatorToken.GenerateToken(existingUser),
                            }
                        );
                }
                return NotFound("Người dùng không tồn tại");
            }
            catch (Exception err)
            {
                return StatusCode(500, "Lỗi máy chủ: " + err.Message);
            }



        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserRegistration(int id)
        {
            try
            {
                var existingUser = await _userDbContext.Users.FindAsync(id);
                if (existingUser == null)
                {
                    return NotFound("Người dùng không tồn tại");
                }
                //Detele
                _userDbContext.Remove(existingUser);
                await _userDbContext.SaveChangesAsync();
                return Ok(
                     new ApiResponse
                     {
                         Success = true,
                         Message = "Xóa người dùng thành công",
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

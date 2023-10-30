using System;
using System.Linq;
using FunitureApp.Data;
using FunitureApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FunitureApp.untils
{
    public class JwtAuthorize : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            bool IsAuthenticated = context.HttpContext.User.Identity.IsAuthenticated;
            if (!IsAuthenticated)
            {
                context.Result = new JsonResult( new ApiResponse(false,"UnAuthorize",null));
            }
            else
            {
                    int authId = Int32.Parse(context.HttpContext.User.Claims.Where(u => u.Type == "Id").FirstOrDefault().Value);
                    using (var db = new DbFunitureContext())
                    {
                        var user = db.Users.Where(u => u.Id == authId).FirstOrDefault();
                        if (user == null)
                        {
                            context.Result = new JsonResult(new ApiResponse(false, "UnAuthorize", null));
                        return;
                        }
                       // if (!user.)
                        //{
                        //    context.Result =  new GenericResult<object>("Tài khoản bị khóa", (int)ResultCode.UnAuthorize, null); return;
                        ///}

                    }
                }
            
            return;
        }
    }
}


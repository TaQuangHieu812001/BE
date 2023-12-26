using FunitureApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunitureApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class BankingTransferController : ControllerBase
    {
        [HttpGet]
       public IActionResult GetBankingTransfer()
        {
            try
            {
                var bankingTransfer = new BankingTransfer
                {
                    accountBanking = "19036702967010",
                    nameBanking    = "TechComBank",
                    userAccount    = "Ta Quang Hieu",
                    branchAdress   = "Ha Noi"
                };
                return Ok(new ApiResponse
                {
                    Success = true,
                    Data =
                    bankingTransfer,
                    
                });

            }catch(Exception err)
            {
                return StatusCode(500, "Server error" + err.Message);
            }
        }
    }
}

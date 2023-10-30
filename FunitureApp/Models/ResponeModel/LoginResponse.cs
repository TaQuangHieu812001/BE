using System;
namespace FunitureApp.Models.ResponeModel
{
	public class LoginResponse
	{
		
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime? CreateOn { get; set; }
        public string CreateBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string token { get; set; }
	    
    }
}


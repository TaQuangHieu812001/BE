using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunitureApp.Models
{
    public class UserSetting
    {
        public int Id { get; set; }
        public bool Dark_mode { get; set; }
        public string Delivery_status_change { get; set; }
        public string Notification { get; set; }
        public int UserId { get; set; }

    }
}

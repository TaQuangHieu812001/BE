using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunitureApp.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string CommentUser { get; set; }
        public int Star_rate { get; set; }
        public int Product_id { get; set; }
        public int UserId { get; set; }
        public DateTime? Create_at { get; set; }

    }
}

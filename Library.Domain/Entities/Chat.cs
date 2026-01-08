using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Entities
{
    public class Chat
    {
        public int Id { get; set; }
        public long UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public bool? IsForm { get; set; }

        public string? Type { get; set; }
     
        public ICollection<ChatNotifications>? ChatNotifications { get; set; }
    }
}

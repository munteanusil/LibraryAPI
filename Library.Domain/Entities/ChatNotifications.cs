using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Entities
{
    public class ChatNotifications
    {
        public long NotificationId { get; set; }

        public Notification? Notification { get; set; }

        public long ChatId { get; set; }

        public Chat? Chat { get; set; }
    }
}

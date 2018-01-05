using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApi.Models
{
    public class Message
    {
        public long Id { get; set; }
        public virtual Chat Chat { get; set; }
        public User Sender { get; set; }
        public long SenderId { get; set; }
        public long ReceiverId { get; set; }
        public long ChatId { get; set; }
        public string Content { get; set; }
        //public ICollection<Seen> Seens { get; set; }
    }
}

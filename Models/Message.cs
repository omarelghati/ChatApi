using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApi.Models
{
    public class Message
    {
        public long Id { get; set; }
        [NotMapped]
        public User Sender { get; set; }
        [NotMapped]
        public User Receiver { get; set; }

        public long ChatId { get; set; }

        public long SenderId { get; set; }

        public long ReceiverId { get; set; }

        public string CreationTime{ get; set; }

        public string Content { get; set; }

        public bool Seen { get; set; }
        public Message()
        {
            Sender = new User();
            Receiver = new User();

        }
    }
}

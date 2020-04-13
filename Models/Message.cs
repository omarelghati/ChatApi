using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApi.Models
{
    public class Message
    {
        public string Id { get; set; }

        [NotMapped]
        public User Sender { get; set; }

        [NotMapped]
        public User Receiver { get; set; }

        public string ChatId { get; set; }

        public string SenderId { get; set; }

        public string ReceiverId { get; set; }

        public DateTime CreationTime { get; set; }

        [NotMapped]
        public string ReadableTime { get; set; }

        public string Content { get; set; }

        public bool Seen { get; set; }
        public Message()
        {
            Sender = new User();
            Receiver = new User();

        }
    }
}

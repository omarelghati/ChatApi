using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApi.Models
{
    public class Chat
    {
        public long Id { get; set; }
        [NotMapped]
        public virtual List<User> Members { get; set; }

        public long Member1 { get; set; }

        public long Member2 { get; set; }

        public virtual List<Message> Messages { get; set; }
        public Chat()
        {
            Messages = new List<Message>();
        }
    }
}

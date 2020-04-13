using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApi.Models
{
    public class Chat
    {
        public string Id { get; set; }
        
        [NotMapped]
        public virtual List<User> Members { get; set; }

        public string Member1 { get; set; }

        public string Member2 { get; set; }

        public virtual List<Message> Messages { get; set; }

        public string LastMessage { get; set; }
    }
}

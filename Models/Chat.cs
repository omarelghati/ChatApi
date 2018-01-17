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
        [NotMapped]
        public virtual List<User> Members { get; set; }

        public int Member1 { get; set; }

        public int Member2 { get; set; }

        public virtual List<Message> Messages { get; set; }
    }
}

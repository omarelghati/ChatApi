using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApi.Models
{
    public class Chat
    {
        public long Id { get; set; }
        public virtual ICollection<User> Members { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
    }
}

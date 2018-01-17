using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApi.Models
{
    public class Friendship
    {
        public long SenderId { get; set; }

        public long ReceiverId { get; set; }
        [NotMapped]
        public virtual User Sender { get; set; }
        [NotMapped]
        public virtual User Receiver { get; set; }

        public bool status { get; set; }
        public Friendship()
        {
            this.Sender = new User();
            this.Receiver = new User();
        }
    }
}

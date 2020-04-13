using ChatApi.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApi.Models
{
    public class Friendship
    {
        public string SenderId { get; set; }

        public string ReceiverId { get; set; }
        
        [NotMapped]
        public virtual User Sender { get; set; }
        
        [NotMapped]
        public virtual User Receiver { get; set; }

        public FriendshipStatus Status { get; set; }
    }
}

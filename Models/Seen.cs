using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApi.Models
{
    public class Seen
    {
        public long ReceiverId { get; set; }
        public long ChatId { get; set; }
        public long MessageId { get; set; }
        public bool SeenStatus { get; set; }

    }
}

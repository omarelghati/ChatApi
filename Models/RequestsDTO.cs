using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApi.Models
{
    public class RequestsDTO
    {
        public RequestsDTO()
        {
            Received = new List<User>();
            Sent = new List<User>();
        }
        public List<User> Received { get; set; }

        public List<User> Sent { get; set; }
    }
}

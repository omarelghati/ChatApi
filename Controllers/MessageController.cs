using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ChatApi.Hubs;
using ChatApi.Models;
using ChatApi.Context;

namespace ChatApi.Controllers
{
    [Route("api/message")]
    public class MessageController : Controller
    {
        private IHubContext<MessageHub> _messageHub;
        private ChatContext dbcontext;

        public MessageController(IHubContext<MessageHub> MessageHub,ChatContext dbcontext)
        {
            _messageHub = MessageHub;
            this.dbcontext = dbcontext;
        }
        [HttpPost("post")]
        public IActionResult Post([FromBody] Message message)
        {
            message.CreationTime = DateTime.UtcNow.ToString("HH:mm");
            _messageHub.Clients.All.InvokeAsync("Send",message, message.ReceiverId);
            return Ok(message);
        }

        [HttpGet("getChats/{userid}")]
        public IActionResult getChats(int userid)
        {

            var chats = dbcontext.Chats.Where(m => m.Member1 == userid || m.Member2 == userid).ToList();
            if (chats == null)
                return NotFound("nothing is here");
            foreach (var chat in chats)
            {
                var receiver = dbcontext.Users.FirstOrDefault(u => u.Id == chat.Member2);
                var sender = dbcontext.Users.FirstOrDefault(u => u.Id == chat.Member1);
                chat.Members = new List<User>();
                chat.Members.Add(receiver);
                chat.Members.Add(sender);
                chat.Messages = dbcontext.Messages.Where(m => m.SenderId == userid || m.ReceiverId == userid).ToList();
            }
            //var chat = new Chat();
            //chat.Member1 = 1;
            //chat.Member2 =2 ;
            //var message = new Message();
            //message.Content = "hey";
            //message.SenderId = 1;
            //message.ReceiverId = 2;
            //message.CreationTime = DateTime.Now.ToString();
            //chat.Messages.Add(message);
            //dbcontext.Chats.Add(chat);
            //dbcontext.SaveChanges();

            return new ObjectResult(chats);
        }

    }
}
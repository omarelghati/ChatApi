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
            dbcontext.Messages.Add(message);
            dbcontext.SaveChanges();
            _messageHub.Clients.All.InvokeAsync("Send",message, message.ReceiverId);
            return Ok(message);
        }

        [HttpGet("getChats/{userid}")]
        public IActionResult getChats(int userid)
        {

            var user = dbcontext.Chats.SingleOrDefault(u => u.Id == 2);
            //var chats = dbcontext.Chats.SingleOrDefault(m => m.Member2 == 1);
            //if (chats == null)
            //    return NotFound("nothing is here");
            //foreach (var chat in chats)
            //{
            //    var receiver = dbcontext.Users.FirstOrDefault(u => u.Id == chat.Member2);
            //    var sender = dbcontext.Users.FirstOrDefault(u => u.Id == chat.Member1);
            //    chat.Members = new List<User>();
            //    chat.Members.Add(receiver);
            //    chat.Members.Add(sender);
            //    chat.Messages = dbcontext.Messages.Where(m => m.SenderId == userid || m.ReceiverId == userid).ToList();
            //}
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

            return Ok(user);
        }
        [HttpGet("getSelectedChat/{idSelected}/{idCurrent}")]
        public IActionResult getSelectedChat(int idSelected,int idCurrent)
        {
            var chat = dbcontext.Chats.FirstOrDefault(c => (c.Member1 == idSelected || c.Member2 == idSelected) && (c.Member1 == idCurrent || c.Member2 == idCurrent));
            //var chat = dbcontext.Chats.FirstOrDefault(c => (c.Member1 == 1 && c.Member2 == 2));
            if (chat == null)
            {
                var newchat = new Chat(); newchat.Member2 = idSelected; newchat.Member1 = idCurrent;
                dbcontext.Chats.Add(newchat);
                dbcontext.SaveChanges();
                return Ok(newchat);
            }

            //dbcontext.Messages.Add(m);
            var messages = dbcontext.Messages.Where(ms => ms.ChatId == chat.Id).ToList();
            chat.Messages = messages;
            return Ok(chat);
        }

    }
}
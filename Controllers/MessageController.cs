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
        private ChatContext _context;

        public MessageController(IHubContext<MessageHub> MessageHub, ChatContext dbcontext)
        {
            _messageHub = MessageHub;
            this._context = dbcontext;
        }
        [HttpPost("post")]
        public IActionResult Post([FromBody] Message message)
        {
            message.ReadableTime = DateTime.UtcNow.ToString("HH:mm");
            message.CreationTime = DateTime.UtcNow;
            var chat = _context.Chats.First(c => c.Id == message.ChatId);
            chat.LastMessage = message.Content;
            _context.Messages.Add(message);
            _context.SaveChanges();
            _messageHub.Clients.All.InvokeAsync("Send", message, message.ReceiverId);
            return Ok(message);
        }

        [HttpGet("getChats/{userid}")]
        public IActionResult getChats(string userId)
        {
            var result = default(IActionResult);
            var user = _context.Chats.First(u => u.Id == userId);
            var chats = _context.Chats.Where(m => m.Member2 == userId || m.Member2 == userId).ToList();
            if (chats.Any())
            {
                foreach (var chat in chats)
                {
                    var receiver = _context.Users.First(u => u.Id == chat.Member2);
                    var sender = _context.Users.First(u => u.Id == chat.Member1);
                    chat.Members = new List<User> { receiver, sender };
                    chat.Messages = _context.Messages.Where(m => m.ChatId == chat.Id).ToList();
                }
                //var chat = new Chat();
                //chat.Member1 = 1;
                //chat.Member2 = 2;
                //var message = new Message();
                //message.Content = "hey";
                //message.SenderId = 1;
                //message.ReceiverId = 2;
                //message.CreationTime = DateTime.Now.ToString();
                //chat.Messages.Add(message);
                //dbcontext.Chats.Add(chat);
                //dbcontext.SaveChanges(); 
                result = Ok(chats);
            }
            else
            {
                result = NotFound("nothing is here");
            }

            return result;
        }
        [HttpGet("getSelectedChat/{idSelected}/{idCurrent}")]
        public IActionResult getSelectedChat(string idSelected, string idCurrent)
        {
            var chat = _context.Chats.FirstOrDefault(c => (c.Member1 == idSelected || c.Member2 == idSelected) && (c.Member1 == idCurrent || c.Member2 == idCurrent));
            //var chat = dbcontext.Chats.FirstOrDefault(c => (c.Member1 == 1 && c.Member2 == 2));
            if (chat == null)
            {
                var newchat = new Chat(); newchat.Member2 = idSelected; newchat.Member1 = idCurrent;
                _context.Chats.Add(newchat);
                _context.SaveChanges();
                return Ok(newchat);
            }

            //dbcontext.Messages.Add(m);
            var messages = _context.Messages.Where(ms => ms.ChatId == chat.Id).ToList();
            chat.Messages = messages;
            return Ok(chat);
        }

    }
}
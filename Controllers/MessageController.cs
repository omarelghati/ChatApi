using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ChatApi.Hubs;
using ChatApi.Models;
namespace ChatApi.Controllers
{
    [Route("api/message")]
    public class MessageController : Controller
    {
        private IHubContext<MessageHub> _messageHub;

        public MessageController(IHubContext<MessageHub> MessageHub)
        {
            _messageHub = MessageHub;
        }
        [HttpPost("post")]
        public IActionResult Post([FromBody] Message message)
        {
            _messageHub.Clients.All.InvokeAsync("Send",message.Content);
            return Ok(message);
        }
    }
}
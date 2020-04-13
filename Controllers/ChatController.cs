using System.Linq;
using ChatApi.Context;
using ChatApi.Models;
using ChatApi.Uitilities;
using Microsoft.AspNetCore.Mvc;

namespace ChatApi.Controllers
{
    [Route("api/Chat")]
    [Produces("application/json")]
    public class ChatController : Controller
    {
        private ChatContext _context;

        public ChatController(ChatContext context)
        {
            _context = context;
        }

        [HttpGet("GetAll/{userId}")]
        public IActionResult GetAll(string userId)
        {
            var chats = _context.Chats
                .Where(chat => chat.Member1 == userId || chat.Member2 == userId)
                .ToList();

            return Ok(chats);
        }

        [HttpPost("CreateChat")]
        public IActionResult CreateChat([FromBody] AddMessageDTO messageDTO)
        {
            var chat = Mapper.CreateChat(messageDTO);
            _context.Chats.Add(chat);
            _context.SaveChanges();

            return Ok();
        }

    }
}
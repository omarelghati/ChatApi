
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApi.Models;
using ChatApi.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace ChatProject.Controllers
{
    [Produces("application/json")]
    [Route("api/User")]
    public class UserController : Controller
    {
        public  ChatContext context { get; }
            
        public UserController(ChatContext context)
        {
            this.context = context;
        }

        [HttpPost("getUsers/{id}")]
        public IActionResult getUsers(int id)
        {
            var users = context.Users.ToList();
            var possibleFriends = new List<User>();
            foreach (var user in users)
            {
                if (user.Id != id && context.Friendship.FirstOrDefault(f => f.ReceiverId == user.Id) == null)
                {
                    user.MemberSince = Timer.calculate(user.DateOfJoin); 
                    user.RequestSent = user.PossibleFriends.Where(t => t.SenderId == user.Id).Where(f => f.status == false).ToList();
                    user.RequestReceived = user.PossibleFriends.ToList().Where(t => t.SenderId == user.Id).Where(f => f.status == false).ToList();
                    possibleFriends.Add(user);
                }
            }
            return new ObjectResult(possibleFriends);

        }

        [HttpGet("friends/{id}")]
        public JsonResult getFriendships(int id)
        {
            var sent = context.Friendship.Where(f => f.SenderId == id).Where(x => x.status == false);
            var received = context.Friendship.Where(e => e.ReceiverId == id).Where(x => x.status == false);
            //var friends = context.Friendship.Where(f => f.SenderId==id);
            //var temp = context.Friendship.Where(f => f.ReceiverId == id);
            //friends.Concat(temp);
            //friends = friends.Where(s => s.status == true);
            foreach(var element in sent) {
                element.Sender = context.Users.FirstOrDefault(u => u.Id == element.SenderId);
                element.Receiver = context.Users.FirstOrDefault(u => u.Id == element.ReceiverId);
            }
            foreach(var element in received) {
                element.Sender = context.Users.FirstOrDefault(u => u.Id == element.SenderId);
                element.Receiver = context.Users.FirstOrDefault(u => u.Id == element.ReceiverId);
            }
            return Json(new { sent = sent, received = received });
        }
        [HttpGet("getData")]
        public IActionResult getData(int id)
        {
            return new ObjectResult(context.Users.FirstOrDefault(u => u.Id == id));
        }

        [HttpPost("login")]
        public IActionResult login([FromBody] User user)
        {
            var usr = context.Users.SingleOrDefault(u => u.Username.Equals(user.Username) && u.Password.Equals(user.Password));
            if (usr == null)
                return new ObjectResult("UserNotFound");
            return new ObjectResult(usr);
        }

        [HttpPost("sendRequest/{idSender}/{idReceiver}")]
        public IActionResult sendRequest( int idSender, int idReceiver)
        {
            var sender = context.Users.FirstOrDefault(u => u.Id == idSender);
            var receiver = context.Users.FirstOrDefault(u => u.Id == idReceiver);
            var friendship = new Friendship();
            friendship.SenderId = idSender;
            friendship.ReceiverId = idReceiver;
            friendship.Sender = sender;
            friendship.Receiver = receiver;
            context.Friendship.Add(friendship);
            context.SaveChanges();
            context.Users.FirstOrDefault(u=> u.Id==idSender).PossibleFriends.Add(friendship);
            context.SaveChanges();
            context.Users.FirstOrDefault(u=> u.Id == idReceiver).PossibleFriends.Add(friendship);
            context.SaveChanges();
            return new ObjectResult(sender);

        }
    }
}

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

        [HttpGet("getUsers/{id}")]
        public IActionResult getUsers(int id)
        {
            var curent = context.Users.Where(u => u.Id == id);
            var users = context.Users.ToList().Except(curent);
            var possibleFriends = new List<User>();
            foreach (var user in users)
            {
                if (context.Friendship.FirstOrDefault(f => f.ReceiverId == user.Id || f.SenderId == user.Id) == null)
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
        public IActionResult getFriendships(int id)
        {
            var sent = context.Friendship.Where(f => f.SenderId == id && f.status == true);
            var friends = new List<User>();
            foreach(var snt in sent)
            {
                var tmp = context.Users.FirstOrDefault(u => u.Id == snt.ReceiverId);
                friends.Add(new User { Id = tmp.Id, LName = tmp.LName, FName = tmp.FName, Username = tmp.Username, DateOfJoin = tmp.DateOfJoin });
            }
            var sent2 = context.Friendship.Where(f => f.ReceiverId == id && f.status == true);
            foreach (var snt in sent2)
            {
                var tmp = context.Users.FirstOrDefault(u => u.Id == snt.SenderId);
                friends.Add(new User { Id = tmp.Id,LName = tmp.LName,FName = tmp.FName, Username = tmp.Username,DateOfJoin = tmp.DateOfJoin});
            }
            return new ObjectResult(friends);
        }

        [HttpGet("getSent/{id}")]
        public IActionResult getSent(int id)
        {
            var sent = context.Friendship.Where(f => f.SenderId == id && f.status == false).ToList();
            foreach (var element in sent)
            {
                var tmp = context.Users.SingleOrDefault(u => u.Id == element.SenderId);
                element.Sender = new User { Username = tmp.Username, FName = tmp.FName, LName = tmp.LName};
                tmp = context.Users.SingleOrDefault(u => u.Id == element.ReceiverId);
                element.Receiver = new User {Id = tmp.Id, Username = tmp.Username, FName = tmp.FName, LName = tmp.LName };
            }
            return new ObjectResult(sent);
        }
        [HttpGet("getReceived/{id}")]
        public IActionResult getReceived(int id)
        {
            var sent = context.Friendship.Where(f => f.ReceiverId == id && f.status == false).ToList();
            foreach (var element in sent)
            {
                var tmp = context.Users.SingleOrDefault(u => u.Id == element.SenderId);
                element.Sender = new User { Username = tmp.Username, FName = tmp.FName, LName = tmp.LName };

                tmp = context.Users.SingleOrDefault(u => u.Id == element.ReceiverId);
                element.Receiver = new User { Id = tmp.Id, Username = tmp.Username, FName = tmp.FName, LName = tmp.LName };
            }
            return new ObjectResult(sent);
        }
        [HttpGet("cancelReq/{idr}/{ids}")]
        public IActionResult cancelReq(int idr,int ids)
        {
            var friendship = context.Friendship.FirstOrDefault(u => u.SenderId == ids && u.ReceiverId == idr);
            context.Friendship.Remove(friendship);
            context.SaveChanges();
            return Ok("done");
        }

        [HttpGet("acceptReq/{idr}/{ids}")]
        public IActionResult acceptReq(int idr, int ids)
        {
            var friendship = context.Friendship.FirstOrDefault(u => u.SenderId == ids && u.ReceiverId == idr);
            friendship.status = true;
            context.SaveChanges();
            return Ok("done");
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
            context.Users.FirstOrDefault(u => u.Id == idSender).PossibleFriends.Add(friendship);
            context.Users.FirstOrDefault(u=> u.Id == idReceiver).PossibleFriends.Add(friendship);
            //context.SaveChanges();
            return new ObjectResult(sender);

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApi.Models;
using ChatApi.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChatApi.Enums;
using ChatApi.Uitilities;

namespace ChatProject.Controllers
{
    [Produces("application/json")]
    [Route("api/User")]
    public class UserController : Controller
    {
        private ChatContext _context { get; }

        public UserController(ChatContext context)
        {
            this._context = context;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] User logingUser)
        {
            var user = _context.Users.SingleOrDefault(u => u.Username.Equals(logingUser.Username) && u.Password.Equals(logingUser.Password));
            if (user == null)
            {
                return new ObjectResult("UserNotFound");
            }

            return new ObjectResult(user);
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] User registerUser)
        {
            if (_context.Users.Any(u => u.Username.Equals(registerUser.Username, StringComparison.InvariantCulture)))
            {
                return new ObjectResult("UserExists");
            }
            else
            {
                registerUser.Id = Guid.NewGuid().ToString();
                //registerUser.MemberSince = DateTime.Now.ToString();
                registerUser.DateOfJoin = DateTime.Now;
                _context.Users.Add(registerUser);
                _context.SaveChanges();
            }

            return new ObjectResult("OK");
        }

        [HttpGet("GetUsers/{id}")]
        public IActionResult GetUsers(string id)
        {
            var users = _context.Users.Where(u => u.Id != id).ToList();
            var unknownFriends = new List<User>();
            foreach (var user in users)
            {
                var isKnown = _context.Friendships.Any(f =>
                ((f.ReceiverId == user.Id && f.SenderId == id) ||
                (f.ReceiverId == id && f.SenderId == user.Id)) && f.Status == FriendshipStatus.Pending);

                if (!isKnown)
                {
                    user.MemberSince = Timer.calculate(user.DateOfJoin);
                    //user.RequestSent = user.PossibleFriends
                    //    .Where(t => t.SenderId == user.Id)
                    //    .Where(f => f.Status == FriendshipStatus.PendingSent)
                    //    .ToList();

                    //user.RequestReceived = user.PossibleFriends
                    //    .Where(t => t.SenderId == user.Id)
                    //    .Where(f => f.Status == FriendshipStatus.PendingReceived)
                    //    .ToList();

                    unknownFriends.Add(user);
                }
            }
            return new ObjectResult(unknownFriends);
        }

        [HttpGet("GetPendingRequests/{id}")]
        public IActionResult GetPendingRequests(string id)
        {
            var requests = new RequestsDTO();
            var friendships = _context.Friendships
                .Where(f => (f.SenderId == id || f.ReceiverId == id) && f.Status == FriendshipStatus.Pending)
                .ToList();

            foreach (var friendship in friendships)
            {
                if (friendship.SenderId == id)
                {
                    requests.Sent.Add(_context.Users.First(u => u.Id == friendship.ReceiverId));
                }
                else
                {
                    requests.Received.Add(_context.Users.First(u => u.Id == friendship.SenderId));
                }
            }

            return new ObjectResult(requests);
        }

        [HttpGet("GetSent/{id}")]
        public IActionResult GetSent(string id)
        {
            var setFriendships = _context.Friendships
                .Where(f => f.SenderId == id && f.Status == FriendshipStatus.Pending && f.SenderId == id)
                .ToList();

            foreach (var element in setFriendships)
            {
                element.Sender = _context.Users.SingleOrDefault(u => u.Id == element.SenderId);
                element.Receiver = _context.Users.SingleOrDefault(u => u.Id == element.ReceiverId);
            }
            return new ObjectResult(setFriendships);
        }
        [HttpGet("GetReceived/{id}")]
        public IActionResult GetReceived(string id)
        {
            var receivedFriendships = _context.Friendships
                .Where(f => f.ReceiverId == id && f.Status == FriendshipStatus.Pending && f.ReceiverId == id)
                .ToList();

            foreach (var element in receivedFriendships)
            {
                element.Sender = _context.Users.SingleOrDefault(u => u.Id == element.SenderId);
                element.Receiver = _context.Users.SingleOrDefault(u => u.Id == element.ReceiverId);
            }
            return new ObjectResult(receivedFriendships);
        }

        [HttpPost("cancelrequest/{userId}/{targetId}")]
        public IActionResult CancelFriendshipRequest(string userId, string targetId)
        {
            var friendship = _context.Friendships.First(u =>
                (u.ReceiverId == userId && u.SenderId == targetId) ||
                (u.SenderId == userId && u.ReceiverId == targetId));

            friendship.Status = FriendshipStatus.Deleted;
            _context.SaveChanges();
            return Ok();
        }

        [HttpGet("acceptReq/{idr}/{ids}")]
        public IActionResult AcceptFriendshipRequest(string idr, string ids)
        {
            var friendship = _context.Friendships.First(u => u.SenderId == ids && u.ReceiverId == idr);
            friendship.Status = FriendshipStatus.Accepted;
            _context.SaveChanges();
            return Ok();
        }

        [HttpGet("GetData")]
        public IActionResult GetUserData(string id)
        {
            return new ObjectResult(_context.Users.First(u => u.Id == id));
        }

        [HttpPost("sendRequest/{idSender}/{idReceiver}")]
        public IActionResult SendRequest(string idSender, string idReceiver)
        {
            var sender = _context.Users
                .First(u => u.Id == idSender);

            var receiver = _context.Users
                .First(u => u.Id == idReceiver);

            var friendship = _context.Friendships.FirstOrDefault(f => f.ReceiverId == idReceiver && f.SenderId == idSender);
            if (friendship.IsNull())
            {
                friendship = Mapper.CreateFriendship(sender, receiver);
                _context.Friendships.Add(friendship);
            }
            else
            {
                friendship.Status = FriendshipStatus.Pending;
            }
            _context.SaveChanges();

            return Ok();

        }
    }
}
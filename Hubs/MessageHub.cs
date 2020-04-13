using ChatApi.Models;
using ChatApi.Uitilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApi.Hubs
{
    public class MessageHub : Hub
    {
        public MessageHub(IUserIdProvider user)
        {
            HubUser = user as HubUser;
        }
        private readonly static ConnectionMapping<string> _connections =
            new ConnectionMapping<string>();

        public static string NextClient { get; set; }

        public HubUser HubUser;
        public async Task Send(string who, Message message)
        {
            await Clients.User(who).SendAsync("Send", message);
        }

        public async Task SendAll(Message message)
        {
            await Clients.All.SendAsync("Send", message);
        }

        public override Task OnConnectedAsync()
        {
            _connections.Add(HubUser.UserId, Context.ConnectionId);

            return base.OnConnectedAsync();
        }

        //public async Task Connect(string userId)
        //{
        //    if (_connections.GetConnections(userId).IsNull())
        //    {
        //    }
        //    await OnConnectedAsync();
        //}

        public override Task OnDisconnectedAsync(Exception exception)
        {
            _connections.Remove(HubUser.UserId, Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
        //public void SendData(string idUser)
        //{
        //    this.idUser = idUser;
        //    _connections.Add(idUser, Context.ConnectionId);
        //}
    }

    //public class HubUser : IUserIdProvider
    //{
    //    public string UserId { get; set; }
    //    public string GetUserId(HubConnectionContext connection)
    //    {
    //        return UserId;
    //    }
    //}

    public class HubUser : IUserIdProvider
    {
        private UserIdService _userIdService;
        public string UserId { get { return _userIdService.UserId; } }

        public HubUser(UserIdService UserIdService)
        {
            _userIdService = UserIdService;
        }
        public string GetUserId(HubConnectionContext connection)
        {
            if (!string.IsNullOrEmpty(_userIdService.UserId.ToString()))
            {
                return _userIdService.UserId.ToString();
            }
            return "";
        }
    }


    public class UserIdService
    {
        private IHttpContextAccessor _accessor;

        public UserIdService(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }
        public string UserId
        {
            get
            {
                if (_accessor.IsNull())
                {
                    return string.Empty;
                }
                if (_accessor.HttpContext.User.IsNull())
                {
                    return string.Empty;
                }
                var id = _accessor.HttpContext.User.Claims.Where(p => p.Type == "id").FirstOrDefault();
                var token = _accessor.HttpContext.Request.QueryString.Value.Split("%20")[1];
                //token = token.Substring(3, token.Length - 1);
                var handler = new JwtSecurityTokenHandler();
                var tokenS = handler.ReadToken(token) as JwtSecurityToken;
                var userId = tokenS.Payload["userId"] as string;
                if (userId.IsNull())
                {
                    return string.Empty;
                }
                return userId;
            }
        }
    }
}

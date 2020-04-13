using ChatApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApi.Hubs
{
    public class MessageHub : Hub
    {

        private readonly static ConnectionMapping<string> _connections =
            new ConnectionMapping<string>();
        string idUser;
        public Task Send(string who, Message message)
        {
            return Clients.Client(_connections.GetConnections(who)).InvokeAsync("Send", message);
        }
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            _connections.Remove(idUser, Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
        public void SendData(string idUser)
        {
            this.idUser = idUser;
            _connections.Add(idUser, Context.ConnectionId);
        }
    }
}

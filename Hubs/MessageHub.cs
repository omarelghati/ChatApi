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
        public Task Send(string  message)
        {
            // for(connection in _connections.GetConnections(message.ReceiverId)) {
            //     Clients.Client(connection).InvokeAsync("Send", message);
            // }
            return Clients.All.InvokeAsync("Send",message);
        }

        public override Task OnConnectedAsync()
        {
            string name = Context.User.Identity.Name;
            //_connections.Add();
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}

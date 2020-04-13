//using ChatApi.Hubs;
//using Microsoft.AspNetCore.Mvc.Filters;
//using Microsoft.AspNetCore.SignalR;
//using Microsoft.Extensions.DependencyInjection;
//using System.IdentityModel.Tokens.Jwt;

//namespace ChatApi.Uitilities
//{
//    public class SetupConnectionAttribute : ResultFilterAttribute
//    {
//        private MessageHub _hub;

//        public SetupConnectionAttribute()
//        {
//            var service = new ServiceCollection();
//            service.AddSingleton<MessageHub>();
//            var provider = service.BuildServiceProvider();
//            //_hub = provider.GetService<MessageHub>();
//        }

//        public override void OnResultExecuting(ResultExecutingContext context)
//        {
//            var token = context.HttpContext.Request.Headers["Authorization"].ToString().Split(" ")[1];
//            var handler = new JwtSecurityTokenHandler();
//            var jsonToken = handler.ReadToken(token);
//            var tokenS = handler.ReadToken(token) as JwtSecurityToken;
//            var userId = tokenS.Payload["userId"] as string;
//            //MessageHub.HubUser.UserId = userId;
//            //context.HttpContext.Request.Headers.TryGetValue("Authorization", out StringValue value);
//            base.OnResultExecuting(context);
//        }
//    }
//}
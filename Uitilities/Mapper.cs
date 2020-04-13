using ChatApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApi.Uitilities
{
    public static class Mapper
    {
        public static Friendship CreateFriendship(User sender, User receiver)
        {
            return new Friendship
            {
                ReceiverId = receiver.Id,
                SenderId = sender.Id,
                Status = Enums.FriendshipStatus.Pending
            };
        }

        public static Chat CreateChat(AddMessageDTO messageDTO)
        {
            var chat = new Chat
            {
                Id = Guid.NewGuid().ToString(),
                Member1 = messageDTO.SenderId,
                Member2 = messageDTO.TargetId,
                LastMessage = messageDTO.Message
            };

            chat.Messages = new List<Message>
            {
                new Message
                {
                    Content = messageDTO.Message,
                    ChatId = chat.Id,
                    ReceiverId = messageDTO.TargetId,
                    SenderId = messageDTO.SenderId,
                    Id = Guid.NewGuid().ToString(),
                    CreationTime = DateTime.Now,
                    ReadableTime= DateTime.Now.ToString("HH:mm"),
                    Seen = false
                }
            };

            return chat;
        }
    }
}

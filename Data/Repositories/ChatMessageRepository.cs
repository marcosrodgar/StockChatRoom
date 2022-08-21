using Microsoft.EntityFrameworkCore;
using StockBotChatRoom.Data.Entities;
using StockBotChatRoom.Models;

namespace StockBotChatRoom.Data.Repositories
{
    public class ChatMessageRepository : IChatMessageRepository
    {
        private readonly ChatContext _ctx;

        public ChatMessageRepository(ChatContext ctx)
        {
            _ctx = ctx;
        }

        public void AddMessage(ChatMessage message)
        {
            message.SentOn = DateTime.Now;
            _ctx.ChatMessages.Add(message);
        }

        public IEnumerable<ChatMessageModel> GetMostRecentMessages()
        {
            var mostRecentMessages = _ctx.ChatMessages.Include(message => message.User).OrderByDescending(message => message.SentOn).Take(50);

            return mostRecentMessages.OrderBy(message => message.SentOn).Select(message => new ChatMessageModel
            {
                Content = message.Content,
                SentOn = message.SentOn,
                UserName = message.User.UserName
            });
        }

        public bool SaveChanges()
        {   
            return _ctx.SaveChanges() > 0;
        }
    }
}

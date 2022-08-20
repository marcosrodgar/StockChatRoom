using StockBotChatRoom.Data.Entities;

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

        public IEnumerable<ChatMessage> GetMostRecentMessages()
        {
            return _ctx.ChatMessages.OrderByDescending(message => message.SentOn).Take(50);
        }

        public bool SaveChanges()
        {   
            return _ctx.SaveChanges() > 0;
        }
    }
}

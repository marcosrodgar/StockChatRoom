using StockBotChatRoom.Data.Entities;

namespace StockBotChatRoom.Data.Repositories
{
    public interface IChatMessageRepository
    {
        public IEnumerable<ChatMessage> GetMostRecentMessages();
        public void AddMessage(ChatMessage message);
        public bool SaveChanges();
    }
}

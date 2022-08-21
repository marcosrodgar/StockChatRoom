using StockBotChatRoom.Data.Entities;
using StockBotChatRoom.Models;

namespace StockBotChatRoom.Data.Repositories
{
    public interface IChatMessageRepository
    {
        public IEnumerable<ChatMessageModel> GetMostRecentMessages();
        public void AddMessage(ChatMessage message);
        public bool SaveChanges();
    }
}

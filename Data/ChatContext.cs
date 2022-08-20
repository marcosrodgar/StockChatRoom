using Microsoft.EntityFrameworkCore;
using StockBotChatRoom.Data.Entities;

namespace StockBotChatRoom.Data
{
    public class ChatContext : DbContext
    {
        public DbSet<ChatMessage> ChatMessages { get; set; }

    }
}

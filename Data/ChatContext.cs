using Microsoft.EntityFrameworkCore;
using StockBotChatRoom.Data.Entities;

namespace StockBotChatRoom.Data
{
    public class ChatContext : DbContext
    {
        private readonly IConfiguration _config;

        public ChatContext(IConfiguration config)
        {
            _config = config;
        }
        public DbSet<ChatMessage> ChatMessages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(_config["ConnectionStrings:ChatContextDb"]);
        }

    }
}

using StockBotChatRoom.Data.Entities;
using StockBotChatRoom.Models;

namespace StockBotChatRoom.QueueMessages
{
    public class StockCommandMessage 
    {
        public string QueueName { get; } = "stock.commands";
        public ChatMessageModel ChatMessage { get; set; }
    }
}

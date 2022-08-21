namespace StockBotChatRoom.Models
{
    public class ChatMessageModel
    {
        public string Content { get; set; }
        public DateTime? SentOn { get; set; }
        public string? UserName { get; set; }
    }
}

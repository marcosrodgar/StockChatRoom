namespace StockBotChatRoom.Data.Entities
{
    public class ChatMessage
    {

        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime SentOn { get; set; }

        public  ChatUser? User { get; set; }
    }
}

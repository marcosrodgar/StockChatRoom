namespace StockBotChatRoom.Hubs
{
    using Microsoft.AspNetCore.SignalR;
    using StockBotChatRoom.Models;

    public class ChatHub : Hub
    {
        public async Task SendMessage(ChatMessageModel chatMessage)
        {
            await Clients.All.SendAsync("ReceiveMessage", chatMessage);
        }
    }
}


namespace StockBotChatRoom.Services.Interfaces
{
    using StockBotChatRoom.QueueMessages;

    public interface IQueueService
    {
        void QueueMessage(StockCommandMessage message);
    }
}

namespace StockBotChatRoom.Services.Interfaces
{
    public interface IStockApiService
    {
        public Task<string> GetStockPrice(string stockCode);
    }
}

using StockBotChatRoom.Services.Interfaces;

namespace StockBotChatRoom.Services
{
    public class StockApiService : IStockApiService
    {
        private readonly IConfiguration _config;
        private string ApiUrl { get; set; }
        public StockApiService(IConfiguration config)
        {
            _config = config;
            ApiUrl = _config["StockApiUrl"];
        }

        public async Task<string> GetStockPrice(string stockCode)
        {
            using (var httpClient = new HttpClient())
            {

                httpClient.BaseAddress = new Uri(ApiUrl);

                var response = await httpClient.GetAsync($"?s={stockCode}&f=sd2t2ohlcv&h&e=csv");

                var responseStream = response.Content.ReadAsStream();

                string stockPrice = FindStockPrice(responseStream);

                return stockPrice;
            }
        }

        private string FindStockPrice(Stream responseStream)
        {
            string stockPrice = string.Empty;
            using (var reader = new StreamReader(responseStream))
            {
                int pricePosition = 0;
                var firstLine = reader.ReadLine();
                var headers = firstLine.Split(',');
                for (int i = 0; i < headers.Length; i++)
                {
                    if (headers[i] == "Close")
                    {
                        pricePosition = i;
                        break;
                    }

                }

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    stockPrice = values[pricePosition];

                }
            }

            var result = Decimal.TryParse(stockPrice, out decimal decimalPrice);

            if (!result)
            {
                return string.Empty;
            }

            return stockPrice;

        }
    }

}

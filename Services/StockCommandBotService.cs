using Microsoft.AspNetCore.SignalR;
using StockBotChatRoom.Data.Entities;
using StockBotChatRoom.Hubs;
using StockBotChatRoom.Models;
using StockBotChatRoom.Services.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using Newtonsoft.Json;
using StockBotChatRoom.QueueMessages;

namespace StockBotChatRoom.Services
{
    public class StockCommandBotService : BackgroundService
    {
        private readonly IConfiguration _config;
        private string ApiUrl { get; set; }
        protected readonly IServiceScopeFactory _serviceScopeFactory;

        public StockCommandBotService(IConfiguration config, IServiceScopeFactory serviceScopeFactory, IHubContext<ChatHub> chatHub)
        {
            _config = config;
            _serviceScopeFactory = serviceScopeFactory;
            ApiUrl = _config["StockApiUrl"];
      
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "stock.commands",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += async (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var bodyString = Encoding.UTF8.GetString(body);
                    var commandMessage = JsonConvert.DeserializeObject<StockCommandMessage>(bodyString);
                    Console.WriteLine(" [x] Received {0}", bodyString);
                    ProcessCommand(commandMessage.ChatMessage);
                };

                while (!stoppingToken.IsCancellationRequested)
                {  

                    channel.BasicConsume(queue: "stock.commands",
                                         autoAck: true,
                                         consumer: consumer);
                    await Task.Delay(10000000);
                }
                
                
            }

        }

        public async Task<bool> ProcessCommand(ChatMessageModel message)
        {
            var stockCode = message.Content.Split('=')[1];
            var scope = _serviceScopeFactory.CreateScope();
            var chatHub = scope.ServiceProvider.GetRequiredService<IHubContext<ChatHub>>();

            if (string.IsNullOrEmpty(stockCode))
            {
                return false;
            }

            using(var httpClient = new HttpClient())
            {    
                
                httpClient.BaseAddress = new Uri(ApiUrl);
              
                var response = await httpClient.GetAsync($"?s={stockCode}&f=sd2t2ohlcv&h&e=csv");

                var responseStream = response.Content.ReadAsStream();
                
                string stockPrice = FindStockPrice(responseStream);
                
                if(string.IsNullOrEmpty(stockPrice))
                {
                    var chatMessage = new ChatMessageModel
                    {
                        Content = $"Couldn't find stock quote for the code provided {stockCode}. Please check and try again.",
                        SentOn = DateTime.Now,
                        UserName = "Stock Bot"
                    };

                    await chatHub.Clients.All.SendAsync("ReceiveMessage", chatMessage);
                    throw new Exception("Unable to find stock price");
                    
                }
                else
                {
                  
                    var chatMessage = new ChatMessageModel
                    {
                        Content = $"{stockCode} quote is {stockPrice} per share",
                        SentOn = DateTime.Now,
                        UserName = "Stock Bot"
                    };

                    await chatHub.Clients.All.SendAsync("ReceiveMessage", chatMessage);
                }

            }            

            return false;

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

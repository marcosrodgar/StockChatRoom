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
using Microsoft.AspNetCore.Identity;
using StockBotChatRoom.Data.Repositories;
using StockBotChatRoom.Data;

namespace StockBotChatRoom.Services
{
    public class StockCommandBotService : BackgroundService
    {
     
        private readonly IServiceProvider _serviceProvider;
        private readonly IStockApiService _stockApiService;
  

        public StockCommandBotService(IServiceProvider serviceProvider, IStockApiService stockApiService)
        {     
            _serviceProvider = serviceProvider;
            _stockApiService = stockApiService;
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
                    var stockQuote = ProcessCommand(commandMessage.ChatMessage);
                    SendMessageToChatroom(stockQuote.Result);
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

        public async Task<string> ProcessCommand(ChatMessageModel message)
        {
            var splitMessage = message.Content.Split('=');
            var stockCode = string.Empty;
           
            if(splitMessage.Length < 2)
            {
                await Task.Delay(2000);
                return $"Couldn't understand the command provided: ({message.Content}). Please try again.";
            }
            else
            {
                if (splitMessage[0].ToLower()!="/stock")
                {
                    return $"Couldn't understand the command provided: ({message.Content}). Please try again.";
                }
                stockCode = splitMessage[1];
                if (string.IsNullOrEmpty(stockCode))
                {
                    await Task.Delay(2000);
                    return $"Stock code cannot be blank. Please provide a valid stock code.";
                }
            }
            

            var stockPrice = await _stockApiService.GetStockPrice(stockCode);
                
            if(string.IsNullOrEmpty(stockPrice))
            {
                return $"Couldn't find stock quote for the code provided {stockCode}. Please check and try again.";                                     
            }

            return $"{stockCode.ToUpper()} quote is {stockPrice} per share";        
           
        }

        public async void SendMessageToChatroom(string content)
        {

            using (var scope = _serviceProvider.CreateScope()) 
            {
                var chatContext = scope.ServiceProvider.GetService<ChatContext>();

                var botUser = chatContext.Users.Where(x => x.UserName == "StockBot").Single();

                ChatMessage persistedMessage = new ChatMessage
                {
                    Content = content,
                    SentOn = DateTime.Now,
                    User = botUser
                };
                
                chatContext.ChatMessages.Add(persistedMessage);
                await chatContext.SaveChangesAsync();

                var chatHub = _serviceProvider.GetRequiredService<IHubContext<ChatHub>>();

                var chatMessage = new ChatMessageModel
                {
                    Content = persistedMessage.Content,
                    SentOn = persistedMessage.SentOn,
                    UserName = "StockBot"
                };

                await chatHub.Clients.All.SendAsync("ReceiveMessage", chatMessage);


            }


           
        }

        

    
    }
}

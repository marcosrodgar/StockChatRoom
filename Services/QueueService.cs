using StockBotChatRoom.Services.Interfaces;
using RabbitMQ.Client;
using System.Text;
using StockBotChatRoom.QueueMessages;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;

namespace StockBotChatRoom.Services
{
    public class QueueService : IQueueService
    {
        public void QueueMessage(StockCommandMessage message)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue:message.QueueName ,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var serializedMessage = JsonConvert.SerializeObject(message);

                var body = Encoding.UTF8.GetBytes(serializedMessage);


                channel.BasicPublish(exchange: "",
                                     routingKey: message.QueueName,
                                     basicProperties: null,
                                     body: body);

                
                Console.WriteLine(" [x] Sent {0}", message);
            }

        }
    }
}

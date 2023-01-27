using Microsoft.AspNetCore.SignalR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using StockChat.Hubs;
using System.Text;

namespace StockChat.Events
{
    public class RabbitMQConsumer : BackgroundService
    {

        private readonly IHubContext<ChatHub> _chatHubContext;
        private bool sendMessageToChatRoom;

        public RabbitMQConsumer(IHubContext<ChatHub> chatHubContext)
        {
            _chatHubContext = chatHubContext;
            sendMessageToChatRoom = true;
        }

        public RabbitMQConsumer()
        {
            sendMessageToChatRoom = false;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            
            string stockMessage = string.Empty;

            stoppingToken.Register(() =>
                Console.WriteLine("Background task is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                //Here we specify the Rabbit MQ Server. We use rabbitmq docker image and use it
                var factory = new ConnectionFactory
                {
                    HostName = "localhost"
                };

                //Create the RabbitMQ connection using connection factory details
                var connection = factory.CreateConnection();

                //Here we create channel with session and model
                using var channel = connection.CreateModel();

                //declare the queue after mentioning name and a few property related to that
                channel.QueueDeclare("StockQuote", durable: false, exclusive: false, autoDelete: false, arguments: null);

                //Set Event object which listen message from chanel which is sent by producer
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += async (model, eventArgs) =>
                {
                    var body = eventArgs.Body.ToArray();
                    stockMessage = Encoding.UTF8.GetString(body);
                    if (sendMessageToChatRoom)
                    {
                        await SendMessageToChatRoom(stockMessage);
                    }
                };

                //read the message
                channel.BasicConsume(queue: "StockQuote", autoAck: true, consumer: consumer);

                await Task.Delay(1000, stoppingToken);
            }

        }

        private async Task SendMessageToChatRoom(string message)
        {
            await _chatHubContext.Clients.All.SendAsync("ReceiveMessage", "StockBot", message);
        }

    }
}

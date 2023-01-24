using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace StockChat.Events
{
    public class RabbitMQConsumer : IRabbitMQConsumer
    {
        public string GetStockMessage()
        {
            string stockMessage = string.Empty;

            //Here we specify the Rabbit MQ Server. we use rabbitmq docker image and use it
            var factory = new ConnectionFactory
            {
                HostName = "localhost"
            };

            //Create the RabbitMQ connection using connection factory details as i mentioned above
            var connection = factory.CreateConnection();

            //Here we create channel with session and model
            using var channel = connection.CreateModel();

            //declare the queue after mentioning name and a few property related to that
            channel.QueueDeclare("StockQuote", durable: false, exclusive: false, autoDelete: false, arguments: null);

            //Set Event object which listen message from chanel which is sent by producer
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                stockMessage = Encoding.UTF8.GetString(body);
            };

            //read the message
            channel.BasicConsume(queue: "StockQuote", autoAck: true, consumer: consumer);

            return stockMessage;
        }
    }
}

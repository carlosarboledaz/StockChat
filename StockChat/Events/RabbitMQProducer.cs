using RabbitMQ.Client;
using System.Text;

namespace StockChat.Events
{
    public class RabbitMQProducer: IRabbitMQProducer
    {
        public void SendStockMessage(string message)
        {
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

            var body = Encoding.UTF8.GetBytes(message);

            //put the data on to the StockQuote queue
            channel.BasicPublish(exchange: "", routingKey: "StockQuote", body: body);
        }
    }

}

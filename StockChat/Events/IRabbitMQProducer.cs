namespace StockChat.Events
{
    public interface IRabbitMQProducer
    {
        void SendStockMessage(string message);
    }
}

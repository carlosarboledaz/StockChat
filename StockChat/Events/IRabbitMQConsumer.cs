namespace StockChat.Events
{
    public interface IRabbitMQConsumer
    {
        string GetStockMessage();
    }
}

namespace StockChat.Services
{
    public interface IStockService
    {
        Task<string> GetStockInfo(string stockCode);
    }
}

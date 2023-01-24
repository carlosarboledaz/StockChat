using StockChat.Data.DTO;
using StockChat.Helpers;
using System.Text;

namespace StockChat.Services
{
    public class StockService : IStockService
    {

        private readonly HttpClient _httpClient;

        public StockService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetStockInfo(string stockCode)
        {
            var uri = $"https://stooq.com/q/l/?s={stockCode}&f=sd2t2ohlcv&h&e=csv";

            var responseString = await _httpClient.GetStringAsync(uri);

            byte[] byteArray = Encoding.ASCII.GetBytes(responseString);
            MemoryStream stream = new MemoryStream(byteArray);

            var result = CSVHelper.ReadCSV<StockResult>(stream).ToList();
            var stockQuoteMessage = $"{stockCode.ToUpper()} quote is ${result[0].Close} per share";

            return stockQuoteMessage;
        }

    }
}

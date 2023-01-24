using CsvHelper;
using System.Globalization;

namespace StockChat.Helpers
{
    public static class CSVHelper
    {
        public static IEnumerable<T> ReadCSV<T>(Stream fileStream) 
        {
            using (var reader = new StreamReader(fileStream))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<T>().ToList();
                return records;
            }
        }
    }
}

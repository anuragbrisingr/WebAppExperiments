using System.Collections.Generic;

namespace SignalR.StockTicker
{
    public interface IStockTicker
    {
        IEnumerable<Stock> GetAllStocks();
    }
}
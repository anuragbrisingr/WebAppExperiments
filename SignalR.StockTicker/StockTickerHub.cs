using System.Collections.Generic;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace SignalR.StockTicker
{
    // Hubs are transient i.e., the app creates a Hub class instance for each task on the hub.
    // Hub class will handle receiving connections and calls from the client.

    [HubName("stokTickerMini")]
    public class StockTickerHub : Hub
    {
        private readonly StockTicker _stockTicker;

        // SignalR requires by default a parameterless constructor for the hub class.
        public StockTickerHub() : this(StockTicker.Instance){}

        public StockTickerHub(StockTicker stockTicker)
        {
            _stockTicker = stockTicker;
        }

        // Clients may call this method and use the Collection of Stock data returned by this method.
        // This method can be run synchronously as it is returning data that is In-memory.
        public IEnumerable<Stock> GetAllStocks()
        {
            return _stockTicker.GetAllStocks();
        }
    }
}
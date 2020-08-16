using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace SignalR.StockTicker
{
    // Only one instance of this class has to run on the server as this class mantains and updates the stock details.
    // Hence each StockTickerHub instance must have a reference of this class's Singleton instance.
    // This class has to broadcast all stock data to clients as the data is held in this class.
    // Since ths isn't a Hub class it has to get a reference to SignalR Hub connection context object to broadcas.
    public class StockTicker
    {
        // Singleton instance.
        // Lazy Initialization is not for performance here but to ensure instance-creation is thread safe.
        private readonly static Lazy<StockTicker> _instance = new Lazy<StockTicker>(() =>
                new StockTicker(GlobalHost.ConnectionManager.GetHubContext<StockTickerHub>().Clients));

        public static StockTicker Instance 
        { 
            get
            {
                return _instance.Value;
            }
        }

        // Stocks collection is defined as a ConcurrentDictionary for thread safety.
        // Alternatively it could be defined as a Dictionary explicitly locked when changes are made on the same.
        // In a real application, this sort of data is usually fetched from a DB.
        private readonly ConcurrentDictionary<string, Stock> _stocks = new ConcurrentDictionary<string, Stock>();

        private readonly object _updateStockPricesLock = new object();

        private readonly double _rangePercent = .002;

        private readonly TimeSpan _updateInterval = TimeSpan.FromMilliseconds(250);
        private readonly Random _updateOrNotRandom = new Random();

        private readonly Timer _timer;

        // This flag is designated as volatile to ensure thread safety.
        private volatile bool _updatingStockPrices = false;

        public IHubConnectionContext<dynamic> Clients { get; set; }

        // In a Hub class there is an API fo calling client methods but this class doesn't.
        // Hence this class needs to get the SignalR context instance for the StockTickerHub class to call methods on clients.
        private StockTicker(IHubConnectionContext<dynamic> clients)
        {
            Clients = clients;

            _stocks.Clear();
            var stocks = new List<Stock>
            {
                new Stock { Symbol = "MSFT", Price = 30.31m },
                new Stock { Symbol = "APPL", Price = 578.18m },
                new Stock { Symbol = "GOOG", Price = 570.30m }
            };
            // _stocks sample be like : {"MSFT", stock object}
            stocks.ForEach(stock => _stocks.TryAdd(stock.Symbol, stock));

            // TImer object periodically calls methods that update stock at random.
            // Passes parameter state as null.
            _timer = new Timer(UpdateStockPrices, null, _updateInterval, _updateInterval);
        }

        public IEnumerable<Stock> GetAllStocks()
        {
            return _stocks.Values;
        }

        // Method first locks the _updateStockPricesLok object.
        // Code checks if another thread is already updating prices.
        // Then TryUpdateStockPrice() is called on each _stock dictionary.
        private void UpdateStockPrices(object state)
        {
            lock(_updateStockPricesLock)
            {
                if(!_updatingStockPrices)
                {
                    _updatingStockPrices = true;

                    foreach(var stock in _stocks.Values)
                    {
                        if(TryUpdateStockPrice(stock))
                        {
                            // Invoking method that broadcasts stock price changes.
                            BroadcastStockPrice(stock);
                        }
                    }
                }
            }
        }

        private bool TryUpdateStockPrice(Stock stock)
        {
            var r = _updateOrNotRandom.NextDouble();
            if(r > .1)
            {
                return false;
            }

            var random = new Random((int)Math.Floor(stock.Price));
            var percentChange = random.NextDouble() * _rangePercent;
            var pos = random.NextDouble() > .51;
            var change = Math.Round(stock.Price * (decimal)percentChange, 2);
            change = pos ? change : -change;

            stock.Price += change;
            return true;
        }

        private void BroadcastStockPrice(Stock stock)
        {
            Clients.All.updateStockPrice(stock);
        }
    }
}
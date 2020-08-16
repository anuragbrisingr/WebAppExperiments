using System;

namespace SignalR.StockTicker
{
    public class Stock
    {
        private decimal _price;

        // The two properties below are Set while creating Stock.
        // The first time when Price is set its value is propagated to DayOpen.
        // Henceforth whenever Price is set the app calculates Change and PercentChange based on (Price - DayOpen).

        #region Properties set while creating Stock.
        public string Symbol { get; set; }

        public decimal Price { 
            get 
            {
                return _price;
            } 
            set 
            { 
                if(_price == value)
                {
                    return;
                }

                _price = value;

                if(DayOpen == 0)
                {
                    DayOpen = _price;
                }
            } 
        }

        #endregion

        public decimal DayOpen { get; private set; }

        public decimal Change { 
            get
            {
                return Price - DayOpen;
            }
        }

        public double PercentChange { 
            get
            {
                return (double)Math.Round(Change / Price, 4);
            }
        }
    }
}
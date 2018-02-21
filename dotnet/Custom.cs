using System;
using System.Timers;
using EhterDelta.Bots.Dontnet;

namespace Bot
{
    public class Custom : BaseBot
    {
        public Custom(EtherDeltaConfiguration config, ILogger logger = null) : base(config, logger)
        {
            var timer = new Timer();
            timer.Interval = 5000;
            timer.Elapsed += new ElapsedEventHandler(Do);
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        public void Do(object source, ElapsedEventArgs e)
        {
            var bestBuy = Service.GetBestAvailableBuy();
            var bestSell = Service.GetBestAvailableSell();

            if (bestSell != null && bestBuy != null)
            {
                /*
                 *  We have best buy price
                    We have best sell price

                    Discrepencies occur when the best buy price is higher then the best sell price.
                    Discrepencies occur when the best sell price is lower then the best buy price.

                    For both instances we:- 

                    Buy The Best Sell And Sell To Best Buy
                 *
                 */

                //where the buy price is higher then the lowest sell price
                if (bestBuy.Price > bestSell.Price)
                {
                    Console.WriteLine($"Best buy is greater then best sell... Match sell price and sell for best buy price...");
                    BuyBuyToSellAtBestBuy(bestBuy, bestSell);
                }

                //where the sell price is lower then the highest buy price
                if (bestSell.Price < bestBuy.Price)
                {
                    Console.WriteLine($"Best sell is less then best buy... Match sell price and sell for best buy price...");
                    BuyBuyToSellAtBestBuy(bestBuy, bestSell);
                }
            }
        }

        private void BuyBuyToSellAtBestBuy(Order bestBuy, Order bestSell)
        {
            Console.WriteLine("OMG THIS COULD OF BEEN YOUR PROFIT!");
            var difference = (bestSell.Price - bestBuy.Price);
            var percentageDiff = difference / Math.Abs(bestBuy.Price) * 100;
            Console.WriteLine($"Best available Sell: {bestSell.EthAvailableVolume:N3} @ {bestSell.Price:N9}");
            Console.WriteLine($"Best available Buy: {bestBuy.EthAvailableVolume:N3} @ {bestBuy.Price:N9}");
            Console.WriteLine($"Current difference: {difference} Percentage difference: {percentageDiff}");
            Console.WriteLine($"Potential profit of: {difference * bestSell.EthAvailableVolume} Percentage profit of: {percentageDiff}");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

using Leopard.Data;

namespace ConsoleApplication1
{
    class Program
    {
        static StockTradeInfoDocumentContext stockTradeInfoDocumentContext = new StockTradeInfoDocumentContext();
        static void Main(string[] args)
        {
            while (true)
            {
                var isTradeTime = StockTradeInfoDocumentContext.IsTradeTime(DateTime.Now);
                if (isTradeTime)
                {
                    Task.Factory.StartNew(() => { stockTradeInfoDocumentContext.UpdateStockInfo(); });
                }

                var saveCount = stockTradeInfoDocumentContext.SaveCheck();

                if (saveCount >= StockTradeInfoDocumentContext.MaxSaveCount || (!isTradeTime && saveCount > 0))
                {
                    Console.WriteLine("{0} Saved items: {1} ", DateTime.Now, saveCount);
                }
                else if (saveCount > 0)
                {
                    Console.WriteLine("{0} Add range itmes: {1} ", DateTime.Now, saveCount);
                }

                Thread.Sleep(3000);
            }
        }
    }
}

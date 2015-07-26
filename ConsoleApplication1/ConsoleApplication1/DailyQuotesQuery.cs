using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Leopard.Data;

namespace ConsoleApplication1
{

    public class DailyQuotesQuery : HtmlTableQuery<Stock.Core.Domain.Models.DailyQuotes>
    {
        public override void AddMappings()
        {
            /* 在此實作 [實體屬性] 對應 [資料欄位名稱] */

            AddMapping(item => item.StockID, "證券代號", (s) =>
            {
                if (s.Trim().Length == 4)
                {
                    int value;
                    int.TryParse(s, out value);
                    return value;
                }
                else
                    return 0;
            });
            AddMapping(item => item.AccTradeShares, "成交股數");
            AddMapping(item => item.TradeCount, "成交筆數");
            AddMapping(item => item.TradeValue, "成交金額");
            AddMapping(item => item.OpeningPrice, "開盤價");
            AddMapping(item => item.HighestPrice, "最高價");
            AddMapping(item => item.LowestPrice, "最低價");
            AddMapping(item => item.LatestTradePrice, "收盤價");
            AddMapping(item => item.Change, "漲跌價差");
        }


        public override IEnumerable<HtmlAgilityPack.HtmlNode> GetTables(HtmlAgilityPack.HtmlNode documentNode)
        {
            //div[1]/div/center/table
            //return documentNode.SelectSingleNode("//div[1]/div/center/table");//給每日報表2004-2005使用
            return documentNode.SelectNodes("//div[@id='tbl-containerx']/table");
        }

        public override IEnumerable<string> GetColumns(HtmlAgilityPack.HtmlNode table)
        {
            var tr = table.Descendants("tr").Skip(1).Take(1).FirstOrDefault();
            //return tr.Descendants("td").Select(n => n.InnerText);//給每日報表2004-2005使用
            return tr.Descendants("th").Select(n => n.InnerText);
        }

        public override IEnumerable<HtmlAgilityPack.HtmlNode> GetRows(HtmlAgilityPack.HtmlNode table)
        {
            return table.Descendants("tr").Skip(2);
        }

        public override IEnumerable<string> GetCells(HtmlAgilityPack.HtmlNode row)
        {
            return row.ChildNodes.Where(n => n.Name == "td").Select(n => n.InnerText);
        }
    }

}

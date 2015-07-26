using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Leopard.Data;

namespace ConsoleApplication1
{

    public class DailyQuotesDocumentContext : HtmlDocumentContext
    {

        public override string GetSource()
        {
            var miningDate = new DateTime(2014, 09, 12);

            // 每日收盤行情 http://www.twse.com.tw/ch/trading/exchange/MI_INDEX/MI_INDEX3_print.php?genpage=genpage/Report201407/A11220140724ALLBUT0999_1.php&type=html
            var url = string.Format("http://www.twse.com.tw/ch/trading/exchange/MI_INDEX/MI_INDEX3_print.php?genpage=genpage/Report{0}/A112{1}ALLBUT0999_1.php&type=html",
                                    miningDate.ToString("yyyyMM"),
                                    miningDate.ToString("yyyyMMdd"));

            var response = Client.Execute(new RestSharp.RestRequest(url));
            var content = Encoding.Default.GetString(response.RawBytes);
            return content;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leopard.Data
{
    public abstract class HtmlDocumentContext : DocumentContext<string, HtmlAgilityPack.HtmlNode>
    {
        public virtual RestSharp.IRestClient Client { get; private set; }

        public virtual System.Net.CookieContainer CookieContainer { get; set; }


        public HtmlDocumentContext()
        {
            if (CookieContainer == null)
            {
                CookieContainer = new System.Net.CookieContainer();
            }

            Client = new RestSharp.RestClient()
            {
                CookieContainer = this.CookieContainer,
                UserAgent = "Mozilla/5.0",
            };
        }

        public override bool LoadDocument()
        {
            var html = GetSource();

            if (string.IsNullOrEmpty(html))
            {
                return false;
            }

            var hd = new HtmlAgilityPack.HtmlDocument();
            hd.LoadHtml(html);
            this.Document = hd.DocumentNode;

            return true;
        }
    }
}

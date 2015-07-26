using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leopard.Data
{
    public abstract class JsonDocumentContext<T> : DocumentContext<string, T> where T : class
    {
        public virtual RestSharp.IRestClient Client { get; private set; }

        public JsonDocumentContext()
        {
            Client = new RestSharp.RestClient()
            {
                UserAgent = "Mozilla/5.0",
            };
        }

        public override bool LoadDocument()
        {
            var jsonContent = GetSource();

            if (string.IsNullOrEmpty(jsonContent))
            {
                return false;
            }

            try
            {
                this.Document = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonContent);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

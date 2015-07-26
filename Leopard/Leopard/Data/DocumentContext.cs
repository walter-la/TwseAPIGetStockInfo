using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leopard.Data
{
    public abstract class DocumentContext<TSource, TDocument>
    {
        public virtual TDocument Document { get; internal set; }

        public abstract TSource GetSource();

        public abstract bool LoadDocument();
    }
}

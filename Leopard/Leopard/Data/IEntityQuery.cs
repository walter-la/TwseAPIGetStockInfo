using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leopard.Data
{

    /// <summary>
    /// 繼承介面後，自訂解析器，查詢後得到實體物件。
    /// </summary>
    /// <typeparam name="TDocument">查詢的文件來源</typeparam>
    public interface IEntityQuery<TDocument, TEntity>
    {
        /// <summary>
        /// 從文件中查詢實體物件。
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="document"></param>
        /// <returns></returns>
        IEnumerable<TEntity> Query(TDocument document);
    }
}

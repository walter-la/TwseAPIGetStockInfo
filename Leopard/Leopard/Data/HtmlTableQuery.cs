using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace Leopard.Data
{

    /// <summary>
    /// 自訂解析 Html Table 的代碼，提供 DocumentContext 使用查詢解析器。
    /// </summary>
    public abstract class HtmlTableQuery<TEntity> : IEntityQuery<HtmlAgilityPack.HtmlNode, TEntity>
    {
        public virtual IndexColumnMappings<TEntity> IndexColumnMappings { get; private set; }

        public HtmlTableQuery()
        {
            IndexColumnMappings = new IndexColumnMappings<TEntity>();
            AddMappings();
        }

        public abstract void AddMappings();

        public abstract IEnumerable<HtmlAgilityPack.HtmlNode> GetTables(HtmlAgilityPack.HtmlNode document);
        public abstract IEnumerable<string> GetColumns(HtmlAgilityPack.HtmlNode table);
        public abstract IEnumerable<HtmlAgilityPack.HtmlNode> GetRows(HtmlAgilityPack.HtmlNode table);
        public abstract IEnumerable<string> GetCells(HtmlAgilityPack.HtmlNode row);

        public virtual void AddMapping(Expression<Func<TEntity, object>> property, string column, Func<string, object> transformation = null)
        {
            IndexColumnMappings.AddMapping(property, column, transformation);
        }

        public IEnumerable<TEntity> Query(HtmlAgilityPack.HtmlNode document)
        {
            foreach (var table in GetTables(document))
            {
                IndexColumnMappings.MappingColumns(GetColumns(table));

                foreach (var row in GetRows(table))
                {
                    yield return IndexColumnMappings.CreateEntity(GetCells(row));
                }
            }
        }
    }
}

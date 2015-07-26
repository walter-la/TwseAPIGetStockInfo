using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
namespace Leopard.Data
{
    public class IndexColumnMappings<TEntity>
    {
        /// <summary>
        /// 儲存來源資料欄位對應實體欄位與轉換方法
        /// </summary>
        protected readonly Dictionary<string, IPropertyTransformation> _columnMappings = new Dictionary<string, IPropertyTransformation>();

        /// <summary>
        /// 以表格欄位索引來做實體屬性欄位對應
        /// </summary>
        protected readonly Dictionary<int, IPropertyTransformation> _indexColumnMappings = new Dictionary<int, IPropertyTransformation>();

        //public IndexColumnMappings(params ColumnMapping[] columnsMappings)
        //{
        //    foreach (var item in columnsMappings)
        //        if (!_columnMappings.ContainsKey(item.ColumnName))
        //            _columnMappings.Add(item.ColumnName, item.PropertyTransformation);
        //}

        /// <summary>
        /// 增加實體欄位與來源資料欄位名稱對應與轉換方法。
        /// </summary>
        /// <param name="property"></param>
        /// <param name="column"></param>
        /// <param name="transformation"></param>
        public virtual void AddMapping(Expression<Func<TEntity, object>> property, string column, Func<string, object> transformation = null)
        {
            if (!_columnMappings.ContainsKey(column))
            {
                _columnMappings.Add(column, PropertyTransformation.Create<TEntity>(property, transformation));
            }
        }

        public void MappingColumns(IEnumerable<string> columns)
        {
            foreach (var pair in columns.Select((Column, Index) => new { Column, Index }))
                if (_columnMappings.ContainsKey(pair.Column))
                    _indexColumnMappings.Add(pair.Index, _columnMappings[pair.Column]);
        }

        public TEntity CreateEntity(IEnumerable<string> cells)
        {
            var entity = Activator.CreateInstance<TEntity>();
            foreach (var cell in cells.Select((text, index) => new { text, index }))
            {
                if (_indexColumnMappings.ContainsKey(cell.index))
                {
                    var propertyTransformation = _indexColumnMappings[cell.index];
                    propertyTransformation.Property.SetValue(entity, propertyTransformation.Transformation(cell.text));
                }
            }
            return entity;
        }
    }
}

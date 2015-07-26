using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leopard.Data
{
    public class ColumnMapping
    {
        public string ColumnName { get; set; }
        public IPropertyTransformation PropertyTransformation { get; set; }

        public ColumnMapping(string columnName, IPropertyTransformation propertyTransformation)
        {
            this.ColumnName = columnName;
            this.PropertyTransformation = propertyTransformation;
        }
    }
}

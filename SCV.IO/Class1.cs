using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCV.IO
{
    public class ColumnAttribure : Attribute
    {
        public string ColumnName { get; set; }

        public ColumnAttribure(string columnName)
        {
            ColumnName = columnName;
        }
    }
}

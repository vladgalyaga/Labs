using SCV.IO.Interfaces;
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
        private readonly Type _readerType;
        public readonly object[] Args;

        private IPropertyReader reader;

        public IPropertyReader Reader
        {
            get
            {
                reader = reader ?? Activator.CreateInstance(_readerType, Args) as IPropertyReader;
                return reader;
            }
        }
        
        public ColumnAttribure(string columnName, Type readerType, params object[] args)
        {
            ColumnName = columnName;
            _readerType = readerType;
            Args = args;
        }
    }
}

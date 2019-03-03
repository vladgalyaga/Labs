using SCV.IO.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCV.IO.Readers
{
    public class NulableDateTimeReader : IPropertyReader
    {
        public object ReadProperty(string value)
        {
            if (DateTime.TryParse(value, out DateTime result))
            {
                return result;
            }
            else if (Int64.TryParse(value, out Int64 longRes))
            {
                return DateTime.FromOADate(longRes);
            }
            return null;
        }
    }
}

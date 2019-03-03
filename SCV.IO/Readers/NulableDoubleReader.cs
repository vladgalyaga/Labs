using SCV.IO.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCV.IO.Readers
{
    public class NulableDoubleReader : IPropertyReader
    {
        public object ReadProperty(string value)
        {
            if (Double.TryParse(value, out double result))
            {
                return result;
            }
            return null;
        }
    }
}

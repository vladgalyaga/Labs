using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCV.IO.Interfaces
{
    public interface IPropertyReader
    {
        object ReadProperty(string value);
    }
}

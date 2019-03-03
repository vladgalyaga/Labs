using SCV.IO;
using SCV.IO.Readers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabWithFeliks
{
    public class Expenses
    {
        [ColumnAttribure("Date", typeof(NulableDateTimeReader))]
        public DateTime? Date { get; set; }


        [ColumnAttribure("Forecast", typeof(NulableDoubleReader))]
        public double? Forecast { get; set; }

        [ColumnAttribure("HistoricalValue", typeof(NulableDoubleReader))]
        public double? HistoricalValue { get; set; }
        
        public double? CalculatedBeterValue { get; set; }
        
        public double? CalculatedWorstValue { get; set; }


        public double? DeltaRelativeForecast { get; set; }

        public double? DeltaRelativeHistorical { get; set; }

        public double? DeltaHistorical { get; set; }

        public double? GetAdditionalExpenses()
        {
            return HistoricalValue - Forecast;
        }
    }
}

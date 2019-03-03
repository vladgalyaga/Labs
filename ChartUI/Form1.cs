using LabWithFeliks;
using SCV.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ChartUI
{
    public partial class Form1 : Form
    {

        List<Expenses> _expenses = new List<Expenses>();

        public Form1()
        {
            InitializeComponent();
            chart.Series.Clear();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fileContent = new List<string>();
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|(*.csv)|*.csv|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;
                    fileContent = File.ReadAllLines(filePath, Encoding.UTF8).ToList();
                    CSVReader cSVReader = new CSVReader();
                    _expenses = cSVReader.ReadText<Expenses>(fileContent).OrderBy(x => x.Date).ToList();
                }
                DrawChart();
            }
        }


        private void DrawChart()
        {
            chart.ChartAreas.Clear();
            chart.Series.Clear();

            chart.ChartAreas.Add(new ChartArea("Math functions"));
            //Создаем и настраиваем набор точек для рисования графика, в том
            //не забыв указать имя области на которой хотим отобразить этот
            //набор точек.
            Series historicalPoints = new Series("Historical values");
            historicalPoints.ChartType = SeriesChartType.Line;
            historicalPoints.ChartArea = "Math functions";

            Series planedPoints = new Series("Planed values");
            planedPoints.ChartType = SeriesChartType.Line;
            planedPoints.ChartArea = "Math functions";


            Series calculatedPoints = new Series("Calculated bu forecast");
            calculatedPoints.ChartType = SeriesChartType.Line;
            calculatedPoints.ChartArea = "Math functions";

            Series calculatedPoints2 = new Series("Calculated by historical");
            calculatedPoints2.ChartType = SeriesChartType.Line;
            calculatedPoints2.ChartArea = "Math functions";


            _expenses.ForEach(x =>
            {
                historicalPoints.Points.AddXY(x.Date, x.HistoricalValue);
                planedPoints.Points.AddXY(x.Date, x.Forecast);
                calculatedPoints.Points.AddXY(x.Date, x.CalculatedBeterValue);
                calculatedPoints2.Points.AddXY(x.Date, x.CalculatedWorstValue);
            });

            //Добавляем созданный набор точек в Chart
            chart.Series.Add(planedPoints);
            chart.Series.Add(historicalPoints);
            chart.Series.Add(calculatedPoints);
            chart.Series.Add(calculatedPoints2);
        }

        private void calculateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var historicalData = _expenses.Where(x => x.HistoricalValue != null);
            double historicalDataCount = historicalData.Count();
            double startCof = (historicalDataCount - 1) / historicalDataCount;

            double summOfDelta = 0;
            double summOfDeltaHistorical = 0;
            double sumOfCof = 0;
            int dataCount = 0;

            Expenses tmp = null;
            foreach (var data in _expenses)
            {
                data.DeltaHistorical = data.HistoricalValue - (tmp?.HistoricalValue ?? 0);
                tmp = data;
            }

            foreach (var data in historicalData.OrderByDescending(x => x.Date))
            {
                var cof = Math.Pow(startCof, dataCount);
                sumOfCof += cof;

                summOfDelta += cof * data.GetAdditionalExpenses() ?? 0;
                summOfDeltaHistorical += cof * data.DeltaHistorical ?? 0;

                dataCount++;
            }

            var forecastDelta = summOfDelta / sumOfCof;
            var historicalDelta = summOfDeltaHistorical / sumOfCof;
            foreach (var data in _expenses)
            {
                data.CalculatedBeterValue = data.Forecast + forecastDelta;
            }

            tmp = null;
            foreach (var data in _expenses)
            {
                if (data.DeltaHistorical != null)
                {
                    tmp = data;
                    continue;
                }

                if(data.CalculatedWorstValue == null)
                {
                    data.CalculatedWorstValue = (tmp?.HistoricalValue ?? tmp?.CalculatedWorstValue) + historicalDelta;
                }
                tmp = data;
            }


            DrawChart();



            //for (int i = 1; i < _expenses.Count; i++)
            //{
            //    _expenses[i].DeltaRelativeHistorical = _expenses[i].HistoricalValue / _expenses[i - 1].HistoricalValue;
            //    _expenses[i].DeltaRelativeForecast = _expenses[i].Forecast / _expenses[i - 1].Forecast;
            //}
        }
    }
}

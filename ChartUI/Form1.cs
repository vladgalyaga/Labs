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

            //Chart myChart = new Chart();
            ////кладем его на форму и растягиваем на все окно.
            //myChart.Parent = this;
            //myChart.Dock = DockStyle.Fill;
            //myChart.ChartAreas.Add(new ChartArea("Math functions"));
            ////Создаем и настраиваем набор точек для рисования графика, в том
            ////не забыв указать имя области на которой хотим отобразить этот
            ////набор точек.
            //Series mySeriesOfPoint = new Series("Sinus");
            //mySeriesOfPoint.ChartType = SeriesChartType.Line;
            //mySeriesOfPoint.ChartArea = "Math functions";
            //for (double x = -Math.PI; x <= Math.PI; x += Math.PI / 10.0)
            //{
            //    mySeriesOfPoint.Points.AddXY(x, Math.Sin(x));
            //}
            ////Добавляем созданный набор точек в Chart
            //myChart.Series.Add(mySeriesOfPoint);
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

            _expenses.ForEach(x =>
            {
                historicalPoints.Points.AddXY(x.Date, x.HistoricalValue);
                planedPoints.Points.AddXY(x.Date, x.Forecast);
            });

            //Добавляем созданный набор точек в Chart
            chart.Series.Add(planedPoints);
            chart.Series.Add(historicalPoints);
        }

        private void calculateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Expenses tmp;
            for (int i = 0; i < _expenses.Count; i++)
            {

            }
        }
    }
}

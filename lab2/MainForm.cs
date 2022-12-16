using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace DSP2
{
    public partial class MainForm : Form
    {
        Series seriesAmpl; 
        Series seriesRms1; 
        Series seriesRms2;

        public MainForm()
        {
            InitializeComponent();
            chartStat.Series.Clear();
            seriesAmpl = new Series() { ChartType = SeriesChartType.Spline, Color = Color.Red, Name = "Amplitude"};
            seriesRms1 = new Series() { ChartType = SeriesChartType.Spline, Color = Color.Green, Name = "Rms1(Скз)" };
            seriesRms2 = new Series() { ChartType = SeriesChartType.Spline, Color = Color.Blue, Name = "Rms2(Ск0)" };
            chartStat.Series.Add(seriesAmpl);
            chartStat.Series.Add(seriesRms1);
            chartStat.Series.Add(seriesRms2);
        }

        public void CalculateValues()
        {
            seriesAmpl.Points.Clear();
            seriesRms1.Points.Clear();
            seriesRms2.Points.Clear();
            chartStat.ResetAutoValues();

            var N = 1024;
            var K = 3 * N / 4;
            var phi = 0;//Math.PI / 8;
            for (int M = K; M <= 2 * N; M++)
            {
                var signal = new SignalInfo(M,N,phi);
                seriesAmpl.Points.AddXY(M, 1 - signal.Amplitude);
                seriesRms1.Points.AddXY(M, signal.RmsInfelicity1);
                seriesRms2.Points.AddXY(M, signal.RmsInfelicity2);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            CalculateValues();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int mult = (int)numericUpDown1.Value;
            int devision = (int)numericUpDown2.Value;
            int mult1 = (int)numericUpDown3.Value;

            seriesAmpl.Points.Clear();
            seriesRms1.Points.Clear();
            seriesRms2.Points.Clear();
            chartStat.ResetAutoValues();

            var N = 1024;
            var K = mult * N / devision;
            var phi = Math.PI / mult1;
            for (int M = K; M <= 2 * N; M++)
            {
                var signal = new SignalInfo(M, N, phi);
                seriesAmpl.Points.AddXY(M, 1 - signal.Amplitude);
                seriesRms1.Points.AddXY(M, signal.RmsInfelicity1);
                seriesRms2.Points.AddXY(M, signal.RmsInfelicity2);
            }
        }
    }
}

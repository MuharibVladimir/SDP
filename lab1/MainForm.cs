using DSP.Signals;
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

namespace DSP
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            chart.Series.Add(new Series());
            chart.Series.Add(new Series());
            chart.Series.Add(new Series());
            chart.Series.Add(new Series());
        }

        private void button1_Click(object sender, EventArgs e)
        {

            for(int i = 0; i < 5; i ++)
                chart.Series[i].Points.Clear();

            var A = 7;
            var f = 5;
            var fis = new double[5] { Math.PI, 0, Math.PI / 3, Math.PI / 6, Math.PI / 2 };
            for (int i = 0; i < 5; i++)
            {
                chart.Series[i].ChartType = SeriesChartType.Line;
                chart.Series[i].Name = "Signal" + i;

                var signal = new HarmonicSignal(A, f, fis[i]);
                var values = signal.values;
                for(var j = 0; j < values.Length; j++)
                    chart.Series[i].Points.AddXY(j,values[j]);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 5; i++)
                chart.Series[i].Points.Clear();

            var A = 7;
            var fs = new double[5] { 1, 5, 11, 6, 3 };
            var fi = 3 * Math.PI / 4;
            for (int i = 0; i < 5; i++)
            {
                chart.Series[i].ChartType = SeriesChartType.Line;
                chart.Series[i].Name = "Signal" + i;

                var signal = new HarmonicSignal(A, fs[i], fi);
                var values = signal.values;
                for (var j = 0; j < values.Length; j++)
                    chart.Series[i].Points.AddXY(j, values[j]);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 5; i++)
                chart.Series[i].Points.Clear();

            var f = 3;
            var As = new double[5] { 1, 2, 11, 4, 2};
            var fi = 3 * Math.PI / 4;

            for (int i = 0; i < 5; i++)
            {
                chart.Series[i].ChartType = SeriesChartType.Line;
                chart.Series[i].Name = "Signal" + i;

                var signal = new HarmonicSignal(As[i], f, fi);
                var values = signal.values;
                for (var j = 0; j < values.Length; j++)
                    chart.Series[i].Points.AddXY(j, values[j]);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 5; i++)
                chart.Series[i].Points.Clear();

            var pi = Math.PI;

            var A = new double[5] {       9, 9,      9,      9,      9 };
            var f = new double[5] {       1, 2,      3,      4,      5 };
            var fi = new double[5] { pi / 2, 0, pi / 4, pi / 3, pi / 6 };

            var signals = new List<HarmonicSignal>();

            for (int i = 0; i < 5; i++)
            {
                var signal = new HarmonicSignal(A[i], f[i], fi[i]);
                signals.Add(signal);

            }

            var pSignal = new PolyharmonicSignal(512, signals);
            var values = pSignal.values;
            chart.Series[0].ChartType = SeriesChartType.Line;
            chart.Series[0].Name = "Polyharmonic Signal";
            for (var j = 0; j < values.Length; j++)
                chart.Series[0].Points.AddXY(j, values[j]);
        }

        private void buttonLinearPolyharmonic_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 5; i++)
                chart.Series[i].Points.Clear();

            var kA = Double.Parse(textBoxkA.Text);
            var kf = Double.Parse(textBoxkf.Text);
            var kfi = Double.Parse(textBoxkfi.Text);

            var pi = Math.PI;
            var A = new double[5] { 9, 9, 9, 9, 9 };
            var f = new double[5] { 1, 2, 3, 4, 5 };
            var fi = new double[5] { pi / 2, 0, pi / 4, pi / 3, pi / 6 };

            var signals = new List<HarmonicSignal>();

            for (int i = 0; i < 5; i++)
            {
                var signal = new HarmonicSignal(A[i], f[i], fi[i]);
                signals.Add(signal);

            }

            var pSignal = new PolyharmonicSignal(512, signals);
            var values = pSignal.GetValuesChangedByLinearLaw(kA,kf,kfi);
            chart.Series[0].ChartType = SeriesChartType.Line;
            chart.Series[0].Name = "Polyharmonic Signal";
            for (var j = 0; j < values.Length; j++)
                chart.Series[0].Points.AddXY(j, values[j]);
        }
    }
}

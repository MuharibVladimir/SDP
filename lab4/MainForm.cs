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

namespace DSP4
{
    public partial class MainForm : Form
    {
        Series seriesSignal;
        Series seriesAmplitude;
        Series seriesPhase;

        public MainForm()
        {
            InitializeComponent();

            chartSignal.Series.Clear();
            chartAmplitude.Series.Clear();
            chartPhase.Series.Clear();

            seriesSignal = new Series() { ChartType = SeriesChartType.Spline, Color = Color.Red, Name = "Signal" };
            seriesAmplitude = new Series() { ChartType = SeriesChartType.Column, Color = Color.Green, Name = "Amplitude" };
            seriesPhase = new Series() { ChartType = SeriesChartType.Column, Color = Color.Blue, Name = "Phase" };

            chartSignal.Series.Add(seriesSignal);
            chartAmplitude.Series.Add(seriesAmplitude);
            chartPhase.Series.Add(seriesPhase);
        }

        void ClearCharts()
        {
            seriesSignal.Points.Clear();
            seriesAmplitude.Points.Clear();
            seriesPhase.Points.Clear();
            chartSignal.ResetAutoValues();
        }

        public void RefreshChartValues(List<double> vals, List<double> a, List<double> phi)
        {
            ClearCharts();

            for (int i = 0; i < vals.Count; i++)
            {
                seriesSignal.Points.AddXY(i, vals[i]);
            }

            var maxColumn = 70;
            var columnCount = (a.Count < maxColumn) ? a.Count : maxColumn;
            for (int j = 0; j < columnCount; j++)
            {
                seriesAmplitude.Points.AddXY(j, a[j]);
                seriesPhase.Points.AddXY(j, phi[j]);
            }
        }

        public List<double> CalculatePolyharmonicValues(int B1, int B2, int seed = 0)
        {
            var N = Signal.N;
            var random = new Random(N / 2 - 1);
            var vals = new double[N];

            for (int i = 0; i < N; i++)
            {
                var y = B1 * Math.Sin((Math.PI * 2 * i) / N);
                for (int j = 50; j <= 70; j++)
                {
                    var ep = random.Next(0, 2);
                    y += Math.Pow(-1, ep) * B2 * Math.Sin((Math.PI * 2 * i * j) / N);
                }
                vals[i] = y;
            }

            return vals.ToList();
        }

        public (List<double> phases, List<double> amplitudes) CalculatePolyharmonicParams(List<double> vals)
        {
            var N = Signal.N;
            var harmonicCount = N / 2 - 1;
            var amplitudes = new double[harmonicCount];
            var phases = new double[harmonicCount];

            for (int j = 0; j < harmonicCount; j++)
            {
                (var A, var phi) = Signal.CalculatePolyharmonicSignalAmplitudeAndPhase(vals, j);
                amplitudes[j] = A;
                phases[j] = phi;
            }

            return (phases.ToList(), amplitudes.ToList());
        }

        private void buttonDefault_Click(object sender, EventArgs e)
        {
            if (!(int.TryParse(textBoxB1.Text, out var B1)
                 && int.TryParse(textBoxB2.Text, out var B2)))
                return;

            var signalVals = CalculatePolyharmonicValues(B1, B2);
            var signalParams = CalculatePolyharmonicParams(signalVals);
            RefreshChartValues(signalVals, signalParams.amplitudes, signalParams.phases);
        }

        private void buttonSliding_Click(object sender, EventArgs e)
        {
            if (!(int.TryParse(textBoxB1.Text, out var B1)
                && int.TryParse(textBoxB2.Text, out var B2)))
                return;

            var vals = CalculatePolyharmonicValues(B1, B2);
            int k = 1;
            var N = 3;
            var radius = (N - 1) / 2;
            for (int i = radius; i < vals.Count - radius; i++)
            {
                var sortedRange = vals.GetRange(i - radius, N);
                sortedRange.Sort();
                for (int j = 0; j < k; j++)
                {
                    sortedRange.RemoveAt(0);
                    sortedRange.RemoveAt(sortedRange.Count - 1);
                }
                vals[i] = sortedRange.Sum() / (N - 2 * k);
            }

            var signalParams = CalculatePolyharmonicParams(vals);
            RefreshChartValues(vals, signalParams.amplitudes, signalParams.phases);
        }

        private void buttonParabola_Click(object sender, EventArgs e)
        {
            if (!(int.TryParse(textBoxB1.Text, out var B1)
                && int.TryParse(textBoxB2.Text, out var B2)))
                return;

            var vals = CalculatePolyharmonicValues(B1, B2);

            for (int i = 0 + 3; i < vals.Count - 3; i++)
            {
                vals[i] = (1 / 231.0) * (5 * vals[i - 3] - 30 * vals[i - 2] + 75 * vals[i - 1]
                    + 131 * vals[i] + 75 * vals[i + 1] - 30 * vals[i + 2] + 5 * vals[i + 3]);
            }

            var signalParams = CalculatePolyharmonicParams(vals);
            RefreshChartValues(vals, signalParams.amplitudes, signalParams.phases);
        }

        private void buttonMedianFiltration_Click(object sender, EventArgs e)
        {
            if (!(int.TryParse(textBoxB1.Text, out var B1)
                && int.TryParse(textBoxB2.Text, out var B2)))
                return;

            var vals = CalculatePolyharmonicValues(B1, B2);
            var wSize = 7;
            var radius = (wSize - 1) / 2;
            for (int i = radius; i < vals.Count - radius; i++)
            {
                var sortedRange = vals.GetRange(i - radius, wSize);
                sortedRange.Sort();
                vals[i] = sortedRange[radius];
            }

            var signalParams = CalculatePolyharmonicParams(vals);
            RefreshChartValues(vals, signalParams.amplitudes, signalParams.phases);
        }
    }
}

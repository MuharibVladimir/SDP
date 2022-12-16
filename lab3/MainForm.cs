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

namespace DSP3
{
    public partial class MainForm : Form
    {
        Series seriesSignal;
        Series seriesRestoredSignal;
        Series seriesAmplitude;
        Series seriesPhase;

        DateTime? stamp = null;

        public MainForm()
        {
            InitializeComponent();

            chartSignal.Series.Clear();
            chartAmplitude.Series.Clear();
            chartPhase.Series.Clear();
            chartRestoredSignal.Series.Clear();

            seriesSignal = new Series() { ChartType = SeriesChartType.Spline, Color = Color.Red, Name = "Signal" };
            seriesRestoredSignal = new Series() { ChartType = SeriesChartType.Spline, Color = Color.Purple, Name = "Restored" };
            seriesAmplitude = new Series() { ChartType = SeriesChartType.Column, Color = Color.Green, Name = "Amplitude" };
            seriesPhase = new Series() { ChartType = SeriesChartType.Column, Color = Color.Blue, Name = "Pahse" };

            chartSignal.Series.Add(seriesSignal);
            chartAmplitude.Series.Add(seriesAmplitude);
            chartPhase.Series.Add(seriesPhase);
            chartRestoredSignal.Series.Add(seriesRestoredSignal);
        }

        void ClearCharts()
        {
            seriesSignal.Points.Clear();
            seriesAmplitude.Points.Clear();
            seriesPhase.Points.Clear(); 
            seriesRestoredSignal.Points.Clear();

            chartRestoredSignal.ResetAutoValues();
            chartSignal.ResetAutoValues();
        }

        public void RefreshChartValues(List<double> vals, List<double> a, List<double> phi)
        {
            ClearCharts();

            for (int i = 0; i < vals.Count; i++)
            {
                seriesSignal.Points.AddXY(i, vals[i]);
            }

            var maxColumn = 33;
            var columnCount = (a.Count < maxColumn) ? a.Count : maxColumn;
            for (int j = 0; j < columnCount; j++)
            {
                seriesAmplitude.Points.AddXY(j, a[j]);
                seriesPhase.Points.AddXY(j, phi[j]);
            }
        }

        public (List<double>, List<double>, List<double>) CalculateHarmonicValues()
        {
            var N = 1024;
            var vals = new List<double>();
            var amplitudes = new List<double>();
            var phases = new List<double>();
            for (int i = 0; i < N; i++)
            {
                var y = 30 * Math.Cos((Math.PI * 2 * i) / N - (3 * Math.PI / 4));
                vals.Add(y);
            }

            var harmonicCount = N / 2 - 1;

            for (int j = 0; j < harmonicCount; j++)
            {
                (var A, var phi) = Signal.CalculateHarmonicSignalAmplitudeAndPhase(vals, j);
                amplitudes.Add(A);
                phases.Add(phi);
            }

            return (vals, amplitudes, phases);
        }

        public (List<double>, List<double>, List<double>) CalculatePolyharmonicValues(int seed)
        {
            var N = 1024;
            var pi = Math.PI;

            var amplitudeSet = new int[] { 3, 5, 6, 8, 10, 13, 16 };
            var phaseSet = new double[] { pi/6, pi/4, pi/3, pi/2, 3*pi/4, pi};

            var random = new Random(seed);

            var harmonicCount = N / 2 - 1;

            var vals = new double[N];
            var amplitudes = new double[harmonicCount];
            var phases = new double[harmonicCount];

            for (int j = 1; j <= 30; j++)
            {
                var A = amplitudeSet[random.Next(0, amplitudeSet.Length)];
                var phi = phaseSet[random.Next(0, phaseSet.Length)];
                for (int i = 0; i < N; i++)
                {
                    var y = A * Math.Cos((Math.PI * 2 * i * j) / N - phi);
                    vals[i] += y;
                }
            }

            for (int j = 0; j < harmonicCount; j++)
            {
                (var A, var phi) = Signal._CalculatePolyharmonicSignalAmplitudeAndPhase(vals.ToList(), j);
                amplitudes[j] += (A);
                phases[j] += (phi);
            }

            return (vals.ToList(), amplitudes.ToList(), phases.ToList());
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
        }

        private void buttonRestore_Click(object sender, EventArgs e)
        {
            seriesRestoredSignal.Points.Clear();
            chartRestoredSignal.ResetAutoValues();

            (var vals, var a, var phi) = CalculateHarmonicValues();
            var restoredVals = Signal.RestoreHarmonicSignal(vals.Count, a, phi);

            for (int i = 0; i < restoredVals.Count; i++)
            {
                seriesRestoredSignal.Points.AddXY(i, restoredVals[i]);
            }
        }

        private void buttonGenerateHarmonic_Click(object sender, EventArgs e)
        {
            (var vals, var a, var phi) = CalculateHarmonicValues();
            RefreshChartValues(vals, a, phi);
        }

        private void buttonGeneratePolyharmonic_Click(object sender, EventArgs e)
        {
            stamp = DateTime.Now;
            (var vals, var a, var phi) = CalculatePolyharmonicValues(stamp.Value.GetHashCode());
            RefreshChartValues(vals, a, phi);
        }

        private void buttonRestorePolyharmonic_Click(object sender, EventArgs e)
        {
            if (stamp == null)
                return;

            seriesRestoredSignal.Points.Clear();
            chartRestoredSignal.ResetAutoValues();

            (var vals, var a, var phi) = CalculatePolyharmonicValues(stamp.Value.GetHashCode());
            var restoredVals = Signal.RestorePolyharmonicSignal(vals.Count, a, phi);

            for (int i = 0; i < restoredVals.Count; i++)
            {
                seriesRestoredSignal.Points.AddXY(i, restoredVals[i]);
            }
        }

        private void buttonRestorePolyWithoutPhi_Click(object sender, EventArgs e)
        {
            if (stamp == null)
                return;

            seriesRestoredSignal.Points.Clear();
            chartRestoredSignal.ResetAutoValues();

            (var vals, var a, var phi) = CalculatePolyharmonicValues(stamp.Value.GetHashCode());
            var restoredVals = Signal.RestorePolyharmonicSignal(vals.Count, a);

            for (int i = 0; i < restoredVals.Count; i++)
            {
                seriesRestoredSignal.Points.AddXY(i, restoredVals[i]);
            }
        }

        private void buttonFilter_Click(object sender, EventArgs e)
        {
            var freqString = textBoxIgnorableFreq.Text;
            if (string.IsNullOrWhiteSpace(freqString))
                return;

            var freq = freqString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                 .Select(f => (int.TryParse(f, out var res)) ? res : -1)
                                 .Where(f => f > -1)
                                 .Distinct()
                                 .ToList();

            if (stamp == null)
                return;

            seriesRestoredSignal.Points.Clear();
            chartRestoredSignal.ResetAutoValues();

            (var vals, var a, var phi) = CalculatePolyharmonicValues(stamp.Value.GetHashCode());
            var restoredVals = Signal.RestorePolyharmonicSignal(vals.Count, a,phi, freq);

            for (int i = 0; i < restoredVals.Count; i++)
            {
                seriesRestoredSignal.Points.AddXY(i, restoredVals[i]);
            }
        }

        private void buttonFilterRange_Click(object sender, EventArgs e)
        {
            var from = (int)numericUpDownFreqFrom.Value;
            var to = (int)numericUpDownFreqTo.Value;
            var freq = from != -1 ? Enumerable.Range(-1, from).ToList() : new List<int>() ;
            freq.AddRange(Enumerable.Range(to, 50-to).ToList());

            if (stamp == null)
                return;

            seriesRestoredSignal.Points.Clear();
            chartRestoredSignal.ResetAutoValues();

            (var vals, var a, var phi) = CalculatePolyharmonicValues(stamp.Value.GetHashCode());
            var restoredVals = Signal.RestorePolyharmonicSignal(vals.Count, a, phi, freq);

            for (int i = 0; i < restoredVals.Count; i++)
            {
                seriesRestoredSignal.Points.AddXY(i, restoredVals[i]);
            }
        }
    }
}

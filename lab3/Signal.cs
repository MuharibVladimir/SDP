using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSP3
{
    public static class Signal
    {
        private static double[] TSin;

        public static int N = 1024;

        static Signal()
        {
            TSin = new double[N];
            for (int i = 0; i < N; i++)
            {
                TSin[i] = Math.Sin(2 * Math.PI * i / N);
            }
        }

        public static (double, double) CalculateHarmonicSignalAmplitudeAndPhase(List<double> values, int harmonicNumber)
        {
            var vals = values;
            var N = vals.Count;

            double cosSum = 0, sinSum = 0;
            for (int n = 0; n < N; n++)
            {
                cosSum += vals[n] * Math.Cos(2 * Math.PI * n * harmonicNumber / N);
                sinSum += vals[n] * Math.Sin(2 * Math.PI * n * harmonicNumber / N);
            }

            var Acos = 2 * cosSum / N;
            var Asin = 2 * sinSum / N;
            var An = Math.Sqrt(Math.Pow(Asin, 2) + Math.Pow(Acos, 2));
            var phi = Math.Atan2(Asin , Acos);

            return (An, phi);
        }

        public static (double, double) CalculatePolyharmonicSignalAmplitudeAndPhase(List<double> values, int harmonicNumber)
        {
            var vals = values;
            var N = vals.Count;

            double cosSum = 0, sinSum = 0;
            for (int n = 0; n < N; n++)
            {
                cosSum += vals[n] * Math.Cos(2 * Math.PI * n * harmonicNumber / N);
                sinSum += vals[n] * Math.Sin(2 * Math.PI * n * harmonicNumber / N);
            }

            var Acos = 2 * cosSum / N;
            var Asin = 2 * sinSum / N;
            var An = Math.Sqrt(Math.Pow(Asin, 2) + Math.Pow(Acos, 2));
            var phi = Math.Atan2(Asin , Acos);

            return (An, phi);
        }

        public static (double, double) _CalculatePolyharmonicSignalAmplitudeAndPhase(List<double> values, int harmonicNumber)
        {
            var vals = values;
            var N = vals.Count;

            double cosSum = 0, sinSum = 0;
            int ss = 0, sc = N / 4;
            for (int n = 0; n < N; n++)
            {
                cosSum += vals[n] * TSin[sc];
                sinSum += vals[n] * TSin[ss];
                ss = (ss + harmonicNumber) % N;
                sc = (sc + harmonicNumber) % N;
            }

            var Acos = 2 * cosSum / N;
            var Asin = 2 * sinSum / N;
            var An = Math.Sqrt(Math.Pow(Asin, 2) + Math.Pow(Acos, 2));
            var phi = Math.Atan2(Asin, Acos);

            return (An, phi);
        }

        public static List<double> RestoreHarmonicSignal(int N, List<double> Amplitudes, List<double> phases)
        {
            var vals = new List<double>();

            for (int n = 0; n < N ; n++)
            {
                var result = 0.0;

                for (int j = 0; j < Amplitudes.Count; j++)
                {
                    result += Amplitudes[j] * Math.Cos((2 * Math.PI * j * n) / N - phases[j]);
                }

                vals.Add(result);
            }

            return vals;
        }

        public static List<double> RestorePolyharmonicSignal(int N, List<double> Amplitudes, List<double> phases)
        {
            var vals = new List<double>();

            for (int n = 0; n < N; n++)
            {
                var result = 0.0;

                if (Amplitudes.Any())
                    result = Amplitudes[0] / 2;

                for (int j = 1; j < Amplitudes.Count; j++)
                {
                    result += Amplitudes[j] * Math.Cos((2 * Math.PI * j * n) / N - phases[j]);
                }

                vals.Add(result);
            }

            return vals;
        }

        public static List<double> RestorePolyharmonicSignal(int N, List<double> Amplitudes)
        {
            var vals = new List<double>();

            for (int n = 0; n < N; n++)
            {
                //for (int j = 0; j < (N / 2) - 1; j++)
                var result = Amplitudes.FirstOrDefault() / 2;

                for (int j = 1; j < Amplitudes.Count; j++)
                {
                    result += Amplitudes[j] * Math.Cos((2 * Math.PI * j * n) / N);
                }

                vals.Add(result);
            }

            return vals;
        }

        public static List<double> RestorePolyharmonicSignal(int N, List<double> Amplitudes, List<double> phases, List<int> filter)
        {
            var vals = new List<double>();

            for (int n = 0; n < N; n++)
            {
                double result = 0;
                //for (int j = 0; j < (N / 2) - 1; j++)
                if (!filter.Contains(0))
                    result = Amplitudes.FirstOrDefault() / 2;

                for (int j = 1; j < Amplitudes.Count; j++)
                {
                    if(!filter.Contains(j))
                        result += Amplitudes[j] * Math.Cos((2 * Math.PI * j * n) / N - phases[j]);
                }

                vals.Add(result);
            }

            return vals;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSP.Signals
{
    public class PolyharmonicSignal
    {
        public readonly int N;
        private List<HarmonicSignal> signals = new List<HarmonicSignal>();

        public PolyharmonicSignal(int N, HarmonicSignal signal)
        {
            if (signal.N != N)
                throw new Exception();

            this.signals.Add(signal);
        }

        public PolyharmonicSignal(int N, params HarmonicSignal[] signals)
        {
            if (signals.Any(s => s.N != N))
                throw new Exception();

            this.signals.AddRange(signals);
        }

        public PolyharmonicSignal(int N, List<HarmonicSignal> signals)
        {
            if (signals.Any(s => s.N != N))
                throw new Exception();

            this.signals.AddRange(signals);
        }

        public void AddSignal(HarmonicSignal signal)
        {
            if (signal.N != N)
                throw new Exception();

            this.signals.Add(signal);
        }

        public double[] values
        {
            get
            {
                var result = signals[0].values;
                for (int i = 1; i < signals.Count; i++)
                {
                    var values = signals[i].values;
                    for (int j = 0; j < values.Length; j++)
                    {
                        result[j] += values[j];
                    }
                }

                return result;
            }
        }

        public double[] GetValuesChangedByLinearLaw(double kA, double kf, double kfi)
        {
            var result = signals[0].GetValuesChangedByLinearLaw(kA, kf, kfi);
            for (int i = 1; i < signals.Count; i++)
            {
                var values = signals[i].GetValuesChangedByLinearLaw(kA, kf, kfi);
                for (int j = 0; j < values.Length; j++)
                {
                    result[j] += values[i];
                }
            }

            return result;
        }
    }
}

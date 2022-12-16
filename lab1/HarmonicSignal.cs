using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSP.Signals
{
    public class HarmonicSignal
    {
        public readonly int N;
        public double A { get; set; }
        public double f { get; set; }
        public double fi { get; set; }

        public HarmonicSignal(double A, double f, double fi, int N = 512)
        {
            this.A = A;
            this.f = f;
            this.fi = fi;
            this.N = N;
        }

        public double[] values {
            get 
            {
                var result = new double[N];
                for (int n = 0; n < N; n++)
                {
                    result[n] = A * Math.Sin((2 * Math.PI * f * n) / N + fi);
                }

                return result;
            }
        }

        public double[] GetValuesChangedByLinearLaw(double kA, double kf, double kfi)
        {
            var result = new double[N];

            var A = this.A;
            var f = this.f;
            var fi = this.fi;

            for (int n = 0; n < N; n++)
            {
                A += A * (kA - 1) * f / N;
                fi += fi * (kfi - 1) * f / N;
                f += f * (kf - 1) * f / N;
                result[n] = A * Math.Sin((2 * Math.PI * f * n) / N + fi);
            }

            return result;
        }
    }
}

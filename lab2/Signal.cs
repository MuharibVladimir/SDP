using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSP2
{
    public class SignalInfo
    {
        public double M;
        public int N;
        public double phi = 0;

        public SignalInfo(int M, int N)
        {
            this.N = N;
            this.M = M;
        }

        public SignalInfo(int M, int N,double phi)
        {
            this.N = N;
            this.M = M;
            this.phi = phi;
        }

        public List<double> values {
            get 
            {
                var values = new List<double>();
                for (int n = 0; n <= M; n++)
                {
                    values.Add(Math.Sin((2 * Math.PI * n) / N + (phi / 180 * Math.PI)));
                }
                return values;
            }
        }

        public double RMS1
        {
            get
            {
                var values = this.values;
                return Math.Sqrt((1 / (M + 1)) * values.Aggregate((prev, curr) => prev + Math.Pow(curr, 2)));
            }
        }

        public double RMS2
        {
            get
            {
                var values = this.values;
                var part1 = (1 / (M + 1)) * values.Aggregate((prev, curr) => prev + Math.Pow(curr, 2));
                var part2 = Math.Pow((1 / (M + 1)) * values.Aggregate((prev, curr) => prev + curr), 2);
                return Math.Sqrt(part1 - part2);
            }
        }

        public double RmsInfelicity1
        {
            get
            {
                return 0.707 - RMS1;
            }
        }

        public double RmsInfelicity2
        {
            get
            {
                return 0.707 - RMS2;
            }
        }

        public double Amplitude
        {
            get
            {
                var vals = values;

                double cosSum = 0, sinSum = 0;
                for (int n = 0; n < M; n++)
                {
                    cosSum += vals[n] * Math.Cos(2 * Math.PI * n / N);
                    sinSum += vals[n] * Math.Sin(2 * Math.PI * n / N);
                }

                double An, phase;

                An = Math.Sqrt(Math.Pow(2 * sinSum / M, 2) + Math.Pow(2 * cosSum / M, 2));
                return An;
            }
        }

        public double Phase
        {
            get
            {
                var vals = values;

                double cosSum = 0, sinSum = 0;
                for (int n = 0; n < M; n++)
                {
                    cosSum += vals[n] * Math.Cos(2 * Math.PI * n / M);
                    sinSum += vals[n] * Math.Sin(2 * Math.PI * n / M);
                }

                double AcosN, AsinN, An, phase;

                AcosN = cosSum * (2 / M);
                AsinN = sinSum * (2 / M);
                phase = Math.Atan(AsinN / AcosN);
                return phase;
            }
        }
    }
}

using System;

namespace LythosBearing
{
    public static class Layered
    {
        public class PunchingResult
        {
            public double qUlt { get; set; }
            public double Ks { get; set; }
        }

        public static PunchingResult PunchingModel(double qBottom, double b, double l, double d, double h, double gamma, double c1, double phi1, double ks, double caRatio)
        {
            double shapeFactor = 1.0;
            if (l > 0)
                shapeFactor = 1.0 + b / l;

            double phi1Rad = phi1 * Math.PI / 180.0;
            double ca = caRatio * c1;

            if (ks <= 0)
                ks = 1.0 - Math.Sin(phi1Rad);

            double punchingTerm = 2 * ca * h / b * shapeFactor + gamma * h * h / b * ks * Math.Tan(phi1Rad) * shapeFactor;

            double qTop = qBottom + punchingTerm - gamma * h;

            return new PunchingResult { qUlt = qTop, Ks = ks };
        }

        public static double SpreadModel(double qBottom, double b, double l, double d, double h, double spreadAngle)
        {
            if (h <= 0) return qBottom;

            double spreadRad = spreadAngle * Math.PI / 180.0;
            double bPrime = b + 2 * h * Math.Tan(spreadRad);
            double lPrime = (l > 0) ? l + 2 * h * Math.Tan(spreadRad) : 0;

            double qTop;
            if (l > 0)
            {
                qTop = qBottom * (bPrime * lPrime) / (b * l);
            }
            else
            {
                qTop = qBottom * (bPrime / b);
            }

            return qTop;
        }
    }
}

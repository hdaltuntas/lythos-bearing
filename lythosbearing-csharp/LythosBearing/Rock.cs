using System;

namespace LythosBearing
{
    public static class Rock
    {
        public class HoekBrownResult
        {
            public double cPrime { get; set; }
            public double phiPrime { get; set; }
        }

        public static HoekBrownResult HoekBrown(double sigmaci, double gsi, double mi, double d, double pAtm = 0.1)
        {
            double mb = mi * Math.Exp((gsi - 100) / (28 - 14 * d));
            double s = Math.Exp((gsi - 100) / (9 - 3 * d));
            double a = 0.5 + 1.0 / 6.0 * (Math.Exp(-gsi / 15) - Math.Exp(-20 / 3.0));

            // Sig 3 max approximation
            double sig3max = 0.47 * Math.Pow(sigmaci * mb * pAtm, 1.0 / a);
            double sig3n = sig3max / sigmaci;

            double phiRad = Math.Asin(6 * a * mb * Math.Pow(s + mb * sig3n, a - 1) / (2 * (1 + a) * (2 + a) + 6 * a * mb * Math.Pow(s + mb * sig3n, a - 1)));
            double phiPrime = phiRad * 180.0 / Math.PI;

            double cPrime = sigmaci * ((1 + 2 * a) * s + (1 - a) * mb * sig3n) * Math.Pow(s + mb * sig3n, a - 1) / ((1 + a) * (2 + a) * Math.Sqrt(1 + (6 * a * mb * Math.Pow(s + mb * sig3n, a - 1)) / ((1 + a) * (2 + a))));

            return new HoekBrownResult { cPrime = cPrime * 1000.0, phiPrime = phiPrime }; // cPrime in kPa if sigmaci in MPa
        }

        public static double Ksp(double c, double spacing, double aperture)
        {
            double ksp = 1.0;
            if (aperture > 0)
            {
                 ksp = 3 * spacing / aperture;
                 if (ksp > 1.0) ksp = 1.0;
            }
            return ksp;
        }
    }
}

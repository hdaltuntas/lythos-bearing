using System;

namespace LythosBearing
{
    public static class Seismic
    {
        public class SeismicResult
        {
            public double H { get; set; }
            public double V { get; set; }
            public double z_gamma { get; set; }
        }

        public static SeismicResult ApplySeismicLoads(double v, double h, double kh, double kv, double phi, bool soilInertia)
        {
            double vSeismic = v * (1 - kv);
            double hSeismic = h + kh * v;

            double zGamma = 1.0;
            if (soilInertia)
            {
                double phiRad = phi * Math.PI / 180.0;
                double tanPhi = Math.Tan(phiRad);
                if (tanPhi > 0 && kh < tanPhi)
                {
                    zGamma = Math.Pow(1 - kh / tanPhi, 0.35);
                }
                else if (kh >= tanPhi)
                {
                    zGamma = 0.0;
                }
            }

            return new SeismicResult { H = hSeismic, V = vSeismic, z_gamma = zGamma };
        }
    }
}

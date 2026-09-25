using System;
using System.Collections.Generic;

namespace LythosBearing
{
    public static class Factors
    {
        public static readonly Dictionary<string, double> NcPhi0 = new Dictionary<string, double>
        {
            { "terzaghi", 5.7 },
            { "meyerhof", 5.14 },
            { "hansen", 5.14 },
            { "vesic", 5.14 },
            { "ec7", 5.14 },
            { "skempton", 5.14 }
        };

        public static readonly Dictionary<string, bool> HasDepth = new Dictionary<string, bool>
        {
            { "terzaghi", false },
            { "meyerhof", true },
            { "hansen", true },
            { "vesic", true },
            { "ec7", false },
            { "skempton", true }
        };

        public static readonly Dictionary<string, bool> HasInclination = new Dictionary<string, bool>
        {
            { "terzaghi", false },
            { "meyerhof", true },
            { "hansen", true },
            { "vesic", true },
            { "ec7", true },
            { "skempton", false }
        };

        public static readonly Dictionary<string, bool> HasBase = new Dictionary<string, bool>
        {
            { "terzaghi", false },
            { "meyerhof", false },
            { "hansen", true },
            { "vesic", true },
            { "ec7", true },
            { "skempton", false }
        };

        public static readonly Dictionary<string, bool> HasGround = new Dictionary<string, bool>
        {
            { "terzaghi", false },
            { "meyerhof", false },
            { "hansen", true },
            { "vesic", true },
            { "ec7", false },
            { "skempton", false }
        };

        private const double PhiZero = 1e-6;

        public class BearingFactors
        {
            public double Nc { get; set; }
            public double Nq { get; set; }
            public double Ngamma { get; set; }
        }

        public static BearingFactors GetBearingFactors(string method, double phi)
        {
            if (phi < PhiZero)
            {
                double nc = NcPhi0.ContainsKey(method) ? NcPhi0[method] : 5.14;
                return new BearingFactors { Nc = nc, Nq = 1.0, Ngamma = 0.0 };
            }

            double phiRad = phi * Math.PI / 180.0;
            double nq = Math.Exp(Math.PI * Math.Tan(phiRad)) * Math.Pow(Math.Tan(Math.PI / 4.0 + phiRad / 2.0), 2);
            double ncVal = (nq - 1.0) / Math.Tan(phiRad);
            double ngamma = 0.0;

            if (method == "terzaghi")
            {
                double a = Math.Exp((3.0 * Math.PI / 4.0 - phiRad / 2.0) * Math.Tan(phiRad));
                nq = a * a / (2.0 * Math.Pow(Math.Cos(Math.PI / 4.0 + phiRad / 2.0), 2));
                ncVal = (nq - 1.0) / Math.Tan(phiRad);
                // Bowles fit
                ngamma = 0.5 * Math.Tan(phiRad) * (Math.Pow(a, 2) / Math.Pow(Math.Cos(Math.PI / 4.0 + phiRad / 2.0), 2) - 1.0);
                ngamma = 10.0 * Math.Pow(Math.Tan(phiRad), 2) * Math.Tan(Math.PI / 4.0 + phiRad / 2.0) - 2.5 * Math.Tan(phiRad);
                if (phi < 1e-3) ngamma = 0; // fallback
            }
            else if (method == "meyerhof")
            {
                ngamma = (nq - 1.0) * Math.Tan(1.4 * phiRad);
            }
            else if (method == "hansen")
            {
                ngamma = 1.5 * (nq - 1.0) * Math.Tan(phiRad);
            }
            else if (method == "vesic")
            {
                ngamma = 2.0 * (nq + 1.0) * Math.Tan(phiRad);
            }
            else if (method == "ec7")
            {
                ngamma = 2.0 * (nq - 1.0) * Math.Tan(phiRad);
            }

            return new BearingFactors { Nc = ncVal, Nq = nq, Ngamma = ngamma };
        }

        public class ShapeFactors
        {
            public double sc { get; set; }
            public double sq { get; set; }
            public double sgamma { get; set; }
        }

        public static ShapeFactors GetShapeFactors(string method, string shape, double b, double l, double phi)
        {
            double ratio = (shape == "strip" || l == 0) ? 0.0 : b / l;
            if (shape == "circle") ratio = 1.0;

            double phiRad = phi * Math.PI / 180.0;
            double nq = Math.Exp(Math.PI * Math.Tan(phiRad)) * Math.Pow(Math.Tan(Math.PI / 4.0 + phiRad / 2.0), 2);
            double nc = (phi < PhiZero) ? (NcPhi0.ContainsKey(method) ? NcPhi0[method] : 5.14) : (nq - 1) / Math.Tan(phiRad);

            double sc = 1.0;
            double sq = 1.0;
            double sgamma = 1.0;

            if (method == "terzaghi")
            {
                if (shape == "rectangle" || shape == "square")
                {
                    sc = 1.0 + 0.3 * ratio;
                    sq = 1.0;
                    sgamma = 1.0 - 0.2 * ratio;
                }
                else if (shape == "circle")
                {
                    sc = 1.3;
                    sq = 1.0;
                    sgamma = 0.6;
                }
            }
            else if (method == "meyerhof")
            {
                if (phi >= 10.0)
                {
                    sc = 1.0 + 0.2 * ratio * Math.Pow(Math.Tan(Math.PI / 4.0 + phiRad / 2.0), 2);
                    sq = sc;
                    sgamma = sc;
                }
                else
                {
                    sc = 1.0 + 0.2 * ratio;
                    sq = 1.0;
                    sgamma = 1.0;
                }
            }
            else if (method == "hansen")
            {
                if (phi < PhiZero)
                {
                    sc = 0.2 * ratio;
                }
                else
                {
                    sc = 1.0 + (nq / nc) * ratio;
                }
                sq = 1.0 + ratio * Math.Sin(phiRad);
                sgamma = 1.0 - 0.4 * ratio;
            }
            else if (method == "vesic")
            {
                sc = 1.0 + (nq / nc) * ratio;
                if (phi < PhiZero) sc = 0.2 * ratio;
                sq = 1.0 + ratio * Math.Tan(phiRad);
                sgamma = 1.0 - 0.4 * ratio;
            }
            else if (method == "ec7")
            {
                if (shape == "strip")
                {
                    sc = 1.0;
                    sq = 1.0;
                    sgamma = 1.0;
                }
                else
                {
                    sq = 1.0 + ratio * Math.Sin(phiRad);
                    sgamma = 1.0 - 0.3 * ratio;
                    if (phi < PhiZero)
                    {
                        sc = 0.2 * ratio;
                    }
                    else
                    {
                        sc = (sq * nq - 1.0) / (nq - 1.0);
                    }
                }
            }

            return new ShapeFactors { sc = sc, sq = sq, sgamma = sgamma };
        }

        // Detailed translation would continue for depth_factors, inclination_factors, base_factors, ground_factors, compressibility_factors, skempton_nc, etc.
    }
}

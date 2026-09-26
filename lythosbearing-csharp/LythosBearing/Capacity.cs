using System;

namespace LythosBearing
{
    public static class Capacity
    {
        public class EffectiveAreaResult
        {
            public double B { get; set; }
            public double L { get; set; }
            public double Area { get; set; }
            public double eB { get; set; }
            public double eL { get; set; }
        }

        public static EffectiveAreaResult EffectiveArea(string shape, double b, double l, double eb, double el)
        {
            eb = Math.Abs(eb);
            el = Math.Abs(el);

            if (shape == "strip")
            {
                double beff = b - 2 * eb;
                return new EffectiveAreaResult { B = Math.Max(beff, 0), L = 1.0, Area = Math.Max(beff, 0), eB = eb, eL = 0 };
            }

            if (shape == "circle")
            {
                double e = Math.Sqrt(eb * eb + el * el);
                if (e >= b / 2.0)
                    return new EffectiveAreaResult { B = 0, L = 0, Area = 0, eB = e, eL = 0 };

                double r = b / 2.0;
                double a = Math.Acos(e / r);
                double area = r * r * (2 * a - Math.Sin(2 * a));
                double beff = 2 * r * (1 - e / r);
                double leff = area / beff;
                return new EffectiveAreaResult { B = beff, L = leff, Area = area, eB = e, eL = 0 };
            }

            // Rectangle / Square
            if (eb >= b / 2.0 || el >= l / 2.0)
                return new EffectiveAreaResult { B = 0, L = 0, Area = 0, eB = eb, eL = el };

            double bEff = b - 2 * eb;
            double lEff = l - 2 * el;
            double beffMin = Math.Min(bEff, lEff);
            double leffMax = Math.Max(bEff, lEff);

            return new EffectiveAreaResult { B = beffMin, L = leffMax, Area = beffMin * leffMax, eB = eb, eL = el };
        }

        public class CapacityTerms
        {
            public double qUlt { get; set; }
            public double qc { get; set; }
            public double qq { get; set; }
            public double qgamma { get; set; }
        }

        public static CapacityTerms GeneralEquation(string method, double c, double q, double gamma, double bEff, Factors.BearingFactors n, Factors.ShapeFactors s, double dc=1, double dq=1, double dgamma=1, double ic=1, double iq=1, double igamma=1, double bc=1, double bq=1, double bgamma=1, double gc=1, double gq=1, double ggamma=1, double phi=0, bool undrained=false)
        {
            double qc = 0, qq = 0, qg = 0;

            if (undrained && method == "hansen")
            {
                double sc = s.sc;
                qc = 5.14 * c * (1 + sc + dc - ic - bc - gc);
                qq = q;
                qg = 0;
            }
            else if (undrained && method == "vesic")
            {
                double sc = s.sc;
                qc = 5.14 * c * (1 + sc + dc - ic - bc - gc);
                qq = q;
                qg = 0;
            }
            else if (undrained && method == "ec7")
            {
                double sc = s.sc;
                qc = 5.14 * c * (1 + sc + dc - ic - bc - gc);
                qq = q;
                qg = 0;
            }
            else if (undrained && method == "skempton")
            {
                qc = c * n.Nc * s.sc * dc;
                qq = q;
                qg = 0;
            }
            else if (undrained && (method == "terzaghi" || method == "meyerhof"))
            {
                qc = c * n.Nc * s.sc * dc * ic * bc * gc;
                qq = q * n.Nq * s.sq * dq * iq * bq * gq;
                qg = 0.5 * gamma * bEff * n.Ngamma * s.sgamma * dgamma * igamma * bgamma * ggamma;
            }
            else
            {
                qc = c * n.Nc * s.sc * dc * ic * bc * gc;
                qq = q * n.Nq * s.sq * dq * iq * bq * gq;
                qg = 0.5 * gamma * bEff * n.Ngamma * s.sgamma * dgamma * igamma * bgamma * ggamma;
            }

            return new CapacityTerms { qUlt = qc + qq + qg, qc = qc, qq = qq, qgamma = qg };
        }
    }
}

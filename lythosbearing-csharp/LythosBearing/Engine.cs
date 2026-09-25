using System;
using System.Collections.Generic;

namespace LythosBearing
{
    public class Engine
    {
        public class ProfileLayer
        {
            public double Thickness { get; set; }
            public double Gamma { get; set; }
            public double GammaSat { get; set; }
            public double CPrime { get; set; }
            public double PhiPrime { get; set; }
            public double Cu { get; set; }
            public bool IsCohesive { get; set; }
        }

        public class Profile
        {
            public List<ProfileLayer> Layers { get; set; }
            public double WaterDepth { get; set; }
            public double GammaWater { get; set; }

            public Profile()
            {
                Layers = new List<ProfileLayer>();
                WaterDepth = 1000;
                GammaWater = 9.81;
            }

            public double TotalStress(double depth)
            {
                double z = 0;
                double stress = 0;
                foreach (var layer in Layers)
                {
                    double top = z;
                    double bottom = z + layer.Thickness;

                    if (depth <= top) break;

                    double segmentBottom = Math.Min(depth, bottom);
                    double dz = segmentBottom - top;

                    if (segmentBottom > WaterDepth && top < WaterDepth)
                    {
                        double dzDry = WaterDepth - top;
                        double dzWet = segmentBottom - WaterDepth;
                        stress += dzDry * layer.Gamma + dzWet * layer.GammaSat;
                    }
                    else if (top >= WaterDepth)
                    {
                        stress += dz * layer.GammaSat;
                    }
                    else
                    {
                        stress += dz * layer.Gamma;
                    }

                    z = bottom;
                }
                return stress;
            }

            public double PorePressure(double depth)
            {
                if (depth <= WaterDepth) return 0;
                return (depth - WaterDepth) * GammaWater;
            }

            public double EffectiveStress(double depth)
            {
                return TotalStress(depth) - PorePressure(depth);
            }
        }

        public class BearingAnalysis
        {
            public Profile SoilProfile { get; set; }

            public BearingAnalysis()
            {
                SoilProfile = new Profile();
            }

            public double ComputeCapacity(string method, double b, double l, double d, double eb, double el, double v, double h, double phi, double c, double gammaBase, double qSurcharge)
            {
                var effectiveArea = Capacity.EffectiveArea(b > l ? "rectangle" : "strip", b, l, eb, el);
                var factors = Factors.GetBearingFactors(method, phi);
                var shapeFactors = Factors.GetShapeFactors(method, "rectangle", effectiveArea.B, effectiveArea.L, phi);

                var capacity = Capacity.GeneralEquation(method, c, qSurcharge, gammaBase, effectiveArea.B, factors, shapeFactors,
                                                        1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, phi, false);

                return capacity.qUlt;
            }
        }
    }
}

using System;

namespace LythosBearing
{
    public static class Insitu
    {
        public static double SPT(double n60, double b, double d, double settlementTolerance)
        {
            double kd = 1 + 0.33 * d / b;
            if (kd > 1.33) kd = 1.33;

            double qa;
            if (b <= 1.2)
            {
                qa = 12 * n60 * kd * (settlementTolerance / 25.0);
            }
            else
            {
                qa = 8 * n60 * Math.Pow((b + 0.3) / b, 2) * kd * (settlementTolerance / 25.0);
            }

            return qa;
        }

        public static double CPT(double qc, double b, double settlementTolerance)
        {
            double qa;
            if (b <= 1.2)
            {
                qa = qc / 30.0 * (settlementTolerance / 25.0);
            }
            else
            {
                qa = qc / 50.0 * Math.Pow((b + 0.3) / b, 2) * (settlementTolerance / 25.0);
            }
            // Return as kPa (qc is typically in MPa, assuming qc in MPa * 1000 for kPa calculation conceptually)
            // But preserving numeric structure. Let's assume input qc is in MPa, output qa in MPa or kPa depending on user.
            // Following simple Python translation pattern:
            return qa * 1000.0;
        }

        public static double PMT(double pl, double p0, double kp)
        {
            double qNet = kp * (pl - p0);
            return qNet; // Net capacity
        }
    }
}

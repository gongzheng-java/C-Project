using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImport
{
    class CoordCovert
    {
        private const double PI = Math.PI;
        //UTM
        private const double sm_a = 6378137.0;
        private const double sm_b = 6356752.314;
        private const double sm_EccSquared = 6.69437999013e-03;
        private const double UTMScaleFactor = 0.9996;
        //数学度数-》弧度度数
        public double DegToRad(double deg)
        {
            return (deg / 180.0 * PI);
        }
        //弧度度数-》数学度数
        public double RadToDeg(double rad)
        {
            return (rad / PI * 180.0);
        }
        //根据纬度弧度值计算纬度上弧度距离
        private double ArcLengthOfMeridian(double phi)
        {
            double alpha, beta, gamma, delta, epsilon, n;
            double result;

            /* Precalculate n */
            n = (sm_a - sm_b) / (sm_a + sm_b);

            /* Precalculate alpha */
            alpha = ((sm_a + sm_b) / 2.0) * (1.0 + (Math.Pow(n, 2.0) / 4.0) + (Math.Pow(n, 4.0) / 64.0));

            /* Precalculate beta */
            beta = (-3.0 * n / 2.0) + (9.0 * Math.Pow(n, 3.0) / 16.0) + (-3.0 * Math.Pow(n, 5.0) / 32.0);

            /* Precalculate gamma */
            gamma = (15.0 * Math.Pow(n, 2.0) / 16.0) + (-15.0 * Math.Pow(n, 4.0) / 32.0);

            /* Precalculate delta */
            delta = (-35.0 * Math.Pow(n, 3.0) / 48.0) + (105.0 * Math.Pow(n, 5.0) / 256.0);

            /* Precalculate epsilon */
            epsilon = (315.0 * Math.Pow(n, 4.0) / 512.0);

            /* Now calculate the sum of the series and return */
            result = alpha * (phi + (beta * Math.Sin(2.0 * phi)) + (gamma * Math.Sin(4.0 * phi)) + (delta * Math.Sin(6.0 * phi)) + (epsilon * Math.Sin(8.0 * phi)));

            return result;
        }
        //获取中央经度弧度值
        private double UTMCentralMeridian(int zone)
        {
            return DegToRad(-183.0 + (zone * 6.0));
        }

        //
        private double FootpointLatitude(double y)
        {
            double y_, alpha_, beta_, gamma_, delta_, epsilon_, n;
            double result;

            /* Precalculate n (Eq. 10.18) */
            n = (sm_a - sm_b) / (sm_a + sm_b);

            /* Precalculate alpha_ (Eq. 10.22) */
            /* (Same as alpha in Eq. 10.17) */
            alpha_ = ((sm_a + sm_b) / 2.0) * (1 + (Math.Pow(n, 2.0) / 4) + (Math.Pow(n, 4.0) / 64));

            /* Precalculate y_ (Eq. 10.23) */
            y_ = y / alpha_;

            /* Precalculate beta_ (Eq. 10.22) */
            beta_ = (3.0 * n / 2.0) + (-27.0 * Math.Pow(n, 3.0) / 32.0) + (269.0 * Math.Pow(n, 5.0) / 512.0);

            /* Precalculate gamma_ (Eq. 10.22) */
            gamma_ = (21.0 * Math.Pow(n, 2.0) / 16.0) + (-55.0 * Math.Pow(n, 4.0) / 32.0);

            /* Precalculate delta_ (Eq. 10.22) */
            delta_ = (151.0 * Math.Pow(n, 3.0) / 96.0) + (-417.0 * Math.Pow(n, 5.0) / 128.0);

            /* Precalculate epsilon_ (Eq. 10.22) */
            epsilon_ = (1097.0 * Math.Pow(n, 4.0) / 512.0);

            /* Now calculate the sum of the series (Eq. 10.21) */
            result = y_ + (beta_ * Math.Sin(2.0 * y_)) + (gamma_ * Math.Sin(4.0 * y_)) + (delta_ * Math.Sin(6.0 * y_)) + (epsilon_ * Math.Sin(8.0 * y_));

            return result;
        }

        private double[] MapLatLonToXY(double phi, double lambda, double lambda0)
        {
            double N, nu2, ep2, t, t2, l;
            double l3coef, l4coef, l5coef, l6coef, l7coef, l8coef;
            double tmp;

            /* Precalculate ep2 */
            ep2 = (Math.Pow(sm_a, 2.0) - Math.Pow(sm_b, 2.0)) / Math.Pow(sm_b, 2.0);

            /* Precalculate nu2 */
            nu2 = ep2 * Math.Pow(Math.Cos(phi), 2.0);

            /* Precalculate N */
            N = Math.Pow(sm_a, 2.0) / (sm_b * Math.Sqrt(1 + nu2));

            /* Precalculate t */
            t = Math.Tan(phi);
            t2 = t * t;
            tmp = (t2 * t2 * t2) - Math.Pow(t, 6.0);

            /* Precalculate l */
            l = lambda - lambda0;

            /* Precalculate coefficients for l**n in the equations below
            so a normal human being can read the expressions for easting
            and northing
            -- l**1 and l**2 have coefficients of 1.0 */
            l3coef = 1.0 - t2 + nu2;

            l4coef = 5.0 - t2 + 9 * nu2 + 4.0 * (nu2 * nu2);

            l5coef = 5.0 - 18.0 * t2 + (t2 * t2) + 14.0 * nu2 - 58.0 * t2 * nu2;

            l6coef = 61.0 - 58.0 * t2 + (t2 * t2) + 270.0 * nu2 - 330.0 * t2 * nu2;

            l7coef = 61.0 - 479.0 * t2 + 179.0 * (t2 * t2) - (t2 * t2 * t2);

            l8coef = 1385.0 - 3111.0 * t2 + 543.0 * (t2 * t2) - (t2 * t2 * t2);

            /* Calculate easting (x) */
            double[] xy = new double[2];
            xy[0] = N * Math.Cos(phi) * l + (N / 6.0 * Math.Pow(Math.Cos(phi), 3.0) * l3coef * Math.Pow(l, 3.0))
                + (N / 120.0 * Math.Pow(Math.Cos(phi), 5.0) * l5coef * Math.Pow(l, 5.0))
                + (N / 5040.0 * Math.Pow(Math.Cos(phi), 7.0) * l7coef * Math.Pow(l, 7.0));

            /* Calculate northing (y) */
            xy[1] = ArcLengthOfMeridian(phi)
                + (t / 2.0 * N * Math.Pow(Math.Cos(phi), 2.0) * Math.Pow(l, 2.0))
                + (t / 24.0 * N * Math.Pow(Math.Cos(phi), 4.0) * l4coef * Math.Pow(l, 4.0))
                + (t / 720.0 * N * Math.Pow(Math.Cos(phi), 6.0) * l6coef * Math.Pow(l, 6.0))
                + (t / 40320.0 * N * Math.Pow(Math.Cos(phi), 8.0) * l8coef * Math.Pow(l, 8.0));

            return xy;
        }

        private double[] MapXYToLatLon(double x, double y, double lambda0)
        {
            double phif, Nf, Nfpow, nuf2, ep2, tf, tf2, tf4, cf;
            double x1frac, x2frac, x3frac, x4frac, x5frac, x6frac, x7frac, x8frac;
            double x2poly, x3poly, x4poly, x5poly, x6poly, x7poly, x8poly;

            /* Get the value of phif, the footpoint latitude. */
            phif = FootpointLatitude(y);

            /* Precalculate ep2 */
            ep2 = (Math.Pow(sm_a, 2.0) - Math.Pow(sm_b, 2.0)) / Math.Pow(sm_b, 2.0);

            /* Precalculate cos (phif) */
            cf = Math.Cos(phif);

            /* Precalculate nuf2 */
            nuf2 = ep2 * Math.Pow(cf, 2.0);

            /* Precalculate Nf and initialize Nfpow */
            Nf = Math.Pow(sm_a, 2.0) / (sm_b * Math.Sqrt(1 + nuf2));
            Nfpow = Nf;

            /* Precalculate tf */
            tf = Math.Tan(phif);
            tf2 = tf * tf;
            tf4 = tf2 * tf2;

            /* Precalculate fractional coefficients for x**n in the equations
            below to simplify the expressions for latitude and longitude. */
            x1frac = 1.0 / (Nfpow * cf);

            Nfpow *= Nf;   /* now equals Nf**2) */
            x2frac = tf / (2.0 * Nfpow);

            Nfpow *= Nf;   /* now equals Nf**3) */
            x3frac = 1.0 / (6.0 * Nfpow * cf);

            Nfpow *= Nf;   /* now equals Nf**4) */
            x4frac = tf / (24.0 * Nfpow);

            Nfpow *= Nf;   /* now equals Nf**5) */
            x5frac = 1.0 / (120.0 * Nfpow * cf);

            Nfpow *= Nf;   /* now equals Nf**6) */
            x6frac = tf / (720.0 * Nfpow);

            Nfpow *= Nf;   /* now equals Nf**7) */
            x7frac = 1.0 / (5040.0 * Nfpow * cf);

            Nfpow *= Nf;   /* now equals Nf**8) */
            x8frac = tf / (40320.0 * Nfpow);

            /* Precalculate polynomial coefficients for x**n.
            -- x**1 does not have a polynomial coefficient. */
            x2poly = -1.0 - nuf2;

            x3poly = -1.0 - 2 * tf2 - nuf2;

            x4poly = 5.0 + 3.0 * tf2 + 6.0 * nuf2 - 6.0 * tf2 * nuf2 - 3.0 * (nuf2 * nuf2) - 9.0 * tf2 * (nuf2 * nuf2);

            x5poly = 5.0 + 28.0 * tf2 + 24.0 * tf4 + 6.0 * nuf2 + 8.0 * tf2 * nuf2;

            x6poly = -61.0 - 90.0 * tf2 - 45.0 * tf4 - 107.0 * nuf2 + 162.0 * tf2 * nuf2;

            x7poly = -61.0 - 662.0 * tf2 - 1320.0 * tf4 - 720.0 * (tf4 * tf2);

            x8poly = 1385.0 + 3633.0 * tf2 + 4095.0 * tf4 + 1575 * (tf4 * tf2);

            /* Calculate latitude */
            double[] xy = new double[2];
            xy[1] = phif + x2frac * x2poly * (x * x) + x4frac * x4poly * Math.Pow(x, 4.0) + x6frac * x6poly * Math.Pow(x, 6.0) + x8frac * x8poly * Math.Pow(x, 8.0);

            /* Calculate longitude */
            xy[0] = lambda0 + x1frac * x + x3frac * x3poly * Math.Pow(x, 3.0) + x5frac * x5poly * Math.Pow(x, 5.0) + x7frac * x7poly * Math.Pow(x, 7.0);

            return xy;
        }
        public double[] LatLonToUTMXY(double lon, double lat, int zone)
        {
            //int zone = (int)(lon / 6.0) + 31;
            double[] xy = MapLatLonToXY(lat, lon, UTMCentralMeridian(zone));

            /* Adjust easting and northing for UTM system. */
            xy[0] = xy[0] * UTMScaleFactor + 500000.0;
            xy[1] = xy[1] * UTMScaleFactor;
            if (xy[1] < 0.0)
                xy[1] += 10000000.0;

            return xy;
        }

        //WEB墨卡托
        public const double R = 20037508.3437892;
        //经纬度转WEB墨卡托
        public double[] SphereToPlane(double JD, double WD)
        {
            double[] outXY = new double[2];
            outXY[0] = JD * R / 180.0;
            outXY[1] = Math.Log(Math.Tan((90.0 + WD) * Math.PI / 360.0)) / (Math.PI / 180.0);
            outXY[1] = outXY[1] * R / 180.0;

            return outXY;
        }

        public double[] UTMXYToRadius(double x, double y, int zone, bool southhemi)
        {
            double cmeridian;

            x -= 500000.0;
            x /= UTMScaleFactor;

            /* If in southern hemisphere, adjust y accordingly. */
            if (southhemi)
                y -= 10000000.0;

            y /= UTMScaleFactor;

            cmeridian = UTMCentralMeridian(zone);
            double[] xy = MapXYToLatLon(x, y, cmeridian);

            return xy;
        }

        public double[] UTMXYToToLonLat(double x, double y, int zone, bool southhemi)
        {
            double cmeridian;
            x -= 500000.0;
            x /= UTMScaleFactor;

            /* If in southern hemisphere, adjust y accordingly. */
            if (southhemi)
                y -= 10000000.0;

            y /= UTMScaleFactor;

            cmeridian = UTMCentralMeridian(zone);
            double[] xy = MapXYToLatLon(x, y, cmeridian);

            //度数/360=弧度/2π
            double[] lonlat = new double[2];
            double lon = xy[0] * 180 / PI;
            double lat = xy[1] * 180 / PI;
            lonlat[0] = lon;
            lonlat[1] = lat;
            return lonlat;
        }

    
        /// <summary>
        /// 计算两点位置的距离，返回两点的距离，单位 米
        /// 该公式为GOOGLE提供，误差小于0.2米
        /// </summary>
        /// <param name="lat1">第一点纬度</param>
        /// <param name="lng1">第一点经度</param>
        /// <param name="lat2">第二点纬度</param>
        /// <param name="lng2">第二点经度</param>
        /// <returns></returns>
        public  double GetDistance(double lat1, double lng1, double lat2, double lng2)
        {
            double radLat1 = Rad(lat1);
            double radLng1 = Rad(lng1);
            double radLat2 = Rad(lat2);
            double radLng2 = Rad(lng2);
            double a = radLat1 - radLat2;
            double b = radLng1 - radLng2;
            double result = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) + Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2))) * sm_a;
            return result;
        }

        /// <summary>
        /// 经纬度转化成弧度
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        private  double Rad(double d)
        {
            return (double)d * Math.PI / 180d;
        }

    }


}

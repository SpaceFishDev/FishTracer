using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace FishTracer
{
    struct Vector3
    {
        [DllImport("msvcrt.dll")]
        public static extern double sqrt(double x);

        public double[] e = new double[3];
        public double X { get => e[0]; set { e[0] = value; _lengthSqr = double.NaN; } }
        public double Y { get => e[1]; set { e[1] = value; _lengthSqr = double.NaN; } }
        public double Z { get => e[2]; set { e[2] = value; _lengthSqr = double.NaN; } }
        public static Vector3 RandomUnitInSphere()
        {
            Vector3 v3 = new Vector3(Vector3.RandomDouble(-1, 1), Vector3.RandomDouble(-1, 1), Vector3.RandomDouble(-1, 1));
            return v3.Normalized;
        }
        public override string ToString()
        {
            return $"X = {X}, Y = {Y}, Z = {Z}";
        }
        public Vector3(double x, double y, double z)
        {
            e = new double[3]{x,y,z};
        }
        public Vector3(double[] e)
        {
            if (e.Length != 3)
            {
                throw new Exception("Vector3 only has 3 elements bro, whattt whyayyyayy!");
            }
            Array.Copy(e, this.e, 3);
        }
        public static double Clamp(double x, double min, double max)
        {
            if (x < min) return min;
            if (x > max) return max;
            return x;
        }
        public static Vector3 RandomInHemisphere(Vector3 normal)
        {
            Vector3 USphere = Vector3.RandomUnitInSphere();
            if (Vector3.dot(USphere, normal) > 0.001)
            {
                return USphere;
            }
            return -USphere;
        }
        public static double FastSqrt(double x)
        {
            const double threehalfs = 1.5;
            double y = x;
            long i = BitConverter.DoubleToInt64Bits(y);
            i = i >> 40;
            i = i << 40;
            i = 0x5FE6EB50C7B537A9 - (i >> 1);
            y = BitConverter.Int64BitsToDouble(i);
            y = y * (threehalfs - (x * 0.5 * y * y));
            y = y * (threehalfs - (x * 0.5 * y * y));
            return 1 / y;
        }
        public static double InvSqrt(double x)
        {
            const double threehalfs = 1.5;
            double y = x;
            long i = BitConverter.DoubleToInt64Bits(y);
            i = 0x5FE6EB50C7B537A9 - (i >> 1);
            y = BitConverter.Int64BitsToDouble(i);
            y = y * (threehalfs - (x * 0.5 * y * y));
            y = y * (threehalfs - (x * 0.5 * y * y));
            return y;
        }
        private double _length = double.NaN;
        public double Length
        {
            get
            {
                if (double.IsNaN(_length))
                {
                    _length = Vector3.FastSqrt(LengthSqr);
                }
                return _length;
            }
        }
        private double _lengthSqr = double.NaN;
        public double LengthSqr
        {
            get
            {
                if (double.IsNaN(_lengthSqr))
                {
                    _lengthSqr = e[0] * e[0] + e[1] * e[1] + e[2] * e[2];
                }
                return _lengthSqr;
            }
        }
        public static Vector3 operator -(Vector3 v) => new Vector3(-v.X, -v.Y, -v.Z);
        public static Vector3 operator +(Vector3 u, Vector3 v) => new Vector3(u.X + v.X, u.Y + v.Y, u.Z + v.Z);
        public static Vector3 operator -(Vector3 u, Vector3 v) => new Vector3(u.X - v.X, u.Y - v.Y, u.Z - v.Z);
        public static Vector3 operator *(Vector3 u, Vector3 v) => new Vector3(u.X * v.X, u.Y * v.Y, u.Z * v.Z);
        public static Vector3 operator /(Vector3 u, Vector3 v) => new Vector3(u.X / v.X, u.Y / v.Y, u.Z / v.Z);
        public static Vector3 operator +(Vector3 u, double v) => new Vector3(u.X + v, u.Y + v, u.Z + v);
        public static Vector3 operator *(Vector3 u, double v) => new Vector3(u.X * v, u.Y * v, u.Z * v);
        public static Vector3 operator *(double u, Vector3 v) => new Vector3(u * v.X, u * v.Y, u * v.Z);
        public static Vector3 operator /(double u, Vector3 v) => new Vector3(u / v.X, u / v.Y, u / v.Z);
        public static Vector3 operator -(double u, Vector3 v) => new Vector3(u - v.X, u - v.Y, u - v.Z);
        public static Vector3 operator +(double u, Vector3 v) => new Vector3(u + v.X, u + v.Y, u + v.Z);
        public static Vector3 operator /(Vector3 u, double v) => new Vector3(u.X / v, u.Y / v, u.Z / v);
        public static Vector3 operator -(Vector3 u, double v) => new Vector3(u.X - v, u.Y - v, u.Z - v);
        public double this[int index]
        {
            get => e[index];
            set => e[index] = value;
        }
        public static Random rnd = new Random();
        public static int RndNum = 0;
        public static List<double> Randoms;
        public static double RandomDouble(double min = 0, double max = 1)
        {
            if(Randoms == null)
            {
                Randoms = new List<double>(capacity: 10000000);
                for(int i = 0; i != 10000000; ++i)
                {
                    Randoms.Add(rnd.NextDouble());
                }
                return Randoms[RndNum++];
            }
            if(RndNum < Randoms.Count)
            {
                return Randoms[RndNum++];
            }
            double d = rnd.NextDouble();
            ++RndNum;
            return d * (max - min) + min;
        }
        public static Vector3 Random(double min = 0, double max = 1)
        {
            return new Vector3(RandomDouble(min, max), RandomDouble(min, max), RandomDouble(min, max));
        }
        public static Vector3 Reflect(Vector3 v, Vector3 n)
        {
            return v - 2 * dot(v, n) * n;
        }
        public bool NearZero
        {
            get
            {
                double s = 1e-8;
                bool n = false;
                foreach(double val in e)
                {
                    n |= Math.Abs(val) < s;
                }
                return n;
            }
        }
        public int[] Color()
        {
            int[] r = new int[3] {(int)(e[0] * 255), (int)(e[1] * 255), (int)(e[2] * 255)};
            return r;
        }
        public static double dot(Vector3 v, Vector3 u) => u[0] * v[0] + u[1] * v[1] + u[2] * v[2];
        public static double cross(Vector3 v, Vector3 u) => u[0] * v[0] - u[1] * v[1] - u[2] * v[2];
        public Vector3 Normalized => this / Length;
    }

}
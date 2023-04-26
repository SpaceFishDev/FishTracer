using FishTracer.RayObjects;
using System;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.InteropServices;

namespace FishTracer
{
    struct Vector3
    {

        public double[] e = new double[3];
        public Vector3 Copy()
        {
            return new Vector3(X,Y,Z);
        }
        public double X { get => (e == null) ? 0 : e[0]; set { if (e == null) { e = new double[3]{0, 0, 0}; } e[0] = value; _lengthSqr = double.NaN; } }
        public double Y { get => (e == null) ? 0 : e[1]; set { e[1] = value; _lengthSqr = double.NaN; } }
        public double Z { get => (e == null) ? 0 : e[2]; set { e[2] = value; _lengthSqr = double.NaN; } }
        public static bool operator==(Vector3 a, Vector3 b) => a[0] == b[0] && a[1] == b[1] && a[2] == b[2];
        public static bool operator !=(Vector3 a, Vector3 b) => !(a == b);
        public static Vector3 RandomUnitInSphere()
        {
            Vector3 v3 = new Vector3(Vector3.RandomDouble(-1, 1), Vector3.RandomDouble(-1, 1), Vector3.RandomDouble(-1, 1));
            return v3;
        }
        public override string ToString()
        {
            return $"X = {X}, Y = {Y}, Z = {Z}";
        }

        public Vector3()
        {
            e = new double[3] { 0,0,0};
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
        public Vector3 Transform(System.Numerics.Matrix4x4 matrix)
        {
            double x = X * matrix.M11 + Y * matrix.M21 + Z * matrix.M31 + matrix.M41;
            double y = X * matrix.M12 + Y * matrix.M22 + Z * matrix.M32 + matrix.M42;
            double z = X * matrix.M13 + Y * matrix.M23 + Z * matrix.M33 + matrix.M43;
            return new Vector3(x, y, z);
        }
        public Vector3 TransformNormal(Matrix4x4 matrix)
        {
            return Transform(matrix).Normalized;
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
            get
            {
                if (e == null) 
                {
                    e = new double[3] { 0, 0, 0 }; 
                } 
                return e[index]; 
            }
            set => e[index] = value;
        }

        public static Random rnd = new Random();
        public static double RandomDouble(double min = 0, double max = 1)
        {
            return rnd.NextDouble() * (max - min) + min;
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
                    n |= Cube.FastAbs(val) < s;
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
        public static Vector3 cross(Vector3 v, Vector3 u)
        {
            Vector3 n = new Vector3();
            n.X = v.Y * u.Z - v.Z * u.Y;
            n.Y = v.Z * u.X - v.X * u.Z;
            n.Z = v.Y * u.Y - v.Y * u.X;
            return n;
        }
        public Vector3 Normalized => this / Length;
        public static Vector3 Blend( Vector3 color, Vector3 backColor, double amount)
        {
            double r = (color.X * amount + backColor.X * (1 - amount));
            double g = (color.Y * amount + backColor.Y * (1 - amount));
            double b = (color.Z * amount + backColor.Z * (1 - amount));
            color.e[0] = r; //(r , g , b);
            color.e[1] = g; //(r , g , b);
            color.e[2] = b; //(r , g , b);
            return color;
        }
        public static Vector3 Refract(Vector3 uv, Vector3 n, double etai_over_etat)
        {
            uv = uv.Normalized;
            double cos_theta = Math.Max(Vector3.dot(-uv, n), 0.0);
            Vector3 r_out_perp = etai_over_etat * (uv + cos_theta * n);
            Vector3 r_out_parallel = -Vector3.FastSqrt(Math.Max(0.0, 1.0 - r_out_perp.Length * r_out_perp.Length)) * n;
            return r_out_perp + r_out_parallel;
        }
    }
}
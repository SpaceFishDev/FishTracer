using System;

namespace FishTracer.RayObjects
{
    class Cube : RayObject
    {
        public Vector3 Center;
        public Vector3 MinPoint;
        public Vector3 MaxPoint;
        public Material Material;

        public Cube(Vector3 center, double size, Material material)
        {
            this.Center = center;
            this.MinPoint = center - (size / 2);
            this.MaxPoint = center + (size / 2);
            Console.WriteLine(MinPoint.ToString() + " " + MaxPoint.ToString());
            this.Material = material;
        }

        public override bool RayCollision(ref Ray ray, double minT, double maxT, out HitRecord record)
        {
            record = new HitRecord();
            Vector3 inverseDirection = 1.0 / ray.Direction;
            Vector3 tMin = (MinPoint - ray.Position) * inverseDirection;
            Vector3 tMax = (MaxPoint - ray.Position) * inverseDirection;

            if (tMin.X > tMax.X)
            {
                double temp = tMin.X;
                tMin.X = tMax.X;
                tMax.X = temp;
            }
            if (tMin.Y > tMax.Y)
            {
                double temp = tMin.Y;
                tMin.Y = tMax.Y;
                tMax.Y = temp;
            }
            if (tMin.Z > tMax.Z)
            {
                double temp = tMin.Z;
                tMin.Z = tMax.Z;
                tMax.Z = temp;
            }

            double tMinMax = Math.Min(Math.Min(tMax.X, tMax.Y), tMax.Z);
            double tMaxMin = Math.Max(Math.Max(tMin.X, tMin.Y), tMin.Z);

            if (tMinMax < tMaxMin)
            {
                return false;
            }

            double t = tMaxMin;

            if (t > maxT || t < minT)
            {
                return false;
            }

            Vector3 p = ray.At(t);
            Vector3 norm = CalcNormal(p, ray.Direction.Normalized);
            record.t = t;
            record.p = p;
            record.normal = norm;
            record.FrontFace = true;
            record.material = Material;
            return true;
        }
        Vector3 Result = new Vector3(-1, 0, 0);
        Vector3 Result1 = new Vector3(0, 0, 1);
        public static double FastAbs(double x)
        {
            unsafe
            {
                unchecked
                {
                    long* x_long = ((long*)(&x));
                    *x_long = (*x_long) & 0x7FFFFFFFFFFFFFFF;
                }
            }
            return x;
        }
        private Vector3 CalcNormal(Vector3 p, Vector3 rayDirection)
        {
            double distX = FastAbs(p.X - (MinPoint.X + MaxPoint.X) / 2);
            double distY = FastAbs(p.Y - (MinPoint.Y + MaxPoint.Y) / 2);
            double distZ = FastAbs(p.Z - (MinPoint.Z + MaxPoint.Z) / 2);
            
            Vector3 normal = (distX >= distY && distX >= distZ) ? 
                ((p.X < Center.X) ? Result : 
                new Vector3(1, 0, 0)) : (distY >= distX && distY >= distZ) ? 
                new Vector3(0, (p.Y < Center.Y) ? -1 : 1, 0) : (p.Z < Center.Z) ? 
                new Vector3(0, 0, -1) : Result1;
            return Vector3.dot(normal, rayDirection) < 0 ? normal : -normal;
        }
    }
}

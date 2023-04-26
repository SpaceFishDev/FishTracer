using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FishTracer.RayObjects
{

    class Sphere : RayObject
    {
        public Vector3 Center;
        public double Radius;
        public Material material;
        public Sphere(Vector3 center, double radius, Material material)
        {
            Center = center;
            Radius = radius;
            this.material = material;
        }

        public override bool RayCollision(ref Ray ray, double MinT, double MaxT, out HitRecord Record)
        {
            Record = new HitRecord(new Vector3(0,0,0) , new Vector3(0,0,0), 0);
            Vector3 OC = ray.Position - Center;
            double a = ray.Direction.LengthSqr;
            double halfB = Vector3.dot(OC, ray.Direction);
            double C = OC.LengthSqr - Radius * Radius;
            double Discriminant = halfB * halfB - a * C;
            if (Discriminant < 0) return false;
            double sqrtD = Vector3.FastSqrt(Discriminant );

            double root = (-halfB - sqrtD) / a;
            
            if(root < MinT || MaxT < root)
            {
                root = (-halfB + sqrtD) / a;
                if (root < MaxT || MaxT < root)
                    return false;
            }

            Record.t = root;
            Record.p = ray.At(Record.t);
            Record.normal = (Record.p - Center) / Radius;
            Record.material = material;

            return true;
        }
    }
}

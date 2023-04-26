using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishTracer.RayObjects
{
    struct HitRecord
    {
        public Vector3 p;
        public Vector3 normal;
        public double t;
        public bool FrontFace;
        public Material material;
        public HitRecord()
        {
            t = 0;
            p = new Vector3(0,0,0);
            normal = new Vector3(0,0,0);
        }
        public HitRecord(Vector3 p, Vector3 normal, double t)
        {
            this.p = p;
            this.normal = normal;
            this.t = t;
        }
        public void SetFaceNormal(ref Ray ray, Vector3 outwardNormal)
        {
            FrontFace = Vector3.dot(ray.Direction, outwardNormal) < 0;
            normal = FrontFace ? outwardNormal : -outwardNormal; 
        }
    }
    abstract class RayObject
    {
        public abstract bool RayCollision
        (
            ref Ray Ray, 
            double MinT, 
            double MaxT,
            out HitRecord Record
        );
    }
}

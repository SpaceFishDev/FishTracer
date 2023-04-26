using FishTracer.RayObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace FishTracer.Objects
{
    class Triangle : RayObject
    {
        public Vector3 Vertex1;
        public Vector3 Vertex2;
        public Vector3 Vertex3;
        public Material material;

        public Triangle(Vector3 vertex1, Vector3 vertex2, Vector3 vertex3, Vector3 Position, Material material)
        {
            Vertex1 = vertex1 + Position;
            Vertex2 = vertex2 + Position;
            Vertex3 = vertex3 + Position;
            this.material = material;
        }
        public override bool RayCollision(ref Ray ray, double MinT, double MaxT, out HitRecord Record)
        {
            Record = new HitRecord();
            Vector3 edge1 = Vertex2 - Vertex1;
            Vector3 edge2 = Vertex3 - Vertex1;
            Vector3 h = Vector3.cross(ray.Direction, edge2);
            double a = Vector3.dot(edge1, h);
            if (MathF.Abs((float)a) < float.Epsilon)
            {
                return false; // ray is parallel to triangle 
            }
            double f = 1.0 / a;
            Vector3 s = ray.Position - Vertex1;
            double u = f * Vector3.dot(s, h);
            if (u < 0.0 || u > 1.0)
            {
                return false; // ray is outside of triangle
            }

            Vector3 q = Vector3.cross(s, edge1);
            double v = f * Vector3.dot(ray.Direction, q);
            if (v < 0.0 || u + v > 1.0)
            {
                return false;
            }

            double t = f * Vector3.dot(edge2, q);

            if (t < 0.0)
            {
                return false;
            }

            Record.t = t;
            Record.p = ray.At(t);
            Record.normal = Vector3.cross(edge1, edge2).Normalized;
            Record.material = material;
            Record.FrontFace = false;
            Record.SetFaceNormal(ref ray, Record.normal);
            return true;
        }
    }
}

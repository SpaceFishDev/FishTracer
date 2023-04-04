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

        public Triangle(Vector3 vertex1, Vector3 vertex2, Vector3 vertex3, Material material)
        {
            Vertex1 = vertex1;
            Vertex2 = vertex2;
            Vertex3 = vertex3;
            this.material = material;
        }

        public override bool RayCollision(ref Ray ray, double MinT, double MaxT, out HitRecord Record)
        {
            Vector3 edge1 = Vertex2 - Vertex1;
            Vector3 edge2 = Vertex3 - Vertex1;
            Vector3 normal = Vector3.cross(edge1, edge2);
            double rayDirDotNormal = Vector3.dot(ray.Direction, normal);
            Record = new HitRecord();
            if(Math.Abs(rayDirDotNormal) < double.Epsilon)
            {
                return false;
            }
            var d = Vector3.dot(Vertex1 - ray.Position, normal) / rayDirDotNormal;

            if(d < 0)
            {
                return false;
            }

            
            Vector3 edge3 = Vertex1 - Vertex3;

            Vector3 v1 = Record.p - Vertex1;
            Vector3 v2 = Record.p - Vertex2;
            Vector3 v3 = Record.p - Vertex3;

            Vector3 cross1 = Vector3.cross(edge1, v1);
            Vector3 cross2 = Vector3.cross(edge2, v2);
            Vector3 cross3 = Vector3.cross(edge3, v3);

            if(Vector3.dot(cross1, normal) < 0 || Vector3.dot(cross2, normal) < 0 || Vector3.dot(cross3, normal) < 0)
            {
                return false;
            }
            
            Record.t = d;
            Record.p = ray.At(d);
            Record.SetFaceNormal(ref ray, normal);
            Record.material = material;

            return true;
        }
    }
}

namespace FishTracer
{
    using RayObjects;
    using System.Drawing;
    using System.Runtime.InteropServices;

    class Metal : Material
    {
        public Vector3 Albedo;
        public double Fuzz;
        public Metal(Vector3 albedo, double f = 0)
        {
            this.Albedo = albedo;
            this.Fuzz = f / 5;
        }

        public override bool Scatter(Ray ray, HitRecord record, out Vector3 attenuation, out Ray Scattered)
        {
            Vector3 reflected = Vector3.Reflect(ray.Direction.Normalized, record.normal);
            Scattered = new Ray(record.p, reflected - Fuzz * Vector3.RandomUnitInSphere());
            attenuation = Albedo;
            return (Vector3.dot(Scattered.Direction, record.normal) > 0);   
        }
    }
}
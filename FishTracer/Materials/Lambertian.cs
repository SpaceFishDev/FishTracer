namespace FishTracer
{
    using RayObjects;

    class Lambertian : Material
    {
        public Vector3 Albedo;
        public Lambertian(Vector3 albedo)
        {
            Albedo = albedo;
        }
        public override bool Scatter(Ray ray, HitRecord record, out Vector3 attenuation, out Ray Scattered)
        {
            Vector3 scatteredDirection = record.normal + Vector3.Random(0, 1);
            if (scatteredDirection.NearZero)
            {
                scatteredDirection = record.normal;
            }
            Scattered = new Ray(record.p, scatteredDirection);
            attenuation = Albedo;
            return true;
        }
    }
}
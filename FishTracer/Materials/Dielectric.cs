namespace FishTracer
{
    using RayObjects;

    class Dielectric : Material
    {
        public double Ir { get; set; } // Index of Refraction

        public Dielectric(double index_of_refraction)
        {
            Ir = index_of_refraction;
        }

        public override bool Scatter(Ray ray, HitRecord record, out Vector3 attenuation, out Ray Scattered)
        {
            attenuation = new Vector3(1.0, 1.0, 1.0);
            double refraction_ratio = record.FrontFace ? (1.0 / Ir) : Ir;

            Vector3 unit_direction = ray.Direction.Normalized;
            double cos_theta = Math.Min(Vector3.dot(-unit_direction, record.normal), 1.0);
            double sin_theta = Vector3.FastSqrt(1.0 - cos_theta * cos_theta);

            bool cannot_refract = refraction_ratio * sin_theta > 1.0;
            Vector3 direction;
            if (cannot_refract || Reflectance(cos_theta, refraction_ratio) > Vector3.RandomDouble())
            {
                direction = Vector3.Reflect(unit_direction, record.normal);
            }
            else
            {
                direction = Vector3.Refract(unit_direction, record.normal, refraction_ratio);
            }
            Scattered = new Ray(record.p, direction);
            return true;
        }

        private static double Reflectance(double cosine, double ref_idx)
        {
            // Use Schlick's approximation for reflectance.
            double r0 = (1 - ref_idx) / (1 + ref_idx);
            r0 = r0 * r0;
            return r0 + (1 - r0) * Math.Pow((1 - cosine), 5);
        }
    }
}
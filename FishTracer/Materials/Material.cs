namespace FishTracer
{
    using RayObjects;

    abstract class Material
    {
        public abstract bool Scatter(Ray ray, HitRecord record, out Vector3 attenuation, out Ray Scattered);
    }
}
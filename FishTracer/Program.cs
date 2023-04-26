namespace FishTracer
{
    using FishTracer.Objects;
    using RayObjects;
    using System.Reflection.Emit;

    class PointLight
    {
        public Vector3 Position;
        public Vector3 Color;
        public double Intensity;

        public PointLight(Vector3 position, Vector3 color, double intensity)
        {
            Position = position;
            Color = color;
            Intensity = intensity;
        }
        public Vector3 CalculateColor(HitRecord record, RayObject world)
        {
            Vector3 lightDir = Position - record.p;
            double distance = lightDir.Length;
            lightDir = lightDir.Normalized;
            Ray r = new Ray(record.p, lightDir);
            if (world.RayCollision(ref r, 0.001, distance, out var rec))
            {
                lightDir[0] = 0;
                lightDir[1] = 0;
                lightDir[2] = 0;
                return lightDir;
            }
            double attenuation = 1 / (rec.t * rec.t);
            return Color * Intensity * attenuation;
        }
    }

    class Program
    {

        public static void Main()
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            RayObjectList World = new RayObjectList();
            var materialGround = new Lambertian(new Vector3(1, 0.1, 0.1));
            var materialCenter = new Lambertian(new Vector3(0.5, 0.0, 0.6));
            var materialLeft = new Metal (new Vector3(0.8,0.8,0.8), 0.1);
            var materialRight = new Metal(new Vector3(0.8, 0.7, 0.8), 0.0);
            Dielectric glass = new Dielectric(1.5);

            RayCaster rayCaster = new RayCaster("RC", 800, 5, 16.0 / 9.0, 1);
            //World.Add(new Cube(new Vector3(0, 0, -1), 0.5, glass));
            World.Add(new Cube(new Vector3(0, 0.5, -1), 0.5, materialCenter));
            World.Add(new Sphere(new Vector3(-1, 0.5, -1), 0.5, materialLeft));
            World.Add(new Sphere(new Vector3(1, 0.5, -1), 0.5, materialRight));
            World.Add(new Cube(new Vector3(0,-30, -1.0), 60, materialGround));
            World.Add(new Cube(new Vector3(-30, 0.5, -1.0), 30, materialGround));
            World.Add(new Cube(new Vector3(30, 0.5, -1.0), 30, materialGround));
            World.Add(new Cube(new Vector3(0, 30, -1.0), 30, materialGround));
            World.Add(new Cube(new Vector3(0, 0, -30), 30, materialGround));
            sw.Restart();
            //int q_ = 0;
            //for (int q = 32 + 1; q > 0; q -= 2)
            //{
            //    Parallel.For(0, rayCaster.window.Width / q, i =>
            //    {
            //        int actualI = i * q;
            //        for (int j = 0; j < rayCaster.window.Height; j += q)
            //        {
            //            int actualJ = rayCaster.window.Height - j - 1;
            //            Vector3 color = new Vector3();
            //            color = rayCaster.Cast(actualI, actualJ, World);
            //            rayCaster.window.PutSquare(actualI, actualJ, actualI + q, actualJ + q, color.Color());
            //        }
            //    });
            //    rayCaster.window.Title = $"Rendering... {(q_ / 32.0) * 100}%";
            //    rayCaster.window.Update(true);
            //    q_ += 2;
            //}
            int total = 0;
            for (int j = rayCaster.window.Height; j > 0; --j)
            {
                Parallel.For(0, rayCaster.window.Width, i =>
                {
                    Vector3 color = new Vector3();
                    color = rayCaster.Cast(i, j, World, lights: new List<PointLight>(1) {new PointLight(new Vector3(0,2,-2), new Vector3(1,1,0), 30) });
                    rayCaster.window.PutPixel(i, j, color.Color());
                });
                rayCaster.window.Update(true);
            }
            sw.Stop();
            rayCaster.window.Title = $"Rendering done!";
            
            double TimeTaken = sw.ElapsedMilliseconds;
            Console.WriteLine($"TimeTaken = {TimeTaken}ms");
            rayCaster.window.Update(true);
            while (!rayCaster.window.Quit)
            {
                rayCaster.window.Update(true);
            }

        }
    }
}
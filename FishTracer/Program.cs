namespace FishTracer
{
    using RayObjects;

    class Program
    {

        public static void Main()
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            RayObjectList World = new RayObjectList();
            Lambertian materialGround = new Lambertian(new Vector3(0.8,0.8,0.0));
            Lambertian materialCenter = new Lambertian(new Vector3(0.5, 0.0, 0.6));
            Metal materialLeft = new Metal(new Vector3(0.8,0.8,0.8), 0.3);
            Metal materialRight = new Metal(new Vector3(0.8, 0.6, 0.2), 1.0);

            World.Add(new Sphere(new Vector3(0,-1000.5, -1.0), 1000, materialGround));
            World.Add(new Sphere(new Vector3(0,0, -1), 0.5, materialCenter));
            World.Add(new Sphere(new Vector3(-1,0, -1), 0.5, materialLeft));
            World.Add(new Sphere(new Vector3(1, 0, -1), 0.5, materialRight));

            RayCaster rayCaster = new RayCaster("RC", 400, 4, 16.0/9.0, 1);
            sw.Restart();
            for (int j = rayCaster.window.Height; j > 0; --j)
            {
                Parallel.For(0, rayCaster.window.Width, i =>
                {
                    Vector3 color = new Vector3();
                    color = rayCaster.Cast(i, j, World);
                    rayCaster.window.PutPixel(i, j, color.Color());
                });
                rayCaster.window.Update(true);
            }

            sw.Stop();
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
namespace FishTracer
{
    using RayObjects;


    class RayCaster
    {
        public Window window;
        public double ViewPortWidth;
        public double ViewPortHeight;
        public double AspectRatio;
        public Vector3 LowerLeft;
        public Vector3 origin;
        public double FocalLength;
        public double SamplesPerPixel = 128;
        Vector3 Vertical;
        Vector3 Horizontal;
        Ray usableRay = new Ray(new Vector3(), new Vector3());
        public RayCaster(string Title, double Width, double viewPortWidth, double aspectRatio, double FocalLength)
        {
            AspectRatio = aspectRatio;
            this.FocalLength = FocalLength;
            this.window = new Window((int)Width, (int)(Width / AspectRatio), Title);
            ViewPortWidth = viewPortWidth;
            ViewPortHeight = viewPortWidth / AspectRatio;
            origin = new Vector3(0, 2, 1);
            Vertical = new Vector3(0, ViewPortHeight, 0);
            Horizontal = new Vector3(ViewPortWidth, 0, 0);
            LowerLeft = origin - Horizontal / 2 - Vertical / 2 - new Vector3(0, 0, FocalLength);
        }
        public Vector3 Cast(double x, double y, RayObject World, List<PointLight>? lights = null)
        {
            Vector3 pix = new Vector3(0, 0, 0);
            for (int j = 0; j != SamplesPerPixel; ++j)
            {
                double u = (x + Vector3.RandomDouble(0, 1)) / (double)window.Width;
                double v = (y + Vector3.RandomDouble(0, 1)) / (double)window.Height;
                GetRay(u, v);
                pix += CastRay(usableRay, World, lights: lights);
            }
            pix.X = Vector3.FastSqrt(pix.X / SamplesPerPixel);
            pix.Y = Vector3.FastSqrt(pix.Y / SamplesPerPixel);
            pix.Z = Vector3.FastSqrt(pix.Z / SamplesPerPixel);
            return pix;
        }
        public Vector3 CastRay(Ray r, RayObject World, double depth = 128, List<PointLight>? lights = null)
        {
            if(lights == null)
            {
                lights = new List<PointLight>();
            }
            if (depth < 0)
            {
                return new Vector3(0, 0, 1);
            }
            HitRecord rec;
            Vector3 color = new Vector3(0,0,0);

            if (World.RayCollision(ref r, 0.0001, double.PositiveInfinity, out rec))
            {
                foreach (PointLight light in lights)
                {
                    Vector3 lightColor = light.CalculateColor(rec, World);

                    Vector3 toLight = light.Position - rec.p;
                    double distance = toLight.Length;
                    toLight = toLight.Normalized;

                    double cosTheta = Vector3.dot(rec.normal, toLight);
                    if (cosTheta > 0)
                    {
                        // Calculate the diffuse color
                        if (rec.material is Lambertian)
                            color += ((Lambertian)(rec.material)).Albedo * lightColor * cosTheta;
                        else
                            color += ((Metal)(rec.material)).Albedo * lightColor * cosTheta;


                        // Calculate the specular color
                        Vector3 reflection = Vector3.Reflect(-toLight, rec.normal);
                        double cosAlpha = Vector3.dot(r.Direction, reflection);
                        if (cosAlpha > 0)
                        {
                            if (rec.material is Lambertian)
                                color += ((Lambertian)(rec.material)).Albedo * lightColor * Math.Pow(cosAlpha, 1);
                            else
                                color += ((Metal)(rec.material)).Albedo * lightColor * Math.Pow(cosAlpha, 1);

                        }
                    }
                }
                Ray scattered;

                if (rec.material.Scatter(r, rec, out color, out scattered))
                {
                    return color * CastRay(scattered, World, depth - 1, lights);
                }
                return color;
            }
            else
            {
                Vector3 UnitDir = r.Direction.Normalized;
                double t = 0.5 * (UnitDir.Y + 1.0);
                color = (1.0 - t) * new Vector3(1, 1, 1) + t * new Vector3(0.5, 0.7, 1);
            }
            return color;

        }
        public void GetRay(double u, double v)
        {
            usableRay.Position = origin;
            usableRay.Direction = LowerLeft + u * Horizontal + v * Vertical - origin;
        }
    }
}
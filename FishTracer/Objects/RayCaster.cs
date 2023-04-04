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
        public double SamplesPerPixel = 100;
        Vector3 Vertical;
        Vector3 Horizontal;
        public RayCaster(string Title, double Width, double viewPortWidth, double aspectRatio, double FocalLength)
        {
            AspectRatio = aspectRatio;
            this.FocalLength = FocalLength;
            this.window = new Window((int)Width, (int)(Width / AspectRatio), Title);
            ViewPortWidth = viewPortWidth;
            ViewPortHeight = viewPortWidth / AspectRatio;
            origin = new Vector3(0,0,0);
            Vertical = new Vector3(0, ViewPortHeight, 0);
            Horizontal = new Vector3(ViewPortWidth, 0, 0);
            LowerLeft = origin - Horizontal / 2 - Vertical / 2 - new Vector3(0, 0, FocalLength);
        }
        public Vector3 Cast(double x, double y, RayObject World)
        {
            Vector3 pix = new Vector3(0,0,0);
            for (int j = 0; j != SamplesPerPixel; ++j)
            {
                double u = (x + Vector3.RandomDouble(0, 1)) / (double)window.Width;
                double v = (y + Vector3.RandomDouble(0, 1)) / (double)window.Height;
                Ray r = GetRay(u, v);
                pix += CastRay(r, World);
            }
            pix.X = Vector3.FastSqrt(pix.X  / SamplesPerPixel);
            pix.Y = Vector3.FastSqrt(pix.Y  / SamplesPerPixel);
            pix.Z = Vector3.FastSqrt(pix.Z  / SamplesPerPixel);
            return pix;
        }
        public Vector3 CastRay(Ray r, RayObject World, double depth = 40)
        {
            if(depth < 0)
            {
                return new Vector3(0,0,0);
            }
            HitRecord rec;
            Vector3 color;
            if (World.RayCollision(ref r, 0, double.PositiveInfinity, out rec))
            {
                Ray scattered;
                if(rec.material.Scatter(r, rec, out color, out scattered))
                {
                    return color * CastRay(scattered, World, depth-1);
                }
                color = new Vector3(0,0,0);
            }
            else
            {
                Vector3 UnitDir = r.Direction.Normalized;
                double t = 0.5 * (UnitDir.Y + 1.0);
                color = (1.0 - t) * new Vector3(1, 1, 1) + t * new Vector3(0.5, 0.7, 1);
            }
            return color;

        }
        public Ray GetRay(double u, double v)
        {
            return new Ray(origin, LowerLeft + u*Horizontal + v*Vertical - origin);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishTracer
{
    struct Ray
    {
        public Vector3 Position;
        public Vector3 Direction;

        public Ray(Vector3 position, Vector3 direction)
        {
            Position = position;
            Direction = direction;
        }

        public Vector3 At(double t)
        {
            return Position + Direction * t;
        }
    }
}

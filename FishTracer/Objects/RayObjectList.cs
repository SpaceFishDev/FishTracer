using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishTracer.RayObjects
{
    internal class RayObjectList : RayObject
    {
        public List<RayObject> rayObjects;
        public RayObjectList()
        {
            rayObjects = new List<RayObject>();
        }
        public RayObjectList(RayObject obj)
        {
            rayObjects = new List<RayObject>();
            rayObjects.Add(obj);
        }
        public void Add(RayObject obj)
        {
            rayObjects.Add(obj);
        }
        public static RayObjectList operator+(RayObjectList a,  RayObject b)
        {
            a.Add(b);
            return a;
        }
        public List<RayObject> GetObjects()
        {
            return rayObjects;
        }
        public override bool RayCollision(ref Ray Ray, double MinT, double MaxT, out HitRecord Record)
        {
            Record = new HitRecord();
            HitRecord temp;
            bool HitAnything = false;
            double closest = MaxT;

            foreach(var obj in rayObjects)      
            {
                if (obj.RayCollision(ref Ray, MinT, closest, out temp))
                {
                    HitAnything = true;
                    closest = temp.t;
                    Record = temp;
                }   
            }

            return HitAnything;
        }
    }
}

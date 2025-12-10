using Raylib_cs;
using ASTEROIDS;

namespace ASTEROIDS
{
    public class CollisionComponent
    {
        public float Radius;                     // törmäyssäde
        public TransformComponent Transform;     // omistavan objektin transform

        public CollisionComponent(float radius, TransformComponent transform)
        {
            Radius = radius;
            Transform = transform;
        }
    }
}

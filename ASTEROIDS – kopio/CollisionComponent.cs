using System.Numerics;
using Raylib_cs;

namespace ASTEROIDS
{
    public class CollisionComponent : Component
    {
        public float Radius { get; private set; }
        private TransformComponent transform;

        public CollisionComponent(float radius, TransformComponent transformRef)
        {
            Radius = radius;
            transform = transformRef;
        }

        public bool CheckCollision(CollisionComponent other)
        {
            return Raylib.CheckCollisionCircles(transform.position, Radius, other.transform.position, other.Radius);
        }
    }
}


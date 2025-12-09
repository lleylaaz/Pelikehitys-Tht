using System.Numerics;
using Raylib_cs;

namespace ASTEROIDS
{
    public class CollisionComponent
    {
        public float radius;
        public TransformComponent transform;

        public CollisionComponent(float radius, TransformComponent transform)
        {
            this.radius = radius;
            this.transform = transform;
        }

        public bool CheckCollision(CollisionComponent other)
        {
            return Raylib.CheckCollisionCircles(
                transform.position, radius,
                other.transform.position, other.radius
            );
        }
    }
}

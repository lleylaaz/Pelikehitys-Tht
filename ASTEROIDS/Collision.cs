using System;
using System.Numerics;

namespace ASTEROIDS
{
    internal class Collision
    {
        private float collisionRadius;
        public Vector2 Position { get; set; }

        public Collision(Vector2 position, float collisionRadius)
        {
            this.Position = position;
            this.collisionRadius = collisionRadius;
        }

        public bool CheckCollision(Collision other)
        {
            float distance = Vector2.Distance(this.Position, other.Position);
            return distance < (this.collisionRadius + other.collisionRadius);
        }
    }
}


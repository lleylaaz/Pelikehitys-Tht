using System.Numerics;
using Raylib_cs;

// Käytin johonkin kohtiin ChatGPT.

namespace ASTEROIDS
{
    public class Asteroids
    {
        public TransformComponent transform;
        public SpriteComponent sprite;
        public CollisionComponent collision;
        public float size;
        public float Radius { get; private set; }

        public Asteroids(Vector2 position, Vector2 velocity, Texture2D texture, float size)
        {
            this.size = size;
            transform = new TransformComponent(position);
            transform.velocity = velocity;
            Radius = (texture.Width * size) / 2.0f;
            sprite = new SpriteComponent(texture, size);
            collision = new CollisionComponent(Radius, transform);
        }
    }
}


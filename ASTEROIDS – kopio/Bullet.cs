using System.Numerics;
using Raylib_cs;

namespace ASTEROIDS
{
    public class Bullet
    {
        public TransformComponent transform;
        public SpriteComponent sprite;
        public CollisionComponent collision;
        public float Radius { get; private set; }
        public bool isAlive;
        public float lifetime;

        public Bullet(Vector2 position, Vector2 direction, Texture2D texture)
        {
            isAlive = true;
            transform = new TransformComponent(position);
            transform.direction = Vector2.Normalize(direction);
            transform.velocity = transform.direction * 400.0f;
            transform.rotationRadians = MathF.Atan2(transform.direction.Y, transform.direction.X) + MathF.PI / 2;

            sprite = new SpriteComponent(texture);
            collision = new CollisionComponent(texture.Width / 2f, transform);
            Radius = texture.Width / 2f;

            lifetime = 2.0f;
        }

        public void Update(float deltaTime)
        {
            transform.Move();
            lifetime -= deltaTime;
            if (lifetime <= 0f)
                isAlive = false;
        }

        public void Draw()
        {
            sprite.Draw(transform.position, transform.rotationRadians);
        }
    }
}





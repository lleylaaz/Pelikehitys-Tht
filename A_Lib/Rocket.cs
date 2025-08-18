using Raylib_cs;
using System.Numerics;
using A_Lib;

// Käytin johonkin kohtiin ChatGPT.

namespace ASTEROIDS
{
    public class Rocket
    {
        public TransformComponent transform;
        public SpriteComponent sprite;
        public CollisionComponent collision;

        public float Radius { get; private set; }
        public float spriteWidth;
        public float spriteHeight;

        public Rocket(Vector2 position, Texture2D texture)
        {
            transform = new TransformComponent(position);
            sprite = new SpriteComponent(texture);
            collision = new CollisionComponent(texture.Width / 2f, transform);

            spriteWidth = texture.Width;
            spriteHeight = texture.Height;
            Radius = texture.Width / 2f;
        }

        public Vector2 GetBulletSpawnPosition()
        {
            Vector2 forward = GetDirection();
            float offset = MathF.Max(spriteWidth, spriteHeight) / 2f;
            return transform.position + forward * offset;
        }

        public void Update(float deltaTime)
        {
            float rotationSpeed = 3.0f;

            if (Raylib.IsKeyDown(KeyboardKey.A))
                transform.rotationRadians -= rotationSpeed * deltaTime;

            if (Raylib.IsKeyDown(KeyboardKey.D))
                transform.rotationRadians += rotationSpeed * deltaTime;

            float angle = transform.rotationRadians - MathF.PI / 2;
            transform.direction = new Vector2(MathF.Cos(angle), MathF.Sin(angle));

            if (Raylib.IsKeyDown(KeyboardKey.W))
                transform.velocity += transform.direction * 100f * deltaTime;

            transform.Move();
        }

        public void Draw()
        {
            sprite.Draw(transform.position, transform.rotationRadians);
        }

        public Vector2 GetDirection()
        {
            float angle = transform.rotationRadians - MathF.PI / 2;
            return new Vector2(MathF.Cos(angle), MathF.Sin(angle));
        }
    }
}

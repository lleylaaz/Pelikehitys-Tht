using System.Numerics;
using Raylib_cs;
using A_Lib;

// Käytin johonkin kohtiin ChatGPT.

namespace ASTEROIDS
{
    public class TransformComponent : Component
    {
        public Vector2 position;
        public Vector2 velocity;
        public Vector2 acceleration;
        public float maxSpeed;
        public Vector2 direction;
        public float rotationRadians;

        public TransformComponent(Vector2 startPosition)
        {
            position = startPosition;
            velocity = Vector2.Zero;
            acceleration = Vector2.Zero;
            maxSpeed = 300f;
            direction = new Vector2(0, -1);
            rotationRadians = 0f;
        }

        public override void Update(float deltaTime)
        {
            Move();
        }

        public void Move()
        {
            float dt = Raylib.GetFrameTime();
            velocity += acceleration * dt;

            if (velocity.Length() > maxSpeed)
                velocity = Vector2.Normalize(velocity) * maxSpeed;

            position += velocity * dt;

            int screenW = Raylib.GetScreenWidth();
            int screenH = Raylib.GetScreenHeight();

            if (position.X < 0) position.X += screenW;
            else if (position.X >= screenW) position.X -= screenW;

            if (position.Y < 0) position.Y += screenH;
            else if (position.Y >= screenH) position.Y -= screenH;
        }

        public void Turn(float amountRadians)
        {
            rotationRadians += amountRadians;
            direction = Vector2.Transform(direction, Matrix3x2.CreateRotation(amountRadians));
        }
    }
}

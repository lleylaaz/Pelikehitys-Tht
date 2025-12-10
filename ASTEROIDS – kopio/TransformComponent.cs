using System.Numerics;
using Raylib_cs;

namespace ASTEROIDS
{
    public class TransformComponent
    {
        public Vector2 position;
        public Vector2 velocity;
        public Vector2 acceleration;
        public float maxSpeed;
        public Vector2 direction;
        public float rotationRadians;

        public TransformComponent(Vector2 startPos, float maxSpeed = 400f)
        {
            position = startPos;
            velocity = Vector2.Zero;
            acceleration = Vector2.Zero;
            direction = new Vector2(0, -1);
            this.maxSpeed = maxSpeed;
            rotationRadians = 0f;
        }

        public void Move()
        {
            float dt = Raylib.GetFrameTime();

            velocity += acceleration * dt;

            if (velocity.Length() > maxSpeed)
                velocity = Vector2.Normalize(velocity) * maxSpeed;

            position += velocity * dt;

            // wrap screen
            int screenW = Raylib.GetScreenWidth();
            int screenH = Raylib.GetScreenHeight();

            if (position.X < 0) position.X += screenW;
            else if (position.X >= screenW) position.X -= screenW;

            if (position.Y < 0) position.Y += screenH;
            else if (position.Y >= screenH) position.Y -= screenH;

            // nollataan kiihtyvyys jokaisen päivityksen jälkeen
            acceleration = Vector2.Zero;
        }
    }
}

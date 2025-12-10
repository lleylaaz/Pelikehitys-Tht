using System.Numerics;
using Raylib_cs;

namespace ASTEROIDS
{
    public class Rocket
    {
        public TransformComponent Transform;
        public SpriteComponent Sprite;
        public CollisionComponent Collision;

        public float Speed = 250f;
        public float Drag = 0.98f;
        public int HP = 100;

        public Rocket(Vector2 startPos, Texture2D texture)
        {
            Transform = new TransformComponent(startPos);
            Sprite = new SpriteComponent(texture);
            Collision = new CollisionComponent(texture.Width / 2, Transform);
        }

        public void UpdateInput(float dt)
        {
            float rotationSpeed = 4.0f; // rad/s
            float accelerationAmount = 200f; // pikseliä/s

            // Kääntäminen
            if (Raylib.IsKeyDown(KeyboardKey.A))
                Transform.rotationRadians -= rotationSpeed * dt;
            if (Raylib.IsKeyDown(KeyboardKey.D))
                Transform.rotationRadians += rotationSpeed * dt;

            // Liike eteenpäin
            if (Raylib.IsKeyDown(KeyboardKey.W))
            {
                Transform.acceleration = new Vector2(
                    MathF.Sin(Transform.rotationRadians),
                    -MathF.Cos(Transform.rotationRadians)  // y-akseli ylös
                ) * accelerationAmount;
            }
            else
            {
                Transform.acceleration = Vector2.Zero;
            }
        }

        public Vector2 GetBulletSpawnPosition()
        {
            float offset = 40f; // etäisyys raketin nokasta, säädä spriteen sopivaksi
            return Transform.position + new Vector2(
                MathF.Sin(Transform.rotationRadians),
                -MathF.Cos(Transform.rotationRadians)
            ) * offset;
        }

        public void Draw()
        {
            // Piirretään sprite kulman mukaan
            Sprite.Draw(Transform.position, Transform.rotationRadians);
        }
    }
}

using System.Numerics;
using Raylib_cs;

namespace ASTEROIDS
{
    public class Rocket
    {
        public TransformComponent Transform;
        public SpriteComponent Sprite;
        public CollisionComponent Collision;

        public Texture2D fireTexture;   // liekki lisätty raketin taakse

        public float Speed = 250f;
        public float Drag = 0.98f;
        public int HP = 100;

        public Rocket(Vector2 startPos, Texture2D texture, Texture2D fireTexture)
        {
            Transform = new TransformComponent(startPos);
            Sprite = new SpriteComponent(texture);
            Collision = new CollisionComponent(texture.Width / 2, Transform);

            this.fireTexture = fireTexture;
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
                    -MathF.Cos(Transform.rotationRadians)
                ) * accelerationAmount;
            }
            else
            {
                Transform.acceleration = Vector2.Zero;
            }
        }

        public Vector2 GetBulletSpawnPosition()
        {
            float offset = 40f;
            return Transform.position + new Vector2(
                MathF.Sin(Transform.rotationRadians),
                -MathF.Cos(Transform.rotationRadians)
            ) * offset;
        }

        public void Draw()
        {
            // Piirrä liekki vain kun kiihdytetään
            if (Raylib.IsKeyDown(KeyboardKey.W))
            {
                // Etäisyys raketin keskipisteestä taaksepäin 
                float flameDistance = 35f;

                // forward-vektori 
                Vector2 forward = new Vector2(
                    MathF.Sin(Transform.rotationRadians),
                    -MathF.Cos(Transform.rotationRadians)
                );

                // liekin sijainti
                Vector2 flamePos = Transform.position - forward * flameDistance;

                float radToDeg = 180f / MathF.PI;
                float flameRotationDeg = Transform.rotationRadians * radToDeg + 180f;

                Rectangle src = new Rectangle(0, 0, fireTexture.Width, fireTexture.Height);
                float scale = 1.5f;
                Rectangle dest = new Rectangle(flamePos.X, flamePos.Y, fireTexture.Width * scale, fireTexture.Height * scale);
                Vector2 origin = new Vector2((fireTexture.Width * scale) / 2f, (fireTexture.Height * scale) / 2f);

                Raylib.DrawTexturePro(
                    fireTexture,
                    src,
                    dest,
                    origin,
                    flameRotationDeg,
                    Color.White
                );
            }

            // Piirrä raketti
            Sprite.Draw(Transform.position, Transform.rotationRadians);
        }

    }
}

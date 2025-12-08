using System.Numerics;
using Raylib_cs;

namespace ASTEROIDS
{
    // Ammus-luokka
    public class Bullet
    {
        public Vector2 Position;  // Ammuksen sijainti
        public float Rotation;    // Ammuksen kulma
        public float Speed = 500f; // Ammuksen nopeus
        public bool IsAlive = true; // Onko ammus elossa

        Texture2D Tex; // Ammuksen tekstuuri

        // Konstruktori
        public Bullet(Vector2 startPos, float rot, Texture2D texture)
        {
            Position = startPos;
            Rotation = rot;
            Tex = texture;
        }

        // Päivittää ammuksen sijainnin
        public void Update(float dt)
        {
            Vector2 dir = new Vector2(
                MathF.Cos(Rotation - MathF.PI / 2),
                MathF.Sin(Rotation - MathF.PI / 2)
            );
            Position += dir * Speed * dt;

            // Poistetaan ammus ruudun ulkopuolelta
            if (Position.X < 0 || Position.X > 800 || Position.Y < 0 || Position.Y > 600)
                IsAlive = false;
        }

        // Piirtää ammun tekstuurin
        public void Draw()
        {
            float angleDegrees = Rotation * (180f / MathF.PI);

            Raylib.DrawTexturePro(
                Tex,
                new Rectangle(0, 0, Tex.Width, Tex.Height),
                new Rectangle(Position.X, Position.Y, Tex.Width, Tex.Height),
                new Vector2(Tex.Width / 2, Tex.Height / 2),
                angleDegrees,
                Color.White
            );
        }
    }
}

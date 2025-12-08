using System.Numerics;
using Raylib_cs;

namespace ASTEROIDS
{
    // Asteroidiluokka
    public class Asteroid
    {
        public Vector2 Position;  // Sijainti
        public Vector2 Velocity;  // Nopeus
        public float Size;        // Koko skaala
        public float Radius;      // Törmäyksen säde
        Texture2D Tex;            // Tekstuuri

        // Konstruktori
        public Asteroid(Vector2 pos, Vector2 vel, Texture2D tex, float size)
        {
            Position = pos;
            Velocity = vel;
            Size = size;
            Tex = tex;
            Radius = (Tex.Width * size) * 0.5f; // Lasketaan törmäyksen säde
        }

        // Päivittää asteroidin sijainnin
        public void Update(float dt)
        {
            Position += Velocity * dt;

            // Jos menee ruudun ulkopuolelle, ilmestyy toisella puolella
            if (Position.X < 0) Position.X = 800;
            if (Position.X > 800) Position.X = 0;
            if (Position.Y < 0) Position.Y = 600;
            if (Position.Y > 600) Position.Y = 0;
        }

        // Piirtää asteroidin tekstuurin
        public void Draw()
        {
            Raylib.DrawTexturePro(
                Tex,
                new Rectangle(0, 0, Tex.Width, Tex.Height),
                new Rectangle(Position.X, Position.Y, Tex.Width * Size, Tex.Height * Size),
                new Vector2((Tex.Width * Size) / 2, (Tex.Height * Size) / 2),
                0f,
                Color.White
            );
        }
    }
}

using System.Numerics;
using Raylib_cs;

namespace ASTEROIDS
{
    public class Rocket
    {
        // Raketin sijainti ruudulla
        public Vector2 Position;

        // Nopeus (liikemäärä)
        public Vector2 Velocity = Vector2.Zero;

        // Raketin rotaatio radiaaneina
        public float Rotation = 0f;

        // Raketin kiihtyvyys W-näppäintä painettaessa
        public float Speed = 200f;

        // Kitka joka hidastaa rakettia
        public float Drag = 0.98f;

        // Pelaajan HP
        public int HP = 100;

        // Raketin tekstuuri
        Texture2D Tex;

        public Rocket(Vector2 startPos, Texture2D texture)
        {
            Position = startPos;
            Tex = texture;
        }

        public void Update(float dt)
        {
            // Kääntyminen vasemmalle
            if (Raylib.IsKeyDown(KeyboardKey.A))
                Rotation -= 3f * dt;

            // Kääntyminen oikealle
            if (Raylib.IsKeyDown(KeyboardKey.D))
                Rotation += 3f * dt;

            // Liikkuminen eteenpäin W:llä
            if (Raylib.IsKeyDown(KeyboardKey.W))
            {
                // Lasketaan eteenpäin suunta
                Vector2 dir = new Vector2(
                    MathF.Cos(Rotation - MathF.PI / 2),
                    MathF.Sin(Rotation - MathF.PI / 2)
                );

                // Lisää nopeutta siihen suuntaan
                Velocity += dir * Speed * dt;
            }

            // Hidastetaan rakettia kitkalla
            Velocity *= Drag;

            // Päivitetään sijainti
            Position += Velocity * dt;

            // Ruudun reunojen teleporttaus
            if (Position.X < 0) Position.X = 800;
            if (Position.X > 800) Position.X = 0;

            if (Position.Y < 0) Position.Y = 600;
            if (Position.Y > 600) Position.Y = 0;
        }

        // Missä kohtaa ammuksen tulisi ilmestyä
        public Vector2 GetBulletSpawnPosition()
        {
            Vector2 dir = new Vector2(
                MathF.Cos(Rotation - MathF.PI / 2),
                MathF.Sin(Rotation - MathF.PI / 2)
            );

            return Position + dir * 35f;   // 35 pikseliä raketin kärjestä
        }

        public void Draw()
        {
            // Muutetaan radiaanit asteiksi piirtämistä varten
            float angleDegrees = Rotation * (180f / MathF.PI);

            // Piirretään tekstuuri keskipisteestä
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

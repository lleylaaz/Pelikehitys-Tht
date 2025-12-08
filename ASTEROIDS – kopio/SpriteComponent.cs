using Raylib_cs;
using System.Numerics;

namespace ASTEROIDS
{
    public class SpriteComponent
    {
        Texture2D texture; // Spriten kuva
        float scale;       // Skaalauskerroin

        public SpriteComponent(Texture2D tex, float s)
        {
            texture = tex;
            scale = s;
        }

        public void Draw(TransformComponent transform)
        {
            // Piirron alkuperä keskelle
            Vector2 origin = new Vector2(texture.Width / 2f, texture.Height / 2f);

            // DrawTexturePro piirtää tekstuurin transformin mukaan
            Raylib.DrawTexturePro(
                texture,
                new Rectangle(0, 0, texture.Width, texture.Height), // koko kuva
                new Rectangle(transform.position.X, transform.position.Y,
                    texture.Width * scale, texture.Height * scale), // kohde + skaala
                origin,
                transform.rotation,                                 // kierto
                Color.White
            );
        }
    }
}

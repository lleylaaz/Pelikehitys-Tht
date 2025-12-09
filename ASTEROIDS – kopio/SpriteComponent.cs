using System.Numerics;
using Raylib_cs;

namespace ASTEROIDS
{
    public class SpriteComponent
    {
        private Texture2D texture;

        public SpriteComponent(Texture2D tex)
        {
            texture = tex;
        }

        // Lisätty scale-parametri
        public void Draw(Vector2 position, float rotation, float scale = 1f)
        {
            Vector2 origin = new Vector2(texture.Width / 2f, texture.Height / 2f);
            Raylib.DrawTexturePro(
                texture,
                new Rectangle(0, 0, texture.Width, texture.Height),
                new Rectangle(position.X, position.Y, texture.Width * scale, texture.Height * scale),
                origin,
                rotation * (180f / MathF.PI), // Raylib odottaa asteita
                Color.White
            );
        }
    }
}

using System.Numerics;
using Raylib_cs;
using A_lib;

// Käytin johonkin kohtiin ChatGPT.

namespace ASTEROIDS
{
    public class SpriteComponent : Component
    {
        private Texture2D texture;
        private Rectangle drawSource;
        private Vector2 origin;
        private float size;

        public SpriteComponent(Texture2D texture, float size = 1f)
        {
            this.texture = texture;
            this.size = size;
            drawSource = new Rectangle(0, 0, texture.Width, texture.Height);
            origin = new Vector2(texture.Width / 2f, texture.Height / 2f);
        }

        public void Draw(Vector2 position, float rotationRadians)
        {
            if (texture.Width == 0) return;
            float rotationDegrees = rotationRadians * Raylib.RAD2DEG;
            Rectangle drawDest = new Rectangle(position.X, position.Y, texture.Width * size, texture.Height * size);
            Vector2 scaledOrigin = origin * size;

            Raylib.DrawTexturePro(texture, drawSource, drawDest, scaledOrigin, rotationDegrees, Color.White);
        }
    }
}


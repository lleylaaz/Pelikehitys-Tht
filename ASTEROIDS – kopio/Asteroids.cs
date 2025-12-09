using System.Numerics;
using Raylib_cs;

namespace ASTEROIDS
{
    public class Asteroids
    {
        public TransformComponent Transform;
        public SpriteComponent Sprite;
        public CollisionComponent Collision;
        public float Size;
        public float Radius;

        public Asteroids(Vector2 pos, Vector2 vel, Texture2D texture, float size)
        {
            Size = size;
            Transform = new TransformComponent(pos);
            Transform.velocity = vel;
            Sprite = new SpriteComponent(texture);
            Radius = (texture.Width * size) / 2f;
            Collision = new CollisionComponent(Radius, Transform);
        }
        public void Draw()
        {
            Sprite.Draw(Transform.position, 0f, Size); // Käytetään Size-muuttujaa skaalaamiseen
        }
    }
}

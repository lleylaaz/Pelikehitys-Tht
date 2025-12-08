using System.Numerics;

namespace ASTEROIDS
{
    public class TransformComponent
    {
        public Vector2 position;  // Objektiin keskipisteen sijainti
        public Vector2 velocity;  // Liikevektori
        public float rotation;    // Kierto radiaaneina

        public TransformComponent(Vector2 pos)
        {
            position = pos;
            velocity = Vector2.Zero;
            rotation = 0f;
        }
    }
}

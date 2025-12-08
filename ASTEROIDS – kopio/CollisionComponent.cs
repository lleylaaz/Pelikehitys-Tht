using System.Numerics;

namespace ASTEROIDS
{
    // Komponentti, joka hallitsee törmäyksiä
    public class CollisionComponent
    {
        // Viittaus transform komponenttiin (sijainti ja rotaatio)
        public TransformComponent transform;

        // Törmäyksen säde
        public float radius;

        // Konstruktori
        public CollisionComponent(float r, TransformComponent t)
        {
            radius = r;
            transform = t;
        }

        // Tarkistaa törmäyksen toiseen komponenttiin
        public bool CheckCollision(CollisionComponent other)
        {
            float dist = Vector2.Distance(transform.position, other.transform.position);
            return dist < (radius + other.radius);
        }
    }
}

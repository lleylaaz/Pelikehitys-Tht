using Raylib_cs;
using A_Lib;

namespace ASTEROIDS
{
    // Käytin johonkin kohtiin ChatGPT.
    public abstract class Component
    {
        public virtual void Update(float deltaTime) { }
        public virtual void Draw() { }
    }
}


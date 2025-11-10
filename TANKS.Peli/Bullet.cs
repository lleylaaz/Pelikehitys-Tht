using System.Numerics;
using Raylib_cs;

// Käytin välillä ChatGPT:tä johonkin kohtiin.
class Bullet
{
    public Vector2 Position;
    public float Rotation;
    private const float Speed = 5;
    public bool Active = true;

    public Bullet(float x, float y, float rotation)
    {
        Position = new Vector2(x, y);
        Rotation = rotation;
    }

    public void Update()
    {
        if (!Active) return;

        Position.X += Speed * MathF.Cos(Rotation * (MathF.PI / 180));
        Position.Y += Speed * MathF.Sin(Rotation * (MathF.PI / 180));

        if (Position.X < 0 || Position.X > Raylib.GetScreenWidth() || Position.Y < 0 || Position.Y > Raylib.GetScreenHeight())
        {
            Active = false;
        }
    }

    public void Draw()
    {
        if (Active)
            Raylib.DrawCircle((int)Position.X, (int)Position.Y, 5, Color.Black);
    }
}







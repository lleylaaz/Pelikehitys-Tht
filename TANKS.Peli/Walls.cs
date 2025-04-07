using Raylib_cs;
using System.Numerics;

class Walls
{
    public Vector2 Position;
    public float Width;
    public float Height;
    public Color Color;

    public Walls(float x, float y, float width, float height, Color color)
    {
        Position = new Vector2(x, y);
        Width = width;
        Height = height;
        Color = color;
    }

    public void Draw()
    {
        Raylib.DrawRectangleV(Position, new Vector2(Width, Height), Color);
    }

    public Rectangle GetBounds()
    {
        return new Rectangle(Position.X, Position.Y, Width, Height);
    }
}







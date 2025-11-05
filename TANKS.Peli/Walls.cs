using Raylib_cs;
using System.Numerics;

class Walls
{
    public Vector2 Position;
    public float Width;
    public float Height;
    public Color Color;
    public Texture2D Texture;

    public Walls(float x, float y, float width, float height, Color color, Texture2D texture = default)
    {
        Position = new Vector2(x, y);
        Width = width;
        Height = height;
        Color = color;
        Texture = texture;
    }

    public void DrawScaled()
    {
        if (Texture.Id != 0)
        {
            Raylib.DrawTexturePro(
                Texture,
                new Rectangle(0, 0, Texture.Width, Texture.Height),
                new Rectangle(Position.X, Position.Y, Width, Height),
                Vector2.Zero,
                0f,
                Color.White
            );
        }
        else
        {
            Raylib.DrawRectangleV(Position, new Vector2(Width, Height), Color);
        }
    }

    public void DrawTiled()
    {
        if (Texture.Id == 0)
        {
            Raylib.DrawRectangleV(Position, new Vector2(Width, Height), Color);
            return;
        }

        int texWidth = Texture.Width;
        int texHeight = Texture.Height;

        for (float y = Position.Y; y < Position.Y + Height; y += texHeight)
        {
            for (float x = Position.X; x < Position.X + Width; x += texWidth)
            {
                int drawWidth = (int)Math.Min(texWidth, Position.X + Width - x);
                int drawHeight = (int)Math.Min(texHeight, Position.Y + Height - y);

                Raylib.DrawTexturePro(
                    Texture,
                    new Rectangle(0, 0, drawWidth, drawHeight),
                    new Rectangle(x, y, drawWidth, drawHeight),
                    Vector2.Zero,
                    0f,
                    Color.White
                );
            }
        }
    }


    public Rectangle GetBounds()
    {
        return new Rectangle(Position.X, Position.Y, Width, Height);
    }
}



using System.Numerics;
using Raylib_cs;

// Käytin välillä ChatGPT:tä johonkin kohtiin.
class Tank
{
    public Vector2 Position;
    public float Rotation;
    public Color TankColor;
    public Texture2D Texture;
    public List<Bullet> Projectiles = new List<Bullet>();

    private float speed = 2f;
    private double lastShootTime = 0;
    private double shootInterval = 0.25;
    private Vector2 startPosition;

    private float turretLength = 25f;

    private float rotationOffset = 90f;

    public Tank(float x, float y, Color color, Texture2D texture = default)
    {
        Position = new Vector2(x, y);
        startPosition = Position;
        TankColor = color;
        Texture = texture;
        Rotation = 0f;
    }

    public void ResetPosition()
    {
        Position = startPosition;
        Projectiles.Clear();
    }

    public Rectangle GetBounds()
    {
        return new Rectangle(Position.X - 20, Position.Y - 20, 40, 40);
    }

    public bool Update(KeyboardKey forward, KeyboardKey backward, KeyboardKey turnLeft, KeyboardKey turnRight, KeyboardKey shootKey, List<Walls> walls, Tank opponent)
    {
        Vector2 oldPosition = Position;

        if (Raylib.IsKeyDown(turnLeft)) Rotation -= 2f;
        if (Raylib.IsKeyDown(turnRight)) Rotation += 2f;

        float moveRad = Rotation * (MathF.PI / 180f);

        if (Raylib.IsKeyDown(forward))
        {
            Position.X += MathF.Cos(moveRad) * speed;
            Position.Y += MathF.Sin(moveRad) * speed;
        }
        if (Raylib.IsKeyDown(backward))
        {
            Position.X -= MathF.Cos(moveRad) * speed;
            Position.Y -= MathF.Sin(moveRad) * speed;
        }

        foreach (var wall in walls)
        {
            if (Raylib.CheckCollisionRecs(GetBounds(), wall.GetBounds()))
            {
                Position = oldPosition;
                break;
            }
        }

        if (Raylib.IsKeyPressed(shootKey) && (Raylib.GetTime() - lastShootTime > shootInterval))
        {
            lastShootTime = Raylib.GetTime();
            float rad = Rotation * (MathF.PI / 180);

            float projectileX = Position.X + MathF.Cos(rad) * 25f;
            float projectileY = Position.Y + MathF.Sin(rad) * 25f;

            Projectiles.Add(new Bullet(projectileX, projectileY, Rotation));
        }


        bool opponentWasHit = false;
        foreach (var projectile in Projectiles)
        {
            projectile.Update();

            foreach (var wall in walls)
            {
                if (Raylib.CheckCollisionCircleRec(projectile.Position, 5, wall.GetBounds()))
                    projectile.Active = false;
            }

            if (Raylib.CheckCollisionCircleRec(projectile.Position, 5, opponent.GetBounds()))
            {
                projectile.Active = false;
                opponentWasHit = true;
            }
        }

        Projectiles.RemoveAll(p => !p.Active);
        return opponentWasHit;
    }

    public void Draw()
    {
        if (Texture.Id != 0)
        {
            Raylib.DrawTexturePro(
                Texture,
                new Rectangle(0, 0, Texture.Width, Texture.Height),
                new Rectangle(Position.X, Position.Y, 40, 40),
                new Vector2(20, 20),
                Rotation - 90f,
                Color.White
            );

        }
        else
        {
            Raylib.DrawRectangleV(Position - new Vector2(20, 20), new Vector2(40, 40), TankColor);
        }

        foreach (var projectile in Projectiles)
        {
            projectile.Draw();
        }
    }
}

using System.Collections.Generic;
using System.Numerics;
using Raylib_cs;

class Tank
{
    public Vector2 Position;
    public float Rotation;
    public Color TankColor;
    public List<Bullet> Projectiles = new List<Bullet>();

    private float speed = 2.5f;
    private double lastShootTime = 0;
    private double shootInterval = 1.0;
    private Vector2 startPosition;

    public Tank(float x, float y, Color color)
    {
        Position = new Vector2(x, y);
        startPosition = Position;
        Rotation = 0;
        TankColor = color;
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

    public bool Update(KeyboardKey up, KeyboardKey down, KeyboardKey left, KeyboardKey right, KeyboardKey shootKey, List<Walls> walls, Tank opponent)
    {
        Vector2 oldPosition = Position;

        if (Raylib.IsKeyDown(up)) { Position.Y -= speed; Rotation = 270; }
        if (Raylib.IsKeyDown(down)) { Position.Y += speed; Rotation = 90; }
        if (Raylib.IsKeyDown(left)) { Position.X -= speed; Rotation = 180; }
        if (Raylib.IsKeyDown(right)) { Position.X += speed; Rotation = 0; }

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
            float radian = Rotation * (MathF.PI / 180);
            float projectileX = Position.X + MathF.Cos(radian) * 20;
            float projectileY = Position.Y + MathF.Sin(radian) * 20;
            Projectiles.Add(new Bullet(projectileX, projectileY, Rotation));
        }

        bool opponentWasHit = false;

        foreach (var projectile in Projectiles)
        {
            projectile.Update();

            foreach (var wall in walls)
            {
                if (Raylib.CheckCollisionCircleRec(projectile.Position, 5, wall.GetBounds()))
                {
                    projectile.Active = false;
                }
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
        DrawTank(Position, Rotation, TankColor);

        foreach (var projectile in Projectiles)
        {
            projectile.Draw();
        }
    }

    private void DrawTank(Vector2 position, float rotation, Color color)
    {
        Vector2 tankSize = new Vector2(40, 40);
        Vector2 turretSize = new Vector2(10, 20);
        Vector2 topLeft = position - tankSize / 2.0f;

        Raylib.DrawRectangleV(topLeft, tankSize, color);

        float radian = rotation * (MathF.PI / 180);
        Vector2 turretOffset = new Vector2(MathF.Cos(radian), MathF.Sin(radian)) * (tankSize.X / 2.0f);
        Vector2 turretPos = position + turretOffset;
        Vector2 turretTopLeft = turretPos - turretSize / 2.0f;

        if (rotation == 0 || rotation == 180)
        {
            Raylib.DrawRectangleV(turretTopLeft, new Vector2(20, 10), Color.Black);
        }
        else
        {
            Raylib.DrawRectangleV(turretTopLeft, new Vector2(10, 20), Color.Black);
        }
    }
}


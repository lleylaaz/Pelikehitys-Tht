using System.Numerics;
using Raylib_cs;

namespace Raylib_test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Vector2 A = new Vector2(800/2, 0);
            Vector2 B = new Vector2(0, 800/2);
            Vector2 C = new Vector2(800, 800 * 3/4);

            Vector2 dirA = new Vector2(1, 1);
            Vector2 dirB = new Vector2(1, -1);
            Vector2 dirC = new Vector2(-1, -1);

            float speed = 200f;

            Raylib.InitWindow(800, 800, "Raylib_test");
            float deltaTime;
            
            while (!Raylib.WindowShouldClose())
            {
                deltaTime = Raylib.GetFrameTime();

                A += dirA * speed * deltaTime;
                B += dirB * speed * deltaTime;
                C += dirC * speed * deltaTime;

                if (A.X < 0 || A.X > 800) dirA.X *= -1;
                if (A.Y < 0 || A.Y > 800) dirA.Y *= -1;

                if (B.X < 0 || B.X > 800) dirB.X *= -1;
                if (B.Y < 0 || B.Y > 800) dirB.Y *= -1;

                if (C.X < 0 || C.X > 800) dirC.X *= -1;
                if (C.Y < 0 || C.Y > 800) dirC.Y *= -1;

                // Piirtäminen
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.DarkPurple);

                Raylib.DrawLineV(A, B, Color.Green);
                Raylib.DrawLineV(B, C, Color.Yellow);
                Raylib.DrawLineV(C, A, Color.SkyBlue);
                Raylib.DrawText("", 220, 60, 32, Color.White);

                Raylib.EndDrawing();
            }
            Raylib.CloseWindow();
        }
    }
}

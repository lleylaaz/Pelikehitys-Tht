using System;
using Raylib_cs;
using System.Numerics;

// Käytin ChatGPT, koska en itse osaa aloittaa koodin tekoa.
class Program
{
    static void Main()
    {
        // Ikkuna
        const int screenWidth = 800;
        const int screenHeight = 600;
        Raylib.InitWindow(screenWidth, screenHeight, "DVD Bouncing Text");

        // FPS
        Raylib.SetTargetFPS(60);

        // Tekstin ominaisuudet
        Vector2 position = new Vector2(screenWidth / 2, screenHeight / 2); // Keskelle
        Vector2 direction = new Vector2(1, 1); // Molempiin suuntiin
        float speed = 100.0f;

        int fontSize = 50;
        Vector2 textSize = Raylib.MeasureTextEx(Raylib.GetFontDefault(), "DVD", fontSize, 2);

        // Värit
        Color textColor = Color.White;

        while (!Raylib.WindowShouldClose())
        {
            float frameTime = Raylib.GetFrameTime();

            // Sijainti
            position += direction * speed * frameTime;

            // Törmäykset
            bool collision = false;

            if (position.X <= 0 || position.X + textSize.X >= screenWidth)
            {
                direction.X *= -1;
                collision = true;
            }
            if (position.Y <= 0 || position.Y + textSize.Y >= screenHeight)
            {
                direction.Y *= -1;
                collision = true;
            }

            // Tekstin väri muuttuu törmäyksissä ja logon nopeus nousee
            if (collision)
            {
                textColor = new Color(
                    Raylib.GetRandomValue(0, 255),
                    Raylib.GetRandomValue(0, 255),
                    Raylib.GetRandomValue(0, 255),
                    255
                );
                speed += 20.0f; // Lisää nopeutta
            }

            // Ruutu (Ikkuna)
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);

            Raylib.DrawText("DVD", (int)position.X, (int)position.Y, fontSize, textColor);

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}

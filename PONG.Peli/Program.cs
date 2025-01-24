using System;
using System.Numerics;
using Raylib_cs;

class PongGame
{
    static void Main(string[] args)
    {
        // Käytin vähän ChatGPT:tä koodin tekoon

        // Tekee Raylib ikkunan
        const int screenWidth = 800;
        const int screenHeight = 600;
        Raylib.InitWindow(screenWidth, screenHeight, "PONG Peli");
        Raylib.SetTargetFPS(60);

        // Ns mailojen asetukset
        int paddleWidth = 10;
        int paddleHeight = 100;
        int paddleSpeed = 5;

        // Pelaajan 1 asetuket
        int player1X = 50;
        int player1Y = screenHeight / 2 - paddleHeight / 2;
        int player1Score = 0;
        Vector2 player1ScorePos = new Vector2(screenWidth / 4, 50);

        // Pelaajan 2 asetukset
        int player2X = screenWidth - 50 - paddleWidth;
        int player2Y = screenHeight / 2 - paddleHeight / 2;
        int player2Score = 0;
        Vector2 player2ScorePos = new Vector2(3 * screenWidth / 4, 50);

        // Pallon asetukset
        Vector2 ballPosition = new Vector2(screenWidth / 2, screenHeight / 2);
        Vector2 ballDirection = new Vector2(1, 1);
        float ballSpeed = 4.0f;

        while (!Raylib.WindowShouldClose())
        {
            // Pelaajan 1 liikkeet
            if (Raylib.IsKeyDown(KeyboardKey.W) && player1Y > 0)
            {
                player1Y -= paddleSpeed;
            }
            if (Raylib.IsKeyDown(KeyboardKey.S) && player1Y < screenHeight - paddleHeight)
            {
                player1Y += paddleSpeed;
            }

            // Pelaajan 2 liikkeet
            if (Raylib.IsKeyDown(KeyboardKey.Up) && player2Y > 0)
            {
                player2Y -= paddleSpeed;
            }
            if (Raylib.IsKeyDown(KeyboardKey.Down) && player2Y < screenHeight - paddleHeight)
            {
                player2Y += paddleSpeed;
            }

            // Pallon liikkeet
            ballPosition.X += ballDirection.X * ballSpeed;
            ballPosition.Y += ballDirection.Y * ballSpeed;

            // Pallon törmäykset seinien kanssa
            if (ballPosition.Y <= 0 || ballPosition.Y >= screenHeight)
            {
                ballDirection.Y *= -1;
            }

            // Pallon törmäys
            if ((ballPosition.X <= player1X + paddleWidth && ballPosition.Y >= player1Y && ballPosition.Y <= player1Y + paddleHeight) ||
                (ballPosition.X >= player2X - paddleWidth && ballPosition.Y >= player2Y && ballPosition.Y <= player2Y + paddleHeight))
            {
                ballDirection.X *= -1;
            }

            // Pallo rajan ulkopolella, eli maalin teko
            if (ballPosition.X <= 0)
            {
                player2Score++;
                ballPosition = new Vector2(screenWidth / 2, screenHeight / 2);
                ballDirection = new Vector2(1, 1);
            }
            if (ballPosition.X >= screenWidth)
            {
                player1Score++;
                ballPosition = new Vector2(screenWidth / 2, screenHeight / 2);
                ballDirection = new Vector2(-1, 1);
            }

            // Piirtää taustan
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Gray);

            // Piirtää ns pelimailat
            Raylib.DrawRectangle(player1X, player1Y, paddleWidth, paddleHeight, Color.Blue);
            Raylib.DrawRectangle(player2X, player2Y, paddleWidth, paddleHeight, Color.Red);

            // Piirtää Pallon
            Raylib.DrawCircle((int)ballPosition.X, (int)ballPosition.Y, 10, Color.White);

            // Piirtää pisteet
            Raylib.DrawText(player1Score.ToString(), (int)player1ScorePos.X, (int)player1ScorePos.Y, 40, Color.Blue);
            Raylib.DrawText(player2Score.ToString(), (int)player2ScorePos.X, (int)player2ScorePos.Y, 40, Color.Red);

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}

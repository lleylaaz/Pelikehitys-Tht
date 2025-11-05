using Raylib_cs;
using System.Numerics;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        const int screenWidth = 800;
        const int screenHeight = 600;
        Raylib.InitWindow(screenWidth, screenHeight, "TANKS");
        Raylib.SetTargetFPS(60);

        Texture2D brickTexture = Raylib.LoadTexture("Brick_Wall_Texture.jpg");
        Texture2D blueTankTexture = Raylib.LoadTexture("Blue_Tank_Texture.jpg");
        Texture2D redTankTexture = Raylib.LoadTexture("Red_Tank_Texture.jpg");

        GameState gameState = GameState.MainMenu;
        Menu menu = new Menu();

        Tank blueTank = new Tank(100, 500, Color.Blue, blueTankTexture);
        Tank redTank = new Tank(700, 500, Color.Red, redTankTexture);

        int blueScore = 0;
        int redScore = 0;

        List<Walls> walls = GenerateWalls(screenWidth, screenHeight, brickTexture);

        while (!Raylib.WindowShouldClose())
        {
            switch (gameState)
            {
                case GameState.MainMenu:
                case GameState.GameOver:
                    menu.Update(ref gameState, blueScore, redScore);
                    menu.Draw(gameState, blueScore, redScore);
                    break;

                case GameState.Playing:
                    bool blueWasHit = redTank.Update(KeyboardKey.Up, KeyboardKey.Down, KeyboardKey.Left, KeyboardKey.Right, KeyboardKey.Enter, walls, blueTank);
                    bool redWasHit = blueTank.Update(KeyboardKey.W, KeyboardKey.S, KeyboardKey.A, KeyboardKey.D, KeyboardKey.Space, walls, redTank);

                    if (blueWasHit)
                    {
                        redScore++;
                        gameState = GameState.GameOver;
                        ResetGame(blueTank, redTank, out walls, screenWidth, screenHeight, brickTexture);
                    }
                    else if (redWasHit)
                    {
                        blueScore++;
                        gameState = GameState.GameOver;
                        ResetGame(blueTank, redTank, out walls, screenWidth, screenHeight, brickTexture);
                    }

                    Raylib.BeginDrawing();
                    Raylib.ClearBackground(Color.White);

                    foreach (var wall in walls)
                        wall.DrawTiled();

                    blueTank.Draw();
                    redTank.Draw();

                    Raylib.DrawText($"Blue: {blueScore}", 30, 10, 20, Color.Blue);
                    Raylib.DrawText($"Red: {redScore}", screenWidth - 100, 10, 20, Color.Red);

                    Raylib.EndDrawing();
                    break;
            }
        }

        Raylib.CloseWindow();
    }

    static List<Walls> GenerateWalls(int screenWidth, int screenHeight, Texture2D texture)
    {
        List<Walls> walls = new List<Walls>();
        Random rand = new Random();

        Vector2 blueSpawn = new Vector2(100, 500);
        Vector2 redSpawn = new Vector2(700, 500);
        float safeRadius = 100;

        int maxAttempts = 50;
        int createdWalls = 0;

        while (createdWalls < 5)
        {
            int attempts = 0;
            float x = rand.Next(50, screenWidth - 150);
            float y = rand.Next(50, screenHeight - 150);
            float width = rand.Next(50, 120);
            float height = rand.Next(50, 150);

            Rectangle newWallRect = new Rectangle(x, y, width, height);

            bool valid = Vector2.Distance(new Vector2(x, y), blueSpawn) > safeRadius &&
                         Vector2.Distance(new Vector2(x, y), redSpawn) > safeRadius;

            foreach (var wall in walls)
            {
                if (Raylib.CheckCollisionRecs(newWallRect, wall.GetBounds()))
                {
                    valid = false;
                    break;
                }
            }

            if (valid)
            {
                walls.Add(new Walls(x, y, width, height, Color.White, texture));
                createdWalls++;
            }

            attempts++;
            if (attempts > maxAttempts) break;
        }

        return walls;
    }

    static void ResetGame(Tank blueTank, Tank redTank, out List<Walls> newWalls, int screenWidth, int screenHeight, Texture2D texture)
    {
        blueTank.ResetPosition();
        redTank.ResetPosition();
        newWalls = GenerateWalls(screenWidth, screenHeight, texture);
    }
}

using System.Numerics;
using Raylib_cs;

class MainProgram
{
    static void Main()
    {
        const int screenWidth = 800;
        const int screenHeight = 600;
        Raylib.InitWindow(screenWidth, screenHeight, "TANKS! Peli");
        Raylib.SetTargetFPS(60);

        Tank blueTank = new Tank(100, 500, Color.Blue);
        Tank redTank = new Tank(700, 500, Color.Red);

        int blueScore = 0;
        int redScore = 0;

        List<Walls> walls = new List<Walls>();
        Random rand = new Random();

        Vector2 blueSpawn = new Vector2(100, 100);
        Vector2 redSpawn = new Vector2(screenWidth - 100, 100);

        int wallsCreated = 0;
        while (wallsCreated < 3)
        {
            float x = rand.Next(50, screenWidth - 100);
            float y = rand.Next(50, screenHeight - 100);
            float width = rand.Next(50, 100);
            float height = rand.Next(100, 150);
            Color color = Color.DarkBrown;

            Vector2 wallCenter = new Vector2(x + width / 2, y + height / 2);

            float blueDist = Vector2.Distance(wallCenter, blueSpawn);
            float redDist = Vector2.Distance(wallCenter, redSpawn);

            if (blueDist > 100 && redDist > 100)
            {
                walls.Add(new Walls(x, y, width, height, color));
                wallsCreated++;
            }
        }


        while (!Raylib.WindowShouldClose())
        {
            bool blueWasHit = redTank.Update(KeyboardKey.Up, KeyboardKey.Down, KeyboardKey.Left, KeyboardKey.Right, KeyboardKey.Enter, walls, blueTank);
            bool redWasHit = blueTank.Update(KeyboardKey.W, KeyboardKey.S, KeyboardKey.A, KeyboardKey.D, KeyboardKey.Space, walls, redTank);

            if (blueWasHit)
            {
                redScore++;
                ResetGame(blueTank, redTank, out walls, screenWidth, screenHeight);
            }
            else if (redWasHit)
            {
                blueScore++;
                ResetGame(blueTank, redTank, out walls, screenWidth, screenHeight);
            }

            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.LightGray);

            foreach (Walls wall in walls)
                wall.Draw();

            blueTank.Draw();
            redTank.Draw();

            Raylib.DrawText($"Sininen: {blueScore}", 10, 10, 20, Color.Blue);
            Raylib.DrawText($"Punainen: {redScore}", screenWidth - 150, 10, 20, Color.Red);

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }

    static List<Walls> GenerateWalls(int screenWidth, int screenHeight)
    {
        List<Walls> walls = new List<Walls>();
        Random rand = new Random();

        for (int i = 0; i < 3; i++)
        {
            float x = rand.Next(50, screenWidth - 150);
            float y = rand.Next(50, screenHeight - 150);
            float width = rand.Next(50, 100);
            float height = rand.Next(100, 150);
            walls.Add(new Walls(x, y, width, height, Color.DarkBrown));
        }

        return walls;
    }

    static void ResetGame(Tank blueTank, Tank redTank, out List<Walls> newWalls, int screenWidth, int screenHeight)
    {
        blueTank.ResetPosition();
        redTank.ResetPosition();
        newWalls = GenerateWalls(screenWidth, screenHeight);
    }
}









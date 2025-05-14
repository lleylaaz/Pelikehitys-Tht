using Raylib_cs;
using System.Collections.Generic;
using System.Numerics;

namespace ASTEROIDS
{
    internal class Program
    {
        Texture2D player;
        Vector2 position;
        Vector2 direction = new Vector2(0.0f, -1.0f);
        Vector2 velocity;
        float rotation = 0.0f;
        float speed = 0.0f;
        float acceleration = 50.0f;
        float maxSpeed = 100.0f;
        Rocket? playerRocket;

        int level = 1;
        int score = 0;

        List<Asteroids> asteroids = new List<Asteroids>();
        Texture2D asteroidTexture;
        Random rand = new Random();

        List<Bullet> bullets = new List<Bullet>();
        Texture2D bulletTexture;

        static void Main(string[] args)
        {
            Program game = new Program();
            game.Run();
        }

        bool CheckCollision(Vector2 pos1, float r1, Vector2 pos2, float r2)
        {
            float dist = Vector2.Distance(pos1, pos2);
            return dist < (r1 + r2);
        }

        void CreateLevel(int level)
        {
            asteroids.Clear();
            int asteroidCount = 0;

            if (level == 1)
                asteroidCount = 2;
            else if (level == 2)
                asteroidCount = 4;
            else if (level == 3)
                asteroidCount = 6;

            for (int i = 0; i < asteroidCount; i++)
            {
                Vector2 pos;
                do
                {
                    pos = new Vector2(rand.Next(100, 700), rand.Next(100, 500));
                } while (Vector2.Distance(pos, new Vector2(Raylib.GetScreenWidth() / 2, Raylib.GetScreenHeight() / 2)) < 100);

                Vector2 vel = new Vector2(rand.Next(-100, 100), rand.Next(-100, 100));
                asteroids.Add(new Asteroids(pos, vel, asteroidTexture, 1.0f));
            }
        }

        public void Run()
        {
            Raylib.InitWindow(800, 650, "ASTEROIDS");
            Raylib.SetTargetFPS(60);
            const float deltaTime = 1.0f / 60.0f;

            bulletTexture = Raylib.LoadTexture("laserRed07.png");
            asteroidTexture = Raylib.LoadTexture("meteorBrown_big1.png");
            Texture2D playerTexture = Raylib.LoadTexture("playerShip1_red.png");

            Vector2 startPosition = new Vector2(Raylib.GetScreenWidth() / 2.0f, Raylib.GetScreenHeight() / 2.0f);
            playerRocket = new Rocket(startPosition, playerTexture);

            bool startMenu = true;
            bool gameOver = false;
            bool gameWon = false;
            bool levelCompleted = false;
            int level = 1;
            int maxLevel = 3;
            int lives = 5;
            int score = 0;

            Rectangle startButton = new Rectangle(300, 300, 200, 50);
            Rectangle nextLevelButton = new Rectangle(290, 300, 200, 50);
            Rectangle quitButton = new Rectangle(300, 300, 200, 50);
            Rectangle continueButton = new Rectangle(300, 370, 200, 50);

            CreateLevel(level);

            while (!Raylib.WindowShouldClose())
            {
                float DdeltaTime = Raylib.GetFrameTime();

                if (startMenu)
                {
                    Raylib.BeginDrawing();
                    Raylib.ClearBackground(Color.Black);

                    Raylib.DrawText("Welcome to Asteroids", 190, 200, 40, Color.White);
                    Raylib.DrawRectangleRec(startButton, Color.DarkGray);
                    Raylib.DrawText("START", (int)startButton.X + 55, (int)startButton.Y + 10, 30, Color.White);

                    if (Raylib.IsMouseButtonPressed(MouseButton.Left))
                    {
                        Vector2 mouse = Raylib.GetMousePosition();
                        if (Raylib.CheckCollisionPointRec(mouse, startButton))
                        {
                            startMenu = false;
                        }
                    }

                    Raylib.EndDrawing();
                    continue;
                }

                if (!gameOver && !levelCompleted)
                {
                    foreach (Asteroids asteroid in asteroids)
                        asteroid.Update(deltaTime);

                    foreach (Bullet bullet in bullets)
                        bullet.Update(deltaTime);

                    bullets.RemoveAll(b => !b.isAlive);
                    playerRocket.Update(deltaTime);

                    if (Raylib.IsKeyPressed(KeyboardKey.Space))
                    {
                        Vector2 bulletDir = playerRocket.GetDirection();
                        Vector2 bulletPos = playerRocket.position + bulletDir * 30;
                        bullets.Add(new Bullet(bulletPos, bulletDir, bulletTexture));
                    }

                    List<Asteroids> newAsteroids = new List<Asteroids>();
                    for (int i = bullets.Count - 1; i >= 0; i--)
                    {
                        for (int j = asteroids.Count - 1; j >= 0; j--)
                        {
                            if (CheckCollision(bullets[i].position, bullets[i].Radius,
                                               asteroids[j].position, asteroids[j].Radius))
                            {
                                bullets[i].isAlive = false;
                                score += (int)(100 * asteroids[j].size);

                                if (asteroids[j].size > 0.25f)
                                {
                                    for (int k = 0; k < 2; k++)
                                    {
                                        float angle = (float)(rand.NextDouble() * Math.PI * 2);
                                        Vector2 dir = new Vector2(MathF.Cos(angle), MathF.Sin(angle));
                                        Vector2 pos = asteroids[j].position;
                                        Vector2 vel = dir * 80.0f;
                                        newAsteroids.Add(new Asteroids(pos, vel, asteroidTexture, asteroids[j].size / 2.0f));
                                    }
                                }

                                asteroids.RemoveAt(j);
                                break;
                            }
                        }
                    }
                    asteroids.AddRange(newAsteroids);

                    foreach (Asteroids asteroid in asteroids)
                    {
                        if (CheckCollision(playerRocket.position, playerTexture.Width / 2,
                                           asteroid.position, asteroid.Radius))
                        {
                            lives--;
                            playerRocket.position = new Vector2(Raylib.GetScreenWidth() / 2.0f, Raylib.GetScreenHeight() / 2.0f);
                            playerRocket.velocity = Vector2.Zero;

                            if (lives <= 0)
                            {
                                gameOver = true;
                            }

                            break;
                        }
                    }

                    if (asteroids.Count == 0)
                    {
                        levelCompleted = true;
                        if (level == maxLevel)
                            gameWon = true;
                    }
                }

                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.Black);

                Raylib.DrawText($"Level: {level}/{maxLevel}", 10, 10, 20, Color.LightGray);
                Raylib.DrawText($"Score: {score}", 10, 40, 20, Color.LightGray);
                Raylib.DrawText($"Lives: {lives}", 10, 70, 20, Color.LightGray);

                if (gameOver)
                {
                    Raylib.DrawText("GAME OVER", 280, 200, 40, Color.Red);

                    Raylib.DrawRectangleRec(quitButton, Color.DarkGray);
                    Raylib.DrawText("QUIT", (int)quitButton.X + 65, (int)quitButton.Y + 10, 30, Color.White);

                    Raylib.DrawRectangleRec(continueButton, Color.DarkGray);
                    Raylib.DrawText("CONTINUE", (int)continueButton.X + 22, (int)continueButton.Y + 10, 30, Color.White);

                    if (Raylib.IsMouseButtonPressed(MouseButton.Left))
                    {
                        Vector2 mouse = Raylib.GetMousePosition();

                        if (Raylib.CheckCollisionPointRec(mouse, quitButton))
                        {
                            Raylib.CloseWindow();
                        }

                        else if (Raylib.CheckCollisionPointRec(mouse, continueButton))
                        {
                            gameOver = false;
                            gameWon = false;
                            levelCompleted = false;

                            lives = 5;
                            bullets.Clear();
                            asteroids.Clear();
                            CreateLevel(level);

                            playerRocket.position = new Vector2(Raylib.GetScreenWidth() / 2.0f, Raylib.GetScreenHeight() / 2.0f);
                            playerRocket.velocity = Vector2.Zero;
                        }
                    }
                }
                else if (levelCompleted)
                {
                    if (gameWon)
                    {
                        Raylib.DrawText("YOU WON THE GAME!", 240, 300, 30, Color.Green);
                    }
                    else
                    {
                        Raylib.DrawText("YOU WIN!", 290, 200, 40, Color.Yellow);
                        Raylib.DrawRectangleRec(nextLevelButton, Color.DarkGray);
                        Raylib.DrawText("NEXT LEVEL", (int)nextLevelButton.X + 20, (int)nextLevelButton.Y + 10, 25, Color.White);

                        if (Raylib.IsMouseButtonPressed(MouseButton.Left))
                        {
                            Vector2 mouse = Raylib.GetMousePosition();
                            if (Raylib.CheckCollisionPointRec(mouse, nextLevelButton))
                            {
                                level++;
                                CreateLevel(level); 
                                bullets.Clear();
                                playerRocket.position = new Vector2(Raylib.GetScreenWidth() / 2.0f, Raylib.GetScreenHeight() / 2.0f);
                                playerRocket.velocity = Vector2.Zero;
                                levelCompleted = false;
                            }
                        }
                    }
                }
                else
                {
                    foreach (Asteroids asteroid in asteroids)
                        asteroid.Draw();

                    foreach (Bullet bullet in bullets)
                        bullet.Draw();

                    playerRocket.Draw();
                }

                Raylib.EndDrawing();
            }
        }
    }
}

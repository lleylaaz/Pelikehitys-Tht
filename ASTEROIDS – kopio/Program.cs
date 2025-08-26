using System.Collections.Generic;
using System.Numerics;
using A_Lib;
using ASTEROIDS;
using Raylib_cs;

namespace A_Lib
{
    public class Program
    {
        Rocket playerRocket;
        List<Asteroids> asteroids = new List<Asteroids>();
        List<Bullet> bullets = new List<Bullet>();
        Random rand = new Random();

        Texture2D asteroidTexture;
        Texture2D bulletTexture;
        Texture2D playerTexture;

        int level = 1;
        int score = 0;
        int lives = 5;
        int maxLevel = 3;

        // Esimerkki asetukset
        int difficulty = 1; 
        int volume = 50;    

        bool startMenu = true;
        bool gameOver = false;
        bool gameWon = false;
        bool levelCompleted = false;
        bool pauseMenu = false;
        bool settingsMenu = false;

        float playerDamageCooldown = 0f;

        Rectangle startButton = new Rectangle(300, 300, 200, 50);
        Rectangle nextLevelButton = new Rectangle(290, 300, 200, 50);
        Rectangle quitButton = new Rectangle(300, 300, 200, 50);
        Rectangle continueButton = new Rectangle(300, 370, 200, 50);

        // Pause- ja settings-valikon napit
        Rectangle resumeButton = new Rectangle(300, 250, 200, 50);
        Rectangle settingsButton = new Rectangle(300, 320, 200, 50);
        Rectangle backButton = new Rectangle(300, 400, 200, 50);

        static void Main(string[] args)
        {
            Program game = new Program();
            game.Run();
        }

        void CreateLevel(int level)
        {
            asteroids.Clear();

            int baseAsteroids = level switch
            {
                1 => 2,
                2 => 4,
                3 => 6,
                _ => 0
            };

            int asteroidCount = difficulty switch
            {
                0 => baseAsteroids,       
                1 => baseAsteroids + 2,    
                2 => baseAsteroids + 4,    
                _ => baseAsteroids
            };

            for (int i = 0; i < asteroidCount; i++)
            {
                Vector2 pos;
                do
                {
                    pos = new Vector2(rand.Next(100, 700), rand.Next(100, 500));
                }
                while (Vector2.Distance(pos, playerRocket.transform.position) < 150);

                Vector2 vel = new Vector2(rand.Next(-100, 100), rand.Next(-100, 100));
                asteroids.Add(new Asteroids(pos, vel, asteroidTexture, 1.0f));
            }
        }

        public void Run()
        {
            Raylib.InitWindow(800, 650, "ASTEROIDS");
            Raylib.SetTargetFPS(60);

            playerTexture = Raylib.LoadTexture("playerShip1_red.png");
            playerRocket = new Rocket(new Vector2(Raylib.GetScreenWidth() / 2f, Raylib.GetScreenHeight() / 2f), playerTexture);
            bulletTexture = Raylib.LoadTexture("laserRed07.png");
            asteroidTexture = Raylib.LoadTexture("meteorBrown_big1.png");

            playerRocket.spriteWidth = playerTexture.Width;

            CreateLevel(level);

            while (!Raylib.WindowShouldClose())
            {
                float deltaTime = Raylib.GetFrameTime();
                playerDamageCooldown -= deltaTime;

                if (startMenu)
                {
                    Raylib.BeginDrawing();
                    Raylib.ClearBackground(Color.Black);
                    Raylib.DrawText("Welcome to Asteroids", 200, 200, 40, Color.White);
                    Raylib.DrawRectangleRec(startButton, Color.Gray);
                    Raylib.DrawText("START", 350, 310, 30, Color.Black);
                    Raylib.EndDrawing();

                    if (Raylib.IsMouseButtonPressed(MouseButton.Left) &&
                        Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), startButton))
                    {
                        startMenu = false;
                    }
                    continue;
                }

                // PAUSE KEY
                if (!startMenu && !gameOver && !levelCompleted && !gameWon && !settingsMenu)
                {
                    if (Raylib.IsKeyPressed(KeyboardKey.P) || Raylib.IsKeyPressed(KeyboardKey.Escape))
                    {
                        pauseMenu = !pauseMenu;
                    }
                }

                // PAUSE MENU
                if (pauseMenu)
                {
                    Raylib.BeginDrawing();
                    Raylib.ClearBackground(Color.Black);

                    Raylib.DrawText("GAME PAUSED", 280, 150, 40, Color.White);

                    Raylib.DrawRectangleRec(resumeButton, Color.Gray);
                    Raylib.DrawText("RESUME", 340, 260, 25, Color.Black);

                    Raylib.DrawRectangleRec(settingsButton, Color.Gray);
                    Raylib.DrawText("SETTINGS", 330, 330, 25, Color.Black);

                    Raylib.DrawRectangleRec(backButton, Color.Gray);
                    Raylib.DrawText("MAIN MENU", 330, 410, 25, Color.Black);

                    Raylib.EndDrawing();

                    if (Raylib.IsMouseButtonPressed(MouseButton.Left))
                    {
                        Vector2 mouse = Raylib.GetMousePosition();
                        if (Raylib.CheckCollisionPointRec(mouse, resumeButton))
                        {
                            pauseMenu = false;
                        }
                        else if (Raylib.CheckCollisionPointRec(mouse, settingsButton))
                        {
                            settingsMenu = true;
                            pauseMenu = false;
                        }
                        else if (Raylib.CheckCollisionPointRec(mouse, backButton))
                        {
                            startMenu = true;
                            pauseMenu = false;
                            bullets.Clear();
                            asteroids.Clear();
                            level = 1;
                            lives = 5;
                            CreateLevel(level);
                            playerRocket.transform.position = new Vector2(Raylib.GetScreenWidth() / 2f, Raylib.GetScreenHeight() / 2f);
                            playerRocket.transform.velocity = Vector2.Zero;
                        }
                    }
                    continue;
                }

                // SETTINGS MENU
                if (settingsMenu)
                {
                    Raylib.BeginDrawing();
                    Raylib.ClearBackground(Color.DarkGray);

                    Raylib.DrawText("SETTINGS", 320, 120, 40, Color.White);

                    // Difficulty
                    Raylib.DrawText("Difficulty:", 250, 200, 20, Color.White);
                    string diffText = difficulty switch
                    {
                        0 => "Easy",
                        1 => "Normal",
                        2 => "Hard",
                        _ => "Normal"
                    };
                    Raylib.DrawText(diffText, 400, 200, 20, Color.Yellow);

                    if (Raylib.IsKeyPressed(KeyboardKey.Left))
                        difficulty = (difficulty + 2) % 3;
                    if (Raylib.IsKeyPressed(KeyboardKey.Right))
                        difficulty = (difficulty + 1) % 3;

                    Raylib.DrawText("Volume:", 250, 260, 20, Color.White);
                    Raylib.DrawRectangle(400, 265, 100, 10, Color.DarkGray);
                    Raylib.DrawRectangle(400, 265, volume, 10, Color.Green);
                    Raylib.DrawText(volume.ToString(), 520, 255, 20, Color.White);

                    if (Raylib.IsKeyDown(KeyboardKey.Up) && volume < 100) volume++;
                    if (Raylib.IsKeyDown(KeyboardKey.Down) && volume > 0) volume--;

                    Raylib.DrawRectangleRec(backButton, Color.Gray);
                    Raylib.DrawText("BACK", 360, 410, 25, Color.Black);

                    Raylib.EndDrawing();

                    if (Raylib.IsMouseButtonPressed(MouseButton.Left))
                    {
                        Vector2 mouse = Raylib.GetMousePosition();
                        if (Raylib.CheckCollisionPointRec(mouse, backButton))
                        {
                            settingsMenu = false;
                            pauseMenu = true;
                        }
                    }
                    continue;
                }

                if (!gameOver && !levelCompleted)
                {
                    playerRocket.Update(deltaTime);

                    if (Raylib.IsKeyPressed(KeyboardKey.Space))
                    {
                        Vector2 dir = playerRocket.GetDirection();
                        Vector2 startPos = playerRocket.GetBulletSpawnPosition();
                        bullets.Add(new Bullet(startPos, dir, bulletTexture));
                    }

                    for (int i = bullets.Count - 1; i >= 0; i--)
                    {
                        bullets[i].Update(deltaTime);
                        if (!bullets[i].isAlive)
                            bullets.RemoveAt(i);
                    }

                    List<Asteroids> newAsteroids = new List<Asteroids>();
                    for (int i = bullets.Count - 1; i >= 0; i--)
                    {
                        for (int j = asteroids.Count - 1; j >= 0; j--)
                        {
                            if (Raylib.CheckCollisionCircles(bullets[i].transform.position, bullets[i].Radius,
                                                             asteroids[j].transform.position, asteroids[j].Radius))
                            {
                                bullets[i].isAlive = false;
                                score += (int)(100 * asteroids[j].size);

                                if (asteroids[j].size > 0.25f)
                                {
                                    for (int k = 0; k < 2; k++)
                                    {
                                        float angle = (float)(rand.NextDouble() * Math.PI * 2);
                                        Vector2 dir = new Vector2(MathF.Cos(angle), MathF.Sin(angle));
                                        Vector2 pos = asteroids[j].transform.position;
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
                        asteroid.transform.Move();

                        if (Raylib.CheckCollisionCircles(asteroid.transform.position, asteroid.Radius,
                                                         playerRocket.transform.position, playerRocket.Radius))
                        {
                            if (playerDamageCooldown <= 0f)
                            {
                                lives--;
                                playerDamageCooldown = 1f;

                                playerRocket.transform.position = new Vector2(Raylib.GetScreenWidth() / 2f, Raylib.GetScreenHeight() / 2f);
                                playerRocket.transform.velocity = Vector2.Zero;

                                if (lives <= 0)
                                    gameOver = true;
                            }
                        }
                    }

                    if (asteroids.Count == 0)
                    {
                        levelCompleted = true;
                        if (level == maxLevel)
                            gameWon = true;
                    }
                }

                if (gameOver)
                {
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
                            playerRocket.transform.position = new Vector2(Raylib.GetScreenWidth() / 2f, Raylib.GetScreenHeight() / 2f);
                            playerRocket.transform.velocity = Vector2.Zero;
                        }
                    }
                }

                else if (levelCompleted && !gameWon)
                {
                    if (Raylib.IsMouseButtonPressed(MouseButton.Left))
                    {
                        Vector2 mouse = Raylib.GetMousePosition();
                        if (Raylib.CheckCollisionPointRec(mouse, nextLevelButton))
                        {
                            level++;
                            CreateLevel(level);
                            bullets.Clear();
                            playerRocket.transform.position = new Vector2(Raylib.GetScreenWidth() / 2f, Raylib.GetScreenHeight() / 2f);
                            playerRocket.transform.velocity = Vector2.Zero;
                            levelCompleted = false;
                        }
                    }
                }

                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.Black);

                if (gameOver)
                {
                    Raylib.DrawText("GAME OVER", 280, 200, 40, Color.Red);
                    Raylib.DrawRectangleRec(quitButton, Color.Gray);
                    Raylib.DrawRectangleRec(continueButton, Color.Gray);
                    Raylib.DrawText("QUIT", 370, 310, 30, Color.Black);
                    Raylib.DrawText("CONTINUE", 330, 380, 30, Color.Black);
                }
                else if (levelCompleted && !gameWon)
                {
                    Raylib.DrawText("LEVEL COMPLETE!", 220, 200, 40, Color.Yellow);
                    Raylib.DrawRectangleRec(nextLevelButton, Color.Gray);
                    Raylib.DrawText("NEXT LEVEL", 310, 310, 25, Color.Black);
                }
                else if (gameWon)
                {
                    Raylib.DrawText("YOU WIN!", 300, 300, 50, Color.Green);
                }
                else
                {
                    playerRocket.Draw();

                    foreach (var bullet in bullets)
                        bullet.Draw();

                    foreach (var asteroid in asteroids)
                        asteroid.sprite.Draw(asteroid.transform.position, asteroid.transform.rotationRadians);

                    Raylib.DrawText($"Score: {score}", 10, 10, 20, Color.White);
                    Raylib.DrawText($"Level: {level}", 10, 30, 20, Color.White);
                    Raylib.DrawText($"Lives: {lives}", 10, 50, 20, Color.White);
                }

                Raylib.EndDrawing();
            }

            Raylib.UnloadTexture(playerTexture);
            Raylib.UnloadTexture(bulletTexture);
            Raylib.UnloadTexture(asteroidTexture);
            Raylib.CloseWindow();
        }
    }
}




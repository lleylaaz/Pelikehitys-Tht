using Raylib_cs;
using System.Collections.Generic;
using System.Numerics;

namespace ASTEROIDS
{
    internal class Program
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

        bool startMenu = true;
        bool gameOver = false;
        bool gameWon = false;
        bool levelCompleted = false;

        float playerDamageCooldown = 0f; // uusi cooldown-väli

        Rectangle startButton = new Rectangle(300, 300, 200, 50);
        Rectangle nextLevelButton = new Rectangle(290, 300, 200, 50);
        Rectangle quitButton = new Rectangle(300, 300, 200, 50);
        Rectangle continueButton = new Rectangle(300, 370, 200, 50);

        static void Main(string[] args)
        {
            Program game = new Program();
            game.Run();
        }

        void CreateLevel(int level)
        {
            asteroids.Clear();
            int asteroidCount = level switch
            {
                1 => 2,
                2 => 4,
                3 => 6,
                _ => 0
            };

            for (int i = 0; i < asteroidCount; i++)
            {
                Vector2 pos;
                // varmista ettei liian lähelle pelaajaa
                do
                {
                    pos = new Vector2(rand.Next(100, 700), rand.Next(100, 500));
                }
                while (Vector2.Distance(pos, playerRocket.transform.position) < 150);

                Vector2 vel = new Vector2(rand.Next(-100, 100), rand.Next(-100, 100));
                asteroids.Add(new Asteroids(pos, vel, asteroidTexture, 1.0f));
            }
        }

        bool CheckCollision(Vector2 pos1, float radius1, Vector2 pos2, float radius2)
        {
            return Vector2.Distance(pos1, pos2) < radius1 + radius2;
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
                playerDamageCooldown -= deltaTime; // vähennä cooldownia

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

                if (!gameOver && !levelCompleted)
                {
                    // Päivitä pelaaja
                    playerRocket.Update(deltaTime);

                    // Ammuksen luonti
                    if (Raylib.IsKeyPressed(KeyboardKey.Space))
                    {
                        Vector2 dir = playerRocket.GetDirection();
                        Vector2 startPos = playerRocket.GetBulletSpawnPosition();
                        bullets.Add(new Bullet(startPos, dir, bulletTexture));
                    }

                    // Päivitä ammukset ja poista kuolleet
                    for (int i = bullets.Count - 1; i >= 0; i--)
                    {
                        bullets[i].Update(deltaTime);
                        if (!bullets[i].isAlive)
                            bullets.RemoveAt(i);
                    }

                    // Törmäystarkistus ammus vs asteroidi
                    List<Asteroids> newAsteroids = new List<Asteroids>();
                    for (int i = bullets.Count - 1; i >= 0; i--)
                    {
                        for (int j = asteroids.Count - 1; j >= 0; j--)
                        {
                            if (CheckCollision(bullets[i].transform.position, bullets[i].Radius,
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

                    // Pelaaja vs asteroidi
                    foreach (Asteroids asteroid in asteroids)
                    {
                        asteroid.transform.Move();

                        if (CheckCollision(asteroid.transform.position, asteroid.Radius,
                                           playerRocket.transform.position, playerRocket.Radius))
                        {
                            if (playerDamageCooldown <= 0f) // vahinkoa vain jos cooldown nolla
                            {
                                lives--;
                                playerDamageCooldown = 1f; // 1 sekunnin suoja-aika

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

                // Game over menu
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
                // Next level menu
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

                // Piirto
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.Black);

                if (startMenu)
                {
                    Raylib.DrawText("Welcome to Asteroids", 320, 200, 30, Color.White);
                    Raylib.DrawRectangleRec(startButton, Color.Gray);
                    Raylib.DrawText("START", 360, 310, 20, Color.Black);
                }
                else if (gameOver)
                {
                    Raylib.DrawText("GAME OVER", 280, 200, 40, Color.Red);
                    Raylib.DrawRectangleRec(quitButton, Color.Gray);
                    Raylib.DrawRectangleRec(continueButton, Color.Gray);
                    Raylib.DrawText("QUIT", 370, 310, 30, Color.Black);
                    Raylib.DrawText("CONTINUE", 33-0, 380, 30, Color.Black);
                }
                else if (levelCompleted && !gameWon)
                {
                    Raylib.DrawText("LEVEL COMPLETE!", 270, 200, 30, Color.Yellow);
                    Raylib.DrawRectangleRec(nextLevelButton, Color.Gray);
                    Raylib.DrawText("NEXT LEVEL", 310, 310, 20, Color.Black);
                }
                else if (gameWon)
                {
                    Raylib.DrawText("YOU WIN!", 330, 200, 30, Color.Green);
                    Raylib.DrawText("QUIT", 370, 310, 30, Color.Black);
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

            // Vapauta resurssit
            Raylib.UnloadTexture(playerTexture);
            Raylib.UnloadTexture(bulletTexture);
            Raylib.UnloadTexture(asteroidTexture);
            Raylib.CloseWindow();
        }
    }
}



using System.Numerics;
using Raylib_cs;

namespace ASTEROIDS
{
    class Program
    {
        static GameState currentState = GameState.MainMenu;

        static MainMenu mainMenu;
        static PauseMenu pauseMenu;
        static GameOverMenu gameOverMenu;
        static VictoryMenu victoryMenu;

        static Rocket rocket;
        static List<Bullet> bullets = new();
        static List<Asteroids> asteroids = new();

        static Texture2D rocketTexture;
        static Texture2D bulletTexture;
        static Texture2D asteroidTexture;

        static void Main()
        {
            Raylib.InitWindow(800, 600, "Asteroids");
            Raylib.SetTargetFPS(60);

            rocketTexture = Raylib.LoadTexture("playerShip1_red.png");
            bulletTexture = Raylib.LoadTexture("laserRed07.png");
            asteroidTexture = Raylib.LoadTexture("meteorBrown_big1.png");

            mainMenu = new MainMenu();
            mainMenu.OnStartGame += () => { StartGame(); currentState = GameState.Playing; };
            mainMenu.OnExitGame += () => Raylib.CloseWindow();

            pauseMenu = new PauseMenu();
            pauseMenu.OnResume += () => currentState = GameState.Playing;
            pauseMenu.OnExit += () => currentState = GameState.MainMenu;

            gameOverMenu = new GameOverMenu();
            gameOverMenu.OnRestart += () => { StartGame(); currentState = GameState.Playing; };
            gameOverMenu.OnExit += () => currentState = GameState.MainMenu;

            victoryMenu = new VictoryMenu();
            victoryMenu.OnRestart += () => { StartGame(); currentState = GameState.Playing; };
            victoryMenu.OnExit += () => currentState = GameState.MainMenu;

            while (!Raylib.WindowShouldClose())
            {
                float dt = Raylib.GetFrameTime();

                // ESC avaa/poistaa pause-menun
                if (currentState == GameState.Playing && Raylib.IsKeyPressed(KeyboardKey.Escape))
                    currentState = GameState.Paused;
                else if (currentState == GameState.Paused && Raylib.IsKeyPressed(KeyboardKey.Escape))
                    currentState = GameState.Playing;

                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.Black);

                switch (currentState)
                {
                    case GameState.MainMenu:
                        mainMenu.Draw();
                        break;

                    case GameState.Playing:
                        UpdateGame(dt);
                        DrawGame();
                        break;

                    case GameState.Paused:
                        pauseMenu.Draw();
                        break;

                    case GameState.GameOver:
                        gameOverMenu.Draw();
                        break;

                    case GameState.Victory:
                        victoryMenu.Draw();
                        break;
                }

                Raylib.EndDrawing();
            }

            Raylib.CloseWindow();
        }

        static void StartGame()
        {
            rocket = new Rocket(new Vector2(400, 300), rocketTexture);
            bullets.Clear();
            asteroids.Clear();

            // Luo muutama asteroidi
            asteroids.Add(new Asteroids(new Vector2(100, 100), new Vector2(50, 20), asteroidTexture, 1f));
            asteroids.Add(new Asteroids(new Vector2(700, 400), new Vector2(-40, -30), asteroidTexture, 1f));
        }

        static void UpdateGame(float dt)
        {
            rocket.Update(dt);

            // Ampuminen
            if (Raylib.IsKeyPressed(KeyboardKey.Space))
            {
                Vector2 spawn = rocket.GetBulletSpawnPosition();
                bullets.Add(new Bullet(spawn, rocket.GetDirection(), bulletTexture));
            }

            foreach (var b in bullets) b.Update(dt);
            bullets.RemoveAll(b => !b.isAlive);

            // Törmäykset
            foreach (var b in bullets)
            {
                foreach (var a in asteroids)
                {
                    if (b.collision.CheckCollision(a.collision))
                    {
                        b.isAlive = false;
                        // yksinkertaistetaan: peli päättyy voittoon kun 1 osuma
                        currentState = GameState.Victory;
                        return;
                    }
                }
            }

            foreach (var a in asteroids)
            {
                if (rocket.collision.CheckCollision(a.collision))
                {
                    currentState = GameState.GameOver;
                    return;
                }
            }
        }

        static void DrawGame()
        {
            rocket.Draw();
            foreach (var b in bullets) b.Draw();
            foreach (var a in asteroids) a.sprite.Draw(a.transform.position, a.transform.rotationRadians);
        }
    }
}

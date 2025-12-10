using System.Numerics;
using Raylib_cs;

namespace ASTEROIDS
{
    public class Program
    {
        public GameState currentState = GameState.MainMenu;
        public int level = 1;

        public Rocket rocket;
        public List<Bullet> bullets = new();
        public List<Asteroids> asteroids = new();

        public Texture2D rocketTexture;
        public Texture2D bulletTexture;
        public Texture2D asteroidsTexture;
        public Texture2D fireTexture;

        public MainMenu mainMenu;
        public PauseMenu pauseMenu;
        public GameOverMenu gameOverMenu;
        public VictoryMenu victoryMenu;

        public float hitCooldown = 1.0f;
        private float lastHitTime = -1f;

        public float Score = 0f; // aluksi 0
        public static void Main()
        {
            Program game = new Program();
            game.Run();
        }

        public void Run()
        {
            Raylib.InitWindow(800, 600, "Asteroids");
            Raylib.SetTargetFPS(60);

            rocketTexture = Raylib.LoadTexture("playerShip1_red.png");
            bulletTexture = Raylib.LoadTexture("laserRed07.png");
            asteroidsTexture = Raylib.LoadTexture("meteorBrown_big1.png");
            fireTexture = Raylib.LoadTexture("fire15.png");

            mainMenu = new MainMenu();
            pauseMenu = new PauseMenu();
            gameOverMenu = new GameOverMenu();
            victoryMenu = new VictoryMenu();

            while (!Raylib.WindowShouldClose())
            {
                float dt = Raylib.GetFrameTime();

                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.Black);

                switch (currentState)
                {
                    case GameState.MainMenu:
                        currentState = mainMenu.Update();
                        mainMenu.Draw();
                        if (currentState == GameState.Playing)
                            StartGame();
                        break;

                    case GameState.Playing:
                        UpdateGame(dt);
                        DrawGame();
                        if (Raylib.IsKeyPressed(KeyboardKey.P))
                            currentState = GameState.Paused;
                        break;

                    case GameState.Paused:
                        currentState = pauseMenu.Update();
                        pauseMenu.Draw();
                        break;

                    case GameState.GameOver:
                        currentState = gameOverMenu.Update(this);
                        gameOverMenu.Draw(this); // välitetään Score
                        break;

                    case GameState.Victory:
                        currentState = victoryMenu.Update(this);
                        victoryMenu.Draw(this); // välitetään Score
                        break;
                }

                Raylib.EndDrawing();
            }
        }

        public void StartGame()
        {
            bullets.Clear();
            asteroids.Clear();
            Score = 0f;

            if (rocket == null)
                rocket = new Rocket(new Vector2(400, 300), rocketTexture, fireTexture);

            else
            {
                rocket.Transform.position = new Vector2(400, 300);
                rocket.Transform.velocity = Vector2.Zero;
                rocket.Transform.acceleration = Vector2.Zero;
                rocket.Transform.direction = new Vector2(0, -1);
                rocket.Transform.rotationRadians = 0f;
                rocket.HP = 100;
            }

            int asteroidCount = level * 2;
            for (int i = 0; i < asteroidCount; i++)
            {
                Vector2 spawnPos;
                do
                {
                    spawnPos = new Vector2(RandomFloat(50, 750), RandomFloat(50, 550));
                }
                while (Vector2.Distance(spawnPos, rocket.Transform.position) < 150f);

                asteroids.Add(new Asteroids(spawnPos, RandomVelocity(), asteroidsTexture, 1f));
            }
        }

        Vector2 RandomVelocity()
        {
            return new Vector2(RandomFloat(-100, 100), RandomFloat(-100, 100));
        }

        void UpdateGame(float dt)
        {
            rocket.UpdateInput(dt);    // Käsitellään pelaajan syöte
            rocket.Transform.Move();   // Liikuta rakettia transformin mukaan

            foreach (var a in asteroids)
                a.Transform.Move();

            // Ampuminen
            if (Raylib.IsKeyPressed(KeyboardKey.Space))
            {
                bullets.Add(new Bullet(rocket.GetBulletSpawnPosition(), rocket.Transform.rotationRadians, bulletTexture));
            }

            foreach (var b in bullets)
                b.Update(dt);

            bullets.RemoveAll(b => !b.IsAlive);

            // Ammuksen ja asteroidin törmäys
            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                for (int j = asteroids.Count - 1; j >= 0; j--)
                {
                    float dist = Vector2.Distance(bullets[i].Transform.position, asteroids[j].Transform.position);
                    if (dist < asteroids[j].Radius)
                    {
                        bullets[i].IsAlive = false;

                        Asteroids hit = asteroids[j];
                        asteroids.RemoveAt(j);

                        // Pisteytys asteroidin koon mukaan
                        if (hit.Size > 0.66f)         // iso
                            Score += 100f;
                        else if (hit.Size > 0.33f)    // keskikokoinen
                            Score += 50f;
                        else                          // pieni
                            Score += 25f;

                        // Hajotus pienemmiksi
                        if (hit.Size > 0.25f)
                        {
                            float newSize = hit.Size * 0.5f;

                            asteroids.Add(new Asteroids(
                                hit.Transform.position,
                                RandomVelocity(),
                                asteroidsTexture,
                                newSize
                            ));
                            asteroids.Add(new Asteroids(
                                hit.Transform.position,
                                RandomVelocity(),
                                asteroidsTexture,
                                newSize
                            ));
                        }

                        break;
                    }
                }
            }


            if (asteroids.Count == 0)
            {
                level++;
                currentState = GameState.Victory;
            }

            float timeNow = (float)Raylib.GetTime();

            foreach (var a in asteroids)
            {
                float dist = Vector2.Distance(rocket.Transform.position, a.Transform.position);
                if (dist < a.Radius + 20f)
                {
                    if (timeNow - lastHitTime >= hitCooldown)
                    {
                        lastHitTime = timeNow;
                        int damage = 0;
                        if (a.Size > 0.66f) damage = 20;
                        else if (a.Size > 0.33f) damage = 10;
                        else damage = 5;

                        rocket.HP -= damage;

                        if (rocket.HP <= 0)
                        {
                            rocket.HP = 0;
                            currentState = GameState.GameOver;
                            return;
                        }
                    }
                }
            }
        }

        // Pelin piirto
        public void DrawGame()
        {
            foreach (var a in asteroids)
                a.Draw();

            foreach (var b in bullets)
                b.Draw();

            rocket.Draw();

            // HP bar
            int barWidth = 200;
            int hpBar = (int)(barWidth * (rocket.HP / 100f));

            Raylib.DrawRectangle(20, 20, barWidth, 20, Color.DarkGray);
            Raylib.DrawRectangle(20, 20, hpBar, 20, Color.Green);

            Raylib.DrawText($"HP: {rocket.HP}", 230, 20, 20, Color.White);
            Raylib.DrawText($"Score: {Score}", 20, 50, 20, Color.White);
            Raylib.DrawText($"Level: {level}", 650, 20, 20, Color.White);
        }

        public float RandomFloat(float min, float max)
        {
            return (float)Raylib.GetRandomValue(0, 10) / 10f * (max - min) + min;
        }
    }
}

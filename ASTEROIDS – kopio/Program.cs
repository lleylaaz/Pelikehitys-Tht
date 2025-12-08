using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;

namespace ASTEROIDS
{
    public class Program
    {
        // Pelin tila (valikko, pelaaminen, tauko, game over jne.)
        public GameState currentState = GameState.MainMenu;

        // Nykyinen leveli
        public int level = 1;

        // Pelaajan raketti
        public Rocket rocket;

        // Ammuslista
        public List<Bullet> bullets = new();

        // Asteroidilista
        public List<Asteroid> asteroids = new();

        // Tekstuurit
        public Texture2D rocketTexture;
        public Texture2D bulletTexture;
        public Texture2D asteroidTexture;

        // Valikot
        public MainMenu mainMenu;
        public PauseMenu pauseMenu;
        public GameOverMenu gameOverMenu;
        public VictoryMenu victoryMenu;

        // Pelaajan vahinkojen cooldown
        public float hitCooldown = 1.0f;
        private float lastHitTime = -1f;

        public static void Main()
        {
            Program game = new Program();
            game.Run();
        }

        public void Run()
        {
            // Ikkunan luonti
            Raylib.InitWindow(800, 600, "Asteroids");
            Raylib.SetTargetFPS(60);

            // Ladataan kuvat
            rocketTexture = Raylib.LoadTexture("playerShip1_red.png");
            bulletTexture = Raylib.LoadTexture("laserRed07.png");
            asteroidTexture = Raylib.LoadTexture("meteorBrown_big1.png");

            // Luodaan valikot
            mainMenu = new MainMenu();
            pauseMenu = new PauseMenu();
            gameOverMenu = new GameOverMenu();
            victoryMenu = new VictoryMenu();

            // Pelisilmukka
            while (!Raylib.WindowShouldClose())
            {
                float dt = Raylib.GetFrameTime();

                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.Black);

                // Pelitilojen käsittely
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
                        gameOverMenu.Draw();
                        break;

                    case GameState.Victory:
                        currentState = victoryMenu.Update(this);
                        victoryMenu.Draw();
                        break;
                }

                Raylib.EndDrawing();
            }
        }

        public void StartGame()
        {
            // Nollataan listat
            bullets.Clear();
            asteroids.Clear();

            // Luodaan raketti tai resetöidään se
            if (rocket == null)
                rocket = new Rocket(new Vector2(400, 300), rocketTexture);
            else
            {
                rocket.Position = new Vector2(400, 300);
                rocket.Rotation = 0f;
                rocket.Velocity = Vector2.Zero;
                rocket.HP = 100;
            }

            // Luodaan asteroidit
            int asteroidCount = level * 2;
            for (int i = 0; i < asteroidCount; i++)
            {
                Vector2 spawnPos;

                // Ei spawnata liian lähelle pelaajaa
                do
                {
                    spawnPos = new Vector2(RandomFloat(50, 750), RandomFloat(50, 550));
                }
                while (Vector2.Distance(spawnPos, rocket.Position) < 150f);

                asteroids.Add(new Asteroid(
                    spawnPos,
                    RandomVelocity(),
                    asteroidTexture,
                    1f
                ));
            }
        }

        // Arpoo asteroidin nopeuden
        Vector2 RandomVelocity()
        {
            return new Vector2(RandomFloat(-100, 100), RandomFloat(-100, 100));
        }

        // Päivittää pelin logiikan
        void UpdateGame(float dt)
        {
            // Päivitetään raketti ja asteroidit
            rocket.Update(dt);
            foreach (var a in asteroids) a.Update(dt);

            // Ammus
            if (Raylib.IsKeyPressed(KeyboardKey.Space))
            {
                bullets.Add(new Bullet(rocket.GetBulletSpawnPosition(), rocket.Rotation, bulletTexture));
            }

            // Päivitä ammukset
            foreach (var b in bullets) b.Update(dt);

            // Poista kuolleet ammukset
            bullets.RemoveAll(b => !b.IsAlive);

            // Ammuksen ja asteroidin törmäykset
            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                for (int j = asteroids.Count - 1; j >= 0; j--)
                {
                    float dist = Vector2.Distance(bullets[i].Position, asteroids[j].Position);

                    if (dist < asteroids[j].Radius)
                    {
                        bullets[i].IsAlive = false;

                        Asteroid hit = asteroids[j];
                        asteroids.RemoveAt(j);

                        // Suuret asteroidit hajoavat pienemmiksi
                        if (hit.Size > 0.25f)
                        {
                            float newSize = hit.Size * 0.5f;

                            asteroids.Add(new Asteroid(hit.Position, RandomVelocity(), asteroidTexture, newSize));
                            asteroids.Add(new Asteroid(hit.Position, RandomVelocity(), asteroidTexture, newSize));
                        }

                        break;
                    }
                }
            }

            // Jos kaikki asteroidit on tuhottu = voitto
            if (asteroids.Count == 0)
            {
                level++;
                currentState = GameState.Victory;
            }

            // Asteroidit osuvat pelaajaan (HP ja cooldown)
            float timeNow = (float)Raylib.GetTime();

            foreach (var a in asteroids)
            {
                float dist = Vector2.Distance(rocket.Position, a.Position);

                // Osuma asteroidin säteen mukaan
                if (dist < a.Radius + 20f)
                {
                    if (timeNow - lastHitTime >= hitCooldown)
                    {
                        lastHitTime = timeNow;

                        // Vahinko asteroidin koosta riippuen
                        int damage = 0;
                        if (a.Size > 0.66f) damage = 20;
                        else if (a.Size > 0.33f) damage = 10;
                        else damage = 5;

                        rocket.HP -= damage;

                        // Peli loppuu jos HP = 0
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

        // Piirtää pelin
        public void DrawGame()
        {
            foreach (var a in asteroids) a.Draw();
            foreach (var b in bullets) b.Draw();
            rocket.Draw();

            // HP-palkki
            int barWidth = 200;
            int hpBar = (int)(barWidth * (rocket.HP / 100f));

            Raylib.DrawRectangle(20, 20, barWidth, 20, Color.DarkGray);
            Raylib.DrawRectangle(20, 20, hpBar, 20, Color.Green);

            Raylib.DrawText($"HP: {rocket.HP}", 230, 20, 20, Color.White);
            Raylib.DrawText($"Level: {level}", 650, 20, 20, Color.White);
        }

        // Palauttaa satunnaisen luvun
        public float RandomFloat(float min, float max)
        {
            return (float)Raylib.GetRandomValue(0, 10) / 10f * (max - min) + min;
        }
    }
}

using System.Numerics;
using Raylib_cs;

// Käytin välillä ChatGPT:tä johonkin kohtiin.
public class Menu
{
    private GameState nextState;
    private const int startButtonX = 350;
    private const int startButtonY = 280;
    private const int buttonWidth = 150;
    private const int buttonHeight = 50;

    private const int quitButtonX = 350;
    private const int quitButtonY = 350;

    public Menu()
    {
        nextState = GameState.MainMenu;
    }

    public void Update(ref GameState gameState, int blueScore, int redScore)
    {
        nextState = gameState;
        Vector2 mouse = Raylib.GetMousePosition();

        bool startHover = mouse.X > startButtonX && mouse.X < startButtonX + buttonWidth &&
                          mouse.Y > startButtonY && mouse.Y < startButtonY + buttonHeight;

        if (startHover && Raylib.IsMouseButtonPressed(MouseButton.Left))
        {
            if (gameState == GameState.MainMenu || gameState == GameState.GameOver)
            {
                nextState = GameState.Playing;
            }
        }

        bool quitHover = mouse.X > quitButtonX && mouse.X < quitButtonX + buttonWidth &&
                         mouse.Y > quitButtonY && mouse.Y < quitButtonY + buttonHeight;

        if (quitHover && Raylib.IsMouseButtonPressed(MouseButton.Left))
        {
            Raylib.CloseWindow();
            System.Environment.Exit(0);
        }

        gameState = nextState;
    }

    public void Draw(GameState gameState, int blueScore, int redScore)
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.Black);

        Vector2 mouse = Raylib.GetMousePosition();

        bool startHover = mouse.X > startButtonX && mouse.X < startButtonX + buttonWidth &&
                          mouse.Y > startButtonY && mouse.Y < startButtonY + buttonHeight;

        Color startColor = startHover ? Color.DarkGray : Color.Gray;

        bool quitHover = mouse.X > quitButtonX && mouse.X < quitButtonX + buttonWidth &&
                         mouse.Y > quitButtonY && mouse.Y < quitButtonY + buttonHeight;

        Color quitColor = quitHover ? Color.DarkGray : Color.Gray;

        switch (gameState)
        {
            case GameState.MainMenu:
                Raylib.DrawText("Welcome to TANKS!", 200, 150, 50, Color.White);
                Raylib.DrawRectangle(startButtonX, startButtonY, buttonWidth, buttonHeight, startColor);
                Raylib.DrawText("START", 365, 285, 30, Color.Black);

                Raylib.DrawRectangle(quitButtonX, quitButtonY, buttonWidth, buttonHeight, quitColor);
                Raylib.DrawText("QUIT", 380, 355, 30, Color.Black);
                break;

            case GameState.GameOver:
                string winner = blueScore > redScore ? "Blue Tank Wins!" : "Red Tank Wins!";
                Color winnerColor = blueScore > redScore ? Color.Blue : Color.Red;
                Raylib.DrawText(winner, 250, 150, 40, winnerColor);

                Raylib.DrawRectangle(startButtonX, startButtonY, buttonWidth, buttonHeight, startColor);
                Raylib.DrawText("REMATCH", 360, 285, 25, Color.Black);

                Raylib.DrawRectangle(quitButtonX, quitButtonY, buttonWidth, buttonHeight, quitColor);
                Raylib.DrawText("QUIT", 375, 355, 30, Color.Black);
                break;
        }

        Raylib.EndDrawing();
    }
}

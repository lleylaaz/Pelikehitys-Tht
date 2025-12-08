using ASTEROIDS;
using Raylib_cs;

public class GameOverMenu
{
    // Yritä uudelleen
    UIButton retryButton = new UIButton(300, 250, 200, 60, "Retry");

    // Takaisin päävalikkoon
    UIButton mainMenuButton = new UIButton(300, 330, 200, 60, "Main Menu");

    public GameState Update(Program game)
    {
        // Aloita uudelleen nykyinen taso
        if (retryButton.IsClicked())
        {
            game.StartGame();
            return GameState.Playing;
        }

        // Palaa päävalikkoon ja resetoi level
        if (mainMenuButton.IsClicked())
        {
            game.level = 1;
            game.StartGame();
            return GameState.MainMenu;
        }

        // Pysyy Game Over tilassa
        return GameState.GameOver;
    }

    public void Draw()
    {
        // Game Over -teksti
        Raylib.DrawText("GAME OVER", 220, 120, 60, Color.Red);

        // Painikkeet
        retryButton.Draw();
        mainMenuButton.Draw();
    }
}

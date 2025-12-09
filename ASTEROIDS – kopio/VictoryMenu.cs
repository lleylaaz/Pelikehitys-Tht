using ASTEROIDS;
using Raylib_cs;

public class VictoryMenu
{
    // Napit: seuraava taso ja päävalikko
    UIButton nextButton = new UIButton(300, 250, 200, 60, "Next Level");
    UIButton mainMenuButton = new UIButton(300, 330, 200, 60, "Main Menu");

    public GameState Update(Program game)
    {
        // Seuraava taso -nappi
        if (nextButton.IsClicked())
        {
            game.StartGame();
            return GameState.Playing;
        }

        // Päävalikkoon
        if (mainMenuButton.IsClicked())
        {
            game.level = 1;
            game.StartGame();
            return GameState.MainMenu;
        }

        return GameState.Victory;
    }

    public void Draw(Program game)
    {
        // Teksti
        Raylib.DrawText("VICTORY!", 220, 120, 60, Color.Green);
        Raylib.DrawText($"Score: {game.Score}", 330, 200, 30, Color.White);
        // Piirrä napit
        nextButton.Draw();
        mainMenuButton.Draw();
    }
}

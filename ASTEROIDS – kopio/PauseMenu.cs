using ASTEROIDS;
using Raylib_cs;

public class PauseMenu
{
    // Resume-painike
    UIButton resumeButton = new UIButton(300, 250, 200, 60, "Resume");

    // Palaa päävalikkoon
    UIButton mainMenuButton = new UIButton(300, 330, 200, 60, "Main Menu");

    public GameState Update()
    {
        // Jatkaa peliä
        if (resumeButton.IsClicked())
            return GameState.Playing;

        // Siirtyy päävalikkoon
        if (mainMenuButton.IsClicked())
            return GameState.MainMenu;

        // Pysyy pausessa
        return GameState.Paused;
    }

    public void Draw()
    {
        // Pauseteksti
        Raylib.DrawText("PAUSED", 270, 120, 60, Color.White);

        // Painikkeet
        resumeButton.Draw();
        mainMenuButton.Draw();
    }
}

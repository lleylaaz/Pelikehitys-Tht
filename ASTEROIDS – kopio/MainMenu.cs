using ASTEROIDS;
using Raylib_cs;

public class MainMenu
{
    // Aloitusnappi
    UIButton startButton = new UIButton(300, 250, 200, 60, "Start Game");

    // Lopetusnappi
    UIButton quitButton = new UIButton(300, 330, 200, 60, "Quit");

    public GameState Update()
    {
        // Aloita peli
        if (startButton.IsClicked())
            return GameState.Playing;

        // Sulje peli
        if (quitButton.IsClicked())
            Raylib.CloseWindow();

        // Pysy valikossa
        return GameState.MainMenu;
    }

    public void Draw()
    {
        // Pelin otsikkoteksti
        Raylib.DrawText("ASTEROIDS", 200, 100, 60, Color.White);

        // Painikkeet
        startButton.Draw();
        quitButton.Draw();
    }
}

using Raylib_cs;
using static RayGui;

namespace ASTEROIDS
{
    public class MainMenu
    {
        public event Action OnStartGame;
        public event Action OnExitGame;

        public void Draw()
        {
            GuiLabel(new Rectangle(320, 150, 200, 40), "ASTEROIDS");

            if (GuiButton(new Rectangle(320, 220, 200, 40), "Start Game"))
                OnStartGame?.Invoke();

            if (GuiButton(new Rectangle(320, 280, 200, 40), "Exit"))
                OnExitGame?.Invoke();
        }
    }
}


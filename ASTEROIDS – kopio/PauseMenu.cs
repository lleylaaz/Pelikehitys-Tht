using Raylib_cs;
using static RayGui;

namespace ASTEROIDS
{
    public class PauseMenu
    {
        public event Action OnResume;
        public event Action OnExit;

        public void Draw()
        {
            GuiLabel(new Rectangle(320, 150, 200, 40), "Game Paused");

            if (GuiButton(new Rectangle(320, 220, 200, 40), "Resume"))
                OnResume?.Invoke();

            if (GuiButton(new Rectangle(320, 280, 200, 40), "Exit to Main Menu"))
                OnExit?.Invoke();
        }
    }
}



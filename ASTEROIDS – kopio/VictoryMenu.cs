using Raylib_cs;
using static RayGui;

namespace ASTEROIDS
{
    public class VictoryMenu
    {
        public event Action OnRestart;
        public event Action OnExit;

        public void Draw()
        {
            GuiLabel(new Rectangle(320, 150, 200, 40), "You Win!");

            if (GuiButton(new Rectangle(320, 220, 200, 40), "Play Again"))
                OnRestart?.Invoke();

            if (GuiButton(new Rectangle(320, 280, 200, 40), "Exit to Main Menu"))
                OnExit?.Invoke();
        }
    }
}


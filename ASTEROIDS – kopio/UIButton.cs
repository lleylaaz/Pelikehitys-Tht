using Raylib_cs;
using System.Numerics;

public class UIButton
{
    public Rectangle Rect;     // Nappulan alue
    public string Text;        // Nappulan teksti
    public Color BGColor = Color.Gray;   // Taustaväri
    public Color TextColor = Color.White; // Tekstin väri

    public UIButton(float x, float y, float width, float height, string text)
    {
        Rect = new Rectangle(x, y, width, height);
        Text = text;
    }

    public bool IsClicked()
    {
        // Hakee hiiren sijainnin
        Vector2 mouse = Raylib.GetMousePosition();

        // Palauttaa true jos hiiri on napin päällä ja vasen hiiri klikataan
        return Raylib.IsMouseButtonPressed(MouseButton.Left)
            && Raylib.CheckCollisionPointRec(mouse, Rect);
    }

    public void Draw()
    {
        // Piirtää napin taustan        
        Raylib.DrawRectangleRec(Rect, BGColor);

        // Piirtää tekstin napin sisään
        Raylib.DrawText(
            Text,
            (int)(Rect.X + 10),
            (int)(Rect.Y + Rect.Height / 4),
            20,
            TextColor
        );
    }
}

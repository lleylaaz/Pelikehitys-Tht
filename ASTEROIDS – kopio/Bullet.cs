using System.Numerics;
using ASTEROIDS;
using Raylib_cs;

public class Bullet
{
    public TransformComponent Transform;
    public SpriteComponent Sprite;
    public bool IsAlive = true;
    private float speed = 500f; // bulletin nopeus pikseliä/s

    public Bullet(Vector2 position, float rotation, Texture2D texture)
    {
        Transform = new TransformComponent(position);
        Sprite = new SpriteComponent(texture);

        // Bullet lentää samaan suuntaan kuin raketti
        Transform.velocity = new Vector2(
            MathF.Sin(rotation),
            -MathF.Cos(rotation)
        ) * speed;
    }

    public void Update(float dt)
    {
        Transform.Move();
        // Lisäehto ruudun ulkopuolella kuolemiseen
        if (Transform.position.X < 0 || Transform.position.X > 800 ||
            Transform.position.Y < 0 || Transform.position.Y > 600)
            IsAlive = false;
    }
    public void Draw()
    {
        // Bullet pyörii kulman mukaan
        float rotation = MathF.Atan2(Transform.velocity.Y, Transform.velocity.X) + MathF.PI / 2f;
        Sprite.Draw(Transform.position, rotation);
    }
}

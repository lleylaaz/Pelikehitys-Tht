namespace ASTEROIDS
{
    // Abstrakti komponentti, josta kaikki pelin komponentit voivat periä
    public abstract class Component
    {
        // Onko komponentti käytössä
        public bool Enabled { get; set; } = true;

        // Päivitysmetodi, voi ylikirjoittaa perivissä luokissa
        public virtual void Update(float dt) { }

        // Piirto metodi, voi ylikirjoittaa perivissä luokissa
        public virtual void Draw() { }
    }
}

namespace Ovi.tehtävä
{
    internal class Program
    {
        enum OvenTila
        {
            Auki,
            Kiinni,
            Lukossa
        }
        enum Toiminto
        {
            Avaa,
            Lukitse,
            Avaa_Lukko,
            Sulje
        }
        static void Main(string[] args)
        {
            OvenTila tila = OvenTila.Auki;
            string[] toiminnot = Enum.GetNames<Toiminto>();
            Console.WriteLine("Valitse toiminto.");
            for(int i = 0;  i < toiminnot.Length; i++)
            {
                Console.WriteLine($"{i + 1}: {toiminnot[i]}");
            }

            string vastaus = Console.ReadLine();
            Toiminto valittu;
            for (int i = 0; i < toiminnot.Length; i++) 
            {
                if (toiminnot[i].ToLower() == vastaus.ToLower())
                {
                    valittu = (Toiminto)i;
                }
            }

            if (Enum.TryParse<Toiminto>(vastaus, out valittu)) 
            { 
                Console.WriteLine($"Valitsit {valittu}");
            }
            else 
            {
                Console.WriteLine("Epäkelpo toiminto.");
            }
        }
    }
}

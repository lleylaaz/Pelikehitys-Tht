namespace Ovi.tehtava
{
    internal class Program
    {
        // Määritellään oven tilat
        enum OvenTila
        {
            Auki,
            Kiinni,
            Lukossa
        }

        // Määritellään toiminnot
        enum Toiminto
        {
            Avaa,
            Lukitse,
            Avaa_Lukko,
            Sulje
        }

        static void Main(string[] args)
        {
            // Alustetaan oven tila
            OvenTila tila = OvenTila.Kiinni;

            while (true) // Ikuinen looppi
            {
                // Tulostetaan nykyinen tila
                Console.WriteLine($"\nOven tila: {tila}");
                Console.WriteLine("Valitse toiminto:");

                // Tulostetaan kaikki mahdolliset toiminnot
                string[] toiminnot = Enum.GetNames<Toiminto>();
                for (int i = 0; i < toiminnot.Length; i++)
                {
                    Console.WriteLine($"{i + 1}: {toiminnot[i]}");
                }

                // Luetaan käyttäjän syöte
                string vastaus = Console.ReadLine();
                Toiminto valittu;

                // Tarkistetaan, onko syöte kelvollinen
                if (Enum.TryParse<Toiminto>(vastaus, true, out valittu))
                {
                    Console.WriteLine($"Valitsit: {valittu}");

                    // Käsitellään valinta ja muutetaan oven tilaa
                    switch (valittu)
                    {
                        case Toiminto.Avaa:
                            if (tila == OvenTila.Kiinni)
                            {
                                tila = OvenTila.Auki;
                                Console.WriteLine("Ovi avattiin.");
                            }
                            else
                            {
                                Console.WriteLine("Ovea ei voi avata, ellei se ole kiinni.");
                            }
                            break;

                        case Toiminto.Sulje:
                            if (tila == OvenTila.Auki)
                            {
                                tila = OvenTila.Kiinni;
                                Console.WriteLine("Ovi suljettiin.");
                            }
                            else
                            {
                                Console.WriteLine("Ovea ei voi sulkea, ellei se ole auki.");
                            }
                            break;

                        case Toiminto.Lukitse:
                            if (tila == OvenTila.Kiinni)
                            {
                                tila = OvenTila.Lukossa;
                                Console.WriteLine("Ovi lukittiin.");
                            }
                            else
                            {
                                Console.WriteLine("Ovea ei voi lukita, ellei se ole kiinni.");
                            }
                            break;

                        case Toiminto.Avaa_Lukko:
                            if (tila == OvenTila.Lukossa)
                            {
                                tila = OvenTila.Kiinni;
                                Console.WriteLine("Lukko avattiin.");
                            }
                            else
                            {
                                Console.WriteLine("Lukkoa ei voi avata, koska ovi ei ole lukossa.");
                            }
                            break;

                        default:
                            Console.WriteLine("Tuntematon toiminto.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Epäkelpo toiminto. Yritä uudelleen.");
                }
            }
        }
    }
}


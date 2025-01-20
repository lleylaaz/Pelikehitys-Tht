using System;

namespace NuoliTehtava
{
    internal class Program
    {
        // Enumeraattorit kärjen ja perän tyypeille
        enum KarjenTyyppi
        {
            Puu = 3,
            Teräs = 5,
            Timantti = 50
        }

        enum PeranTyyppi
        {
            Lehti = 0,
            Kanansulka = 1,
            Kotkansulka = 5
        }

        // Nuoli-luokka
        class Nuoli
        {
            // Ominaisuudet (Properties)
            public KarjenTyyppi Karki { get; private set; }
            public PeranTyyppi Pera { get; private set; }
            public double VarsiPituus { get; private set; }

            // Konstruktorissa asetetaan ominaisuuksien arvot
            public Nuoli(KarjenTyyppi karki, PeranTyyppi pera, double varsiPituus)
            {
                Karki = karki;
                Pera = pera;
                VarsiPituus = varsiPituus;
            }

            // Staattiset metodit valmiille nuolipohjille
            public static Nuoli LuoEliittiNuoli()
            {
                return new Nuoli(KarjenTyyppi.Timantti, PeranTyyppi.Kotkansulka, 100);
            }

            public static Nuoli LuoAloittelijaNuoli()
            {
                return new Nuoli(KarjenTyyppi.Puu, PeranTyyppi.Kanansulka, 70);
            }

            public static Nuoli LuoPerusNuoli()
            {
                return new Nuoli(KarjenTyyppi.Teräs, PeranTyyppi.Kanansulka, 85);
            }

            // Metodi hinnan laskemiseen
            public double PalautaHinta()
            {
                double karjenHinta = (double)Karki;
                double peranHinta = (double)Pera;
                double varrenHinta = VarsiPituus * 0.05;
                return karjenHinta + peranHinta + varrenHinta;
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Tervetuloa nuoli kauppaan!");

            // Valinta valmiiden pohjien ja kustomoidun nuolen välillä
            Console.WriteLine("\nValitse haluamasi toiminto:");
            Console.WriteLine("1: Luo eliittinuoli (timanttikärki, 100 cm varsi, kotkansulka)");
            Console.WriteLine("2: Luo aloittelijanuoli (puukärki, 70 cm varsi, kanansulka)");
            Console.WriteLine("3: Luo perusnuoli (teräskärki, 85 cm varsi, kanansulka)");
            Console.WriteLine("4: Luo kustomoitu nuoli");
            int valinta = int.Parse(Console.ReadLine());

            Nuoli nuoli;

            switch (valinta)
            {
                case 1:
                    nuoli = Nuoli.LuoEliittiNuoli();
                    break;
                case 2:
                    nuoli = Nuoli.LuoAloittelijaNuoli();
                    break;
                case 3:
                    nuoli = Nuoli.LuoPerusNuoli();
                    break;
                case 4:
                    nuoli = LuoKustomoituNuoli();
                    break;
                default:
                    Console.WriteLine("Virheellinen valinta. Ohjelma päättyy.");
                    return;
            }

            // Tulostetaan nuolen tiedot ja hinta
            double hinta = nuoli.PalautaHinta();
            Console.WriteLine($"\nLoit nuolen: {nuoli.Karki} kärki, {nuoli.Pera} perä, {nuoli.VarsiPituus} cm varsi.");
            Console.WriteLine($"Nuolen hinta: {hinta} kultaa.");
        }

        static Nuoli LuoKustomoituNuoli()
        {
            // Kärki
            Console.WriteLine("\nValitse kärjen tyyppi:");
            foreach (var karki in Enum.GetValues<KarjenTyyppi>())
            {
                Console.WriteLine($"{(int)karki}: {karki} ({(int)karki} kultaa)");
            }
            KarjenTyyppi valittuKarki = (KarjenTyyppi)int.Parse(Console.ReadLine());

            // Perä
            Console.WriteLine("\nValitse perän tyyppi:");
            foreach (var pera in Enum.GetValues<PeranTyyppi>())
            {
                Console.WriteLine($"{(int)pera}: {pera} ({(int)pera} kultaa)");
            }
            PeranTyyppi valittuPera = (PeranTyyppi)int.Parse(Console.ReadLine());

            // Varren pituus
            Console.WriteLine("\nAnna varren pituus (60-100 cm):");
            double varsiPituus = double.Parse(Console.ReadLine());
            if (varsiPituus < 60 || varsiPituus > 100)
            {
                Console.WriteLine("Varren pituuden täytyy olla välillä 60-100 cm.");
                return null;
            }

            return new Nuoli(valittuKarki, valittuPera, varsiPituus);
        }
    }
}


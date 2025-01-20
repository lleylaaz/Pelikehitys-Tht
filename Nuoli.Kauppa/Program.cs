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
            public KarjenTyyppi Karki { get; private set; }
            public PeranTyyppi Pera { get; private set; }
            public double VarsiPituus { get; private set; }

            public Nuoli(KarjenTyyppi karki, PeranTyyppi pera, double varsiPituus)
            {
                Karki = karki;
                Pera = pera;
                VarsiPituus = varsiPituus;
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

            // Valitse kärki
            Console.WriteLine("\nValitse kärjen tyyppi:");
            foreach (var karki in Enum.GetValues<KarjenTyyppi>())
            {
                Console.WriteLine($"{(int)karki}: {karki} ({(int)karki} kultaa)");
            }
            KarjenTyyppi valittuKarki = (KarjenTyyppi)int.Parse(Console.ReadLine());

            // Valitse perä
            Console.WriteLine("\nValitse perän tyyppi:");
            foreach (var pera in Enum.GetValues<PeranTyyppi>())
            {
                Console.WriteLine($"{(int)pera}: {pera} ({(int)pera} kultaa)");
            }
            PeranTyyppi valittuPera = (PeranTyyppi)int.Parse(Console.ReadLine());

            // Valitse varren pituus
            Console.WriteLine("\nAnna varren pituus (60-100 cm):");
            double varsiPituus = double.Parse(Console.ReadLine());
            if (varsiPituus < 60 || varsiPituus > 100)
            {
                Console.WriteLine("Varren pituuden täytyy olla välillä 60-100 cm.");
                return;
            }

            // Luo nuoli ja laske hinta
            Nuoli nuoli = new Nuoli(valittuKarki, valittuPera, varsiPituus);
            double hinta = nuoli.PalautaHinta();

            // Tulosta nuolen tiedot ja hinta
            Console.WriteLine($"\nLoit nuolen: {valittuKarki} kärki, {valittuPera} perä, {varsiPituus} cm varsi.");
            Console.WriteLine($"Nuolen hinta: {hinta} kultaa.");
        }
    }
}

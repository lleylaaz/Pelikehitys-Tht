using System;
using System.Collections.Generic;
namespace Ruoka.Annokset
{
    class Program
    {
        // Määritellään enumeraattorit
        enum PääraakaAine { Nautaa, Kanaa, Kasviksia }
        enum Lisuke { Perunaa, Riisiä, Pastaa }
        enum Kastike { Curry, Hapanimelä, Pippuri, Chili }

        // Ateria-luokka
        class Ateria
        {
            public PääraakaAine Pääraaka { get; set; }
            public Lisuke Lisuke { get; set; }
            public Kastike Kastike { get; set; }

            public override string ToString()
            {
                return $"{Pääraaka.ToString().ToLower()} ja {Lisuke.ToString().ToLower()} {Kastike.ToString().ToLower()}-kastikkeella";
            }
        }

        static void Main(string[] args)
        {
            List<Ateria> ateriat = new List<Ateria>();

            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine($"Valitse annos {i + 1}:");
                Ateria ateria = LuoAteria();
                ateriat.Add(ateria);
            }

            Console.WriteLine("\nValitsemasi ruoka-annokset:");
            foreach (var ateria in ateriat)
            {
                Console.WriteLine(ateria);
            }
        }

        static Ateria LuoAteria()
        {
            // Kysytään pääraaka-aine
            Console.WriteLine("Valitse pääraaka-aine (0 = Nautaa, 1 = Kanaa, 2 = Kasviksia):");
            PääraakaAine pääraaka = (PääraakaAine)int.Parse(Console.ReadLine());

            // Kysytään lisuke
            Console.WriteLine("Valitse lisuke (0 = Perunaa, 1 = Riisiä, 2 = Pastaa):");
            Lisuke lisuke = (Lisuke)int.Parse(Console.ReadLine());

            // Kysytään kastike
            Console.WriteLine("Valitse kastike (0 = Curry, 1 = Hapanimelä, 2 = Pippuri, 3 = Chili):");
            Kastike kastike = (Kastike)int.Parse(Console.ReadLine());

            // Luodaan ja palautetaan ateria
            return new Ateria
            {
                Pääraaka = pääraaka,
                Lisuke = lisuke,
                Kastike = kastike
            };
        }
    }
}


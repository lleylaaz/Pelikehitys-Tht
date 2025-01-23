using System;
using System.Collections.Generic;
using System.Text;

// Tavara-luokka
public abstract class Tavara
{
    public double Paino { get; }
    public double Tilavuus { get; }

    protected Tavara(double paino, double tilavuus)
    {
        Paino = paino;
        Tilavuus = tilavuus;
    }

    public abstract override string ToString();
}

// Tavarat
public class Nuoli : Tavara
{
    public Nuoli() : base(0.1, 0.05) { }

    public override string ToString()
    {
        return "Nuoli";
    }
}

public class Jousi : Tavara
{
    public Jousi() : base(1, 4) { }

    public override string ToString()
    {
        return "Jousi";
    }
}

public class Köysi : Tavara
{
    public Köysi() : base(1, 1.5) { }

    public override string ToString()
    {
        return "Köysi";
    }
}

public class Vesi : Tavara
{
    public Vesi() : base(2, 2) { }

    public override string ToString()
    {
        return "Vesi";
    }
}

public class Ruoka : Tavara
{
    public Ruoka() : base(1, 0.5) { }

    public override string ToString()
    {
        return "Ruoka";
    }
}

public class Miekka : Tavara
{
    public Miekka() : base(5, 3) { }

    public override string ToString()
    {
        return "Miekka";
    }
}

// Reppu-luokka
public class Reppu
{
    private readonly List<Tavara> tavarat;
    public int MaxTavarat { get; }
    public double MaxPaino { get; }
    public double MaxTilavuus { get; }

    public Reppu(int maxTavarat, double maxPaino, double maxTilavuus)
    {
        MaxTavarat = maxTavarat;
        MaxPaino = maxPaino;
        MaxTilavuus = maxTilavuus;
        tavarat = new List<Tavara>();
    }

    public int TavaroidenMaara => tavarat.Count;
    public double TavaroidenPaino => LaskePaino();
    public double TavaroidenTilavuus => LaskeTilavuus();

    public bool Lisää(Tavara tavara)
    {
        if (TavaroidenMaara + 1 > MaxTavarat || TavaroidenPaino + tavara.Paino > MaxPaino || TavaroidenTilavuus + tavara.Tilavuus > MaxTilavuus)
        {
            return false;
        }

        tavarat.Add(tavara);
        return true;
    }

    private double LaskePaino()
    {
        double paino = 0;
        foreach (var tavara in tavarat)
        {
            paino += tavara.Paino;
        }
        return paino;
    }

    private double LaskeTilavuus()
    {
        double tilavuus = 0;
        foreach (var tavara in tavarat)
        {
            tilavuus += tavara.Tilavuus;
        }
        return tilavuus;
    }

    public override string ToString()
    {
        if (tavarat.Count == 0)
        {
            return "Reppu on tyhjä.";
        }

        var sisalto = new StringBuilder("Reppussa on seuraavat tavarat: ");
        sisalto.Append(string.Join(", ", tavarat));
        return sisalto.ToString();
    }
}

// Pääohjelma
public class Program
{
    public static void Main()
    {
        var reppu = new Reppu(10, 30, 20);

        Console.WriteLine(reppu.ToString()); // Tulostaa repun sisällön

        while (true)
        {
            Console.WriteLine($"Reppu: {reppu.TavaroidenMaara}/{reppu.MaxTavarat} tavaraa, {reppu.TavaroidenPaino}/{reppu.MaxPaino} painoa, ja {reppu.TavaroidenTilavuus}/{reppu.MaxTilavuus} tilavuus.");
            Console.WriteLine("Mitä haluat lisätä?");
            Console.WriteLine("1 - Nuoli\n2 - Jousi\n3 - Köysi\n4 - Vettä\n5 - Ruokaa\n6 - Miekka");

            if (!int.TryParse(Console.ReadLine(), out int valinta))
            {
                Console.WriteLine("Virheellinen valinta. Yritä uudelleen.");
                continue;
            }

            Tavara tavara = valinta switch
            {
                1 => new Nuoli(),
                2 => new Jousi(),
                3 => new Köysi(),
                4 => new Vesi(),
                5 => new Ruoka(),
                6 => new Miekka(),
                _ => null
            };

            if (tavara == null)
            {
                Console.WriteLine("Virheellinen valinta. Yritä uudelleen.");
                continue;
            }

            if (reppu.Lisää(tavara))
            {
                Console.WriteLine("Tavara lisätty reppuun.");
                Console.WriteLine(reppu.ToString()); // Päivitetty sisällön tulostus
            }
            else
            {
                Console.WriteLine("Tavaraa ei voitu lisätä. Reppu täynnä!");
            }
        }
    }
}


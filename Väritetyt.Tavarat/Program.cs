using System;
using System.Collections.Generic;

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
}

// Tavarat
public class Nuoli : Tavara { public Nuoli() : base(0.1, 0.05) { } }
public class Jousi : Tavara { public Jousi() : base(1, 4) { } }
public class Köysi : Tavara { public Köysi() : base(1, 1.5) { } }
public class Vesi : Tavara { public Vesi() : base(2, 2) { } }
public class Ruoka : Tavara { public Ruoka() : base(1, 0.5) { } }
public class Miekka : Tavara { public Miekka() : base(5, 3) { } }
public class Kirves : Tavara { public Kirves() : base(4, 2.5) { } }

// Generinen VaritettyTavara-luokka
public class VaritettyTavara<T> where T : Tavara
{
    public T Tavara { get; }
    public ConsoleColor Vari { get; }

    public VaritettyTavara(T tavara, ConsoleColor vari)
    {
        Tavara = tavara;
        Vari = vari;
    }

    public void NaytaTavara()
    {
        Console.ForegroundColor = Vari;
        Console.WriteLine(Tavara.GetType().Name);
        Console.ResetColor();
    }
}

// Pääohjelma
public class Program
{
    public static void Main()
    {
        var punainenMiekka = new VaritettyTavara<Miekka>(new Miekka(), ConsoleColor.Red);
        var sininenJousi = new VaritettyTavara<Jousi>(new Jousi(), ConsoleColor.Blue);
        var vihreaRuoka = new VaritettyTavara<Ruoka>(new Ruoka(), ConsoleColor.Green);

        punainenMiekka.NaytaTavara();
        sininenJousi.NaytaTavara();
        vihreaRuoka.NaytaTavara();
    }
}

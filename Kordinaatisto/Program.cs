using System;

// Käytin ChatGPT koodiin.

// Määritellään Koordinaatti-struct, joka tallentaa X- ja Y-koordinaatit.
public struct Koordinaatti
{
    // X- ja Y-koordinaatit, jotka voidaan asettaa vain konstruktorissa.
    public int X { get; }
    public int Y { get; }

    // Konstruktorilla asetetaan X ja Y -arvot.
    public Koordinaatti(int x, int y)
    {
        X = x;
        Y = y;
    }

    // Metodi tarkistaa, onko toinen koordinaatti tämän koordinaatin vieressä.
    public bool OnVieressa(Koordinaatti toinen)
    {
        // Lasketaan X ja Y -arvojen erot.
        int xEro = Math.Abs(this.X - toinen.X); 
        int yEro = Math.Abs(this.Y - toinen.Y); 

        // Palautetaan true, jos koordinaatit eroavat vain yhdellä joko X- tai Y-suunnassa.
        return (xEro == 1 && yEro == 0) || (xEro == 0 && yEro == 1);
    }
}

public class Program
{
    public static void Main()
    {
        // Luodaan keskipiste koordinaattiin (0,0)
        Koordinaatti keskipiste = new Koordinaatti(0, 0);

        // Luodaan taulukko eri koordinaateista, joiden vieressä olo testataan.
        Koordinaatti[] koordinaatit = 
        {
            new Koordinaatti(-1, -1), 
            new Koordinaatti(-1, 0),  
            new Koordinaatti(-1, 1),  
            new Koordinaatti(0, -1),  
            new Koordinaatti(0, 0),   
            new Koordinaatti(0, 1),   
            new Koordinaatti(1, -1),  
            new Koordinaatti(1, 0),   
            new Koordinaatti(1, 1)    
        };

        // Käydään läpi jokainen koordinaatti ja tarkistetaan, onko se vieressä.
        foreach (var koord in koordinaatit)
        {
            if (keskipiste.OnVieressa(koord))
            {
                // Jos koordinaatti on vieressä, tulostetaan se.
                Console.WriteLine($"Annettu koordinaatti {koord.X},{koord.Y} on koordinaatin {keskipiste.X},{keskipiste.Y} vieressä.");
            }
            else if (keskipiste.X == koord.X && keskipiste.Y == koord.Y)
            {
                // Jos koordinaatti on sama kuin keskipiste, tulostetaan se.
                Console.WriteLine($"Annettu koordinaatti {koord.X},{koord.Y} on koordinaatissa {keskipiste.X},{keskipiste.Y}.");
            }
            else
            {
                // Muuten se ei ole vieressä.
                Console.WriteLine($"Annettu koordinaatti {koord.X},{koord.Y} ei ole koordinaatin {keskipiste.X},{keskipiste.Y} vieressä.");
            }
        }
    }
}

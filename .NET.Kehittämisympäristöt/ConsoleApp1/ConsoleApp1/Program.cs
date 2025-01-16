namespace ConsoleApp1
{
    internal class Program
    {
        enum PlayerAction
        {
            Attack,
            Defend
        }

        static int RandomDamage(int minDamage, int maxDamage)
        {
            Random generator = new Random();
            return generator.Next(minDamage, maxDamage + 1);
        }

        static void ColorPrint(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
        }
        static void Main(string[] args)
        {
            // Pelin alustus
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Hello, World!");
            ColorPrint("Orc attacks you!", ConsoleColor.DarkRed);
            int ritariHP = 15;
            int orcHP = 15;
            PlayerAction action = PlayerAction.Attack;

            // Näytä tilanne
            Console.WriteLine($"Ritarin HP: {ritariHP} Örkin HP: {orcHP}");

            // while loop kysyy komentoa
            while (true)
            {
                // Näytä komennot
                // 1. Hyökkää 2. Puolusta
                // Kysy komento
                Console.WriteLine("1. Hyökkää 2. Puolusta");

            // Tallenna vastaus
            string? vastaus = Console.ReadLine(); 
            // Jos vastaus on 1 tai 2 hyväksy vastaus
                if (vastaus == "1") 
                {
                    break;
                }
                else if (vastaus == "2")
                {
                    break;
                }

            // Jos jotain muuta, kysy uudestaan
            }
            // while loppuu tähän
        }
    }
}

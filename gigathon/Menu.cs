using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace gigathon
{
    public class Menu
    {
        public string mode { get; private set; } // Tryb gry: "easy" lub "hard"

        string logotype = @"    ____               _                 
   / __ \____ ______  (_)___ _____  _____
  / /_/ / __ `/ ___/ / / __ `/ __ \/ ___/
 / ____/ /_/ (__  ) / / /_/ / / / (__  ) 
/_/    \__,_/____/_/ /\__,_/_/ /_/____/  
                /___/                    

";

        public void GenerateMenu()
        {
            Console.Clear();
            Console.WriteLine(logotype);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Wybierz trudność gry: (kliknij na klawiaturze 1 lub 2)");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("1. Łatwy");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("2. Trudny");

            PickMode(); // Obsługa wyboru trudności
        }

        public void PickMode()
        {
            var clickedKey = Console.ReadKey().Key;

            if (clickedKey == ConsoleKey.D1)
            {
                // Wybrano tryb łatwy
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("---------- Wybrano tryb łatwy ----------");
                mode = "easy";
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\nKliknij Enter aby przejść do gry");
                Console.WriteLine("\nKliknij Backspace aby cofnąć się do menu");
                Console.ResetColor();

                IsReadyToPlay(); // Czeka na potwierdzenie gracza
            }
            else if (clickedKey == ConsoleKey.D2)
            {
                // Wybrano tryb trudny
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("---------- Wybrano tryb trudny ----------");
                mode = "hard";
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\nKliknij Enter aby przejść do gry");
                Console.WriteLine("\nKliknij Backspace aby cofnąć się do menu");
                Console.ResetColor();

                IsReadyToPlay();
            }
            else
            {
                // Nieprawidłowy wybór – powrót do menu
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Nieprawidłowy wybór, spróbuj ponownie.");
                Console.ResetColor();
                Thread.Sleep(500);
                GenerateMenu();
            }
        }

        public bool IsReadyToPlay()
        {
            var clickedKey = Console.ReadKey().Key;

            if (clickedKey == ConsoleKey.Enter)
            {
                Console.Clear();
                return true; // Gracz gotowy – rozpoczynamy grę
            }
            else if (clickedKey == ConsoleKey.Backspace)
            {
                Console.Clear();
                GenerateMenu(); // Powrót do menu
                return false;
            }
            else
            {
                // Każdy inny klawisz powoduje powrót do menu
                Console.Clear();
                GenerateMenu();
                return false;
            }
        }
    }
}

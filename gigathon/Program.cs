using System;
using System.Collections.Generic;
using System.Linq;

namespace gigathon
{
    public enum Suit
    {
        Kier,    
        Karo,  
        Pik,    
        Trefl      
    }

    public enum Rank
    {
        As = 1, Dwa, Trzy, Cztery, Pięc, Sześć, Siedem,
        Osiem, Dziewięć, Dziesięć, Walet, Królowa, Król
    }



    public class Karta
    {
        const int MAX_KART = 52;
        public List<string> karty = new List<string>();
        public List<string> kartyRezerwowe = new List<string>(); // karty rezerwowe
        public List<List<string>> kolumny = new List<List<string>>();
        public string TrybTrudnosci { get; private set; }

        public Karta(string tryb)
        {
            TrybTrudnosci = tryb;
            GenerujKarty();
        }

        private void GenerujKarty()
        {
            List<string> wszystkieKarty = new List<string>();

            for (int i = 0; i < MAX_KART; i++)
            {
                Suit kolor = (Suit)(i / 13);
                Rank wartosc = (Rank)(i % 13 + 1);

                string symbol = kolor switch
                {
                    Suit.Kier => "♥",
                    Suit.Pik => "♠",
                    Suit.Karo => "♦",
                    Suit.Trefl => "♣",
                    _ => "?"
                };

                wszystkieKarty.Add($"{(int)wartosc switch
                {
                    1 => "A",
                    <= 10 => ((int)wartosc).ToString(),
                    11 => "J",
                    12 => "Q",
                    13 => "K",
                    _ => "?"
                }} {symbol}");
            }

            
            Random rnd = new Random();
            wszystkieKarty = wszystkieKarty.OrderBy(x => rnd.Next()).ToList();

            
            karty = wszystkieKarty.Take(28).ToList();
            kartyRezerwowe = wszystkieKarty.Skip(28).ToList();
        }

        public void DobierzKarteZRezerwowych(int indexDocelowy)
        {
            if (kartyRezerwowe.Count > 0)
            {
                Random rnd = new Random();
                int index = rnd.Next(kartyRezerwowe.Count);
                string karta = kartyRezerwowe[index];
                var cel = kolumny[indexDocelowy - 1];
                cel.Add(karta); // dodaj na koniec docelowej
                
                kartyRezerwowe.RemoveAt(index);
            }
            else
            {
                Console.WriteLine("Brak kart rezerwowych.");
            }
        }



        private void TasujKarty()
        {
            Random rnd = new Random();
            karty = karty.OrderBy(x => rnd.Next(karty.Count)).ToList();
        }
        
        public void PrzeniesKarte(int indexZrodlowy, int indexDocelowy)
        {
            if (indexZrodlowy < 1 || indexZrodlowy > kolumny.Count ||
                indexDocelowy < 1 || indexDocelowy > kolumny.Count)
            {
                Console.WriteLine("Nieprawidłowe numery kolumn.");
                return;
            }

            var zrodlo = kolumny[indexZrodlowy - 1];
            var cel = kolumny[indexDocelowy - 1];

            if (zrodlo.Count == 0)
            {
                Console.WriteLine("Kolumna źródłowa jest pusta.");
                return;
            }

            string karta = zrodlo[zrodlo.Count - 1]; // ostatnia karta
            zrodlo.RemoveAt(zrodlo.Count - 1);       // usuń ją ze źródła
            cel.Add(karta);                          // dodaj na koniec docelowej
        }

        public void RozlozKarty()
        {
            TasujKarty();
            kolumny.Clear();
            int index = 0;

            for (int col = 0; col < 7; col++)
            {
                List<string> kolumna = new List<string>();
                for (int i = 0; i <= col; i++)
                {
                    if (index < karty.Count)
                        kolumna.Add(karty[index++]);
                }
                kolumny.Add(kolumna);
            }

            WyswietlKolumny(true);
        }

        public void WyswietlKolumny(bool displayAnnoucment)
        {
            
            Console.WriteLine("\nUkład kart do pasjansa:");
            for (int i = 0; i < kolumny.Count; i++)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"{i + 1}: ");
                for (int j = 0; j < kolumny[i].Count; j++)
                {
                    if (j < kolumny[i].Count - 1)
                    {
                        
                        Console.Write("#");
                    } else
                    {
                        
                       char ikonka = kolumny[i][j].ToString()[kolumny[i][j].Length - 1]; // ostatni znak
                        switch (ikonka){
                            case '♥':
                                Console.ForegroundColor = ConsoleColor.Red;
                                break;
                            case '♦':
                                Console.ForegroundColor = ConsoleColor.Red;
                                break;
                            case '♠':
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                break;
                            case '♣':
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                break;
                            default:
                                Console.ResetColor();
                                break;
                        }
                        Console.Write(kolumny[i][j] + " ");
                    }
                        
                }
                Console.WriteLine();
            }
            Console.ResetColor();
            Console.WriteLine("\n━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");

            if (displayAnnoucment)
            {
                Menu menu = new Menu();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nAby zmienić miejsce jakiejś odkrytej karty kliknij Backspace");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Cyan;
                switch (TrybTrudnosci)
                {
                    case "easy":
                        Console.WriteLine("\nAby dobrać kartę z rezerwowych kliknij Enter");
                        break;
                    case "hard":
                        Console.WriteLine("\nAby dobrać 3 karty z rezerwowych kliknij Enter");
                        break;
                }
                Console.ResetColor();
                Console.WriteLine("\n━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            }
           
        }

    }





    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.Title = "Pasjans - wersja konsolowa";

            Menu menu = new Menu();
            menu.GenerateMenu();


            Karta karta = new Karta(menu.mode);
            karta.RozlozKarty(); 


            while (true)
            {
                var clickedKey = Console.ReadKey(true); 
                if (clickedKey.Key == ConsoleKey.Backspace)
                {
                    Console.Clear();
                    karta.WyswietlKolumny(false);
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("\n\nPodaj numer karty do zamiany: (pierwsza odsłonięta karta z kolumny 1-7)");
                    string pozycja1 = Console.ReadLine();
                    Console.WriteLine("\n\nPodaj pozycję gdzie karta ma się znaleźć (będzie pierwszą kartą w kolumnach 1-7)");
                    Console.ResetColor();
                    string pozycja2 = Console.ReadLine();

                    if (string.IsNullOrEmpty(pozycja1) || pozycja1.Contains(' ') || !int.TryParse(pozycja1, out _))
                    {
                        Console.WriteLine("Nieprawidłowy format wejścia.");
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("\nAby ponowić kliknij Backspace");
                        Console.ResetColor();
                        continue;
                    }

                    if (string.IsNullOrEmpty(pozycja2) || pozycja2.Contains(' ') || !int.TryParse(pozycja2, out _))
                    {
                        Console.WriteLine("Nieprawidłowy format wejścia.");
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("\nAby ponowić kliknij Backspace");
                        Console.ResetColor();
                        continue;
                    }

                    int convertedIntPozycja1 = int.Parse(pozycja1 != null ? pozycja1 : "");
                    int convertedIntPozycja2 = int.Parse(pozycja2 != null ? pozycja2 : "");

                    if (convertedIntPozycja1 < 1 || convertedIntPozycja1 > karta.kolumny.Count ||
                        convertedIntPozycja2 < 1 || convertedIntPozycja2 > karta.kolumny.Count)
                    {
                        Console.WriteLine("Nieprawidłowe numery kolumn.");
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("\nAby ponowić kliknij Backspace");
                        Console.ResetColor();
                        continue;
                    } else
                    {
                        karta.PrzeniesKarte(convertedIntPozycja1, convertedIntPozycja2);
                        Console.Clear();
                        karta.WyswietlKolumny(true);
                    }
                } else if(clickedKey.Key == ConsoleKey.Enter)
                {
                    
                    Console.Clear();
                    karta.WyswietlKolumny(false);
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("\n\nPodaj pozycję gdzie karta ma się znaleźć (będzie pierwszą kartą w kolumnach 1-7)");
                    string pozycjaDobieranej = Console.ReadLine();
                    int convertedPozycjaDobieranej;

                    if (string.IsNullOrEmpty(pozycjaDobieranej) || pozycjaDobieranej.Contains(' ') || !int.TryParse(pozycjaDobieranej, out convertedPozycjaDobieranej))
                    {
                        Console.WriteLine("Nieprawidłowy format wejścia.");
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("\nAby ponowić kliknij Enter");
                        Console.ResetColor();
                        continue;
                    }

                    Console.Clear();
                    karta.WyswietlKolumny(true);
                    
                    if (convertedPozycjaDobieranej < 1 || convertedPozycjaDobieranej > karta.kolumny.Count)
                    {
                        Console.WriteLine("Nieprawidłowe numery kolumn.");
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("\nAby ponowić kliknij Enter");
                        Console.ResetColor();
                    } else
                    {
                        karta.DobierzKarteZRezerwowych(convertedPozycjaDobieranej);
                        Console.Clear();
                        karta.WyswietlKolumny(true);
                    }
                }
                else if (clickedKey.Key == ConsoleKey.Escape)
                {
                    break;
                }
                
            }
            
        }
    }

}
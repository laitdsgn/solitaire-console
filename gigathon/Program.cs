using System;
using System.Collections.Generic;
using System.Linq;

namespace gigathon
{
    // Enum dla kolorów kart
    public enum Suit
    {
        Kier,
        Karo,
        Pik,
        Trefl
    }

    // Enum dla wartości kart
    public enum Rank
    {
        As = 1, Dwa, Trzy, Cztery, Pięc, Sześć, Siedem,
        Osiem, Dziewięć, Dziesięć, Walet, Królowa, Król
    }

    // Klasa reprezentująca talię kart oraz logikę gry
    public class Karta
    {
        const int MAX_KART = 52; // Całkowita liczba kart w talii

        public List<string> karty = new List<string>(); // Główna talia kart
        public List<string> kartyRezerwowe = new List<string>(); // Karty rezerwowe (pozostałe po rozdaniu)
        public List<List<string>> kolumny = new List<List<string>>(); // 7 kolumn kart (typowe dla pasjansa)
        public string TrybTrudnosci { get; private set; } // Tryb gry (easy/hard)

        public Karta(string tryb)
        {
            TrybTrudnosci = tryb;
            GenerujKarty(); // Tworzenie i tasowanie kart
        }

        // Generuje pełną talię i losowo rozdaje karty do głównej talii i rezerwowych
        private void GenerujKarty()
        {
            List<string> wszystkieKarty = new List<string>();

            for (int i = 0; i < MAX_KART; i++)
            {
                Suit kolor = (Suit)(i / 13); // Obliczenie koloru
                Rank wartosc = (Rank)(i % 13 + 1); // Obliczenie wartości karty

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

            // Tasowanie kart
            Random rnd = new Random();
            wszystkieKarty = wszystkieKarty.OrderBy(x => rnd.Next()).ToList();

            // Pierwsze 28 kart trafia do gry, reszta jako rezerwowe
            karty = wszystkieKarty.Take(28).ToList();
            kartyRezerwowe = wszystkieKarty.Skip(28).ToList();
        }

        // Dobiera jedną losową kartę z rezerwowych do wybranej kolumny
        public void DobierzKarteZRezerwowych(int indexDocelowy)
        {
            if (kartyRezerwowe.Count > 0)
            {
                Random rnd = new Random();
                int index = rnd.Next(kartyRezerwowe.Count);
                string karta = kartyRezerwowe[index];
                var cel = kolumny[indexDocelowy - 1];
                cel.Add(karta);
                kartyRezerwowe.RemoveAt(index);
            }
            else
            {
                Console.WriteLine("Brak kart rezerwowych.");
            }
        }

        // Dobiera 3 karty z rezerwowych do podanych kolumn
        public void DobierzKartyZRezerwowych(List<int> indeksyDocelowe)
        {
            if (kartyRezerwowe.Count < indeksyDocelowe.Count)
            {
                Console.WriteLine("Za mało kart rezerwowych.");
                return;
            }

            Random rnd = new Random();

            foreach (int indexDocelowy in indeksyDocelowe)
            {
                int index = rnd.Next(kartyRezerwowe.Count);
                string karta = kartyRezerwowe[index];
                var cel = kolumny[indexDocelowy - 1];
                cel.Add(karta);
                kartyRezerwowe.RemoveAt(index);
            }
        }

        // Tasowanie kart (używane przed rozdaniem)
        private void TasujKarty()
        {
            Random rnd = new Random();
            karty = karty.OrderBy(x => rnd.Next(karty.Count)).ToList();
        }

        // Przenosi kartę z jednej kolumny do innej
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

            string karta = zrodlo[zrodlo.Count - 1]; // Pobierz ostatnią kartę
            zrodlo.RemoveAt(zrodlo.Count - 1);       // Usuń ze źródła
            cel.Add(karta);                          // Dodaj do celu
        }

        // Rozkłada karty na 7 kolumn, każda kolejna z większą liczbą kart
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

        // Wyświetla układ kolumn i dostępne opcje sterowania
        public void WyswietlKolumny(bool displayAnnoucment)
        {
            Console.ResetColor();
            Console.WriteLine("\nUkład kart do pasjansa:\n");

            for (int i = 0; i < kolumny.Count; i++)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"{i + 1}: ");
                for (int j = 0; j < kolumny[i].Count; j++)
                {
                    if (j < kolumny[i].Count - 1)
                    {
                        Console.Write("#"); // zakryta karta
                    }
                    else
                    {
                        // odkryta karta – ustaw kolor w zależności od symbolu
                        char ikonka = kolumny[i][j].ToString()[kolumny[i][j].Length - 1];
                        switch (ikonka)
                        {
                            case '♥':
                            case '♦':
                                Console.ForegroundColor = ConsoleColor.Red;
                                break;
                            case '♠':
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
                // Informacje sterujące i komunikaty
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
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n\n\n\n\n\nAby zakończyć grę kliknij ESCAPE");
            }
        }
    }

    // Klasa główna uruchamiająca aplikację
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

            // Główna pętla gry
            while (true)
            {
                var clickedKey = Console.ReadKey(true);

                if (clickedKey.Key == ConsoleKey.Backspace)
                {
                    // Przenoszenie kart między kolumnami
                    Console.Clear();
                    karta.WyswietlKolumny(false);
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("\n\nPodaj numer karty do zamiany (kolumna 1-7):");
                    string pozycja1 = Console.ReadLine();
                    Console.WriteLine("\n\nPodaj pozycję docelową (kolumna 1-7):");
                    Console.ResetColor();
                    string pozycja2 = Console.ReadLine();

                    // Walidacja danych wejściowych
                    if (string.IsNullOrEmpty(pozycja1) || pozycja1.Contains(' ') || !int.TryParse(pozycja1, out _) ||
                        string.IsNullOrEmpty(pozycja2) || pozycja2.Contains(' ') || !int.TryParse(pozycja2, out _))
                    {
                        Console.WriteLine("Nieprawidłowy format wejścia.");
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("\nAby ponowić kliknij Backspace");
                        Console.ResetColor();
                        continue;
                    }

                    int convertedIntPozycja1 = int.Parse(pozycja1);
                    int convertedIntPozycja2 = int.Parse(pozycja2);

                    if (convertedIntPozycja1 < 1 || convertedIntPozycja1 > karta.kolumny.Count ||
                        convertedIntPozycja2 < 1 || convertedIntPozycja2 > karta.kolumny.Count)
                    {
                        Console.WriteLine("Nieprawidłowe numery kolumn.");
                        continue;
                    }
                    else
                    {
                        karta.PrzeniesKarte(convertedIntPozycja1, convertedIntPozycja2);
                        Console.Clear();
                        karta.WyswietlKolumny(true);
                    }
                }
                else if (clickedKey.Key == ConsoleKey.Enter)
                {
                    // Dobieranie kart z rezerwowych
                    Console.Clear();
                    karta.WyswietlKolumny(false);
                    Console.ForegroundColor = ConsoleColor.Magenta;

                    if (karta.TrybTrudnosci == "hard")
                    {
                        List<int> kolumnyDocelowe = new List<int>();

                        for (int i = 1; i <= 3; i++)
                        {
                            Console.WriteLine($"\nPodaj numer kolumny docelowej dla karty {i} (1-7):");
                            string input = Console.ReadLine();
                            int index;

                            if (string.IsNullOrEmpty(input) || !int.TryParse(input, out index) || index < 1 || index > karta.kolumny.Count)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Nieprawidłowa kolumna. Przerwano dobieranie.");
                                Console.ResetColor();
                                break;
                            }

                            kolumnyDocelowe.Add(index);
                        }

                        if (kolumnyDocelowe.Count == 3)
                        {
                            karta.DobierzKartyZRezerwowych(kolumnyDocelowe);
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nPodaj numer kolumny, do której ma trafić dobrana karta (1-7):");
                        string pozycjaDobieranej = Console.ReadLine();
                        int convertedPozycjaDobieranej;

                        if (string.IsNullOrEmpty(pozycjaDobieranej) || !int.TryParse(pozycjaDobieranej, out convertedPozycjaDobieranej)
                            || convertedPozycjaDobieranej < 1 || convertedPozycjaDobieranej > karta.kolumny.Count)
                        {
                            Console.WriteLine("Nieprawidłowy numer kolumny.");
                        }
                        else
                        {
                            karta.DobierzKarteZRezerwowych(convertedPozycjaDobieranej);
                        }
                    }

                    Console.Clear();
                    karta.WyswietlKolumny(true);
                }
                else if (clickedKey.Key == ConsoleKey.Escape)
                {
                    // Wyjście z gry
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Dziękujemy za grę! Do zobaczenia!");
                    Console.ResetColor();
                    Thread.Sleep(500);
                    break;
                }
            }
        }
    }
}

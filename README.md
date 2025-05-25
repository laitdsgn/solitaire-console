# Solitaire Game in C# (Console)

## 📌 PL: Instrukcja

### ✅ Jak uruchomić projekt

1. Otwórz projekt w Visual Studio lub innym IDE.
2. Upewnij się, że wszystkie pliki (`Program.cs`, `Menu.cs`) znajdują się w tym samym projekcie.
3. Ustaw projekt jako startowy (`Startup Project`).
4. Uruchom aplikację z debugowaniem lub bez (`Ctrl + F5` lub przycisk „Start”).

### 🕹️ Sterowanie i zasady gry

- Po uruchomieniu wybierz poziom trudności:
  - `1` – tryb łatwy (dobierana 1 karta rezerwowa).
  - `2` – tryb trudny (dobierane 3 karty rezerwowe).
- Aby dobrać kartę z rezerwowych naciśnij `Enter` i wpisz gdzie ją przenieść.
- Aby przenieść kartę z jednej kolumny do drugiej naciśnij `Backspace`, a następnie podaj:
  - numer kolumny źródłowej (karta odkryta),
  - numer kolumny docelowej.
- Aby zakończyć grę, naciśnij `Esc`.

### 🧩 Struktura projektu i opis klas

#### `Program.cs`
- Główna pętla gry.
- Obsługuje interakcje użytkownika z klawiaturą.
- Tworzy obiekt `Karta` (logika pasjansa) i `Menu`.

#### `Menu.cs`
- Wyświetla ekran startowy.
- Umożliwia wybór trybu trudności (`easy` lub `hard`).
- Przekazuje wybrany tryb do klasy `Karta`.

#### `Karta.cs`
- Zawiera logikę tworzenia, tasowania i rozdawania kart.
- Przechowuje kolumny oraz rezerwowe karty.
- Odpowiada za przenoszenie kart pomiędzy kolumnami i dobieranie z rezerwowych.
- Umożliwia wyświetlanie stanu gry w konsoli.


---

## 📌 EN: Instructions

### ✅ How to run the project

1. Open the project in Visual Studio or any C# IDE.
2. Make sure all files (`Program.cs`, `Menu.cs`) are included in the project.
3. Set the project as the Startup Project.
4. Run the program using `Ctrl + F5` or the "Start" button.

### 🕹️ Gameplay and controls

- Upon launch, choose difficulty level:
  - `1` – Easy mode (draw 1 card).
  - `2` – Hard mode (draw 3 cards).
- Press `Enter` to draw a card from the reserve.
- Press `Backspace` to move a card between columns. Then input:
  - source column number (1–7),
  - destination column number (1–7).
- Press `Esc` to exit the game.

### 🧩 Project structure and class descriptions

#### `Program.cs`
- Main game loop and keyboard handling.
- Instantiates `Karta` (game logic) and `Menu`.

#### `Menu.cs`
- Displays difficulty selection menu.
- Passes the chosen mode to `Karta`.

#### `Karta.cs`
- Handles generation, shuffling, and dealing cards.
- Stores columns and reserve cards.
- Implements card moving and drawing mechanics.
- Displays game state in console.




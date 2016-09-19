using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hangman{
    public enum GuessType{
        None,
        PreviousCorrect,
        PreviousIncorrect,
        Correct,
        Incorrect,
    }

    public struct GuessResult{
        public string WordDisplay { get; set; }
        public GuessType GuessType { get; set; }
    }

    public class Game {
        public string WordToGuess { get; set; }
        public string WordToGuessUppercase { get; set; }
        public int Lives { get; set; }
        public StringBuilder WordDisplay { get; set; }

        private int _lettersRevealed;
        private readonly List<char> _correctGuesses = new List<char>();
        private readonly List<char> _incorrectGuesses = new List<char>();

        public bool Finished { get { return Won || Lives == 0; } }
        public bool Won { get { return _lettersRevealed == WordToGuess.Length; } }

        public GuessResult MakeGuess(char guess){
            guess = Char.ToUpper(guess);

            GuessResult result = new GuessResult();

            if (_correctGuesses.Contains(guess))
            {
                result.GuessType = GuessType.PreviousCorrect;
            }
            else if (_incorrectGuesses.Contains(guess))
            {
                result.GuessType = GuessType.PreviousIncorrect;
            }
            else if (WordToGuessUppercase.Contains(guess))
            {
                result.GuessType = GuessType.Correct;
                _correctGuesses.Add(guess);

                RevealLetter(guess);
            }
            else
            {
                result.GuessType = GuessType.Incorrect;
                _incorrectGuesses.Add(guess);
                Lives--;
            }

            result.WordDisplay = WordDisplay.ToString();
            return result;
        }

        private void RevealLetter(char letter){
            for (int i = 0; i < WordToGuess.Length; i++) 
            {
                if (WordToGuessUppercase[i] == letter)
                {
                    WordDisplay[i] = WordToGuess[i];
                    _lettersRevealed++;
                }
            }
        }
    }

    public static class Program {
        public static void Main(string[] args){
            var random = new Random();

            string[] wordBank = { "Blue", "Black", "Yellow", "Orange", "Green", "Purple" };

            string wordToGuess = wordBank[random.Next(0, wordBank.Length)];

            var game = new Game {
                Lives = 5,
                WordToGuess = wordToGuess,
                WordToGuessUppercase = wordToGuess.ToUpper(),
                WordDisplay = new StringBuilder(new String('_', wordToGuess.Length)),
            };

            string input;
            char guess;

            while (!game.Finished)
            {
                Console.Write("Guess a letter: ");

                input = Console.ReadLine();
                guess = input[0];

                var guessResult = game.MakeGuess(guess);

                switch(guessResult.GuessType){
                    case GuessType.PreviousCorrect:
                        Console.WriteLine("You've already tried '{0}', and it was correct!", guess);
                        break;
                    case GuessType.PreviousIncorrect:
                        Console.WriteLine("You've already tried '{0}', and it was wrong!", guess);
                        break;
                    case GuessType.Correct:
                        // Just display the updated Word
                        break;
                    case GuessType.Incorrect:
                        Console.WriteLine("Nope, there's no '{0}' in it!", guess);
                        break;
                }
                
                Console.WriteLine(guessResult.WordDisplay);
            }

            if (game.Won)
                Console.WriteLine("You won!");
            else
                Console.WriteLine("You lost! It was '{0}'", wordToGuess);

            Console.Write("Press ENTER to exit...");
            Console.ReadLine();
        }
    }
}
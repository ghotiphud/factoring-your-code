using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

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
        // Assume: wordToGuess doesn't change mid-game
        //         lives decrease with wrong guess
        //         etc.

        private readonly string _wordToGuess;
        private readonly string _wordToGuessUppercase;
        private readonly StringBuilder _wordDisplay;
        private int _lives;
        private int _lettersRevealed;

        private readonly List<char> _correctGuesses = new List<char>();
        private readonly List<char> _incorrectGuesses = new List<char>();

        public bool Finished { get { return Won || _lives == 0; } }
        public bool Won { get { return _lettersRevealed == _wordToGuess.Length; } }

        public Game(string wordToGuess, int lives){
            // Preconditions: Don't pass junk input
            Trace.Assert(!String.IsNullOrWhiteSpace(wordToGuess));
            Trace.Assert(lives > 0);

            _wordToGuess = wordToGuess;
            _wordToGuessUppercase = wordToGuess.ToUpper();
            _lives = lives;

            _wordDisplay = new StringBuilder(new String('_', _wordToGuess.Length));
        }

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
            else if (_wordToGuessUppercase.Contains(guess))
            {
                result.GuessType = GuessType.Correct;
                _correctGuesses.Add(guess);

                RevealLetter(guess);
            }
            else
            {
                result.GuessType = GuessType.Incorrect;
                _incorrectGuesses.Add(guess);
                _lives--;
            }

            result.WordDisplay = _wordDisplay.ToString();
            return result;
        }

        private void RevealLetter(char guess){
            for (int i = 0; i < _wordToGuess.Length; i++) 
            {
                if (_wordToGuessUppercase[i] == guess)
                {
                    _wordDisplay[i] = _wordToGuess[i];
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

            var game = new Game(wordToGuess, 5);

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
                    default:
                        Trace.Fail("Unexpected GuessType returned");
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
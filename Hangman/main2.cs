using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hangman{
    public class Game {
        private readonly string _wordToGuess;
        private readonly string _wordToGuessUppercase;
        private readonly StringBuilder _wordDisplay;
        private int _lives;
        private int _lettersRevealed;

        private readonly List<char> _correctGuesses = new List<char>();
        private readonly List<char> _incorrectGuesses = new List<char>();

        public bool Finished { get; set; }
        public bool Won { get; set; }

        public Game(string wordToGuess, int lives){
            _wordToGuess = wordToGuess;
            _wordToGuessUppercase = wordToGuess.ToUpper();
            _lives = lives;

            _wordDisplay = new StringBuilder(new String('_', _wordToGuess.Length));
        }

        public string MakeGuess(char guess){
            if (_correctGuesses.Contains(guess)) 
            {
                return String.Format("You've already tried '{0}', and it was correct!", guess);
            }
            
            if (_incorrectGuesses.Contains(guess))
            {
                return String.Format("You've already tried '{0}', and it was wrong!", guess);
            }

            if (_wordToGuessUppercase.Contains(guess))
            {
                _correctGuesses.Add(guess);

                for (int i = 0; i < _wordToGuess.Length; i++) 
                {
                    if (_wordToGuessUppercase[i] == guess)
                    {
                        _wordDisplay[i] = _wordToGuess[i];
                        _lettersRevealed++;
                    }
                }

                if (_lettersRevealed == _wordToGuess.Length)
                {
                    Finished = true;
                    Won = true;
                }
            }
            else 
            {
                _incorrectGuesses.Add(guess);
                _lives--;

                if(_lives == 0){
                    Finished = true;
                }

                return String.Format("Nope, there's no '{0}' in it!\n{1}", guess, _wordDisplay);
            }

            return _wordDisplay.ToString();
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

                input = Console.ReadLine().ToUpper();
                guess = input[0];

                var guessResult = game.MakeGuess(guess);

                Console.WriteLine(guessResult);
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
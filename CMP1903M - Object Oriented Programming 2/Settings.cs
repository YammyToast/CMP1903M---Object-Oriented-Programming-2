using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceGame
{
    internal class Settings
    {
        public int playerCount;
        public int botCount;
        public int diceCount;
        public int scoreToWin;
        public int scoreMultiplier;
        public int[] setArray;
        /// Amount of players
        /// Amount of bots
        /// Amount of dice
        /// Score to win
        /// Score multiplier

        public Settings() {
            playerCount = 2;
            botCount = 0;
            diceCount = 5;
            scoreToWin = 30;
            scoreMultiplier = 3;

        }

        public void SettingsDialogue()
        {
            int[] inputVals = new int[5];
            string input;
            int numericInput;
            try
            {

                Console.WriteLine($"Player Count  (Currently {playerCount})");
                Console.Write(" : ");
                input = Console.ReadLine();
                if (!Int32.TryParse(input, out numericInput))
                {
                    throw new InvalidInput("Non-Numeric value entered for Player Count!");
                }
                if (!(numericInput > 1 && numericInput < int.MaxValue)) {
                    throw new InvalidInput("Numeric value for Player Count is out of range.");
                }
                playerCount = numericInput;
               
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
            }
            Console.WriteLine($"Player Count set to -> {playerCount}");

            try {
                Console.WriteLine($"Bot Count  (Currently {botCount})");
                Console.Write(" : ");
                input = Console.ReadLine();
                if (!Int32.TryParse(input, out numericInput))
                {
                    throw new InvalidInput("Non-Numeric value entered for Player Count!");
                }
                if (!(numericInput > 1 && numericInput < int.MaxValue))
                {
                    throw new InvalidInput("Numeric value for Player Count is out of range.");
                }
                botCount = numericInput;
            } 
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.WriteLine($"Bot Count set to -> {botCount}");

            try
            {
                Console.WriteLine($"Dice Count  (Currently {diceCount})");
                Console.Write(" : ");
                input = Console.ReadLine();
                if (!Int32.TryParse(input, out numericInput))
                {
                    throw new InvalidInput("Non-Numeric value entered for Player Count!");
                }
                if (!(numericInput > 2 && numericInput < int.MaxValue))
                {
                    throw new InvalidInput("Numeric value for Player Count is out of range.");
                }
                diceCount = numericInput;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.WriteLine($"Dice Count set to -> {diceCount}");

            try
            {
                Console.WriteLine($"Score to Win  (Currently {scoreToWin})");
                Console.Write(" : ");
                input = Console.ReadLine();
                if (!Int32.TryParse(input, out numericInput))
                {
                    throw new InvalidInput("Non-Numeric value entered for Player Count!");
                }
                if (!(numericInput > 1 && numericInput < int.MaxValue))
                {
                    throw new InvalidInput("Numeric value for Player Count is out of range.");
                }
                scoreToWin = numericInput;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.WriteLine($"Score to Win set to -> {scoreToWin}");

            try
            {
                Console.WriteLine($"Score Multiplier  (Currently {scoreMultiplier})");
                Console.Write(" : ");
                input = Console.ReadLine();
                if (!Int32.TryParse(input, out numericInput))
                {
                    throw new InvalidInput("Non-Numeric value entered for Player Count!");
                }
                if (!(numericInput > 1 && numericInput < int.MaxValue))
                {
                    throw new InvalidInput("Numeric value for Player Count is out of range.");
                }
                scoreMultiplier = numericInput;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.WriteLine($"Score Multiplier set to -> {scoreMultiplier}");
        }

      
    }

    internal class InvalidInput : Exception { 
        public InvalidInput(string message) : base(message) { 
            
        }

        public InvalidInput(string message, Exception inner) : base(message, inner) { 
            
        }
    }
}

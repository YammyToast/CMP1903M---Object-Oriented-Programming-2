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
        public int[,] inputBoundaries;
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
            setArray = new int[5] { playerCount, botCount, diceCount, scoreToWin, scoreMultiplier };
            inputBoundaries = new int[5, 2] { { -1, int.MaxValue }, { -1, int.MaxValue }, { 2, 15 }, { 0, int.MaxValue }, { 1, 100000} };
        }

        public void SettingsDialogue()
        {
            int[] inputVals = new int[5];
            int index = 0;
            foreach (Parameters parameter in Enum.GetValues(typeof(Parameters)))
            {
                try
                {
                    string input = string.Empty;
                    int numericInput = setArray[index];

                    Console.WriteLine($"\n\n [ Set {parameter} -> (Currently {setArray[index]}) ]");
                    Console.Write(" : ");
                    input = Console.ReadLine();
                    if (input != string.Empty)
                    {
                        if (!Int32.TryParse(input, out numericInput))
                        {
                            throw new InvalidInputException($"Non-Numeric value entered for {parameter}!");
                        }
                        if (!(numericInput > inputBoundaries[index, 0] && numericInput < inputBoundaries[index, 1]))
                        {
                            throw new InvalidInputException($"Numeric value for {parameter} is out of range.");
                        }
                        inputVals[index] = numericInput;
                    }
                    else {
                        inputVals[index] = setArray[index];
                    }
                    
                    Console.WriteLine($"{parameter} is now set to -> {numericInput}");
                }
                catch (Exception ex) { 
                    Console.WriteLine(ex.Message);
                }
                index++;
            }
            setArray = inputVals;

            if (setArray[0] + setArray[1] < 2) {
                try
                {
                    setArray[0] = 2;
                    setArray[1] = 0;
                    throw new InvalidPlayerCountException("\n\nThere must be at least 2 players (players / bots) for the game to be played");
                }
                catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                }
            } 

            // Unpack values
            playerCount = setArray[0];
            botCount = setArray[1];
            diceCount = setArray[2];
            scoreToWin = setArray[3];
            scoreMultiplier = setArray[4];
        }

        
      
    }

    internal enum Parameters
    {
        PlayerCount,
        BotCount,
        DiceCount,
        ScoreToWin,
        ScoreMultiplier
    }

    internal class InvalidInputException : Exception { 
        public InvalidInputException(string message) : base(message) { 
            
        }

        public InvalidInputException(string message, Exception inner) : base(message, inner) { 
            
        }
    }

    internal class InvalidPlayerCountException : Exception {
        public InvalidPlayerCountException(string message) : base(message)
        {

        }

        public InvalidPlayerCountException(string message, Exception inner) : base(message, inner)
        {

        }
    }


}

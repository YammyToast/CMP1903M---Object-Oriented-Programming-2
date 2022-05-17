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
        public int upperDiceBoundary;
        public int[] setArray;
        public int[,] inputBoundaries;


        /// <summary>
        /// Constructor for the settings object with altered default values.
        /// </summary>
        /// <param name="playerCount">The amount of players to include in the game.</param>
        /// <param name="botCount">The amount of bots to include in the game.</param>
        /// <param name="diceCount">The amount of dice to use for each roll.</param>
        /// <param name="scoreToWin">The score required to reach to win the game.</param>
        /// <param name="scoreMultiplier">The multiplier used in calculating a turn's score.</param>
        /// <param name="upperDiceBoundary">The amount of faces to include on each of the dice.</param>
        public Settings(int playerCount, int botCount, int diceCount, int scoreToWin,
             int scoreMultiplier,  int upperDiceBoundary) {
            // Sets the pre-selected selecting as 'defaults'.
            this.playerCount = playerCount;
            this.botCount = botCount;
            this.diceCount = diceCount;
            this.scoreToWin = scoreToWin;
            this.scoreMultiplier = scoreMultiplier;
            this.upperDiceBoundary = upperDiceBoundary;
            SetPresets();
        }
        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //!!!! Static Polymorphism Implementation !!!!
        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        /// <summary>
        /// Constructor for Settings without altered default values.
        /// </summary>
        public Settings()  {
            //2, 0, 5, 30, 3, 6
            playerCount = 2;
            botCount = 0;
            diceCount = 5;
            scoreToWin = 30;
            scoreMultiplier = 3;
            upperDiceBoundary = 6;
            SetPresets();
        }

        /// <summary>
        /// Creates preset arrays necessary for iterating in the Dialogue.
        /// </summary>
        public void SetPresets() {
            setArray = new int[6] { playerCount, botCount, diceCount, scoreToWin, scoreMultiplier, upperDiceBoundary };
            inputBoundaries = new int[6, 2] { { -1, int.MaxValue }, { -1, int.MaxValue }, { 2, 15 }, { 1, int.MaxValue }, { 1, 100000 }, { 3, 99 } };
        }


        /// <summary>
        /// Procedure for getting the user-defined settings in a dialogue like fashion.
        /// </summary>
        public void SettingsDialogue()
        {
            // Creates a new array to store the selections.
            int[] inputVals = new int[7];
            int index = 0;

            // ==== MAIN DIAGLOGUE ITERATOR ==== 
            // For each of the selection parameters, create a small dialogue.
            foreach (Parameters parameter in Enum.GetValues(typeof(Parameters)))
            {
                try
                {
                    // Initialise the two values needed to parse the input.
                    string input = string.Empty;
                    int numericInput = setArray[index];

                    // Display the setting that is going to be changed, with the currently set value.
                    Console.WriteLine($"\n\n [ Set {parameter} -> (Currently {setArray[index]}) ]");
                    Console.Write(" : ");
                    input = Console.ReadLine();

                    // Check that the given input is not empty.
                    if (input != string.Empty)
                    {
                        // Attempt to parse the input as an integer, if possible, set value as numericInput.
                        if (!Int32.TryParse(input, out numericInput))
                        {
                            // If the number could not be parsed as an integer, throw a custom exception.
                            throw new InvalidInputException($"Non-Numeric value entered for {parameter}!", parameter);
                        }
                        if (!(numericInput > inputBoundaries[index, 0] && numericInput < inputBoundaries[index, 1]))
                        {
                            // If the number is outside of the setting boundaries throw a custom exception.
                            throw new InvalidInputException($"Numeric value for {parameter} is out of range.", parameter);
                        }
                        // If no exception is thrown, save the input in the inputVals array.
                        inputVals[index] = numericInput;
                    }
                    // If no input is given, assume no change in setting.
                    else {
                        inputVals[index] = setArray[index];
                    }
                    
                    // Display the new value of the setting to the user.
                    Console.WriteLine($"{parameter} is now set to -> {numericInput}");
                }
                catch (InvalidInputException ex)
                {
                    Console.WriteLine(ex.Message);
                    inputVals[(int)ex.parameter] = inputBoundaries[(int)ex.parameter, 0];
                }
                index++;
            }
            // Set the 'output' settings to the settings given in the dialogue.
            setArray = inputVals;

            // As 2 players are required to play the game, check that the sum of the players and bots is at least 2.
            if (setArray[0] + setArray[1] < 2) {
                try
                {
                    // If there aren't enough players given, set a default of 2 players, 0 bots.
                    setArray[0] = 2;
                    setArray[1] = 0;
                    // Throw a custom exception, letting the user know of the error.
                    throw new InvalidPlayerCountException("\n\nThere must be at least 2 players (players / bots) for the game to be played");
                }
                catch (InvalidPlayerCountException ex) {
                    Console.WriteLine(ex.Message);
                }
            }
           

            // Unpack values into attributes.
            playerCount = setArray[0];
            botCount = setArray[1];
            diceCount = setArray[2];
            scoreToWin = setArray[3];
            scoreMultiplier = setArray[4];
            upperDiceBoundary = setArray[5];
        }

        
      
    }

    // Setting Parameters Enum.
    internal enum Parameters
    {
        PlayerCount,
        BotCount,
        DiceCount,
        ScoreToWin,
        ScoreMultiplier,
        UpperDiceBoundary
    }
    

    // ==== CUSTOM EXCEPTIONS ====
    internal class InvalidInputException : Exception {
        public string message;
        public Parameters parameter;

        public InvalidInputException(string message, Parameters parameter) : base(message) {
            this.message = message;
            this.parameter = parameter;
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

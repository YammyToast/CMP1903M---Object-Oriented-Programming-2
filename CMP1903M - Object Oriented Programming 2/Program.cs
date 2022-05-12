using System;
using System.IO;

namespace DiceGame 
{
    class Program {

        // Game Settings Object,
        // Holds all play-chosen variables for the game.
        static Settings gameSettings;

        /// Features:
        /// Game Class
        /// Player Class
        /// Die Class
        /// Inheritance Relationship
        /// Interface Implementation
        /// Exception Handling
        /// Testing Strategy (Outlined in TestHandler)



        public static void Main(string[] args) {

            // Initialise the game settings with default values (as specified in the brief).
            gameSettings = new Settings(2, 0, 5, 30, 3, 6 );

            // Initialise the State loop.
            State state = State.Menu;

            while (state == State.Menu) {

                string stateInput = string.Empty;

                // ======== Settings Display ========
                // Set array of iterable game values to be displayed to the user.
                int[] setValues = new int[6] { gameSettings.playerCount, gameSettings.botCount, gameSettings.diceCount, gameSettings.scoreToWin, gameSettings.scoreMultiplier, gameSettings.upperDiceBoundary };
                int parameterIndex = 0;
                // Writes the game settings in the console.
                Console.WriteLine("\n| Game Settings |");
                foreach (Parameters parameter in Enum.GetValues(typeof(Parameters)))
                {
                    Console.WriteLine($"{parameter} -> {setValues[parameterIndex]}");
                    parameterIndex++;
                }

                Console.WriteLine("\n\n");
                // ======== State Selection Options ========
                // Writes state selection options in the console.
                foreach (State stateStr in Enum.GetValues(typeof(State))) { 
                    // Write all options. Except the Menu-state.
                    if (stateStr != State.Menu) Console.Write($"{(int)stateStr} : {stateStr} | ");
                }

                

                try
                {
                    // ======== State Selection Parsing ========
                    // Get state input
                    Console.Write("\n : ");
                    stateInput = Console.ReadLine();
                    // Try parse state input into State type
                    state = (State)Enum.Parse(typeof(State), stateInput);
                }
                catch (Exception ex) {
                    // If no valid parse can be found, continue state selection loop.
                    state = State.Menu;
                }

                // ======== State Check ========

                // ======== Setup ========
                if (state == State.Settings)
                {
                    Setup();
                    state = State.Menu;
                }
                // ======== Game ========
                else if (state == State.Game)
                {
                    if (gameSettings.botCount == 0) {
                        // Initialise a player vs player game with the selected game settings.
                        PVP game = new PVP(gameSettings.scoreToWin,
                            gameSettings.diceCount,
                            gameSettings.playerCount,
                            gameSettings.scoreMultiplier,
                            gameSettings.upperDiceBoundary
                            );
                        // Create an interface for the game.
                        IGame gameHandler = game;
                        // Initialise the players (as this is PVP no bots are needed).
                        gameHandler.CreatePlayers(gameSettings.playerCount);
                        // Run the game.
                        gameHandler.Game();
                    }
                    else {
                        // Initialise a player vs computer game with selected game settings.
                        PVC game = new PVC(gameSettings.scoreToWin,
                            gameSettings.diceCount,
                            (gameSettings.playerCount + gameSettings.botCount),
                            gameSettings.scoreMultiplier,
                            gameSettings.upperDiceBoundary
                            );
                        // Create an interface for the game.
                        IGame gameHandler = game;
                        // Initialise the players and bots.
                        gameHandler.CreatePlayers(gameSettings.playerCount, gameSettings.botCount);
                        // Run the game.
                        gameHandler.Game();
                    }

                    state = State.Menu;
                }
                // ======== Exit ========
                else if (state == State.Exit)
                {
                    Environment.Exit(0);
                }
                // ======== None ========
                else
                {
                    state = State.Menu;
                }

            }
                
        }

        /// <summary>
        /// Function for the game variable selection procedure.
        /// </summary>
        private static void Setup() {
            // ======== Settings ========
            // Settings are passed back into the settings initialsation to allow for concurrency.
            // The variables are NOT needed, however to provide accurate 'already-set' values, the current
            // variables are needed as defaults.
            Settings settings = new Settings(
                gameSettings.playerCount,
                gameSettings.botCount,
                gameSettings.diceCount,
                gameSettings.scoreToWin,
                gameSettings.scoreMultiplier,
                gameSettings.upperDiceBoundary
                );
            // Opens the settings dialogue script.
            settings.SettingsDialogue();
            // Sets the overall game settings to the dialogue settings.
            gameSettings = settings;

        }

       // State selection Enum.
        public enum State
        {
            Menu,
            Game,
            Settings,
            Exit
        }

    }
}

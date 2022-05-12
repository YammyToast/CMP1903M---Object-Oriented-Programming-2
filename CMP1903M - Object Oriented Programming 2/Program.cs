using System;
using System.IO;

namespace DiceGame 
{
    class Program {

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

            gameSettings = new Settings(2, 0, 5, 30, 3, 1, 6 );

            State state = State.Menu;

            while (state == State.Menu) {

                string stateInput = string.Empty;

                int[] setValues = new int[7] { gameSettings.playerCount, gameSettings.botCount, gameSettings.diceCount, gameSettings.scoreToWin, gameSettings.scoreMultiplier, gameSettings.lowerDiceBoundary, gameSettings.upperDiceBoundary };
                int parameterIndex = 0;
                Console.WriteLine("\n| Game Settings |");
                foreach (Parameters parameter in Enum.GetValues(typeof(Parameters)))
                {
                    Console.WriteLine($"{parameter} -> {setValues[parameterIndex]}");
                    parameterIndex++;
                }

                Console.WriteLine("\n\n");
                foreach (State stateStr in Enum.GetValues(typeof(State))) { 
                    // Write all options. Except the Menu-state.
                    if (stateStr != State.Menu) Console.Write($"{(int)stateStr} : {stateStr} | ");
                }

                

                try
                {
                    // Get state input
                    Console.Write("\n : ");
                    stateInput = Console.ReadLine();
                    // Try parse state input
                    state = (State)Enum.Parse(typeof(State), stateInput);
                }
                catch (Exception ex) {
                    state = State.Menu;
                }

                // ======== State Check ========

                // ======== Setup ========
                if (state == State.Settings)
                {
                    Setup();
                    state = State.Menu;
                }
                // // ======== Game ========
                else if (state == State.Game)
                {
                    if (gameSettings.botCount == 0) {
                        PVP game = new PVP(gameSettings.scoreToWin,
                            gameSettings.diceCount,
                            gameSettings.playerCount,
                            gameSettings.scoreMultiplier,
                            gameSettings.lowerDiceBoundary,
                            gameSettings.upperDiceBoundary
                            );
                        IGame gameHandler = game;
                        gameHandler.CreatePlayers(gameSettings.playerCount);
                        gameHandler.Game();
                    }
                    else {
                        PVC game = new PVC(gameSettings.scoreToWin,
                            gameSettings.diceCount,
                            (gameSettings.playerCount + gameSettings.botCount),
                            gameSettings.scoreMultiplier,
                            gameSettings.lowerDiceBoundary,
                            gameSettings.upperDiceBoundary
                            );
                        IGame gameHandler = game;
                        gameHandler.CreatePlayers(gameSettings.playerCount, gameSettings.botCount);
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

        private static void Setup() {
            Settings settings = new Settings(
                gameSettings.playerCount,
                gameSettings.botCount,
                gameSettings.diceCount,
                gameSettings.scoreToWin,
                gameSettings.scoreMultiplier,
                gameSettings.lowerDiceBoundary,
                gameSettings.upperDiceBoundary
                );
            settings.SettingsDialogue();
            gameSettings = settings;

        }

       
        public enum State
        {
            Menu,
            Game,
            Settings,
            Exit
        }

        public enum Gamemode 
        { 
            PVP,
            PVC
        }
    }
}

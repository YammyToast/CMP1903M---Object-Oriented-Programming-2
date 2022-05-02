using System;
using System.IO;

namespace DiceGame 
{
    class Program {

        static Gamemode gamemode = Gamemode.PVP;
        static int winCondition = 30;

        /// Features:
        /// Game Class
        /// Player Class
        /// Die Class
        /// Inheritance Relationship
        /// Interface Implementation
        /// Exception Handling
        /// Testing Strategy (Outlined in TestHandler)



        public static void Main(string[] args) {


            State state = State.Menu;

            while (state == State.Menu) {

                string stateInput = string.Empty;

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
                if (state == State.Setup)
                {
                    Setup();
                    state = State.Menu;
                }
                // // ======== Game ========
                else if (state == State.Game)
                {
                    if (gamemode == Gamemode.PVP)
                    {
                        PVP game = new PVP(winCondition, 5, 2);
                        IGame gameHandler = game;
                        gameHandler.Game();
                    }
                    else {
                        PVC game = new PVC(winCondition, 5, 2);
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
            // Ask for gamemode & win-condition
            string input = string.Empty;
            int inputCondition = 0;
            // ======== Set Gamemode ========
            Console.Write($"\n\nSet Gamemode (Currently: {gamemode}) [PVP], [PVC] : ");
            try
            {
                input = Console.ReadLine();
                foreach (Gamemode checkGame in Enum.GetValues(typeof(Gamemode)))
                {
                    if (input.ToLower() == checkGame.ToString().ToLower())
                    {
                        // If a valid sort is given, overwrite the default.
                        gamemode = checkGame;
                    }
                }

            }
            catch (Exception ex) {
                Console.WriteLine(ex);
            }
            Console.WriteLine($"Gamemode is now: {gamemode}");

            // ======== Set Win Condition ========
            Console.Write($"Set Win-Condition (Currently: {winCondition}pts) : ");
            try
            {
                input = Console.ReadLine();
                
                if (Int32.TryParse(input, out inputCondition) == true) {
                    if (inputCondition > 0 && inputCondition < Int32.MaxValue) { 
                        winCondition = inputCondition;
                    }
                }

            }
            catch (Exception ex) {
                Console.WriteLine(ex);
            }
            Console.WriteLine($"Win-Condition is now: {winCondition} \n\n");
        }


        public enum State
        {
            Menu,
            Setup,
            Game,
            Exit
        }

        public enum Gamemode 
        { 
            PVP,
            PVC
        }
    }
}

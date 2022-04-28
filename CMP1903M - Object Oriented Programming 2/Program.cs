using System;
using System.IO;

namespace DiceGame 
{
    class Program {

        Gamemode gamemode = Gamemode.PVP;
        int winCondition = 30;

        /// Features:
        /// Game Class
        /// Player Class
        /// Die Class
        /// Inheritance Relationship
        /// Interface Implementation
        /// Exception Handling
        /// Testing Strategy (Outlined in TestHandler)

        /// Game Logic:
        /// Players take turns rolling five dice and score for three-of-a-kind or better. If a player only has
        /// two-of-a-kind, they may re-throw the remaining dice in an attempt to improve the remaining
        /// dice values. If no matching numbers are rolled after two rolls, the player scores 0.

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
                    Console.WriteLine("Setup");
                    state = State.Menu;
                }
                // // ======== Game ========
                else if (state == State.Game)
                {
                    Console.WriteLine("Game");
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

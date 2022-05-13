using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceGame
{
    /// <summary>
    /// Class containing all of the console tables to be output.
    /// </summary>
    internal class Tables
    {
        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        // !!!! All table methods use a similar structure, see DisplayDiceTable for structure comments !!!!
        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        // ==== Dice Roll Table ====
        /// <summary>
        /// Displays a table of the dice values rolled on a turn.
        /// </summary>
        /// <param name="dice">The dice that have been rolled.</param>
        /// <param name="occurences">The highest order of occurence.</param>
        public void DisplayDiceTable(List<Die> dice, int occurences)
        {
            // Gets console width to allow for table to be centered.
            int consoleWidth = Console.WindowWidth;

            // ==== Determine the longest line in the table ====
            string diceBuffer = "";
            foreach (Die die in dice)
            {
                diceBuffer += (die == dice.Last()) ? $" {die.Value } " : $" {die.Value } │";
            }
            // ==== Construct the occurences section of the footer ====
            string occurencesBuffer = $"── Rolled for {occurences} of a kind ──";

            // Check that the occurence buffer isnt' the longest string.
            int longestLength = (diceBuffer.Length > occurencesBuffer.Length) ? diceBuffer.Length : occurencesBuffer.Length;


            // ==== Construct each line of the table ====
            // Construct the header and its necessary character padding.
            string headerBars = new string('─', longestLength);
            string header = $"┌{headerBars[..((headerBars.Length / 2) - 1)]} Dice {headerBars[((headerBars.Length / 2) + 3)..]}┐";

            // Construct the 'dice bar', containing all of the dice values, and any character padding.
            string diceBars = new string(' ', (longestLength - diceBuffer.Length) + 2);
            string diceLine = $"│{diceBars[..(diceBars.Length / 2)]}{diceBuffer}{diceBars[(diceBars.Length / 2)..]}│";

            // Construct the footer and its necessary character padding.
            string footerBars = new string('─', longestLength - occurencesBuffer.Length);
            string footer = $"└{footerBars[..(footerBars.Length / 2)]}─{occurencesBuffer}─{footerBars[(footerBars.Length / 2)..]}┘";

            // Determine the table indent, so that all of the lines are centered in the console.
            int tableIndent = (consoleWidth / 2) - (longestLength / 2);
            string tablePadding = new string(' ', tableIndent);

            // ==== Write the table to the console ====
            Console.WriteLine($"{tablePadding}{header}");
            Console.WriteLine($"{tablePadding}{diceLine}");
            Console.WriteLine($"{tablePadding}{footer}");
        }

        /// <summary>
        /// Displays the scores of all the players in the game at the end of a turn, or after a game has been finished.
        /// </summary>
        /// <param name="playerList">A list of all the player objects that are in the game.</param>
        public void DisplayGameScoreTable(List<Player> playerList)
        {
            int consoleWidth = Console.WindowWidth;

            // ==== Determine the longest line length ====
            int longestLength = 0;
            foreach (Player player in playerList)
            {
                if (player.ID.ToString().Length + player.Score.ToString().Length + 18 > longestLength) longestLength = player.ID.ToString().Length + player.Score.ToString().Length + 18;
            }

            // ==== Construct each line of the table ====
            string headerBars = new string('─', longestLength);
            string header = $"┌{headerBars[..((headerBars.Length / 2) - 3)]} Scores {headerBars[((headerBars.Length / 2) + 3)..]}┐";

            string scoreBars = new string(' ', longestLength);

            string footerBars = new string('─', longestLength);
            string footer = $"└{footerBars[..(footerBars.Length / 2)]}──{footerBars[(footerBars.Length / 2)..]}┘";



            int tableIndent = (consoleWidth / 2) - (longestLength / 2);
            string tablePadding = new string(' ', tableIndent);

            Console.WriteLine("\n\n\n\n\n\n");
            // ==== Write the table to the console ====
            Console.WriteLine($"{tablePadding}{header}");
            foreach (Player player in playerList)
            {

                string playerType = (player.IsBot) ? "Bot" : "Player";

                int currentLineLength = longestLength - (player.ID.ToString().Length + player.Score.ToString().Length + 18);
                Console.WriteLine($"{tablePadding}{scoreBars[..(currentLineLength / 2)]}  {playerType} - {player.ID} -> {player.Score} {scoreBars[(currentLineLength / 2)..]}");
            }
            Console.WriteLine($"{tablePadding}{footer}");
        }

        /// <summary>
        /// Displays a table of the results of a player turn with the score the player got.
        /// </summary>
        /// <param name="rollState">The roll-state reached after a turn.</param>
        /// <param name="score">The score the player got on the turn.</param>
        public void DisplayTurnScoreTable(RollState rollState, int score)
        {

            int consoleWidth = Console.WindowWidth;
            string textBuffer = string.Empty;

            // Displays a different method for each roll state.
            switch (rollState)
            {
                // A special message is displayed if all dice show the same value.
                case RollState.Full:
                    textBuffer = $"MAXIMUM POINTS: {score}";
                    break;
                // If the player earned a re-roll, no score is displayed (as this isn't the final score).
                case RollState.Reroll:
                    textBuffer = $"You got a re-roll!";
                    break;
                default:
                    textBuffer = $"You scored: {score}";
                    break;
            }

            // === Constructs the table ====
            int tableIndent = (consoleWidth / 2) - (textBuffer.Length / 2);
            string tablePadding = new string(' ', tableIndent);

            string tableBars = new string('─', textBuffer.Length + 2);

            // ==== Writes the table to the console ====
            Console.WriteLine($"{tablePadding}┌{tableBars}┐");
            Console.WriteLine($"{tablePadding}│ {textBuffer} │");
            Console.WriteLine($"{tablePadding}└{tableBars}┘");
        }

        
    }
}

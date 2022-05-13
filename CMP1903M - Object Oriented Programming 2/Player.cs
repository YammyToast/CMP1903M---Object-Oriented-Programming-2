using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceGame
{
    /// <summary>
    /// Player object class.
    /// </summary>
    internal class Player
    {
        // Attributes to allow for players to be distingushed from one another.
        public int ID;
        public int Score;
        // Boolean value used to determine whether the player should be treated as a bot.
        private readonly bool isBot;
        public bool IsBot {
            get { return isBot; }
        }

        /// <summary>
        /// Player constructor.
        /// </summary>
        /// <param name="ID">Unique identification value.</param>
        /// <param name="isBot">Player Bot? Boolean.</param>
        public Player(int ID, bool isBot = false) {
            this.ID = ID;
            Score = 0;
            this.isBot = isBot;
        }
    }
}

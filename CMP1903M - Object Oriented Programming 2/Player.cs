using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceGame
{
    internal class Player
    {
        public int ID;
        public int Score;
        public bool isBot;

        public Player(int ID, bool isBot = false) {
            this.ID = ID;
            Score = 0;
            this.isBot = isBot;
        }
    }
}

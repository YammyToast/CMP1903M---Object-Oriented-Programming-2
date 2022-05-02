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

        public Player(int ID) {
            this.ID = ID;
            Score = 0;
        }
    }
}

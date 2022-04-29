using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceGame
{
    internal class Player
    {
        public int Score;
        public List<int> dice;

        public Player(int numberOfDice) {
            dice = new List<int>(numberOfDice);
        }
    }
}

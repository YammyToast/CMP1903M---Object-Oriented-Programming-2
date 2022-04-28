using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceGame
{


    internal class Die
    {
        public int value { get; set; }

        public bool active = true;

        public void Roll(int lowerBoundary, int upperBoundary) { 
            Random rand = new Random();
            int randomNumber = rand.Next(lowerBoundary, upperBoundary);
            value = randomNumber;
        }
    }
}

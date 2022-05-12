using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceGame
{

    /// <summary>
    /// Die class.
    /// </summary>
    internal class Die
    {
        // The value for the 'face' of the dice.
        public int Value { get; set; }
        int lowerBoundary;
        // The amount of faces on the die.
        int upperBoundary;

        public bool Active = true;
        public Die(int lowerBoundary, int upperBoundary) { 
            this.lowerBoundary = lowerBoundary;
            this.upperBoundary = upperBoundary;
        }

        /// <summary>
        /// Rolls a new face for the die.
        /// </summary>
        public void Roll() { 
            Random rand = new Random();
            // Creates a random integer within the boundaries of the faces.
            int randomNumber = rand.Next(lowerBoundary, upperBoundary + 1);
            Value = randomNumber;
        }
    }
}

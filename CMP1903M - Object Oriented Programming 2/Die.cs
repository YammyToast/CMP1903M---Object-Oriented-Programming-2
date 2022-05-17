using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceGame
{

    /// <summary>
    /// Abstracted Die class.
    /// </summary>
    internal class Die
    {
        // The value for the 'face' of the dice.
        private int value; 
        public int Value 
        { 
            get => value;
            set => this.value = value;
        }

        public bool Active 
        {
            get => active;
            set => active = value;
        }

        int lowerBoundary;
        // The amount of faces on the die.
        int upperBoundary;
        
        
        private bool active = true;

        public Die(int upperBoundary) { 
            this.lowerBoundary = 1;
            this.upperBoundary = upperBoundary;
        }

        /// <summary>
        /// Abstracted method for throwing a new face for the die.
        /// </summary>
        public void Roll() { 
            Random rand = new Random();
            // Creates a random integer within the boundaries of the faces.
            int randomNumber = rand.Next(lowerBoundary, upperBoundary + 1);
            value = randomNumber;
        }
    }
}

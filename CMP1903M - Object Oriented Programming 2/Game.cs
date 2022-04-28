using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceGame
{


    interface IGame {

        void Turn();


    }

    internal class Logic {

        List<int> scores = new List<int>(2);
        List<int> diceRolls = new List<int>(5);
            

        int winCondition
        {
            get;
            set;
        }

        /// Game Logic:
        /// Players take turns rolling five dice and score for three-of-a-kind or better. If a player only has
        /// two-of-a-kind, they may re-throw the remaining dice in an attempt to improve the remaining
        /// dice values. If no matching numbers are rolled after two rolls, the player scores 0.
        /// 3 of a kind : 3 points
        /// 4 of a kind: 6 points
        /// 5 of a kind : 12 points
        
        private (RollState state, List<int> diceRolls) RollDice(RollState rollCondition, List<int> diceRolls) { 
            
        }

        // Example dice roll [1, 2, 3, 4, 5]
        // Example dice roll [1, 1, 3, 4, 6]
        // Example dice roll [2, 2, 2, 3, 3]
        // Example dice roll [3, 3, 3, 3, 4]
        // Example dice roll [1, 1, 1, 1, 1]
        private int CalcScore(List<int> diceRolls)
        {
                

        }

    }

    internal class PVP : Logic, IGame
    {
        void IGame.Turn() { 
        
        }       
    }

    internal class PVC  : Logic, IGame
    { 
        
    }

    public enum RollState { 
        None,
        Partial,
        Full
    }

}

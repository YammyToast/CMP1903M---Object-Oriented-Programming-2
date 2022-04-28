namespace DiceGame
{


    interface IGame
    {

        void Turn();


    }

    internal class Logic
    {

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

        // Example dice roll [1, 2, 3, 4, 5]
        // Example dice roll [1, 1, 3, 4, 6]
        // Example dice roll [2, 2, 2, 3, 3]
        // Example dice roll [3, 3, 3, 3, 4]
        // Example dice roll [1, 1, 1, 1, 1]
        public (RollState rollState, List<Die> dice, int score, int occurences) RollDice(RollState rollState, List<Die> dice)
        {
            // ==== Dice Rolling ==== 
            foreach (Die die in dice) {
                if (die.active == true) {
                    die.Roll(1, 6);
                    Console.Write($"{die.value} ");
                }   
            }

            // ==== Occurences ====
            IDictionary<int, int> occurences = new Dictionary<int, int>();
            foreach (Die diceRollsItem in dice)
            {
                if (occurences.ContainsKey(diceRollsItem.value))
                {
                    occurences[diceRollsItem.value]++;
                }
                else
                {
                    occurences.Add(diceRollsItem.value, 1);
                }
            }
            var orderedOccurences = occurences.OrderBy(x => x.Value).Reverse().ToDictionary(x => x.Key, x => x.Value);
            int score = (orderedOccurences.First().Value > 2) ? (int)(3 * Math.Pow(2, (double)orderedOccurences.First().Value - 3)) : 0;
            int occurence = orderedOccurences.First().Value;

            Console.WriteLine($"\n\n{score}");

            if (occurence == 0) {
                rollState = RollState.None;
            } else if (occurence == dice.Count) {
                rollState = RollState.Full;
            } else {
                rollState = RollState.Partial;
            }
            return (rollState, new List<Die>(), score, occurence);
        }

        

    }

    internal class PVP : Logic, IGame
    {
        void IGame.Turn()
        {

        }
    }

    internal class PVC : Logic, IGame
    {
        void IGame.Turn()
        {

        }

    }

    public enum RollState
    {
        None,
        Partial,
        Full
    }

}

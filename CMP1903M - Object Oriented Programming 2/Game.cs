namespace DiceGame
{


    interface IGame
    {

        abstract void Turn();


    }

    internal class Logic
    {

        public List<int> diceRolls = new List<int>(5);
        private List<int> scores = new List<int>(2);
        private int numberOfDice = 0;

        int winCondition
        {
            get;
            set;
        }

        public Logic(int winCondition, int numberOfDice) {
            this.winCondition = winCondition;
            this.numberOfDice = numberOfDice;
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
                if (die.Active == true) {
                    die.Roll(1, 6);

                }   
            }

            // ==== Occurences ====
            IDictionary<int, int> occurences = new Dictionary<int, int>();
            foreach (Die diceRollsItem in dice)
            {
                if (occurences.ContainsKey(diceRollsItem.Value))
                {
                    occurences[diceRollsItem.Value]++;
                }
                else
                {
                    occurences.Add(diceRollsItem.Value, 1);
                }
            }
            // var orderedDice = dice.OrderBy(x => x.value).ToList();
            
            // ==== Ordering and Score Counting ====
            // Finds most occuring dice roll
            var orderedOccurences = occurences.OrderBy(x => x.Value).Reverse().ToDictionary(x => x.Key, x => x.Value);
            // Calculates the score to give the player based on the number of recurring values.
            int score = (orderedOccurences.First().Value > 2) ? (int)(3 * Math.Pow(2, (double)orderedOccurences.First().Value - 3)) : 0;
            int occurence = orderedOccurences.First().Value;
            
            // Orders the dice objects so that matching values are grouped, with the most frequent values first.
            // i.e: 2 2 5 2 6 5   =>   2 2 2 5 5 6
            List<Die> orderedDice = new List<Die>();
            foreach (KeyValuePair<int, int> kvp in orderedOccurences) {
                foreach (Die die in dice) {
                    if (die.Value == kvp.Key) {
                        orderedDice.Add(die);
                    }
                }

            }


            Console.WriteLine($"\n\n{score}");

            // ==== Re-roll Logic ====
            // Set all dice that are part of the valid match to inactive, so they can't be rolled again.
            foreach (Die die in dice) {
                if (die.Value == orderedOccurences.First().Key) {
                    die.Active = false;
                }
                //Console.Write($"{die.active} ");
            }

            // If statements are needed as switch case can only handle constant values.
            if (occurence == 0) {
                rollState = RollState.None;
            } else if (occurence == dice.Count) {
                rollState = RollState.Full;
            } else {
                rollState = RollState.Reroll;
            }


            return (rollState, orderedDice, score, occurence);
        }

        

    }

    internal class PVP : Logic, IGame
    {
        public PVP(int winCondition, int numberOfDice) : base(winCondition, numberOfDice) 
        {
            
        }

        void IGame.Turn()
        {
            List<Die> dice = new List<Die>();
            dice.Add(new Die());
            dice.Add(new Die());
            dice.Add(new Die());
            dice.Add(new Die());
            dice.Add(new Die());
            Console.WriteLine("Rolling");
            RollDice(RollState.Reroll, dice);
        }
    }

    internal class PVC : Logic, IGame
    {
        public PVC(int winCondition, int numberOfDice) : base(winCondition, numberOfDice) { }

        void IGame.Turn()
        {

        }

    }

    public enum RollState
    {
        None,
        Reroll,
        Full
    }

}

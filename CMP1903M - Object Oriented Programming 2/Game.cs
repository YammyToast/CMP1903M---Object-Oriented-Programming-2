namespace DiceGame
{


    interface IGame
    {
        void Game();



    }

    internal class Logic
    {

        protected int winCondition;
        protected int numberOfDice;
        protected int numberOfPlayers;
        protected List<int> scores;

        public Logic(int winCondition, int numberOfDice, int numberOfPlayers) {
            this.winCondition = winCondition;
            this.numberOfDice = numberOfDice;
            this.numberOfPlayers = numberOfPlayers;
            scores = new List<int>(numberOfPlayers);
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
        protected (RollState rollState, List<Die> dice, int score, int occurences) RollDice(RollState rollState, List<Die> dice)
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
            // TODO: Make this algorithm more efficient. Remove elements once sorted?
            List<Die> orderedDice = new List<Die>();
            foreach (KeyValuePair<int, int> kvp in orderedOccurences) {
                foreach (Die die in dice) {
                    if (die.Value == kvp.Key) {
                        orderedDice.Add(die);
                    }
                }

            }


            // ==== Re-roll Logic ====
            // Set all dice that are part of the valid match to inactive, so they can't be rolled again.
            foreach (Die die in dice) {
                if (die.Value == orderedOccurences.First().Key) {
                    die.Active = false;
                }
                //Console.Write($"{die.active} ");
            }

            // If statements are needed as switch case can only handle constant values.
            if (occurence < 2) {
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
        public PVP(int winCondition, int numberOfDice, int numberOfPlayers) : base(winCondition, numberOfDice, numberOfPlayers) {}

        void IGame.Game()
        {
            List<Player> playerList = new List<Player>();

            for (int i = 0; i < numberOfPlayers; i++) {
                playerList.Add(new Player(numberOfDice));
            }

            bool finishedState = false;
            while (finishedState == false) {
                
                foreach (Player player in playerList) { 
                    Turn(player);
                    // Check if the player has won.
                    if (player.Score >= winCondition)
                    {
                        finishedState = true;
                    }
                    else
                    {
                        finishedState = false;
                    }

                }
            
            }
        }

        private RollState Turn(Player player)
        {
            List<Die> dice = new List<Die>();
            dice.Add(new Die());
            dice.Add(new Die());
            dice.Add(new Die());
            Console.WriteLine("Rolling");
            (RollState rollState, List<Die> Dice, int score, int occurences) rollResults = RollDice(RollState.Reroll, dice);
            Console.WriteLine(rollResults.rollState);
            foreach (Die die in rollResults.Dice) {
                Console.WriteLine($"> {die.Value}, {die.Active}");
            }
            return rollResults.rollState;
        }
    }

    internal class PVC : Logic, IGame
    {
        public PVC(int winCondition, int numberOfDice, int numberOfPlayers) : base(winCondition, numberOfDice, numberOfPlayers) { }

        void IGame.Game() {
            Turn();
            
        }
        private RollState Turn()
        {
            return RollState.None;
        }

    }

    public enum RollState
    {
        None,
        Reroll,
        Full
    }

}

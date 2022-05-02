﻿namespace DiceGame
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
            if (occurence == 2) {
                rollState = RollState.Reroll;
            } else if (occurence == dice.Count) {
                rollState = RollState.Full;
            } else {
                rollState = RollState.None;
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

            int playerIndex = 1;
            for (int i = 0; i < numberOfPlayers; i++) {
                playerList.Add(new Player(playerIndex));
                playerIndex++;
            }

            // ==== MAIN LOOP ====
            bool finishedState = false;
            (RollState resultantState, List<Die> dice, int scored) turnResults = (RollState.None, new List<Die>(), -1);
            while (finishedState == false) {

                // Writes the scoreboard at the start of each turn rotation.
                foreach (Player player in playerList) {
                    Console.WriteLine($"> Player {player.ID} : {player.Score}");
                }

                // Cycle through each player, giving them a turn.
                foreach (Player player in playerList) {
                    Console.WriteLine($"Player {player.ID}'s Turn:");

                    List<Die> dice = new List<Die>();
                    for (int i = 0; i < numberOfDice; i++) {
                        dice.Add(new Die());
                    }

                    RollState rollState = RollState.Reroll;
                    while (rollState == RollState.Reroll) {
                        turnResults = Turn(dice);
                        
                        dice = turnResults.dice;
                        rollState = turnResults.resultantState;

                    }
                    player.Score += turnResults.scored;
                    Console.WriteLine($"Scored: {turnResults.scored}\n");

                    // Check if the player has won.
                    if (player.Score >= winCondition)
                    {
                        finishedState = true;
                        break;
                    }
                    else
                    {
                        finishedState = false;
                    }

                }
            
            }
            Console.WriteLine("\n\n\n\n\n\n\n\nGAME HAS ENDED YOU FUCKS");
        }

        private (RollState resultantState, List<Die> dice, int scored) Turn(List<Die> dice)
        {
            // Checks for key-input before rolling.
            Console.WriteLine("\nPress [Enter] to Roll");
            Console.ReadKey();
            // ==== ROLLS THE DICE ====
            (RollState rollState, List<Die> dice, int score, int occurences) rollResults = RollDice(RollState.Reroll, dice);

            // ==== Returns results to the User ====
            foreach (Die die in rollResults.dice)
            {
                Console.Write($"{die.Value} | ");
            }
            Console.WriteLine("\n\n");
            switch (rollResults.rollState) {
                case RollState.None:
                    Console.WriteLine($"Rolled for {rollResults.occurences} of a kind.");
                    break;
                case RollState.Reroll:
                    Console.WriteLine($"Rolled {rollResults.occurences} of a kind, roll for the chance for more! ");
                    break;
                case RollState.Full:
                    Console.WriteLine($"Rolled {rollResults.occurences} for a FULL HOUSE !");
                    break;
            }
            


            return (rollResults.rollState, rollResults.dice, rollResults.score);
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

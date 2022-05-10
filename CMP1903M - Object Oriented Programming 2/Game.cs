namespace DiceGame
{


    interface IGame
    {
        void Game();
        void CreatePlayers(int playerCount, int botCount = 0);



    }

    internal abstract class Logic
    {

        protected int winCondition;
        protected int numberOfDice;
        protected int numberOfPlayers;
        protected List<int> scores;
        protected List<Player> playerList;

        public Logic(int winCondition, int numberOfDice, int numberOfPlayers) {
            this.winCondition = winCondition;
            this.numberOfDice = numberOfDice;
            this.numberOfPlayers = numberOfPlayers;
            scores = new List<int>(numberOfPlayers);
            playerList = new List<Player>();

        }

        virtual protected void DisplayWinMessage(Player player) { 
            
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

        protected void DisplayDiceTable(List<Die> dice, int occurences) {
            // Gets console width to allow for table to be centered.
            int consoleWidth = Console.WindowWidth;

            // ==== Determine the longest line in the table ====
            string diceBuffer = "";
            foreach (Die die in dice) {
                diceBuffer += (die == dice.Last()) ? $" {die.Value } " : $" {die.Value } │";
            }
            string occurencesBuffer = $"── Rolled for {occurences} of a kind ──";

            int longestLength = (diceBuffer.Length > occurencesBuffer.Length) ? diceBuffer.Length : occurencesBuffer.Length;


            // ==== Construct each line of the table ====
            string headerBars = new string('─', longestLength);
            string header = $"┌{headerBars[..((headerBars.Length / 2) - 1)]} Dice {headerBars[((headerBars.Length / 2) + 3)..]}┐";

            string diceBars = new string(' ', (longestLength - diceBuffer.Length) + 2);
            string diceLine = $"│{diceBars[..(diceBars.Length / 2)]}{diceBuffer}{diceBars[(diceBars.Length / 2)..]}│"; 

            string footerBars = new string('─', longestLength - occurencesBuffer.Length);
            string footer = $"└{footerBars[..(footerBars.Length / 2)]}─{occurencesBuffer}─{footerBars[(footerBars.Length / 2)..]}┘"; 

            int tableIndent = (consoleWidth / 2) - (longestLength / 2);
            string tablePadding = new string(' ', tableIndent);

            // ==== Write the table to the console ====
            Console.WriteLine($"{tablePadding}{header}");
            Console.WriteLine($"{tablePadding}{diceLine}");
            Console.WriteLine($"{tablePadding}{footer}");
        }

        protected void DisplayGameScoreTable(List<Player> playerList) {



        }

        protected void DisplayTurnScoreTable(RollState rollState, int score) { 
            
            int consoleWidth = Console.WindowWidth;
            string textBuffer = string.Empty;

            switch (rollState) {
                case RollState.Full:
                    textBuffer = $"MAXIMUM POINTS: {score}";
                    break;
                case RollState.Reroll:
                    textBuffer = $"You got a re-roll!";
                    break;
                default:
                    textBuffer = $"You scored: {score}";
                    break;
            }

            int tableIndent = (consoleWidth / 2) - (textBuffer.Length / 2);
            string tablePadding = new string(' ', tableIndent);

            string tableBars = new string('─', textBuffer.Length + 2);


            Console.WriteLine($"{tablePadding}┌{tableBars}┐");
            Console.WriteLine($"{tablePadding}│ {textBuffer} │");
            Console.WriteLine($"{tablePadding}└{tableBars}┘");
        }


        

    }

    internal class PVP : Logic, IGame
    {
        public PVP(int winCondition, int numberOfDice, int numberOfPlayers) : base(winCondition, numberOfDice, numberOfPlayers) {}

        void IGame.CreatePlayers(int playerCount, int botCount) {

            int playerIndex = 1;
            for (int i = 0; i < numberOfPlayers; i++)
            {
                playerList.Add(new Player(playerIndex, true));
                playerIndex++;
            }
        }

        void IGame.Game()
        {
            
            // ==== MAIN LOOP ====
            bool finishedState = false;
            (RollState resultantState, List<Die> dice, int scored) turnResults = (RollState.None, new List<Die>(), -1);
            while (finishedState == false) {

                // Writes the scoreboard at the start of each turn rotation.
                // TODO: Add scoreboard function.
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
                    RollState lastState = RollState.None;
                    int scored = 0;
                    while (rollState == RollState.Reroll) {
                        turnResults = Turn(dice);

                        dice = turnResults.dice;
                        scored = turnResults.scored;

                        if (turnResults.resultantState == RollState.Reroll && lastState != RollState.Reroll)
                        {
                            DisplayTurnScoreTable(rollState, turnResults.scored);
                        }
                        // Handles case where re-roll occurs twice.
                        else if (turnResults.resultantState == RollState.Reroll && lastState == RollState.Reroll) {
                            scored = 0;
                            // NO MORE REROLLING!!!!!
                            turnResults.resultantState = RollState.None;
                        }

                        rollState = turnResults.resultantState;
                        lastState = turnResults.resultantState;

                    }
                    player.Score += scored;

                    DisplayTurnScoreTable(rollState, scored);


                    // Check if the player has won.
                    if (player.Score >= winCondition)
                    {
                        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                        // Remove this upon creating tie-breaker.
                        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                        DisplayWinMessage(player);
                        finishedState = true;
                        break;
                    }
                    else
                    {
                        finishedState = false;
                    }

                }
            
            }
            
        }

        private (RollState resultantState, List<Die> dice, int scored) Turn(List<Die> dice)
        {
            // Checks for key-input before rolling.
            Console.WriteLine("\nPress [Enter] to Roll");
            Console.ReadKey();
            // ==== ROLLS THE DICE ====
            (RollState rollState, List<Die> dice, int score, int occurences) rollResults = RollDice(RollState.Reroll, dice);

            // ==== Returns results to the User ====
            DisplayDiceTable(rollResults.dice, rollResults.occurences);

            return (rollResults.rollState, rollResults.dice, rollResults.score);
        }

        
    }   

    internal class PVC : Logic, IGame
    {
        public PVC(int winCondition, int numberOfDice, int numberOfPlayers) : base(winCondition, numberOfDice, numberOfPlayers) { }

        void IGame.CreatePlayers(int playerCount, int botCount) {
            int playerIndex = 1;
            for (int i = 0; i < playerCount; i++)
            {
                playerList.Add(new Player(playerIndex, false));
                playerIndex++;
            }
            for (int j = 0; j < botCount; j++) {
                playerList.Add(new Player(playerIndex, true));
                playerIndex++;
            }
        }

        void IGame.Game() {
            // ==== MAIN LOOP ====
            bool finishedState = false;
            (RollState resultantState, List<Die> dice, int scored) turnResults = (RollState.None, new List<Die>(), -1);
            while (finishedState == false)
            {

                // Writes the scoreboard at the start of each turn rotation.
                // TODO: Add scoreboard function.
                foreach (Player player in playerList)
                {
                    Console.WriteLine($"> Player {player.ID} : {player.Score}");
                }

                // Cycle through each player, giving them a turn.
                foreach (Player player in playerList)
                {
                    Console.WriteLine($"Player {player.ID}'s Turn:");

                    List<Die> dice = new List<Die>();
                    for (int i = 0; i < numberOfDice; i++)
                    {
                        dice.Add(new Die());
                    }

                    RollState rollState = RollState.Reroll;
                    RollState lastState = RollState.None;
                    int scored = 0;
                    while (rollState == RollState.Reroll)
                    {
                        turnResults = Turn(dice, player.isBot);

                        dice = turnResults.dice;
                        scored = turnResults.scored;

                        if (turnResults.resultantState == RollState.Reroll && lastState != RollState.Reroll)
                        {
                            DisplayTurnScoreTable(rollState, turnResults.scored);
                        }
                        // Handles case where re-roll occurs twice.
                        else if (turnResults.resultantState == RollState.Reroll && lastState == RollState.Reroll)
                        {
                            scored = 0;
                            // NO MORE REROLLING!!!!!
                            turnResults.resultantState = RollState.None;
                        }

                        rollState = turnResults.resultantState;
                        lastState = turnResults.resultantState;

                    }
                    player.Score += scored;

                    DisplayTurnScoreTable(rollState, scored);


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
            Console.WriteLine("Game Over");

        }
        private (RollState resultantState, List<Die> dice, int scored) Turn(List<Die> dice, bool isBot)
        {
            // Checks for key-input before rolling.
            if (isBot)
            {
                Thread.Sleep(1000);
            }
            else {
                Console.WriteLine("\nPress [Enter] to Roll");
                Console.ReadKey();
            }
            
            // ==== ROLLS THE DICE ====
            (RollState rollState, List<Die> dice, int score, int occurences) rollResults = RollDice(RollState.Reroll, dice);

            // ==== Returns results to the User ====
            DisplayDiceTable(rollResults.dice, rollResults.occurences);

            return (rollResults.rollState, rollResults.dice, rollResults.score);
        }

        protected override void DisplayWinMessage(Player player)
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

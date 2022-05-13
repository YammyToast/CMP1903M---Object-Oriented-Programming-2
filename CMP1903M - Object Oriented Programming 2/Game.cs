namespace DiceGame
{

    // ======== Game Interface ========
    interface IGame
    {
        void Game();
        void CreatePlayers(int playerCount, int botCount = 0);
    }

    // ======== Base Game Logic Class ========
    /// <summary>
    /// Holds all logic that can be shared between the two game modes.
    /// </summary>
    internal abstract class Logic
    {

        protected int winCondition;
        protected int numberOfDice;
        protected int numberOfPlayers;
        protected int scoreMultiplier;
        protected int lowerDiceBoundary;
        protected int upperDiceBoundary;
        protected List<int> scores;
        protected List<Player> playerList;

        protected Tables tableHandler = new Tables();
        /// <summary>
        /// Constructor for the game variables.
        /// </summary>
        /// <param name="winCondition">Score needed to be reached to win the game.</param>
        /// <param name="numberOfDice">Number of dice to be rolled on each turn.</param>
        /// <param name="numberOfPlayers">Number of players playing the game. (Includes bots)</param>
        /// <param name="scoreMultiplier">Score multiplier to be used.</param>
        /// <param name="lowerDiceBoundary">Lowest Rollable Dice Value</param>
        /// <param name="upperDiceBoundary">Highest Rollable Dice Value</param>
        public Logic(int winCondition, int numberOfDice, int numberOfPlayers, int scoreMultiplier, int upperDiceBoundary) {
            this.winCondition = winCondition;
            this.numberOfDice = numberOfDice;
            this.numberOfPlayers = numberOfPlayers;
            this.scoreMultiplier = scoreMultiplier;

            this.upperDiceBoundary = upperDiceBoundary;
            scores = new List<int>(numberOfPlayers);
            playerList = new List<Player>();

        }

      

        /// Game Logic:
        /// Players take turns rolling five dice and score for three-of-a-kind or better. If a player only has
        /// two-of-a-kind, they may re-throw the remaining dice in an attempt to improve the remaining
        /// dice values. If no matching numbers are rolled after two rolls, the player scores 0.
        /// 3 of a kind : 3 points
        /// 4 of a kind: 6 points
        /// 5 of a kind : 12 points

        
        
        /// <summary>
        /// Performs a dice roll on all of the dice in the game.
        /// </summary>
        /// <param name="rollState">Current State for conditional rolling</param>
        /// <param name="dice"></param>
        /// <returns>A tuple of: the new RollState, a list of newly-rolled dice, the score acquired for that roll, and the
        /// amount of occurences of the most commonly occuring dice value.
        /// </returns>
        protected (RollState rollState, List<Die> dice, int score, int occurences) RollDice(RollState rollState, List<Die> dice)
        {
            // ==== Dice Rolling ==== 
            foreach (Die die in dice) {
                // Only rolls dice that are not part of the most commonly occuring value.
                if (die.Active == true) {
                    die.Roll();

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
            // Uses the score multiplier.
            int score = (orderedOccurences.First().Value > 2) ? (int)(scoreMultiplier * Math.Pow(2, (double)orderedOccurences.First().Value - 3)) : 0;
            
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

            }

            // If statements are needed, as switch case can only handle constant values.
            // If the most commonly occuring value occurs twice, allow for one re-roll.
            // If all of the values rolled are the same, set a 'full' state for a special output message.
            // If there are no repeating occurences, set the state to none.
            if (occurence == 2) {
                rollState = RollState.Reroll;
            } else if (occurence == dice.Count) {
                rollState = RollState.Full;
            } else {
                rollState = RollState.None;
            }

            // Return a tuple of all the calculated values.
            return (rollState, orderedDice, score, occurence);
        }

        // ==== End of Game Win Message ====

        // This method is seperated from the other table methods, as it is overridden in the PVC class.
        
        /// <summary>
        /// Displays a 'decorated' line to the console of the player that has won the game.
        /// </summary>
        /// <param name="player">The player object of the player that has won the game</param>
        virtual protected void DisplayWinMessage(Player player)
        {
            int consoleWidth = Console.WindowWidth;
            string lineBuffer = $"────|┤  Player-{player.ID} has won the game! ├|────";

            int tableIndent = (consoleWidth / 2) - (lineBuffer.Length / 2);
            string tablePadding = new string(' ', tableIndent);

            Console.WriteLine("\n\n\n");
            Console.WriteLine($"{tablePadding}{lineBuffer}");
            Console.WriteLine("\n\n\n");
        }


    }

    /// <summary>
    /// Class used for the PVP game mode.
    /// </summary>
    internal class PVP : Logic, IGame
    {
        // Calls the base game Logic Constructor.
        public PVP(int winCondition,
                   int numberOfDice,
                   int numberOfPlayers,
                   int scoreMultiplier,

                   int upperDiceBoundary) : 
                   base(winCondition,
                       numberOfDice,
                       numberOfPlayers,
                       scoreMultiplier,

                       upperDiceBoundary) {}

        /// <summary>
        /// Method used to instantiate the player objects for the game.
        /// </summary>
        void IGame.CreatePlayers(int playerCount, int botCount) {

            int playerIndex = 1;
            for (int i = 0; i < numberOfPlayers; i++)
            {
                playerList.Add(new Player(playerIndex, false));
                playerIndex++;
            }
        }

        /// <summary>
        /// Main Game method used for the PVP gamemode.
        /// </summary>
        void IGame.Game()
        {
            
            // Initialises an empty turnResults object (necessary as C# demands it).
            (RollState resultantState, List<Die> dice, int scored) turnResults = (RollState.None, new List<Die>(), -1);

            // ==== MAIN LOOP ====
            bool finishedState = false;
            while (finishedState == false) {

                // Displays the current scores of all the players in the game at the start of the turn.
                tableHandler.DisplayGameScoreTable(playerList);

                // Cycle through each player, giving them a turn.
                foreach (Player player in playerList) {
                    Console.WriteLine($"Player {player.ID}'s Turn:");

                    // Initialise a set of new dice (pseudo-fairness?)
                    List<Die> dice = new List<Die>();
                    for (int i = 0; i < numberOfDice; i++) {
                        // Creates die with the user-defined boundaries.
                        dice.Add(new Die(lowerDiceBoundary, upperDiceBoundary));
                    }

                    // Initialise the roll-states.
                    RollState rollState = RollState.Reroll;
                    RollState lastState = RollState.None;
                    int scored = 0;
                    // While the user is allowed to reroll.
                    while (rollState == RollState.Reroll) {

                        // ==== Turn Logic ====
                        // Calls the main Turn method.
                        turnResults = Turn(dice);

                        dice = turnResults.dice;
                        scored = turnResults.scored;

                        // If a valid reroll is earned, display the turn results table,
                        // with the message appropriate message.
                        if (turnResults.resultantState == RollState.Reroll && lastState != RollState.Reroll)
                        {
                            tableHandler.DisplayTurnScoreTable(rollState, turnResults.scored);
                        }
                        // Handles case where re-roll occurs twice.
                        else if (turnResults.resultantState == RollState.Reroll && lastState == RollState.Reroll) {
                            scored = 0;
                            // Sets the state to none, to disable consecutive rerolling.
                            turnResults.resultantState = RollState.None;
                        }

                        // Sets the new Roll state, and also saves as the 'last state'.
                        rollState = turnResults.resultantState;
                        lastState = turnResults.resultantState;

                    }

                    // Increment the player's overall score, with the score earned this turn.
                    player.Score += scored;

                    // Displays the turn score table, displaying to the user how many points they scored that turn.
                    tableHandler.DisplayTurnScoreTable(rollState, scored);

                    // ==== Check Win ====
                    // Check if the player has a score higher than the winCondition.
                    if (player.Score >= winCondition)
                    {
                        // Displays the scores of all the players.
                        tableHandler.DisplayGameScoreTable(playerList);
                        // Displays the win message for the player that won.
                        DisplayWinMessage(player);
                        // Ends the game loop.
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

        /// <summary>
        /// Performs one turn for a player.
        /// </summary>
        /// <param name="dice">The list of dice that are to be rolled.</param>
        /// <returns>A tuple of the new Roll-State, the rolled dice, and the score the player earned.</returns>
        private (RollState resultantState, List<Die> dice, int scored) Turn(List<Die> dice)
        {
            // Checks for key-input before rolling.
            Console.WriteLine("\nPress [Enter] to Roll");
            Console.ReadKey();
            // ==== ROLLS THE DICE ====
            (RollState rollState, List<Die> dice, int score, int occurences) rollResults = RollDice(RollState.Reroll, dice);

            // ==== Returns results of the roll to the User ====
            tableHandler.DisplayDiceTable(rollResults.dice, rollResults.occurences);

            return (rollResults.rollState, rollResults.dice, rollResults.score);
        }

        
    }   

    /// <summary>
    /// Class for the Player vs Computer game mode.
    /// </summary>
    internal class PVC : Logic, IGame
    {
        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        // !!!! MOST OF THESE METHODS ARE IDENTICAL TO THE PVP CLASS !!!!
        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        // Differing methods: CreatePlayers, Turn, DisplayWinMessage

        public PVC(int winCondition, int numberOfDice, int numberOfPlayers, int scoreMultiplier, int upperDiceBoundary) : base(winCondition, numberOfDice, numberOfPlayers, scoreMultiplier, upperDiceBoundary) { }

        void IGame.CreatePlayers(int playerCount, int botCount) {
            int playerIndex = 1;
            for (int i = 0; i < playerCount; i++)
            {
                playerList.Add(new Player(playerIndex, false));
                playerIndex++;
            }
            // Adds all of the specified bot players to the player list.
            for (int j = 0; j < botCount; j++) {
                // IsBot attribute is set to true.
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

                tableHandler.DisplayGameScoreTable(playerList);


                foreach (Player player in playerList)
                {
                    string playerType = (player.isBot) ? "Bot" : "Player";
                    Console.WriteLine($"{playerType} {player.ID}'s Turn:");

                    List<Die> dice = new List<Die>();
                    for (int i = 0; i < numberOfDice; i++)
                    {
                        dice.Add(new Die(lowerDiceBoundary, upperDiceBoundary));
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
                            tableHandler.DisplayTurnScoreTable(rollState, turnResults.scored);
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

                    tableHandler.DisplayTurnScoreTable(rollState, scored);


                    // Check if the player has won.
                    if (player.Score >= winCondition)
                    {
                        tableHandler.DisplayGameScoreTable(playerList);
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
        private (RollState resultantState, List<Die> dice, int scored) Turn(List<Die> dice, bool isBot)
        {
            // If the player object 'isBot' attribute is true, then don't wait for a key-input, as this is a bot.
            if (isBot)
            {
                // Wait 1 second to 'simulate' a key input.
                Thread.Sleep(1000);
            }
            else {
                Console.WriteLine("\nPress [Enter] to Roll");
                Console.ReadKey();
            }
            
            // ==== ROLLS THE DICE ====
            (RollState rollState, List<Die> dice, int score, int occurences) rollResults = RollDice(RollState.Reroll, dice);

            // ==== Returns results to the User ====
            tableHandler.DisplayDiceTable(rollResults.dice, rollResults.occurences);

            return (rollResults.rollState, rollResults.dice, rollResults.score);
        }

        /// <summary>
        /// Displays a 'decorated' line to the console of the player that has won the game.
        /// Includes a special variant for when a bot wins.
        /// </summary>
        /// <param name="player">Player object of the winning player.</param>
        protected override void DisplayWinMessage(Player player)
        {
            int consoleWidth = Console.WindowWidth;
            string playerType = (player.isBot) ? "Bot" : "Player";
            string lineBuffer = $"────|┤  {playerType}-{player.ID} has won the game!  ├|────";
            if (playerType == "Bot")
            {
                lineBuffer = $"────|┤  The robots have conquered! {playerType}-{player.ID} has won the game!  ├|────";
            }
            

            int tableIndent = (consoleWidth / 2) - (lineBuffer.Length / 2);
            string tablePadding = new string(' ', tableIndent);

            Console.WriteLine("\n\n\n");
            Console.WriteLine($"{tablePadding}{lineBuffer}");
            Console.WriteLine("\n\n\n");
        }

    }

    // Roll state Enum.
    public enum RollState
    {
        None,
        Reroll,
        Full
    }

}

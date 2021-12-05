using System;
using System.Collections;

namespace Day04
{
    class Game
    {
        public Queue<int> draws = new Queue<int>();
        public List<Card> cards = new List<Card>();
        public int draw = new int();
        public bool isOver;
        public int winningCard;

        public Game(string[] input)
        {          
            isOver = false;
            winningCard = -1;
            InitDraws(input[0]);
            InitCards(input[2..]);
        }
        
        // Parse first line of input as a queue of numbers to draw
        public void InitDraws(string input_draws)
        {
            foreach (string draw in input_draws.Split(','))
            {
                draws.Enqueue(Int32.Parse(draw));
            }
            if (Globals.debug)
            {
                Console.Write("Draws: ");
                foreach (int draw in draws) Console.Write(draw + " ");
                Console.WriteLine();
            }
        }

        // Parse remaining input lines as a list of Card objects
        public void InitCards(string[] input_cards)
        {
            for (int n = 0; n < input_cards.Length; n += 6)
            {
                int start = n, end = n + 5;
                cards.Add(new Card(input_cards[start..end]));
            }
        }

        // Pop a drawn number from the queue of numbers to draw
        public void DrawNumber()
        {
            draw = draws.Dequeue();
            if (Globals.debug)
            {
                Console.WriteLine($"Draw: {draw}");
            }
        }

        // Update each card's score grid if there's a match with the drawn number
        public void UpdateScores()
        {
            for (int n = 0; n < cards.Count; n++)
            {
                for (int row = 0; row < 5; row++)
                {
                    for (int col = 0; col < 5; col++)
                    {
                        if (cards[n].grid[row][col] == draw) {
                            cards[n].score[row, col] = true;
                            if (Globals.debug)
                            {
                                Console.WriteLine($"Draw Match. Draw: {draw}, Card: {n}, Row: {row}, Col: {col}");
                            }
                        }
                    }
                }
            }
        }

        // Check rows and columns of each card to determine if it's a winner
        public void CheckWinners()
        {
            foreach (Card card in cards)
            {
                card.CheckWinner();
                if (card.isWinner)
                {
                    isOver = true;
                    winningCard = cards.LastIndexOf(card);
                }    
            }
        }

        // For Part 2, ignore winning cards and continue playing unless it's the last card
        public void RemoveWinners()
        {
            if (cards.Count > 1)
            {
                isOver = false;

                List<Card> _cards = new List<Card>();
                foreach (Card card in cards)
                {
                    if (!card.isWinner)
                        _cards.Add(card);  
                }
                cards = _cards;
                if (Globals.debug)
                {
                    Console.WriteLine($"Removed any winning cards. There are {cards.Count} card(s) left");
                    Console.WriteLine();
                }
            }
        }
    }
}
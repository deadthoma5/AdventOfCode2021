public static class Globals
{
    public static bool debug = false;
}

namespace Day04
{
    class Program
    {
        static public int EvaluteScore(Card card, int draw)
        {
            int unmarked = 0;
            for (int row = 0; row < card.score.GetLength(0); row++)
            {
                for (int col = 0; col < card.score.GetLength(1); col++)
                {
                    if (!card.score[row, col])
                        unmarked += card.grid[row][col];
                }
            }
            return unmarked * draw;
        }
        

        // Main program entry point
        static public void Main(string[] args)
        {
            // Obtain input from file
            string[] input;
            if (Globals.debug)
                input = File.ReadAllLines("input_test");
            else
                input = File.ReadAllLines("input");

            // Part 1: Create a new game and initialize draws and cards from input
            Game game1 = new Game(input);

            // Part 1: Main program loop
            do
            {
                game1.DrawNumber();
                game1.UpdateScores();
                game1.CheckWinners();
            } while (!game1.isOver);
            if (Globals.debug)
            {
                Console.WriteLine($"Winning card number: {game1.winningCard}");
            }

            // Part 1: Calculate results.
            int part1 = EvaluteScore(game1.cards[game1.winningCard], game1.draw);
            Console.WriteLine($"Part 1: {part1}");

            // Part 2: Create a new game and initialize draws and cards from input
            Game game2 = new Game(input);

            // Part 2: Main program loop
            do
            {
                game2.DrawNumber();
                game2.UpdateScores();
                game2.CheckWinners();
                game2.RemoveWinners();
            } while (!game2.isOver);
            if (Globals.debug)
            {
                Console.WriteLine($"Final winning card number: {game2.winningCard}");
                Console.WriteLine();
            }

            // Part 2: Calculate results.
            int part2 = EvaluteScore(game2.cards[game2.winningCard], game2.draw);
            Console.WriteLine($"Part 2: {part2}");
        }
    }
}
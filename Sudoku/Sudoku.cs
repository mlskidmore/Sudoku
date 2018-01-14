using System;
using System.IO;

namespace Sudoku
{
    class Sudoku
    {
        private int[] board;

        public Sudoku(String boardValues)
        {
            board = new int[81];
            for (int i = 0; i < boardValues.Length; i++)
            {
                board[i] = int.Parse(boardValues[i].ToString());
            }
        }
        public void Solve()
        {
            try
            {
                placeNumber(0);
                Console.WriteLine("\nFalse!\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // Print the solution
                //Console.WriteLine(this);
            }
        }
        public void placeNumber(int pos)
        {
            if (pos == 81)
            {
                // Puzzle solved...
                throw new Exception("\nTrue!\n");
            }
            // If the board cell has a value...
            if (board[pos] > 0)
            {
                // keep moving...
                placeNumber(pos + 1);
                return;
            }
            // When board cell value is 0, search for it and compare to no.s 1 - 9
            for (int n = 1; n <= 9; n++)
            {
                // If ValidateNumber is true, n is not in the board and can be put in it
                int x = pos % 9;
                int y = pos / 9;

                if (validateNumber(n, x, y))
                {
                    board[pos] = n;
                    placeNumber(pos + 1); // check the next board cell
                    board[pos] = 0;
                }
            }
        }
        public bool validateNumber(int val, int x, int y)
        {
            for (int i = 0; i < 9; i++)
            {
                if (board[y * 9 + i] == val ||  // look across each row
                    board[i * 9 + x] == val)    // look down each column
                    // if val is in the board, return and get next n, val
                    return false;
            }
            // If board call value is not found, look in each 3 X 3 grid
            int startX = (x / 3) * 3;
            int startY = (y / 3) * 3;
            for (int i = startY; i < startY + 3; i++)
            {
                for (int j = startX; j < startX + 3; j++)
                {
                    if (board[i * 9 + j] == val)
                        return false;
                }
            }
            // If val is not found, return and put in grid
            return true;
        }
        public override string ToString()
        {
            string sb = "";
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    sb += (board[i * 9 + j] + " ");
                    if (j == 2 || j == 5)
                        sb += ("| ");
                }
                sb += ('\n');
                if (i == 2 || i == 5)
                    sb += ("------+-------+------\n");
            }
            return sb;
        }
        public static string getUserInput(ref string userInput)
        {
            StreamReader reader = new StreamReader(Console.OpenStandardInput());
            
            string inputRow = null;            
            int rowCount = 0;

            while (!reader.EndOfStream)
            {
                for (int i = 0; i < 9; i++)
                {
                    inputRow = reader.ReadLine();       // Read line from console
                    var numbers = inputRow.Split(',');  // Strip commas

                    foreach (var number in numbers)
                    {
                        if (number == "") // Handle for trailing comma in input row
                            continue;
                        // Validate only numbers 0 - 9 are entered
                        if (Int32.TryParse(number, out int num) || int.Parse(number) < 0 || int.Parse(number) > 9 && inputRow.Length == 81)
                        {
                            userInput += number;
                        }
                    }

                    rowCount++;
                }
                if (rowCount == 9)
                    break;
            }

            return userInput;
        }

        static void Main(string[] args)
        {
            string errorString = "You must enter comma separated, positive integers between 0 and 9 (inclusive).";
                        
            try
            {
                Console.WriteLine("Enter Sudoku board - nine rows of comma-separated, positive\nintegers from 0 through 9 (hit enter after each row): ");

                string userInput = null;
                string boardNumbers = null;

                // Get the user input from the console.
                boardNumbers = getUserInput(ref userInput);

                // Start the game...
                new Sudoku(boardNumbers).Solve();
            }
            catch(Exception e)
            {
                Console.WriteLine(errorString);
                Console.WriteLine("Error: " + e.Message);
                Console.WriteLine("Press any key to exit...");
                Console.ReadLine();
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }
    }
}

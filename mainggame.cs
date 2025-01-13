using System;
using System.Diagnostics;
using System.Linq;

class SudokuApp
{
    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to Sudoku!");
        Console.WriteLine("Select difficulty level:");
        Console.WriteLine("1. Easy\n2. Medium\n3. Hard");

        int level;
        while (true)
        {
            Console.Write("Enter your choice (1/2/3): ");
            if (int.TryParse(Console.ReadLine(), out level) && level >= 1 && level <= 3)
                break;
            Console.WriteLine("Invalid choice. Please select 1, 2, or 3.");
        }

        int[,] puzzle = GenerateSudoku(level);
        int[,] solution = (int[,])puzzle.Clone();
        SolveSudoku(solution);

        Console.WriteLine("\nHere is your Sudoku puzzle:");
        PrintGrid(puzzle);

        Stopwatch stopwatch = Stopwatch.StartNew();

        while (true)
        {
            Console.WriteLine("\nEnter your move in the format 'row col value' (e.g., 1 3 5):");
            Console.WriteLine("Enter '0 0 0' to quit.");

            string input = Console.ReadLine();
            if (input == "0 0 0")
            {
                Console.WriteLine("You quit the game. Goodbye!");
                break;
            }

            string[] parts = input.Split(' ');
            if (parts.Length == 3 && int.TryParse(parts[0], out int row) &&
                int.TryParse(parts[1], out int col) && int.TryParse(parts[2], out int value))
            {
                if (row < 1 || row > 9 || col < 1 || col > 9 || value < 1 || value > 9)
                {
                    Console.WriteLine("Invalid input. Numbers must be between 1 and 9.");
                    continue;
                }

                if (puzzle[row - 1, col - 1] != 0)
                {
                    Console.WriteLine("This cell is already filled and cannot be changed.");
                }
                else
                {
                    puzzle[row - 1, col - 1] = value;
                    PrintGrid(puzzle);

                    if (IsGridComplete(puzzle))
                    {
                        stopwatch.Stop();

                        if (GridsAreEqual(puzzle, solution))
                        {
                            Console.WriteLine($"Congratulations! You solved the Sudoku in {stopwatch.Elapsed:mm\':\'ss}.");
                        }
                        else
                        {
                            Console.WriteLine("The solution is incorrect. Please try again.");
                        }

                        break;
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid input format. Please try again.");
            }
        }
    }

    static int[,] GenerateSudoku(int level)
    {
        // Predefined puzzle templates based on difficulty level.
        // 0 represents empty cells.
        return level switch
        {
            1 => new int[9, 9] // Easy
            {
                {5, 3, 0, 0, 7, 0, 0, 0, 0},
                {6, 0, 0, 1, 9, 5, 0, 0, 0},
                {0, 9, 8, 0, 0, 0, 0, 6, 0},
                {8, 0, 0, 0, 6, 0, 0, 0, 3},
                {4, 0, 0, 8, 0, 3, 0, 0, 1},
                {7, 0, 0, 0, 2, 0, 0, 0, 6},
                {0, 6, 0, 0, 0, 0, 2, 8, 0},
                {0, 0, 0, 4, 1, 9, 0, 0, 5},
                {0, 0, 0, 0, 8, 0, 0, 7, 9}
            },
            2 => new int[9, 9] // Medium
            {
                {0, 0, 0, 6, 0, 0, 4, 0, 0},
                {7, 0, 0, 0, 0, 3, 6, 0, 0},
                {0, 0, 0, 0, 9, 1, 0, 8, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 5, 0, 1, 8, 0, 0, 0, 3},
                {0, 0, 0, 3, 0, 6, 0, 4, 5},
                {0, 4, 0, 2, 0, 0, 0, 6, 0},
                {9, 0, 3, 0, 0, 0, 0, 0, 0},
                {0, 2, 0, 0, 0, 0, 1, 0, 0}
            },
            3 => new int[9, 9] // Hard
            {
                {0, 0, 0, 0, 0, 0, 0, 0, 2},
                {0, 0, 0, 0, 0, 0, 6, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0}
            },
            _ => throw new ArgumentException("Invalid level")
        };
    }

    static void PrintGrid(int[,] grid)
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                Console.Write(grid[i, j] == 0 ? ". " : grid[i, j] + " ");
            }
            Console.WriteLine();
        }
    }

    static bool IsGridComplete(int[,] grid)
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (grid[i, j] == 0)
                    return false;
            }
        }
        return true;
    }

    static bool GridsAreEqual(int[,] grid1, int[,] grid2)
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (grid1[i, j] != grid2[i, j])
                    return false;
            }
        }
        return true;
    }

    static bool SolveSudoku(int[,] grid)
    {
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                if (grid[row, col] == 0)
                {
                    for (int num = 1; num <= 9; num++)
                    {
                        if (IsSafe(grid, row, col, num))
                        {
                            grid[row, col] = num;

                            if (SolveSudoku(grid))
                                return true;

                            grid[row, col] = 0;
                        }
                    }
                    return false;
                }
            }
        }
        return true;
    }

    static bool IsSafe(int[,] grid, int row, int col, int num)
    {
        for (int x = 0; x < 9; x++)
        {
            if (grid[row, x] == num || grid[x, col] == num ||
                grid[row / 3 * 3 + x / 3, col / 3 * 3 + x % 3] == num)
                return false;
        }
        return true;
    }
}

using System;
using System.Collections.Generic;
using System.IO;

namespace SudokuGrader
{
    public class Program
    {
        static void Main(string[] args)
        {
            string filePath;
            int[][] sudokuBoard;
            bool validSudokuSolution;

            // use a file path if one was passed in.  else, use hardcoded file path.
            if (args.Length == 1)
            {
                // won't validate for valid file path. System.IO.File.ReadAllLines would throw exception.
                // placing call to ReadAllLines() in a try-catch structure would be ideal.
                filePath = args[0];
            }
            else
            {
                Console.WriteLine(@"usage: dotnet run --project SudokuGrader.csproj C:\path\to\sudoku\txt\file.txt");

                // this is hardcoded for my dev environment.  if run in an evaluator's environment, FileNotFound exception will likely throw.
                filePath = @"C:\Development\SudokuGrader\TestFiles\sudoku.txt";
            }
            
            sudokuBoard = ValidateFileAndReadTo2DArray(filePath);

            // null return value is not ideal.
            if (sudokuBoard == null)
            {
                // file was not parsed correctly.
                Console.WriteLine("No, invalid");
                return;
            }

            bool validRows = ValidateRows(sudokuBoard);
            bool validColumns = ValidateColumns(sudokuBoard);
            bool validSquares = ValidateSquares(sudokuBoard);

            validSudokuSolution =  validRows && validColumns && validSquares;

            // no return codes, just messaging.
            if (validSudokuSolution)
            {
                Console.WriteLine("Yes, valid");
            }
            else
            {
                Console.WriteLine("No, invalid");
            }
        }

        /// <summary>
        ///  Validates "horizontal rows" of a 9x9 2D int array for no duplication of integers 
        /// </summary>
        /// <param name="sudokuBoard"></param>
        /// <returns>True if no duplication exists</returns>
        public static bool ValidateRows(int[][] sudokuBoard)
        {
            for (int i = 0; i < 9; i++)
            {
                // store occurences of digits 1-9 in an array
                // occurence of n stored at hasOccured[n - 1]
                bool[] hasOccured = new bool[9];

                for (int j = 0; j < 9; j++)
                {
                    if (hasOccured[sudokuBoard[i][j] - 1]) 
                    {
                        return false;
                    }
                    else
                    {
                        hasOccured[sudokuBoard[i][j] - 1] = true;
                    }
                }
            }
            return true;
        }

        /// <summary>
        ///  Validates "vertical columns" of a 9x9 2D int array for no duplication of integers 
        /// </summary>
        /// <param name="sudokuBoard"></param>
        /// <returns>True if no duplication exists</returns>
        public static bool ValidateColumns(int[][] sudokuBoard)
        {
            for (int i = 0; i < 9; i++)
            {
                // store occurences of digits 1-9 in an array
                // occurence of n stored at hasOccured[n - 1]
                bool[] hasOccured = new bool[9];

                for (int j = 0; j < 9; j++)
                {
                    if (hasOccured[sudokuBoard[j][i] - 1])
                    {
                        return false;
                    }
                    else
                    {
                        hasOccured[sudokuBoard[j][i] - 1] = true;
                    }
                }
            }
            return true;
        }

        /// <summary>
        ///  Validates 3x3 "magic squares" of a 9x9 2D int array for no duplication of integers 
        /// </summary>
        /// <param name="sudokuBoard"></param>
        /// <returns>True if no duplication exists</returns>
        public static bool ValidateSquares(int[][] sudokuBoard)
        {
            IEnumerable<Tuple<int, int>> iBounds;
            IEnumerable<Tuple<int, int>> jBounds;

            iBounds = jBounds = new List<Tuple<int, int>>
            {
                new Tuple<int, int>(0, 2),
                new Tuple<int, int>(3, 5),
                new Tuple<int, int>(6, 8)
            };

            // Quadruple nested iterations or ~O(N^4)!
            // However, we know that each level of iteration is limited to 3 elements.
            // In this case, accessing each of the 9 "magic squares" is unavoidable to validate each region of the sudoko board.
            // Accessing each  of the 9 elements in each "magic square" for occurence validation is also unavoidable.
            // The run time for this is O(N^4) where N = 3.
            // This algorithm will never run on truly large datasets.

            foreach (Tuple<int, int> iBoundPair in iBounds)
            {
                foreach (Tuple<int, int> jBoundPair in jBounds)
                {
                    bool[] hasOccured = new bool[9];

                    for (int i = iBoundPair.Item1; i <= iBoundPair.Item2; i++)
                    {
                        for (int j = jBoundPair.Item1; j <= jBoundPair.Item2; j++)
                        {
                            if (hasOccured[sudokuBoard[i][j] - 1])
                            {
                                return false;
                            }
                            else
                            {
                                hasOccured[sudokuBoard[i][j] - 1] = true;
                            }
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        ///  Validates that a file contains 9 lines of 9 integers, and stores the file contents in a 9x9 2D int array.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>int[][] or null if errors</returns>
        public static int[][] ValidateFileAndReadTo2DArray(string filePath)
        {
            string[] fileLines;

            try
            {
                fileLines = System.IO.File.ReadAllLines(filePath);
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("Exception, sudoku file not found.");
                Console.WriteLine(ex.Message);
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception, could not read sudoku file.");
                Console.WriteLine(ex.Message);
                return null;
            }

            if (fileLines.Length != 9)
            {
                return null;
            }

            int[][] fileAs2DArray = new int[9][];

            for (int i = 0; i < 9; i++) // fileLines.Length is confirmed to be 9 above.
            {
                string[] row = fileLines[i].Split(' ');

                if (row.Length != 9)
                {
                    return null;
                }

                int[] intRow = new int[9];
                for (int j = 0; j < 9; j++) // fileLines.Length is confirmed to be 9 above.
                {
                    int cellValue;
                    if (!int.TryParse(row[j], out cellValue) || cellValue < 1 || cellValue > 9)
                    {
                        return null;
                    }

                    intRow[j] = cellValue;
                }
                fileAs2DArray[i] = intRow;
            }
            return fileAs2DArray;
        }
    }
}

# SudokuGrader

TO RUN:


1. Clone this repo, open sln in VS, and build
2. In powershell, navigate to directory inline with SudokuGrader.csproj 
3. In powershell, invoke "dotnet run --project SudokuGrader.csproj C:\full\path\to\test\file.txt"


A take-home coding assignment; console app that validates Sudoku puzzles

To all who evaluate this solution:
In the spirit of timeboxing this to <4 hours, I'll prioritize a working solution over 100% ideal design.

Along the way, I'll make notes of design decisions that I'd prefer to clean up.

Some notes are below, in the readme file.  Other notes appear as inline comments.

If any code that appears is a "dealbreaker", please let me know, and I can provide a refactor commit.

General:

I chose not to write automated tests due to time.  Instead, I relied on test files for a manual integration test strategy.

Some validation of the "rules of Sudoku" occur implicitly:

1. During file parsing, I ensure that each row/column has 9 integers where 1 <= x <= 9
2. During row/column/square validation, I ensure that integers are not duplicated.
3. I DO NOT verify that each integer exists - instead that has occured implicity during steps 1 and 2.
4. Explicitly verifying that each integer exists (through the boolean array hasOccured in each validating method) would simply cost an extra 9 array accesses in each of the 27 "regions" (row/col/square)


ReadFileTo2DArray()


Initially, I'll return null if file doesn't parse to a 9x9 grid of numbers where 1<=x<=9.

Ideally, I'd throw a meaningful exception.  I avoid null returns in production code.

Initially, this method will have multiple return locations.

Ideally, I'd have a single return location, instead of falling out with null returns in many locations
	as I catch errors.



ValidateRows() and ValidateColumns()


These methods duplicate each other with the exception of cell indexing ... [i][j] vs [j][i].

Ideally, I could pass a "transposeRowOrColumn" boolean parameter, to specify traversal or row vs column.

I'll leave them separated for simplicity.


ValidateSquares()


To get the cross product of the 3 sets of "square boundaries" for rows and columns (i and j), I am settling on an easy data structure - 2 separate enumerables of high/low boundary pairs.


I could improve the variable names for iBounds, jBounds, iBoundPair, jBoundPair.

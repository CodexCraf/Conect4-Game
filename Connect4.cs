using System;
using System.Collections.Generic;

struct PlayerInfo
{
	public string PlayerName;
	public char PlayerID;
}

public class Program
{
	private static Stack<int> moveHistory = new Stack<int>();
	private static Random random = new Random();
	public static void Main(string[] args)
	{
		Console.WriteLine("Welcome to Connect 4!");
		Console.WriteLine("Select Game Mode:");
		Console.WriteLine("1. Single Player");
		Console.WriteLine("2. Two Players");
		int gameMode = int.Parse(Console.ReadLine());
		if (gameMode != 1 && gameMode != 2)
		{
			Console.WriteLine("Invalid selection. Exiting...");
			return;
		}

		PlayerInfo playerOne, playerTwo;
		playerOne.PlayerID = 'X';
		playerTwo.PlayerID = 'O';
		if (gameMode == 1)
		{
			Console.Write("Enter your name: ");
			playerOne.PlayerName = Console.ReadLine();
			playerTwo.PlayerName = "AI";
		}
		else
		{
			Console.Write("Player One please enter your name: ");
			playerOne.PlayerName = Console.ReadLine();
			Console.Write("Player Two please enter your name: ");
			playerTwo.PlayerName = Console.ReadLine();
		}

		char[, ] board = new char[9, 10];
		int dropChoice, win, full, again;
		do
		{
			ResetBoard(board);
			ResetMoveHistory();
			full = 0;
			win = 0;
			again = 0;
			DisplayBoard(board);
			do
			{
				dropChoice = PlayerDrop(board, playerOne);
				CheckBellow(board, playerOne, dropChoice);
				moveHistory.Push(dropChoice);
				DisplayBoard(board);
				win = CheckFour(board, playerOne);
				if (win == 1)
				{
					PlayerWin(playerOne);
					again = Restart(board);
					if (again == 2)
					{
						break;
					}
				}

				if (gameMode == 1 && again != 2)
				{
					dropChoice = AIPlayer(board);
					CheckBellow(board, playerTwo, dropChoice);
					moveHistory.Push(dropChoice);
					Console.WriteLine("AI's Move:");
					DisplayBoard(board);
					win = CheckFour(board, playerTwo);
					if (win == 1)
					{
						PlayerWin(playerTwo);
						again = Restart(board);
						if (again == 2)
						{
							break;
						}
					}
				}
				else if (gameMode == 2 && again != 2)
				{
					dropChoice = PlayerDrop(board, playerTwo);
					CheckBellow(board, playerTwo, dropChoice);
					moveHistory.Push(dropChoice);
					DisplayBoard(board);
					win = CheckFour(board, playerTwo);
					if (win == 1)
					{
						PlayerWin(playerTwo);
						again = Restart(board);
						if (again == 2)
						{
							break;
						}
					}
				}

				full = FullBoard(board);
				if (full == 7)
				{
					Console.WriteLine("The board is full, it is a draw!");
					again = Restart(board);
				}
			}
			while (again != 2);
		}
		while (again == 1);
	}

	static int PlayerDrop(char[, ] board, PlayerInfo activePlayer)
	{
		int dropChoice;
		bool validInput;
		do
		{
			Console.Write(activePlayer.PlayerName + "'s Turn ");
			Console.Write("Please enter a number between 1 and 7");
			if (activePlayer.PlayerName == "AI")
				Console.Write(" (or 'undo' to undo): ");
			else
				Console.Write(": ");
			string input = Console.ReadLine();
			if (input.ToLower() == "undo" && activePlayer.PlayerName != "AI")
			{
				UndoLastMove(board);
				continue;
			}

			validInput = int.TryParse(input, out dropChoice);
			if (validInput && dropChoice >= 1 && dropChoice <= 7 && board[1, dropChoice] == '*')
			{
				break;
			}

			Console.WriteLine("Invalid input or column is full. Please enter a number between 1 and 7.");
		}
		while (true);
		return dropChoice;
	}

	static void ResetBoard(char[, ] board)
	{
		for (int i = 1; i <= 6; i++)
		{
			for (int j = 1; j <= 7; j++)
			{
				board[i, j] = '*';
			}
		}
	}

	static void ResetMoveHistory()
	{
		moveHistory.Clear();
	}

	static void UndoLastMove(char[, ] board)
	{
		if (moveHistory.Count >= 2)
		{
			int lastMove = moveHistory.Pop();
			int secondLastMove = moveHistory.Pop();
			for (int i = 1; i <= 6; i++)
			{
				if (board[i, lastMove] != '*')
				{
					board[i, lastMove] = '*';
					break;
				}
			}

			for (int i = 1; i <= 6; i++)
			{
				if (board[i, secondLastMove] != '*')
				{
					board[i, secondLastMove] = '*';
					break;
				}
			}
		}
		else
		{
			Console.WriteLine("Cannot undo further.");
		}
	}

	static void CheckBellow(char[, ] board, PlayerInfo activePlayer, int dropChoice)
	{
		int length = 6;
		int turn = 0;
		do
		{
			if (board[length, dropChoice] != 'X' && board[length, dropChoice] != 'O')
			{
				board[length, dropChoice] = activePlayer.PlayerID;
				turn = 1;
			}
			else
			{
				length--;
			}
		}
		while (turn != 1);
	}

	static void DisplayBoard(char[, ] board)
	{
		Console.WriteLine(" 1 2 3 4 5 6 7");
		for (int i = 1; i <= 6; i++)
		{
			Console.Write("|");
			for (int j = 1; j <= 7; j++)
			{
				Console.Write(board[i, j] + " ");
			}

			Console.WriteLine("|");
		}
	}

	static int CheckFour(char[, ] board, PlayerInfo activePlayer)
	{
		char XO = activePlayer.PlayerID;
		int win = 0;
		for (int i = 8; i >= 1; i--)
		{
			for (int j = 9; j >= 1; j--)
			{
				if (board[i, j] == XO && board[i - 1, j - 1] == XO && board[i - 2, j - 2] == XO && board[i - 3, j - 3] == XO)
				{
					win = 1;
				}

				if (board[i, j] == XO && board[i, j - 1] == XO && board[i, j - 2] == XO && board[i, j - 3] == XO)
				{
					win = 1;
				}

				if (board[i, j] == XO && board[i - 1, j] == XO && board[i - 2, j] == XO && board[i - 3, j] == XO)
				{
					win = 1;
				}

				if (board[i, j] == XO && board[i - 1, j + 1] == XO && board[i - 2, j + 2] == XO && board[i - 3, j + 3] == XO)
				{
					win = 1;
				}

				if (board[i, j] == XO && board[i, j + 1] == XO && board[i, j + 2] == XO && board[i, j + 3] == XO)
				{
					win = 1;
				}
			}
		}

		return win;
	}

	static int FullBoard(char[, ] board)
	{
		int full = 0;
		for (int i = 1; i <= 7; i++)
		{
			if (board[1, i] != '*')
			{
				full++;
			}
		}

		return full;
	}

	static void PlayerWin(PlayerInfo activePlayer)
	{
		Console.WriteLine("\n" + activePlayer.PlayerName + " Connected Four, You Win!");
	}

	static int Restart(char[, ] board)
	{
		int restart;
		Console.Write("Would you like to restart? Yes(1) No(2): ");
		restart = int.Parse(Console.ReadLine());
		if (restart == 1)
		{
			ResetBoard(board);
			ResetMoveHistory();
			DisplayBoard(board);
		}
		else
		{
			Console.WriteLine("Goodbye!");
		}

		return restart;
	}

	static int AIPlayer(char[, ] board)
	{
		List<int> availableColumns = new List<int>();
		for (int col = 1; col <= 7; col++)
		{
			if (board[1, col] == '*')
			{
				availableColumns.Add(col);
			}
		}

		if (availableColumns.Count == 0)
		{
			return -1;
		}

		int randomIndex = random.Next(availableColumns.Count);
		return availableColumns[randomIndex];
	}
}

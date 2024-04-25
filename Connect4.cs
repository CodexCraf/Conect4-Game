using System;
using System.Collections.Generic;

// player calss for players or ai
public class Player
{
	public string Name { get; private set; }
	public char Symbol { get; private set; }

	public Player(string name, char symbol)
	{
		Name = name;
		Symbol = symbol;
	}

	// MakeMove method should be implemented in derived classes.
	public virtual int MakeMove(Board board)
	{
		throw new NotImplementedException("MakeMove method should be implemented in derived classes.");
	}
}

// class for human player derived from player class
public class HumanPlayer : Player
{
	public HumanPlayer(string name, char symbol) : base(name, symbol)
	{
	}

	public override int MakeMove(Board board)
	{
		int column;
		do
		{
			Console.Write(Name + "'s turn: ");
			if (!int.TryParse(Console.ReadLine(), out column))
			{
				Console.WriteLine("Invalid input. Please enter a number.");
				continue;
			}

			if (column < 1 || column > board.Width)
			{
				Console.WriteLine("Invalid column. Please choose a column between 1 and 7.");
				continue;
			}

			if (!board.IsValidMove(column))
			{
				Console.WriteLine("Column is full. Please choose another column.");
				continue;
			}

			break;
		}
		while (true);
		return column;
	}
}

//  AIPlayer class for ai
public class AIPlayer : Player
{
	private Random random;
	public AIPlayer(char symbol) : base("AI", symbol)
	{
		random = new Random();
	}

	public override int MakeMove(Board board)
	{
		List<int> availableColumns = board.GetAvailableColumns();
		return availableColumns[random.Next(availableColumns.Count)];
	}
}

// Board class for the game board
public class Board
{
	public int Width { get; private set; }
	public int Height { get; private set; }

	private char[, ] grid;
	public Board(int width, int height)
	{
		Width = width;
		Height = height;
		grid = new char[height, width];
		InitializeBoard();
	}

	private void InitializeBoard()
	{
		for (int row = 0; row < Height; row++)
		{
			for (int col = 0; col < Width; col++)
			{
				grid[row, col] = '*';
			}
		}
	}

	// IsValidMove method checks if a move is valid
	public bool IsValidMove(int column)
	{
		return grid[0, column - 1] == '*';
	}

	// IsFull method checks if the board is full
	public bool IsFull()
	{
		for (int col = 0; col < Width; col++)
		{
			if (grid[0, col] == '*')
			{
				return false;
			}
		}

		return true;
	}

	public List<int> GetAvailableColumns()
	{
		List<int> availableColumns = new List<int>();
		for (int col = 0; col < Width; col++)
		{
			if (IsValidMove(col + 1))
			{
				availableColumns.Add(col + 1);
			}
		}

		return availableColumns;
	}

	// symbol on the board (*)
	public void PlaceSymbol(int column, char symbol)
	{
		for (int row = Height - 1; row >= 0; row--)
		{
			if (grid[row, column - 1] == '*')
			{
				grid[row, column - 1] = symbol;
				break;
			}
		}
	}

	// check cell if its occupied
	public bool IsCellOccupied(int row, int column)
	{
		return grid[row, column] != '*';
	}

	// returns the symbol at a cell
	public char GetSymbol(int row, int column)
	{
		return grid[row, column];
	}

	// Print method prints the board
	public void Print()
	{
		for (int row = 0; row < Height; row++)
		{
			for (int col = 0; col < Width; col++)
			{
				Console.Write("| " + grid[row, col]);
			}

			Console.WriteLine("|");
		}

		Console.WriteLine(" 1  2  3  4  5  6  7");
	}
}

// Connect4Game class for the main game
public class Connect4Game
{
	private Board board;
	private Player player1;
	private Player player2;
	public Connect4Game(Player player1, Player player2, int width = 7, int height = 6)
	{
		this.player1 = player1;
		this.player2 = player2;
		board = new Board(width, height);
	}

	// Start method starts the game
	public void Start()
	{
		Console.WriteLine("Welcome to Connect 4!");
		Console.WriteLine("Game started!");
		Console.WriteLine();
		Player currentPlayer = player1;
		Player winner = null;
		while (winner == null && !board.IsFull())
		{
			board.Print();
			int column = currentPlayer.MakeMove(board);
			board.PlaceSymbol(column, currentPlayer.Symbol);
			if (HasConnectedFour(currentPlayer.Symbol, column))
			{
				winner = currentPlayer;
			}

			currentPlayer = (currentPlayer == player1) ? player2 : player1;
		}

		board.Print();
		if (winner != null)
		{
			Console.WriteLine(winner.Name + " wins!");
		}
		else
		{
			Console.WriteLine("It's a draw!");
		}
	}

	// checks if there are four connected symbols
	private bool HasConnectedFour(char symbol, int column)
	{
		for (int row = 0; row <= board.Height - 4; row++)
		{
			for (int col = 0; col < board.Width; col++)
			{
				if (board.GetSymbol(row, col) == symbol && board.GetSymbol(row + 1, col) == symbol && board.GetSymbol(row + 2, col) == symbol && board.GetSymbol(row + 3, col) == symbol)
				{
					return true;
				}
			}
		}

		for (int row = 0; row < board.Height; row++)
		{
			for (int col = 0; col <= board.Width - 4; col++)
			{
				if (board.GetSymbol(row, col) == symbol && board.GetSymbol(row, col + 1) == symbol && board.GetSymbol(row, col + 2) == symbol && board.GetSymbol(row, col + 3) == symbol)
				{
					return true;
				}
			}
		}

		for (int row = 0; row <= board.Height - 4; row++)
		{
			for (int col = 0; col <= board.Width - 4; col++)
			{
				if (board.GetSymbol(row, col) == symbol && board.GetSymbol(row + 1, col + 1) == symbol && board.GetSymbol(row + 2, col + 2) == symbol && board.GetSymbol(row + 3, col + 3) == symbol)
				{
					return true;
				}
			}
		}

		for (int row = 3; row < board.Height; row++)
		{
			for (int col = 0; col <= board.Width - 4; col++)
			{
				if (board.GetSymbol(row, col) == symbol && board.GetSymbol(row - 1, col + 1) == symbol && board.GetSymbol(row - 2, col + 2) == symbol && board.GetSymbol(row - 3, col + 3) == symbol)
				{
					return true;
				}
			}
		}

		return false;
	}
}

// entry point of the application
public class Program
{
	public static void Main(string[] args)
	{
		int choice;
		do
		{
			Console.WriteLine("Select Game Mode:");
			Console.WriteLine("1. Single Player");
			Console.WriteLine("2. Two Players");
			int gameMode = int.Parse(Console.ReadLine());
			if (gameMode != 1 && gameMode != 2)
			{
				Console.WriteLine("Invalid selection. Exiting...");
				return;
			}

			Console.Write("Enter Player 1 name: ");
			string player1Name = Console.ReadLine();
			Player player1 = new HumanPlayer(player1Name, 'X');
			Player player2;
			if (gameMode == 1)
			{
				player2 = new AIPlayer('O');
			}
			else
			{
				Console.Write("Enter Player 2 name: ");
				string player2Name = Console.ReadLine();
				player2 = new HumanPlayer(player2Name, 'O');
			}

			Connect4Game game = new Connect4Game(player1, player2);
			game.Start();
			do
			{
				Console.WriteLine("Would you like to play again? (1 for yes, 0 for no): ");
			}
			while (!int.TryParse(Console.ReadLine(), out choice) || (choice != 0 && choice != 1));
			Console.WriteLine("Goodbye!");
		}
		while (choice == 1);
	}
}

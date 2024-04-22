using System;

class Board
{
    private char[,] gameBoard; // Renamed from 'board'
    private const int Rows = 6;
    private const int Cols = 7;

    public Board()
    {
        gameBoard = new char[Rows, Cols];
        InitializeBoard();
    }

    private void InitializeBoard()
    {
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Cols; j++)
            {
                gameBoard[i, j] = ' ';
            }
        }
    }

    public void Display()
    {
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Cols; j++)
            {
                Console.Write("|" + gameBoard[i, j]);
            }
            Console.WriteLine("|");
        }
        Console.WriteLine("-----------------");
    }

    public bool IsColumnFull(int col)
    {
        return gameBoard[0, col] != ' ';
    }

    public bool DropPiece(int col, char piece)
    {
        for (int i = Rows - 1; i >= 0; i--)
        {
            if (gameBoard[i, col] == ' ')
            {
                gameBoard[i, col] = piece;
                return true;
            }
        }
        return false;
    }

    public bool CheckWinner(char piece)
    {
        // Check rows
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Cols - 3; j++)
            {
                if (gameBoard[i, j] == piece && gameBoard[i, j + 1] == piece && gameBoard[i, j + 2] == piece && gameBoard[i, j + 3] == piece)
                    return true;
            }
        }

        // Check columns
        for (int i = 0; i < Rows - 3; i++)
        {
            for (int j = 0; j < Cols; j++)
            {
                if (gameBoard[i, j] == piece && gameBoard[i + 1, j] == piece && gameBoard[i + 2, j] == piece && gameBoard[i + 3, j] == piece)
                    return true;
            }
        }

        // Check diagonals \
        for (int i = 0; i < Rows - 3; i++)
        {
            for (int j = 0; j < Cols - 3; j++)
            {
                if (gameBoard[i, j] == piece && gameBoard[i + 1, j + 1] == piece && gameBoard[i + 2, j + 2] == piece && gameBoard[i + 3, j + 3] == piece)
                    return true;
            }
        }

        // Check diagonals /
        for (int i = 3; i < Rows; i++)
        {
            for (int j = 0; j < Cols - 3; j++)
            {
                if (gameBoard[i, j] == piece && gameBoard[i - 1, j + 1] == piece && gameBoard[i - 2, j + 2] == piece && gameBoard[i - 3, j + 3] == piece)
                    return true;
            }
        }

        return false;
    }
}

abstract class Player
{
    public string Name { get; set; }
    public char Piece { get; set; }

    public abstract int MakeMove(Board board);
}

class HumanPlayer : Player
{
    public override int MakeMove(Board board)
    {
        while (true)
        {
            try
            {
                Console.Write($"{Name}, enter column number (0-6): ");
                int col = int.Parse(Console.ReadLine());
                if (col >= 0 && col < 7 && !board.IsColumnFull(col))
                    return col;
                else
                    Console.WriteLine("Invalid column or column is full. Try again.");
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input. Please enter a number.");
            }
        }
    }
}

class Connect4Game
{
    private Board gameBoard;
    private Player[] players;
    private int currentPlayer;

    public Connect4Game()
    {
        gameBoard = new Board();
        players = new Player[] { new HumanPlayer { Name = "Player 1", Piece = 'X' }, new HumanPlayer { Name = "Player 2", Piece = 'O' } };
        currentPlayer = 0;
    }

    public void Play()
    {
        while (true)
        {
            gameBoard.Display();
            int col = players[currentPlayer].MakeMove(gameBoard);
            gameBoard.DropPiece(col, players[currentPlayer].Piece);

            if (gameBoard.CheckWinner(players[currentPlayer].Piece))
            {
                gameBoard.Display();
                Console.WriteLine($"{players[currentPlayer].Name} wins!");
                break;
            }
            if (AllColumnsFull())
            {
                gameBoard.Display();
                Console.WriteLine("It's a draw!");
                break;
            }

            currentPlayer = (currentPlayer + 1) % 2;
        }
    }

    private bool AllColumnsFull()
    {
        for (int col = 0; col < 7; col++)
        {
            if (!gameBoard.IsColumnFull(col))
                return false;
        }
        return true;
    }
}

class Program
{
    static void Main(string[] args)
    {
        Connect4Game game = new Connect4Game();
        game.Play();
    }
}

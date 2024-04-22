using System;

class Board
{
    private char[,] board;
    private const int Rows = 6;
    private const int Cols = 7;

    public Board()
    {
        board = new char[Rows, Cols];
        InitializeBoard();
    }

    private void InitializeBoard()
    {
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Cols; j++)
            {
                board[i, j] = ' ';
            }
        }
    }

    public void Display()
    {
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Cols; j++)
            {
                Console.Write("|" + board[i, j]);
            }
            Console.WriteLine("|");
        }
        Console.WriteLine("-----------------");
    }

    public bool IsColumnFull(int col)
    {
        return board[0, col] != ' ';
    }

    public bool DropPiece(int col, char piece)
    {
        for (int i = Rows - 1; i >= 0; i--)
        {
            if (board[i, col] == ' ')
            {
                board[i, col] = piece;
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
                if (board[i, j] == piece && board[i, j + 1] == piece && board[i, j + 2] == piece && board[i, j + 3] == piece)
                    return true;
            }
        }

        // Check columns
        for (int i = 0; i < Rows - 3; i++)
        {
            for (int j = 0; j < Cols; j++)
            {
                if (board[i, j] == piece && board[i + 1, j] == piece && board[i + 2, j] == piece && board[i + 3, j] == piece)
                    return true;
            }
        }

        // Check diagonals \
        for (int i = 0; i < Rows - 3; i++)
        {
            for (int j = 0; j < Cols - 3; j++)
            {
                if (board[i, j] == piece && board[i + 1, j + 1] == piece && board[i + 2, j + 2] == piece && board[i + 3, j + 3] == piece)
                    return true;
            }
        }

        // Check diagonals /
        for (int i = 3; i < Rows; i++)
        {
            for (int j = 0; j < Cols - 3; j++)
            {
                if (board[i, j] == piece && board[i - 1, j + 1] == piece && board[i - 2, j + 2] == piece && board[i - 3, j + 3] == piece)
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
    private Board board;
    private Player[] players;
    private int currentPlayer;

    public Connect4Game()
    {
        board = new Board();
        players = new Player[] { new HumanPlayer { Name = "Player 1", Piece = 'X' }, new HumanPlayer { Name = "Player 2", Piece = 'O' } };
        currentPlayer = 0;
    }

    public void Play()
    {
        while (true)
        {
            board.Display();
            int col = players[currentPlayer].MakeMove(board);
            board.DropPiece(col, players[currentPlayer].Piece);

            if (board.CheckWinner(players[currentPlayer].Piece))
            {
                board.Display();
                Console.WriteLine($"{players[currentPlayer].Name} wins!");
                break;
            }
            if (AllColumnsFull())
            {
                board.Display();
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
            if (!board.IsColumnFull(col))
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

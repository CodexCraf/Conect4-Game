using System;

struct PlayerInfo
{
    public string PlayerName;
    public char PlayerID;
}

class Program
{
    static void Main(string[] args)
    {
        PlayerInfo playerOne, playerTwo;
        char[,] board = new char[9, 10];
        int dropChoice, win, full, again;

        Console.WriteLine("Let's Play Connect 4\n");

        Console.Write("Player One please enter your name: ");
        playerOne.PlayerName = Console.ReadLine();
        playerOne.PlayerID = 'X';

        Console.Write("Player Two please enter your name: ");
        playerTwo.PlayerName = Console.ReadLine();
        playerTwo.PlayerID = 'O';

        full = 0;
        win = 0;
        again = 0;

        DisplayBoard(board);
        do
        {
            dropChoice = PlayerDrop(board, playerOne);
            CheckBellow(board, playerOne, dropChoice);
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

            dropChoice = PlayerDrop(board, playerTwo);
            CheckBellow(board, playerTwo, dropChoice);
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
            full = FullBoard(board);
            if (full == 7)
            {
                Console.WriteLine("The board is full, it is a draw!");
                again = Restart(board);
            }

        } while (again != 2);
    }

    static int PlayerDrop(char[,] board, PlayerInfo activePlayer)
    {
        int dropChoice;
        do
        {
            Console.Write($"{activePlayer.PlayerName}'s Turn ");
            Console.Write("Please enter a number between 1 and 7: ");
            string input = Console.ReadLine();

            if (int.TryParse(input, out dropChoice))
            {
                if (dropChoice >= 1 && dropChoice <= 7 && board[1, dropChoice] != 'X' && board[1, dropChoice] != 'O')
                {
                    break;
                }
            }

            Console.WriteLine("Invalid input. Please enter a number between 1 and 7.");
        } while (true);

        return dropChoice;
    }

    static void CheckBellow(char[,] board, PlayerInfo activePlayer, int dropChoice)
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

        } while (turn != 1);
    }

    static void DisplayBoard(char[,] board)
    {
        int rows = 6, columns = 7;

        for (int i = 1; i <= rows; i++)
        {
            Console.Write("|");
            for (int ix = 1; ix <= columns; ix++)
            {
                if (board[i, ix] != 'X' && board[i, ix] != 'O')
                {
                    board[i, ix] = '*';
                }

                Console.Write(board[i, ix]);
            }

            Console.WriteLine("|");
        }
    }

    static int CheckFour(char[,] board, PlayerInfo activePlayer)
    {
        char XO = activePlayer.PlayerID;
        int win = 0;

        for (int i = 8; i >= 1; i--)
        {
            for (int ix = 9; ix >= 1; ix--)
            {
                if (board[i, ix] == XO &&
                    board[i - 1, ix - 1] == XO &&
                    board[i - 2, ix - 2] == XO &&
                    board[i - 3, ix - 3] == XO)
                {
                    win = 1;
                }

                if (board[i, ix] == XO &&
                    board[i, ix - 1] == XO &&
                    board[i, ix - 2] == XO &&
                    board[i, ix - 3] == XO)
                {
                    win = 1;
                }

                if (board[i, ix] == XO &&
                    board[i - 1, ix] == XO &&
                    board[i - 2, ix] == XO &&
                    board[i - 3, ix] == XO)
                {
                    win = 1;
                }

                if (board[i, ix] == XO &&
                    board[i - 1, ix + 1] == XO &&
                    board[i - 2, ix + 2] == XO &&
                    board[i - 3, ix + 3] == XO)
                {
                    win = 1;
                }

                if (board[i, ix] == XO &&
                    board[i, ix + 1] == XO &&
                    board[i, ix + 2] == XO &&
                    board[i, ix + 3] == XO)
                {
                    win = 1;
                }
            }
        }

        return win;
    }

    static int FullBoard(char[,] board)
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
        Console.WriteLine($"\n{activePlayer.PlayerName} Connected Four, You Win!");
    }

    static int Restart(char[,] board)
    {
        int restart;

        Console.Write("Would you like to restart? Yes(1) No(2): ");
        restart = int.Parse(Console.ReadLine());
        if (restart == 1)
        {
            for (int i = 1; i <= 6; i++)
            {
                for (int ix = 1; ix <= 7; ix++)
                {
                    board[i, ix] = '*';
                }
            }
        }
        else
        {
            Console.WriteLine("Goodbye!");
        }

        return restart;
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace GemGame
{
    class Engine
    {
        #region Fields
        private const int INITIAL_SCORE = 0;
        private const int NAME_WINDOW_WIDTH = 50;
        private const int NAME_WINDOW_HEIGHT = 10;
        private const int PLAYER_FIELD_WIDTH = 33;
        private const int PLAYER_FIELD_HEIGHT = 46;

        private const string WINDOW_TITLE = "GEM GAME";
        private  ConsoleColor[] boxColors = { ConsoleColor.Yellow, ConsoleColor.Blue, ConsoleColor.Red, ConsoleColor.Green, ConsoleColor.Cyan, ConsoleColor.Magenta };
     

        private int cursorX = 0;
        private int cursorY = 0;
        private static int[] lastSelection = { -1, -1 };


        private bool menuCommand = false;
        private bool endGame = false;
        private bool soundFlag = true;
        private bool elementSelected = false;

        private Player player;
        private static GameField gamefield = new GameField();
        #endregion

        public Engine()
        {

        }
        public void StartEngine()
        {
            this.DrawGameWindow(PLAYER_FIELD_WIDTH, PLAYER_FIELD_HEIGHT, WINDOW_TITLE);
            this.DrawMenu();
            this.StartMenuLoop();
        }
        private void DrawInGameMenu()
        {
            Console.ForegroundColor = ConsoleColor.White;
            for (int i = 0; i <= PLAYER_FIELD_WIDTH - 1; i++)
            {
                Console.SetCursorPosition(i, 33);
                Console.Write(Chars.MAINCHAR);
            }
            for (int i = 0; i <= PLAYER_FIELD_HEIGHT - 34 - 2; i++)
            {
                Console.SetCursorPosition(0, 34 + i);
                Console.Write(Chars.MAINCHAR);
                Console.SetCursorPosition(PLAYER_FIELD_WIDTH - 1, 34 + i);
                Console.Write(Chars.MAINCHAR);

            }
            for (int i = 0; i <= PLAYER_FIELD_WIDTH - 1; i++)
            {
                Console.SetCursorPosition(i, PLAYER_FIELD_HEIGHT - 2);
                Console.Write(Chars.MAINCHAR);
            }

            Console.SetCursorPosition(2, 35);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Player: ");
            Console.SetCursorPosition(2, 36);
            Console.Write("Score: ");
            Console.SetCursorPosition(2, 37);
            Console.Write("Move - ");
            Console.SetCursorPosition(2, 38);
            Console.Write("Select Box - ");
            Console.SetCursorPosition(2, 39);
            Console.Write("Quit To Menu - ");

            Console.SetCursorPosition(2 + "Player: ".Length, 35);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(player.Name);
            Console.SetCursorPosition(2 + "Score: ".Length, 36);
            Console.Write(player.Score);
            Console.SetCursorPosition(2 + "Move - ".Length, 37);
            Console.Write("Arrow Keys");
            Console.SetCursorPosition(2 + "Select Box - ".Length, 38);
            Console.Write("Enter");
            Console.SetCursorPosition(2 + "Quit To Menu - ".Length, 39);
            Console.Write("Esc");

        }


        private void DrawGameWindow(int x, int y, string title)
        {
            Console.SetWindowSize(x, y);
            Console.SetBufferSize(x, y);
            Console.CursorVisible = false;
            Console.Title = title;

        }

        private void ChangeCursorPosition()
        {
            if (elementSelected)
            {
                DoSwap(cursorX, cursorY);
            }

            gamefield.DrawIfCursorPosition();

            gamefield[cursorX, cursorY].isCursorPosition = true;
            gamefield[cursorX, cursorY].DrawBox();
        }

        private void DoSwap(int x, int y)
        {
            Swap(gamefield[lastSelection[0], lastSelection[1]], gamefield[cursorX, cursorY]);

            if (CheckIfEmpty(FindBoxesToRemove()))
            {
                Swap(gamefield[lastSelection[0], lastSelection[1]], gamefield[cursorX, cursorY]);
            }
            else
            {
                gamefield[cursorX, cursorY].isCursorPosition = false;
                gamefield[cursorX, cursorY].DrawBox();
                FallDownAndGenerateGems();
            }
        }
        private void Swap(Box i, Box j)
        {
            int tempX = i.X;
            int tempY = i.Y;
            i.X = j.X;
            i.Y = j.Y;
            j.X = tempX;
            j.Y = tempY;

            elementSelected = false;
            i.isSelected = false;
            j.isSelected = false;
            i.DrawBox();
            j.DrawBox();

            Box tempGem = gamefield[lastSelection[0], lastSelection[1]];
            gamefield[lastSelection[0], lastSelection[1]] = gamefield[cursorX, cursorY];
            gamefield[cursorX, cursorY] = tempGem;
        }

        private void FallDownAndGenerateGems()
        {
            Random rand = new Random();
            bool[,] boxesToRemove = new bool[8, 8];
            do
            {
                boxesToRemove = FindBoxesToRemove();

                int gemCount = GetBoxesToRemove(boxesToRemove);

                gamefield.DestroyGems(boxesToRemove);

                while (!gamefield.isFull())
                {
                    for (int y = gamefield.GetLength(0) - 2; y >= 0; y--)
                    {
                        for (int x = gamefield.GetLength(1) - 1; x >= 0; x--)
                        {
                            if (gamefield[x, y].Color != ConsoleColor.Black && gamefield[x, y + 1].Color == ConsoleColor.Black)
                            {
                                Thread.Sleep(50);
                                lastSelection[0] = x;
                                lastSelection[1] = y;
                                cursorX = x;
                                cursorY = y + 1;
                                Swap(gamefield[x, y], gamefield[x, y + 1]);
                            }

                            if (y == 0 && gamefield[x, y].Color == ConsoleColor.Black)
                            {
                                gamefield[x, y].Color = boxColors[rand.Next(0, boxColors.Length)];
                                Thread.Sleep(30);
                                gamefield[x, y].initBox(Chars.LIGHTCHAR);
                                gamefield[x, y].DrawBox();
                                Thread.Sleep(50);
                                gamefield[x, y].initBox(Chars.MEDIUMCHAR);
                                gamefield[x, y].DrawBox();
                                Thread.Sleep(50);
                                gamefield[x, y].initBox(Chars.DARKCHAR);
                                gamefield[x, y].DrawBox();
                                Thread.Sleep(50);
                                gamefield[x, y].initBox(Chars.MAINCHAR);
                                gamefield[x, y].DrawBox();
                            }
                        }
                    }
                }

                Array.Clear(boxesToRemove, 0, boxesToRemove.Length);

            } while (!CheckIfEmpty(FindBoxesToRemove()));
        }

        private bool[,] FindBoxesToRemove()
        {
            int currentSeq = 1;
            int bestSeq = int.MinValue;
            int bestSeqX = 0;
            int bestSeqY = 0;
            int bestSeqDirection = 1;
            bool finishFlag = false;
            bool[,] selectedCells = new bool[gamefield.GetLength(0), gamefield.GetLength(1)];

            do
            {
                //Vertical Check
                for (int x = 0; x < gamefield.GetLength(0); x++)
                {
                    for (int y = 0; y < gamefield.GetLength(1) - 1; y++)
                    {
                        if (gamefield[x, y].Color == gamefield[x, y + 1].Color && selectedCells[x, y] == false)
                        {
                            currentSeq++;
                        }
                        else
                        {
                            currentSeq = 1;
                        }

                        if (currentSeq > bestSeq)
                        {
                            bestSeq = currentSeq;
                            bestSeqX = x;
                            bestSeqY = y + 1;
                            bestSeqDirection = 1;

                        }
                    }
                    currentSeq = 1;
                }
                //Horizontal Check
                for (int y = 0; y < gamefield.GetLength(1); y++)
                {
                    for (int x = 0; x < gamefield.GetLength(0) - 1; x++)
                    {
                        if (gamefield[x, y].Color == gamefield[x + 1, y].Color && selectedCells[x, y] == false)
                        {
                            currentSeq++;
                        }
                        else
                        {
                            currentSeq = 1;
                        }

                        if (currentSeq > bestSeq)
                        {
                            bestSeq = currentSeq;
                            bestSeqX = x + 1;
                            bestSeqY = y;
                            bestSeqDirection = 2;

                        }
                    }
                    currentSeq = 1;
                }
                if (bestSeq >= 3)
                {
                    switch (bestSeqDirection)
                    {
                        case 1:
                            for (int i = bestSeqY; i >= Math.Abs(bestSeq - bestSeqY - 1); i--)
                            {
                                selectedCells[bestSeqX, i] = true;
                            }
                            break;
                        case 2:
                            for (int i = bestSeqX; i >= Math.Abs(bestSeq - bestSeqX - 1); i--)
                            {
                                selectedCells[i, bestSeqY] = true;
                            }
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    finishFlag = true;
                }
                currentSeq = 1;
                bestSeq = int.MinValue;
                bestSeqX = 0;
                bestSeqY = 0;
                bestSeqDirection = 1;
            } while (finishFlag == false);

            return selectedCells;
        }
        public static int GetBoxesToRemove(bool[,] boxesToRemove)
        {
            int jewelCount = 0;

            for (int y = 0; y < boxesToRemove.GetLength(0); y++)
            {
                for (int x = 0; x < boxesToRemove.GetLength(1); x++)
                {
                    if (boxesToRemove[x, y])
                    {
                        jewelCount += 10;
                    }
                }
            }
            return jewelCount;
        }
        private void DrawMenu()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(Console.WindowWidth / 5, Console.WindowHeight / 3 - 3);
            Console.WriteLine("WELCOME TO GEM GAME!");
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(Console.WindowWidth / 3 + 1, Console.WindowHeight / 3 - 1);
            Console.WriteLine("MAIN MENU");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(Console.WindowWidth / 3 - 1, Console.WindowHeight / 3 + 3);
            Console.WriteLine("N - New game");
            Console.SetCursorPosition(Console.WindowWidth / 3 - 1, Console.WindowHeight / 3 + 5);
            Console.WriteLine("L - Load game");
        }


        private string GetPlayerName()
        {
            Console.SetWindowSize(NAME_WINDOW_WIDTH, NAME_WINDOW_HEIGHT);
            Console.SetBufferSize(NAME_WINDOW_WIDTH, NAME_WINDOW_HEIGHT);

            string playerName = "";
            string maxSymbols = "(Max 13 symbols)";
            string playerNameQuestion = Names.GetNames();
            int cursorWidth = (Console.WindowWidth - playerNameQuestion.Length) / 2;
            if (cursorWidth < 0) cursorWidth = 0;
            while ((playerName == "") || (playerName.Length > 13))
            {
                Console.Clear();
                Console.SetCursorPosition(cursorWidth, Console.WindowHeight / 2 - 1);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(playerNameQuestion);
                Console.SetCursorPosition((Console.WindowWidth - maxSymbols.Length) / 2, Console.WindowHeight / 2 + 1);
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(maxSymbols);
                Console.SetCursorPosition(Console.WindowWidth / 2 - 2, Console.WindowHeight / 2 );
                Console.ForegroundColor = ConsoleColor.White;
                Console.CursorVisible = true;
                playerName = Console.ReadLine();
            }
            Console.CursorVisible = false;
            return playerName;
        }
        private void StartMenuLoop()
        {
            menuCommand = false;

            while (!menuCommand)
            {
                ConsoleKeyInfo keyPressed = Console.ReadKey(true);
                switch (keyPressed.Key)
                {
                    case ConsoleKey.N:
                        {
                            endGame = false;
                            player = new Player(this.GetPlayerName(), INITIAL_SCORE);
                            Console.Clear();
                            Console.SetWindowSize(PLAYER_FIELD_WIDTH, PLAYER_FIELD_HEIGHT);
                            Console.SetBufferSize(PLAYER_FIELD_WIDTH, PLAYER_FIELD_HEIGHT);
                            this.DrawInGameMenu();
                            gamefield = new GameField();
                            gamefield.InitializeGameField(Chars.MAINCHAR);
                            gamefield.DrawGameField();
                            FallDownAndGenerateGems();
                            this.StartGameLoop();
                            menuCommand = true;
                            break;
                        }
                    case ConsoleKey.Escape:
                        {
                            Environment.Exit(0);
                            menuCommand = true;
                            break;
                        }
                    default:
                        {
                            menuCommand = false;
                            break;
                        }
                }

            }

        }
        private bool CheckIfEmpty(bool[,] boxesToRemove)
        {
            for (int i = 0; i < boxesToRemove.GetLength(0); i++)
            {
                for (int j = 0; j < boxesToRemove.GetLength(1); j++)
                {
                    if (boxesToRemove[i, j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        private void StartGameLoop()
        {
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyPressed = Console.ReadKey(true);
                    switch (keyPressed.Key)
                    {
                        case ConsoleKey.LeftArrow:
                            {
                                if ((cursorX > 0) && (!endGame))
                                {
                                    cursorX--;
                                    if (soundFlag) { Console.Beep(400, 100); }
                                    ChangeCursorPosition();

                                }
                                break;

                            }
                        case ConsoleKey.RightArrow:
                            {
                                if ((cursorX < 7) && (!endGame))
                                {
                                    cursorX++;
                                    if (soundFlag) { Console.Beep(400, 100); }
                                    ChangeCursorPosition();
                                }
                                break;
                            }
                        case ConsoleKey.UpArrow:
                            {
                                if ((cursorY > 0) && (!endGame))
                                {
                                    cursorY--;
                                    if (soundFlag) { Console.Beep(400, 100); }
                                    ChangeCursorPosition();
                                }
                                break;
                            }
                        case ConsoleKey.DownArrow:
                            {
                                if ((cursorY < 7) && (!endGame))
                                {
                                    cursorY++;
                                    if (soundFlag) { Console.Beep(400, 100); }
                                    ChangeCursorPosition();
                                }
                                break;
                            }
                        case ConsoleKey.Enter:
                            {
                                if (!endGame)
                                {
                                    if (soundFlag) { Console.Beep(400, 100); }
                                    gamefield[cursorX, cursorY].isSelected = true;
                                    gamefield[cursorX, cursorY].DrawBox();
                                    elementSelected = true;
                                    lastSelection[0] = cursorX;
                                    lastSelection[1] = cursorY;
                                }
                                break;
                            }
                        case ConsoleKey.Escape:
                            {
                                endGame = true;
                                this.DrawMenu();
                                this.StartMenuLoop();
                                break;
                            }





                    }
                }
            }
            
           

        }

    }
}

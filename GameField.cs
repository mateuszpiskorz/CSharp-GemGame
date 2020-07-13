using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace GemGame
{
    class GameField
    {

        private const int GAME_ROW = 8;
        private const int GAME_COL = 8;
        private Box[,] boxes = new Box[GAME_ROW, GAME_COL];
        private static ConsoleColor[] BoxColors = { ConsoleColor.Yellow, ConsoleColor.Red, ConsoleColor.Magenta, ConsoleColor.Green, ConsoleColor.Blue };


        public Box this[int i, int j]
        {
            get
            {
                return this.boxes[i, j];
            }
            set
            {
                this.boxes[i, j] = value;
            }
        }
        public int GetLength(int dim)
        {
            if (dim == 0)
            {
                return boxes.GetLength(0);
            }
            else
            {
                return boxes.GetLength(1);
            }
        }

        public GameField()
        {

        }
        public void InitializeGameField(char symbol)
        {
            Random rand = new Random();
            for (int i = 0; i < boxes.GetLength(0); i++)
            {
                for (int j = 0; j < boxes.GetLength(1); j++)
                {
                    Box box = new Box(i * 4 + 1, j * 4 + 1, symbol, BoxColors[rand.Next(0, BoxColors.Length)]);
                    box.initBox(symbol);
                    boxes[i, j] = box;

                }
            }

        }

        public void DrawIfCursorPosition()
        {
            for (int i = 0; i < boxes.GetLength(0); i++)
            {
                for (int j = 0; j < boxes.GetLength(1); j++)
                {
                    if (boxes[i, j].isCursorPosition)
                    {
                        boxes[i, j].isCursorPosition = false;
                    }
                    boxes[i, j].DrawBox();
                }
            }
        }

        public void DrawGameField()
        {
            for (int i = 0; i < boxes.GetLength(0); i++)
            {
                for (int j = 0; j < boxes.GetLength(1); j++)
                { 
                    boxes[i, j].DrawBox();
                }

            }
        }
        public void DestroyGems(bool[,] boxesToRemove)
        {
            Thread.Sleep(400); //TODO: Adjust Speed
            for (int y = 0; y < boxes.GetLength(0); y++)
            {
                for (int x = 0; x < boxes.GetLength(1); x++)
                {
                    if (boxesToRemove[x, y] == true)
                    {
                        this.boxes[x, y].initBox(Chars.DARKCHAR); 
                        this.boxes[x, y].DrawBox();

                        Thread.Sleep(50);
                        this.boxes[x, y].initBox(Chars.MEDIUMCHAR);   
                        this.boxes[x, y].DrawBox();

                        Thread.Sleep(50);
                        this.boxes[x, y].initBox(Chars.LIGHTCHAR);   
                        this.boxes[x, y].DrawBox();

                        Thread.Sleep(50);
                        boxes[x, y].Color = ConsoleColor.Black;

                        Thread.Sleep(50);
                        this.boxes[x, y].initBox(Chars.MAINCHAR); 
                        this.boxes[x, y].DrawBox();
                    }
                }
            }
        }

        public bool isFull()
        {
            for (int y = 0; y < boxes.GetLength(0); y++)
            {
                for (int x = 0; x < boxes.GetLength(1); x++)
                {
                    if (boxes[x, y].Color == ConsoleColor.Black)
                    {
                        return false;
                    }
                    
                }
            }
            return true;
        }
    }
}

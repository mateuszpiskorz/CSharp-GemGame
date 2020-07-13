using System;
using System.Collections.Generic;
using System.Text;

namespace GemGame
{
    class Box
    {
       
        private int x;
        private int y;
        private char symbol;
        private ConsoleColor color;

        private char[,] boxSize = new char[3, 3];

        public bool isSelected;
        public bool isCursorPosition;

        #region Properties
        public int X {
            get
            {
                if (x < 0)
                {
                    throw new Exception("The Box 'X' coordinate can't be negative.");
                }
                return x;

            }
            set
            {
                if (x < 0)
                {
                    throw new Exception("The Box 'X' coordinate can't be negative.");

                }
                x = value;

            }
        }

        public int Y
        {
            get
            {
                if (y < 0)
                {
                    throw new Exception("The Box 'Y' coordinate can't be negative.");
                }
                return y;

            }
            set
            {
                if (y < 0)
                {
                    throw new Exception("The Box 'Y' coordinate can't be negative.");

                }
                y = value;

            }
        }

        public char Symbol
        {
            get { return symbol; }
            set
            {
                if (!Char.IsSymbol(value))
                {
                    throw new Exception("Box symbol is not valid symbol.");
                }
                symbol = value;
            }
        }

        public ConsoleColor Color { get; set; }
        #endregion

        public Box():this(0,0, Chars.MAINCHAR, ConsoleColor.Black)
        {
        }

        public Box(int x, int y, char symbol, ConsoleColor color) : this(x, y, symbol, color, false, false)
        {
        }

        public Box(int x, int y, char symbol, ConsoleColor color, bool isSelected, bool isCursorPosition)
        {
            this.x = x;
            this.y = y;
            this.symbol = symbol;
            this.color = color;
            this.isSelected = isSelected;
            this.isCursorPosition = isCursorPosition;
        }

        public void initBox(char symbol)
        {
            
            for (int i = 0; i < this.boxSize.GetLength(0); i++)
            {
                for (int j = 0; j < this.boxSize.GetLength(1); j++)
                {
                    boxSize[i, j] = symbol;
                }
            }
        }

        public void DrawBox()
        {
            Console.ForegroundColor = this.color;
            for (int i = 0; i < this.boxSize.GetLength(0); i++)
            {
                for (int j = 0; j < this.boxSize.GetLength(1); j++)
                {
                    Console.SetCursorPosition(this.x + i , this.y + j);
                    Console.Write(boxSize[i, j]);
                    
                }
            }
            if (!isSelected)
            {
                Console.SetCursorPosition(this.x + 1, this.y - 1);
                Console.Write(' ');
                Console.SetCursorPosition(this.x + 3, this.y + 1);
                Console.Write(' ');
                Console.SetCursorPosition(this.x + 3, this.y);
                Console.Write(' ');
                Console.SetCursorPosition(this.x + 3, this.y + 2);
                Console.Write(' ');
                Console.SetCursorPosition(this.x + 1, this.y + 3);
                Console.Write(' ');
                Console.SetCursorPosition(this.x - 1, this.y + 1);
                Console.Write(' ');
                Console.SetCursorPosition(this.x - 1, this.y);
                Console.Write(' ');
                Console.SetCursorPosition(this.x - 1, this.y + 2);
                Console.Write(' ');
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(this.x + 3, this.y + 1);
                Console.Write('|');
                Console.SetCursorPosition(this.x + 3, this.y);
                Console.Write('|');
                Console.SetCursorPosition(this.x + 3, this.y + 2);
                Console.Write('|');
                Console.SetCursorPosition(this.x - 1, this.y + 1);
                Console.Write('|');
                Console.SetCursorPosition(this.x - 1, this.y);
                Console.Write('|');
                Console.SetCursorPosition(this.x - 1, this.y + 2);
                Console.Write('|');
            }
            if (!isCursorPosition)
            {
                Console.SetCursorPosition(this.x - 1, this.y - 1);
                Console.Write(' ');
                Console.SetCursorPosition(this.x + 3, this.y - 1);
                Console.Write(' ');
                Console.SetCursorPosition(this.x + 3, this.y + 3);
                Console.Write(' ');
                Console.SetCursorPosition(this.x - 1, this.y + 3);
                Console.Write(' ');
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(this.x - 1, this.y - 1);
                Console.Write('\u250c');
                Console.SetCursorPosition(this.x + 3, this.y - 1);
                Console.Write('\u2510');
                Console.SetCursorPosition(this.x + 3, this.y + 3);
                Console.Write('\u2518');
                Console.SetCursorPosition(this.x - 1, this.y + 3);
                Console.Write('\u2514');
            }
        }
    }
}

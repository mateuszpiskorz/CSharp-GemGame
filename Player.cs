using System;
using System.Collections.Generic;
using System.Text;

namespace GemGame
{
    class Player
    {
        private string name;
        private int score;

        public string Name { get; set; }
        public int Score { get; set; }

        public Player(string name, int score)
        {
            this.Name = name;
            this.Score = score;
        }
    }
}

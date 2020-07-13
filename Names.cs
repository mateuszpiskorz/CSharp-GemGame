using System;
using System.Collections.Generic;
using System.Text;

namespace GemGame
{
    static class Names
    {
        
        public static string GetNames()
        {
            Random rnd = new Random();
            int rndVal = rnd.Next(0, 9);
            switch (rndVal)
            {
                case 0:
                    {
                        return "Hello MR. ...: ";
                        
                    }
                case 1:
                    {
                        return "Hi, my name is: ";
                    }
                case 2:
                    {
                        return "No Luke, I am ...: ";
                    }
                case 3:
                    {
                        return "State your name: ";
                    }
                case 4:
                    {
                        return "Wake up ...: ";
                    }
                case 5:
                    {
                        return "I am Optimus ...: ";
                    }
                case 6:
                    {
                        return "It's a plane. It's a train. No! It's ...:";
                    }
                case 7:
                    {
                        return "The truth is, I am ...:";
                    }
                default:
                    {
                        return "Something went wrong. Name please: ";
                    }
            }
        }
 
    }
}

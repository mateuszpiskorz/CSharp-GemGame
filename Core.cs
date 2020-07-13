using System;
using System.Diagnostics;

namespace GemGame
{
    class Core
    {
        #region Fields
        

        
        #endregion
        static void Main(string[] args)
        {
            Engine GemEngine = new Engine();
            GemEngine.StartEngine();
            //Debug.WriteLine(Console.LargestWindowHeight + "/" + Console.LargestWindowWidth);
            
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _05_BackgroundTextures
{
    internal class Program
    {
        static void Main(string[] args)
        {
            GameWindow gw = new GameWindow(1280, 720, 4, 8, true);
            gw.Run();
            gw.Dispose();
        }
    }
}

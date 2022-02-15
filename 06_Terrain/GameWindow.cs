using KWEngine2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _06_Terrain
{
    internal class GameWindow : GLWindow
    {
        public GameWindow(int width, int height, int fsaa, int textureAnisotropy, bool vSync)
            : base(width, height, OpenTK.GameWindowFlags.FixedWindow, fsaa, vSync, false, textureAnisotropy, 512)
        {
            GameWorld w = new GameWorld();
            SetWorld(w);
        }
    }
}

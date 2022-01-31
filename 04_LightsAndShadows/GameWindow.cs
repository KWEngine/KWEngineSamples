using KWEngine2;

namespace _04_LightsAndShadows
{
    class GameWindow : GLWindow
    {
        public GameWindow(int width, int height, int fsaa, int textureAnisotropy, bool vSync)
            : base(width, height, OpenTK.GameWindowFlags.FixedWindow, fsaa, vSync, false, textureAnisotropy, 512)
        {
            GameWorld w = new GameWorld();
            SetWorld(w);
        }
    }
}

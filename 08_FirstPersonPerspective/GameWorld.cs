using KWEngine2;
using KWEngine2.Collision;
using KWEngine2.GameObjects;
using KWEngine2.Helper;
using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _08_FirstPersonPerspective
{
    internal class GameWorld : World
    {
        public override void Act(KeyboardState ks, MouseState ms)
        {
           
        }

        public override void Prepare()
        {
            FOV = 90;

            // Set background texture:
            SetTextureSkybox(@".\textures\skybox4.jpg");

            // Set mouse sensitivity (use negative values for inverting the y-axis):
            KWEngine.MouseSensitivity = 0.001f;

            // Initialize player object:
            Player p = new Player();
            p.SetModel("KWCube");
            p.SetScale(1, 2, 1);        // make the player 2 units high
            p.SetPosition(0, 1, 0);
            p.IsCollisionObject = true;
            AddGameObject(p);
            SetFirstPersonObject(p);

            // Place the floor:
            Floor f = new Floor();
            f.SetModel("KWCube");
            f.IsCollisionObject = true;
            f.SetScale(50, 1, 50);
            f.SetPosition(0, -0.5f, 0);
            f.SetTexture(@".\textures\tiles.jpg");
            f.SetTextureRepeat(25, 25);
            AddGameObject(f);
        }
    }
}

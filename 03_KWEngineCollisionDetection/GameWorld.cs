using KWEngine2;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _03_KWEngineCollisionDetection
{
    class GameWorld : World
    {
        public override void Act(KeyboardState ks, MouseState ms)
        {

        }

        public override void Prepare()
        {
            KWEngine.LoadModelFromFile("ubot", @".\models\ubot.fbx");


        }
    }
}

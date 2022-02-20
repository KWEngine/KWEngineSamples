using KWEngine2.GameObjects;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _03_CollisionDetection
{
    class Obstacle : GameObject
    {
        public override void Act(KeyboardState ks, MouseState ms)
        {
            this.SetGlow(0, 0, 0, 0);
        }
    }
}

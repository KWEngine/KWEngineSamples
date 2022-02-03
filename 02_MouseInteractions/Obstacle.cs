using KWEngine2.GameObjects;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02_MouseInteractions
{
    class Obstacle : GameObject
    {
        public override void Act(KeyboardState ks, MouseState ms)
        {
            // Test if the mouse cursor is inside of the instance's
            // hitbox. This is not pixel perfect if the camera's 
            // viewing angle is not axis-aligned. 
            if(IsMouseCursorInsideMyHitbox(ms))
            {
                SetGlow(1, 0, 1, 0.5f);
            }
            else
            {
                SetGlow(1, 0, 1, 0);
            }
        }
    }
}

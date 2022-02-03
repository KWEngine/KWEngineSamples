using KWEngine2;
using KWEngine2.GameObjects;
using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02_MouseInteractions
{
    class CursorSphere : GameObject
    {
        public override void Act(KeyboardState ks, MouseState ms)
        {
            // Get the mouse cursor projected on y-axis at height 1:
            Vector3 mouseCursor3D = GetMouseIntersectionPoint(ms, 1, Plane.Y);

            // Move the sphere to this position:
            SetPosition(mouseCursor3D);
        }
    }
}

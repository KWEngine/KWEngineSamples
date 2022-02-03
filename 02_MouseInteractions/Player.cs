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

namespace _02_MouseInteractions
{
    class Player : GameObject
    {
        private float _animationTime = 0;

        public override void Act(KeyboardState ks, MouseState ms)
        {
            // Let the Engine project the mouse cursor to a plane where Y is the up-axis.
            // The height of the final coordinates will be the center of the caller's (player's)
            // hitbox:
            Vector3 mouseCursorPosition3D = GetMouseIntersectionPoint(ms, Plane.Y);

            // Let the player rotate in order to face the current mouse cursor. 
            // TurnTowardsXZ uses the position's XZ values and ignores the Y value.
            TurnTowardsXZ(mouseCursorPosition3D);

            // Animate the player model's idle animation:
            DoAnimation();
        }

        private void DoAnimation()
        {
            _animationTime = (_animationTime + 0.0025f * KWEngine.DeltaTimeFactor) % 1f;
            AnimationID = 0;
            AnimationPercentage = _animationTime;
        }
    }
}

using KWEngine2;
using KWEngine2.GameObjects;
using KWEngine2.Collision;
using KWEngine2.Helper;
using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_KWEngineMovement
{
    class Player : GameObject
    {
        public override void Act(KeyboardState ks, MouseState ms)
        {
            // Get a reference to the current world:
            GameWorld currentWorld = (CurrentWorld as GameWorld);

            // Set the movement speed to 0.1f:
            float moveSpeed = 0.1f * KWEngine.DeltaTimeFactor;

            // Get the current mouse cursor position in 3d:
            Vector3 mousePosition = GetMouseIntersectionPoint(ms);

            // TurnTowardsXY means:
            // Turn towards the given position but only use the xy
            // axes and do not face towards or away from the
            // camera because the camera is on the z-axis.
            TurnTowardsXY(mousePosition);   

            // Call the responsible method to simulate movement:
            if(currentWorld.IsModeAxisAligned())
            {
                DoMovementAxisAligned(ks, ms, moveSpeed);
            }
            else
            {
                DoMovementObjectBased(ks, ms, moveSpeed);
            }

            // If player object gets out of screen bounds, reset 
            // its position to the center of the world:
            if(!IsInsideScreenSpace)
            {
                SetPosition(0, 0, 0);
            }
        }

        // Move on fixed world axes (x and y in this case):
        private void DoMovementAxisAligned(KeyboardState ks, MouseState ms, float moveSpeed)
        {
            if (ks.IsKeyDown(Key.Left) || ks.IsKeyDown(Key.A))
            {
                MoveOffset(-moveSpeed, 0.0f, 0.0f);
            }
            if (ks.IsKeyDown(Key.Right) || ks.IsKeyDown(Key.D))
            {
                MoveOffset(moveSpeed, 0.0f, 0.0f);
            }
            if (ks.IsKeyDown(Key.Up) || ks.IsKeyDown(Key.W))
            {
                MoveOffset(0.0f, moveSpeed, 0.0f);
            }
            if (ks.IsKeyDown(Key.Down) || ks.IsKeyDown(Key.S))
            {
                MoveOffset(0.0f, -moveSpeed, 0.0f);
            }
        }

        // Move in current view direction:
        private void DoMovementObjectBased(KeyboardState ks, MouseState ms, float moveSpeed)
        {
            // Get current view direction:
            Vector3 currentLookAtVector = GetLookAtVector();

            // Rotate the view direction by 90° for strafe-left/right movement:
            Vector3 currentLookAtVectorRotatedBy90 = HelperVector.RotateVector(currentLookAtVector, -90, Plane.Camera);

            // Strafe left/right:
            if (ks.IsKeyDown(Key.Left) || ks.IsKeyDown(Key.A))
            {
                MoveAlongVector(currentLookAtVectorRotatedBy90, -moveSpeed);
            }
            if (ks.IsKeyDown(Key.Right) || ks.IsKeyDown(Key.D))
            {
                MoveAlongVector(currentLookAtVectorRotatedBy90, moveSpeed);
            }

            // Move forward/backward:
            if (ks.IsKeyDown(Key.Up) || ks.IsKeyDown(Key.W))
            {
                Move(moveSpeed);
            }
            if (ks.IsKeyDown(Key.Down) || ks.IsKeyDown(Key.S))
            {
                Move(-moveSpeed);
            }
        }
    }
}

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
    internal class Player : GameObject
    {
        public override void Act(KeyboardState ks, MouseState ms)
        {
            // First, move the camera according to the current mouse movement:
            MoveFPSCamera(ms);

            // Collect player keyboard inputs here:
            float movementForwardBackward = 0;
            float movementStrafeLeftRight = 0;

            if(ks.IsKeyDown(Key.W) || ks.IsKeyDown(Key.Up))
            {
                movementForwardBackward += 1;
            }
            if (ks.IsKeyDown(Key.S) || ks.IsKeyDown(Key.Down))
            {
                movementForwardBackward -= 1;
            }
            if (ks.IsKeyDown(Key.A) || ks.IsKeyDown(Key.Left))
            {
                movementStrafeLeftRight -= 1;
            }
            if (ks.IsKeyDown(Key.D) || ks.IsKeyDown(Key.Right))
            {
                movementStrafeLeftRight += 1;
            }

            // Move the player according to the keyboard inputs for 0.1 units per frame:
            MoveAndStrafeFirstPerson(movementForwardBackward, movementStrafeLeftRight, 0.1f * KWEngine.DeltaTimeFactor);
        }
    }
}

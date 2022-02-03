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

namespace _03_CollisionDetection
{
    class Player : GameObject
    {
        private float _moveSpeed = 0.1f;

        public override void Act(KeyboardState ks, MouseState ms)
        {
            float movementX = 0;
            float movementY = 0;
            float movementZ = 0;

            if(ks.IsKeyDown(Key.W) || ks.IsKeyDown(Key.Up))
            {
                movementY += _moveSpeed * KWEngine.DeltaTimeFactor;
            }
            if (ks.IsKeyDown(Key.S) || ks.IsKeyDown(Key.Down))
            {
                movementY -= _moveSpeed * KWEngine.DeltaTimeFactor;
            }
            if (ks.IsKeyDown(Key.A) || ks.IsKeyDown(Key.Left))
            {
                movementX -= _moveSpeed * KWEngine.DeltaTimeFactor;
            }
            if (ks.IsKeyDown(Key.D) || ks.IsKeyDown(Key.Right))
            {
                movementX += _moveSpeed * KWEngine.DeltaTimeFactor;
            }

            if (movementX != 0 || movementY != 0)
            {
                MoveOffset(movementX, movementY, movementZ);
            }

            // Get the HUDObject reference from the current GameWorld instance:
            HUDObject h1 = ((GameWorld)CurrentWorld).GetH1();

            List<Intersection> intersections = GetIntersections();
            foreach(Intersection i in intersections)
            {
                // If this loop is running, there must be at least one collision!
                // So, set the HUDObject instance to display its name:
                h1.SetText("Collider: " + i.Object.Name);

                // Undo the current collision by moving as advised by the MTV
                // (minimum-translation-vector):
                MoveOffset(i.MTV.X, i.MTV.Y, 0); // don't use the i.MTV.Z component in 2D because it might contain small values because of rounding errors!
            }

            // If the list was empty, let the HUDObject display "none":
            if(intersections.Count == 0)
            {
                h1.SetText("Collider: none");
            }
        }
    }
}

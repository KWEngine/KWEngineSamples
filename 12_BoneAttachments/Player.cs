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

namespace _12_BoneAttachments
{
    internal class Player : GameObject
    {
        private enum MovementType { Idle, Walk, Pickup, Attack };

        private float _speed = 0.1f;
        private float _animPercent = 0.0f;
        public override void Act(KeyboardState ks, MouseState ms)
        {
            MovementType movementType = MovementType.Idle;

            if (ks.IsKeyDown(Key.W) && Position.Z > -5)
            {
                MoveOffset(0, 0, -_speed * KWEngine.DeltaTimeFactor);
                movementType = MovementType.Walk;
            }
            if (ks.IsKeyDown(Key.S) && Position.Z < 5)
            {
                MoveOffset(0, 0, _speed * KWEngine.DeltaTimeFactor);
                movementType = MovementType.Walk;
            }
            if (ks.IsKeyDown(Key.A) && Position.X > -5)
            {
                MoveOffset(-_speed * KWEngine.DeltaTimeFactor, 0, 0);
                movementType = MovementType.Walk;
            }
            if (ks.IsKeyDown(Key.D) && Position.X < 5)
            {
                MoveOffset(_speed * KWEngine.DeltaTimeFactor, 0, 0);
                movementType = MovementType.Walk;
            }

            DoAnimation(movementType);
        }

        private void DoAnimation(MovementType movementType, bool justSwitched = false)
        {
            if(movementType == MovementType.Idle)
            {
                AnimationID = 0;
                _animPercent = (_animPercent + 0.005f * KWEngine.DeltaTimeFactor) % 1;
                AnimationPercentage = _animPercent;
            }
            else if(movementType == MovementType.Walk)
            {
                AnimationID = 3;
                _animPercent = (_animPercent + 0.025f * KWEngine.DeltaTimeFactor) % 1;
                AnimationPercentage = _animPercent;
            }
        }
    }
}

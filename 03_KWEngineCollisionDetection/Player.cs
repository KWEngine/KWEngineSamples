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

namespace _03_KWEngineCollisionDetection
{
    enum JumpState
    {
        Stand,
        Jump,
        Fall
    }

    class Player : GameObject
    {
        private readonly Vector3 GRAVITY = new Vector3(0, 0.01f, 0);        // gravity (always stays the same)
        private readonly Vector3 VELOCITYJUMP = new Vector3(0, 0.25f, 0);   // velocity per jump

        private float _animationTime = 0;
        private JumpState _status = JumpState.Fall;                         // three status for stand, jump and fall
        private Vector3 _velocity = Vector3.Zero;                           // jump velocity (will become > 0 at jump start and then starts decreasing)
        private float _walkSpeed = 0.1f;
        private bool _upKeyReleased = true;

        public override void Act(KeyboardState ks, MouseState ms)
        {
            bool stateJustSwitched = false;
            ProcessMovement(ks);

            if (IsJumpKeyPressed(ks))
            {
                if (_status == JumpState.Stand && _upKeyReleased == true)
                { 
                    _status = JumpState.Jump;
                    _velocity = VELOCITYJUMP;
                    _upKeyReleased = false;
                    stateJustSwitched = true;

                    if(IsLeftKeyPressed(ks))
                    {
                        _velocity.X -= _walkSpeed;
                    }
                    if (IsRightKeyPressed(ks))
                    {
                        _velocity.X += _walkSpeed;
                    }
                }
            }
            else
            {
                _upKeyReleased = true;
            }

            if (_status == JumpState.Jump)
            {
                MoveOffset(_velocity);
                _velocity = _velocity - GRAVITY * KWEngine.DeltaTimeFactor;
                if (_velocity.Y <= 0)
                {
                    _velocity.Y = 0;
                    _status = JumpState.Fall;
                    stateJustSwitched = true;
                }
            }
            else if (_status == JumpState.Fall)
            {
                _velocity = _velocity - GRAVITY * KWEngine.DeltaTimeFactor;
                MoveOffset(_velocity);
            }

            List<Intersection> intersectionList = GetIntersections();
            foreach (Intersection i in intersectionList)
            {
                MoveOffset(i.MTV);
                if (i.MTV.Y > 0 && _status == JumpState.Fall)
                {
                    _status = JumpState.Stand;
                    _velocity = Vector3.Zero;
                    _animationTime = 0;
                }
            }

            // Animate the player model's idle animation:
            DoAnimation(ks, stateJustSwitched);
        }

        private void ProcessMovement(KeyboardState ks)
        {
            if (_status == JumpState.Stand)
            {
                if (IsLeftKeyPressed(ks))
                {
                    SetRotation(0, -90, 0);
                    MoveOffset((-_walkSpeed) * KWEngine.DeltaTimeFactor, 0, 0);
                }
                if (IsRightKeyPressed(ks))
                {
                    SetRotation(0, 90, 0);
                    MoveOffset((_walkSpeed) * KWEngine.DeltaTimeFactor, 0, 0);
                }
            }
            else
            {
                if(!IsJumpKeyPressed(ks))
                {
                    if(_velocity.Y > 0)
                    {
                        _velocity.Y = MathHelper.Clamp(_velocity.Y - (GRAVITY.Y / 1.5f) * KWEngine.DeltaTimeFactor, 0, VELOCITYJUMP.Y);
                    }
                }


                bool leftRight = false;
                if (IsLeftKeyPressed(ks))
                {
                    _velocity.X = MathHelper.Clamp(_velocity.X - (_walkSpeed / 8) * KWEngine.DeltaTimeFactor, -_walkSpeed, _walkSpeed);
                    leftRight = true;
                }
                if (IsRightKeyPressed(ks))
                {
                    _velocity.X = MathHelper.Clamp(_velocity.X + (_walkSpeed / 8) * KWEngine.DeltaTimeFactor, -_walkSpeed, _walkSpeed);
                    leftRight = true;
                }
                if(!leftRight)
                {
                    if(_velocity.X < 0)
                    {
                        _velocity.X = MathHelper.Clamp(_velocity.X + (_walkSpeed / 32) * KWEngine.DeltaTimeFactor, -_walkSpeed, 0);
                    }
                    else
                    {
                        _velocity.X = MathHelper.Clamp(_velocity.X - (_walkSpeed / 32) * KWEngine.DeltaTimeFactor, 0, _walkSpeed);
                    }
                    
                }
            }
        }

        private bool IsJumpKeyPressed(KeyboardState ks)
        {
            return ks.IsKeyDown(Key.Up) || ks.IsKeyDown(Key.W);
        }

        private bool IsLeftKeyPressed(KeyboardState ks )
        {
            return ks.IsKeyDown(Key.Left) || ks.IsKeyDown(Key.A);
        }

        private bool IsRightKeyPressed(KeyboardState ks)
        {
            return ks.IsKeyDown(Key.Right) || ks.IsKeyDown(Key.D);
        }

        private void DoAnimation(KeyboardState ks, bool animationJustSwitched)
        {
            if(_status == JumpState.Stand)
            {
                if(IsLeftKeyPressed(ks) || IsRightKeyPressed(ks))
                {
                    // walk animation
                    _animationTime = (_animationTime + 0.025f * KWEngine.DeltaTimeFactor) % 1f;
                    AnimationID = 2;
                    AnimationPercentage = _animationTime;
                }
                else
                {
                    // idle animation
                    _animationTime = (_animationTime + 0.0025f * KWEngine.DeltaTimeFactor) % 1f;
                    AnimationID = 0;
                    AnimationPercentage = _animationTime;
                }
            }
            else
            {
                AnimationID = 3;
                if (_status == JumpState.Jump)
                {
                    if(animationJustSwitched)
                    {
                        _animationTime = 0;
                    }
                    _animationTime = (_animationTime + 0.025f * KWEngine.DeltaTimeFactor) % 1f;
                    AnimationPercentage = _animationTime;
                }
                else
                {
                    _animationTime = (_animationTime + 0.025f * KWEngine.DeltaTimeFactor) % 1f;
                    AnimationPercentage = _animationTime;
                }
            }
        }
    }
}

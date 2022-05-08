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

namespace _13_2DSpritesheets
{
    internal class Player : GameObject
    {
        private enum JumpState
        {
            Stand,
            Jump,
            Fall
        }

        private readonly Vector3 GRAVITY = new Vector3(0, 0.5f, 0);        // gravity (always stays the same)
        private readonly Vector3 VELOCITYJUMP = new Vector3(0, 10f, 0);   // velocity per jump

        private float _animationTime = 0;
        private readonly float _animationOffset = 0.1f;
        private readonly float _animationLineOffset = 0.3333333f;
        private long _animationLastChange = 0;
        private JumpState _currentState = JumpState.Fall;                   // three states for stand, jump and fall (default)
        private Vector3 _velocity = Vector3.Zero;                           // jump velocity (will become > 0 at jump start and then starts decreasing)
        private float _walkSpeed = 2.0f;                                    // default walking speed
        private bool _upKeyReleased = true;                                 // true, if the up key has been released between jumps

        public override void Act(KeyboardState ks, MouseState ms)
        {
            bool stateJustSwitched = false;                                 // becomes true if state just switched

            ProcessMovement(ks);                                            // handles basic movement inputs

            if (IsJumpKeyPressed(ks))                                       // if jump is pressed, switch state
            {                                                               // and apply velocity:
                if (_currentState == JumpState.Stand && _upKeyReleased == true)
                {
                    _currentState = JumpState.Jump;
                    _velocity = VELOCITYJUMP;
                    _upKeyReleased = false;
                    stateJustSwitched = true;

                    if (IsLeftKeyPressed(ks))
                    {
                        _velocity.X = -_walkSpeed;
                    }
                    if (IsRightKeyPressed(ks))
                    {
                        _velocity.X = _walkSpeed;
                    }
                }
            }
            else
            {
                _upKeyReleased = true;
            }

            // if player is in jump or fall state, gravity is subtracted from its current velocity.
            // if velocity passes < 0, state switches from jump (upwards) to fall (downwards):
            if (_currentState == JumpState.Jump)
            {
                MoveOffset(_velocity);
                _velocity = _velocity - GRAVITY * KWEngine.DeltaTimeFactor;
                if (_velocity.Y <= 0)
                {
                    _currentState = JumpState.Fall;
                    stateJustSwitched = true;
                }
            }
            else if (_currentState == JumpState.Fall)
            {
                MoveOffset(_velocity);
                _velocity = _velocity - GRAVITY * KWEngine.DeltaTimeFactor;
            }
            else if (_currentState == JumpState.Stand)
            {
                // While standing, just move the player down a little bit.
                // If the collision detection loop (see foreach-loop down below)
                // had to correct the player upwards, he still is standing
                // on the ground. If not, the player must have moved off of an
                // edge.
                MoveOffset(-GRAVITY / 10 * KWEngine.DeltaTimeFactor);
                //MoveOffset(_velocity);
            }

            // Turns true, if the player had a collision with any object
            // directly underneath him:
            bool yUpCorrection = false;

            // Get a list of all intersections for the player:
            List<Intersection> intersectionList = GetIntersections();
            // Iterate through all intersections and apply each
            // intersection's minimum-translation-vector (mtv):
            foreach (Intersection i in intersectionList)
            {
                // If the player has to be moved upwards in order
                // to solve the collision, he must be standing on
                // something. Switch yUpCorrection to true then:
                if (i.MTV.Y > 0 && i.MTV.Y > i.MTV.X && i.MTV.Y > i.MTV.Z)
                    yUpCorrection = true;

                // If the player has to be corrected downwards and still is jumping up,
                // he has hit his head somewhere. The jump state must then switch to
                // the fall state:
                // (Math.Round ensures that rounding errors don't interfere here)
                if (Math.Round(i.MTV.Y, 3) < 0 && _currentState == JumpState.Jump)
                {
                    _velocity.Y = 0;
                    _currentState = JumpState.Fall;
                }

                MoveOffset(i.MTV);

                // If the mtv tells the player to correct its position
                // upwards, the player object must have hit the floor.
                // Switch state to "stand" then:
                if (yUpCorrection && _currentState == JumpState.Fall)
                {
                    stateJustSwitched = true;
                    _currentState = JumpState.Stand;
                    _velocity = Vector3.Zero;                               // reset velocity to 0 as well!
                    _animationTime = 0;
                }
            }

            // If the player is in 'stand' mode and is no longer
            // touching the ground, switch state to 'fall':
            if (!yUpCorrection && _currentState == JumpState.Stand)
            {
                _currentState = JumpState.Fall;
                stateJustSwitched = true;
            }

            // Process the player model's animations:
            DoAnimation(ks, stateJustSwitched);
        }

        private void DoAnimation(KeyboardState ks, bool animationJustSwitched)
        {
            int line = 0; // 0 = idle, 1 = walk, 2 = jump

            long currentTime = GetCurrentTimeInMilliseconds();

            // if the last frame has not beed displayed for at least 33ms, do not change animation frame:
            if(currentTime - _animationLastChange < 33.333333f)
            {
                return;
            }

            // If the player is on the floor...
            if (_currentState == JumpState.Stand)
            {
                // ..check if it is walking left or right:
                if (IsLeftKeyPressed(ks) || IsRightKeyPressed(ks))
                {
                    // switch to walk animation id:
                    line = 1;
                    SetTextureOffset(_animationTime, line * _animationLineOffset);
                    _animationTime = (_animationTime + _animationOffset);
                    if (_animationTime >= 1)
                        _animationTime = 0;
                }
                else
                {
                    // If player is standing still, switch to idle animation id:
                    line = 0;
                    SetTextureOffset(_animationTime, line * _animationLineOffset);
                    _animationTime = _animationTime + _animationOffset;
                    if (_animationTime >= 1)
                        _animationTime = 0;
                }
            }
            else
            {
                // If the player is not on the floor, switch to its jump animation id:
                line = 2;
                if (_currentState == JumpState.Jump)
                {
                    // If the jump just started, make sure to rewind the animation percentage to 0
                    // in order to play the jump animation from the beginning:
                    if (animationJustSwitched)
                    {
                        _animationTime = 0;
                    }
                    SetTextureOffset(_animationTime, line * _animationLineOffset);
                    _animationTime = _animationTime + _animationOffset;
                    if (_animationTime >= 1)
                        _animationTime = 0;
                }
                else
                {
                    SetTextureOffset(_animationTime, line * _animationLineOffset);
                    _animationTime = _animationTime + _animationOffset;
                    if (_animationTime >= 1)
                        _animationTime = 0;
                }
            }
            _animationLastChange = currentTime;
        }

        private void ProcessMovement(KeyboardState ks)
        {
            // if the player is on the floor (incl. running as well),
            // turn the object in walking direction and move if by
            // its walking speed:
            if (_currentState == JumpState.Stand)
            {
                if (IsLeftKeyPressed(ks))
                {
                    SetRotation(0, -180, 0);
                    MoveOffset((-_walkSpeed) * KWEngine.DeltaTimeFactor, 0, 0);
                    _velocity.X = -_walkSpeed * KWEngine.DeltaTimeFactor;
                }
                if (IsRightKeyPressed(ks))
                {
                    SetRotation(0, 0, 0);
                    MoveOffset((_walkSpeed) * KWEngine.DeltaTimeFactor, 0, 0);
                    _velocity.X = _walkSpeed * KWEngine.DeltaTimeFactor;
                }
            }
            else
            {
                // If the player is jumping but has already released the jump key
                // decrease the upwards velocity a little more in order to make
                // the jump shorter:
                if (!IsJumpKeyPressed(ks))
                {
                    if (_velocity.Y > 0)
                    {
                        _velocity.Y = MathHelper.Clamp(_velocity.Y - (GRAVITY.Y / 1.5f) * KWEngine.DeltaTimeFactor, 0, VELOCITYJUMP.Y);
                    }
                }

                // If the player is in the air and left/right is pressed, make sure that the player only has 1/8th
                // of the speed (sideways). This effectively reduces air control:
                bool leftRight = false;
                if (IsLeftKeyPressed(ks))
                {
                    _velocity.X = MathHelper.Clamp(_velocity.X - (_walkSpeed / 4) * KWEngine.DeltaTimeFactor, -_walkSpeed, _walkSpeed);
                    leftRight = true;
                }
                if (IsRightKeyPressed(ks))
                {
                    _velocity.X = MathHelper.Clamp(_velocity.X + (_walkSpeed / 4) * KWEngine.DeltaTimeFactor, -_walkSpeed, _walkSpeed);
                    leftRight = true;
                }

                // If the player did a side jump and is in the air but has released left/right keys
                // make sure to slowly reduce the sideways velocity.
                // This makes jumps shorter (in horizontal direction) if the direction keys
                // (left/right) are only pressed at the beginning of the jump:
                if (!leftRight)
                {
                    if (_velocity.X < 0)
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

        private bool IsLeftKeyPressed(KeyboardState ks)
        {
            return ks.IsKeyDown(Key.Left) || ks.IsKeyDown(Key.A);
        }

        private bool IsRightKeyPressed(KeyboardState ks)
        {
            return ks.IsKeyDown(Key.Right) || ks.IsKeyDown(Key.D);
        }
    }
}

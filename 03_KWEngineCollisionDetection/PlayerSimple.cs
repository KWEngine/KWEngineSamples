using KWEngine2;
using KWEngine2.Collision;
using KWEngine2.GameObjects;
using KWEngine2.Helper;
using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;

namespace _03_KWEngineCollisionDetection
{
    class PlayerSimple : GameObject
    {
        private enum JumpState
        {
            Stand,
            Jump,
            Fall
        }

        private readonly float GRAVITY = 0.0125f;                             // gravity (always stays the same)
        private readonly float VELOCITYJUMP = 0.25f;                        // velocity per jump

        private float _animationTime = 0;
        private JumpState _currentState = JumpState.Fall;                   // three states for stand, jump and fall (default)
        private float _velocity = 0f;                                       // jump velocity (will become > 0 at jump start and then starts decreasing)
        private float _walkSpeed = 0.1f;                                    // default walking speed
        private bool _upKeyReleased = true;                                 // true, if the up key has been released between jumps

        public override void Act(KeyboardState ks, MouseState ms)
        {
            bool stateJustSwitched = false;                                 // becomes true if state just switched
            
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

            if (IsJumpKeyPressed(ks))                                       // if jump is pressed, switch state
            {                                                               // and apply velocity:
                if (_currentState == JumpState.Stand && _upKeyReleased == true)
                {
                    HelperAudio.SoundPlay(@".\sfx\jumpUp.ogg");
                    _currentState = JumpState.Jump;
                    _velocity = VELOCITYJUMP;
                    _upKeyReleased = false;
                    stateJustSwitched = true;
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
                MoveOffset(0, _velocity, 0);
                _velocity = _velocity - GRAVITY * KWEngine.DeltaTimeFactor;
                if (_velocity <= 0)
                {
                    _currentState = JumpState.Fall;
                    stateJustSwitched = true;
                }
            }
            else if (_currentState == JumpState.Fall)
            {
                MoveOffset(0, _velocity, 0);
                _velocity = _velocity - GRAVITY * KWEngine.DeltaTimeFactor;
            }

            // Get a list of all intersections for the player:
            List<Intersection> intersectionList = GetIntersections();
            // Iterate through all intersections and apply each
            // intersection's minimum-translation-vector (mtv):
            foreach (Intersection i in intersectionList)
            {
                MoveOffset(i.MTV);
                
                // If the mtv tells the player to correct its position
                // upwards, the player object must have hit the floor.
                // Switch state to "stand" then:
                if (i.MTV.Y > 0 && _currentState == JumpState.Fall)
                {
                    _currentState = JumpState.Stand;                    
                    _velocity = 0f;                                           // reset velocity to 0 as well!
                    _animationTime = 0;
                    HelperAudio.SoundPlay(@".\sfx\jumpLand.ogg");
                }
            }

            // Process the player model's animations:
            DoAnimation(ks, stateJustSwitched);
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
            // If the player is on the floor...
            if(_currentState == JumpState.Stand)
            {
                // ..check if it is walking left or right:
                if(IsLeftKeyPressed(ks) || IsRightKeyPressed(ks))
                {
                    // switch to walk animation id:
                    AnimationID = 2;
                    _animationTime = (_animationTime + 0.025f * KWEngine.DeltaTimeFactor) % 1f;
                    AnimationPercentage = _animationTime;
                }
                else
                {
                    // If player is standing still, switch to idle animation id:
                    AnimationID = 0;
                    _animationTime = (_animationTime + 0.0025f * KWEngine.DeltaTimeFactor) % 1f;
                    AnimationPercentage = _animationTime;
                }
            }
            else
            {
                // If the player is not on the floor, switch to its jump animation id:
                AnimationID = 3;
                if (_currentState == JumpState.Jump)
                {
                    // If the jump just started, make sure to rewind the animation percentage to 0
                    // in order to play the jump animation from the beginning:
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

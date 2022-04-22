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

namespace _11_ThirdPersonPerspective
{
    public class Player : GameObject
    {
        private readonly Vector3 _offsetVertical = new Vector3(0, 1, 0);
        private readonly long _cooldown = 150;
        private long _lastShot = 0;

        private enum PlayerState
        {
            OnFloor = 0,
            Jump = 2,
            Fall = 3
        }

        private float _percentage = 0.0f;
        private PlayerState _state = PlayerState.Fall;
        private bool _running = false;
        private bool _upKeyPressed = false;
        private bool _attacking = false;

        private float _momentum = 0;
        private float _gravity = 0.02f;
        private float _speed = 0.1f;
        private float _degreesLeftRightPerFrame = 40;
        private Vector2 _currentCameraRotationDegrees = new Vector2(0, -2.5f);

        public override void Act(KeyboardState ks, MouseState ms)
        {
            if (Position.Y < -25)                                                               // If player has fallen off the edge,
            {                                                                                   // set it back to the origin.
                SetPosition(0, 0, 0);
                _momentum = 0;
                return;
            }

            // Control the camera position:
            Vector2 msMovement = CurrentWindow.GetMouseCursorMovement(ms);                      // Collect the relative mouse movement for the current frame.
            AddRotationY(-msMovement.X * _degreesLeftRightPerFrame);                            // Rotate the player by 40 times the mouse's x-movement.
            DoCameraPosition(msMovement);                                                       // Call separate method for adjusting the camera.

            // Player movement:
            Vector3 cameraLav = GetLookAtVector();                                              // Get the camera's current look-at-vector.
            Vector3 cameraLavRotated = HelperRotation.RotateVector(cameraLav, -90, Plane.Y);    // Rotate the camera's look-at-vector by 90° for strafing.
            float currentSpeed = _speed * KWEngine.DeltaTimeFactor;                             // Determine the player's speed for the current frame.
            if (ks[Key.A] || ks[Key.D] || ks[Key.W] || ks[Key.S])   
            {
                if (ks[Key.W])
                {
                    MoveAlongVector(cameraLav, currentSpeed);
                }
                if (ks[Key.S])
                {
                    MoveAlongVector(cameraLav, -currentSpeed);
                }
                if (ks[Key.A])
                {
                    MoveAlongVector(-cameraLavRotated, currentSpeed);
                }
                if (ks[Key.D])
                {
                    MoveAlongVector(cameraLavRotated, currentSpeed);
                }

                _running = true;
                _attacking = false;
            }
            else
            {
                if (_running)
                {
                    _percentage = 0;
                    _running = false;
                }
            }

            if (_state == PlayerState.OnFloor && (ks[Key.Space] || ms[MouseButton.Right]))
            {
                if (!_upKeyPressed)
                {
                    _state = PlayerState.Jump;
                    _percentage = 0;
                    _momentum = 0.35f;
                    _upKeyPressed = true;
                    _attacking = false;

                }
            }
            else if (!(ks[Key.Space] || ms[MouseButton.Right]))
            {
                _upKeyPressed = false;
            }

            HUDObject crosshair = CurrentWorld.GetHUDObjectByName("Crosshair"); // Get crosshair HUD object
            Immovable o = HelperIntersection.IsMouseCursorOnAny<Immovable>      // Check if mouse cursor hovers over an object of type Immovable
                (ms, out Vector3 intersectionPoint, 0, 0);
            
            // If the cursor hovers over an appropriate object, color the crosshair red:
            if(o != null)                                                       
            {
                // If mouse cursor hovers over Immovable objects, then color it red:
                if(crosshair != null)
                {
                    crosshair.SetColor(1, 0, 0, 5);
                }

                // If player is pressing the left shift key or the left mouse button, generate a shot object:
                if (ks[Key.ShiftLeft] || ms[MouseButton.Left])
                {
                    DoShoot(intersectionPoint);
                }
            }
            else
            {
                crosshair.SetColor(1, 1, 1, 1);
            }
            

            DoStates();
            DoCollisionDetection();
            DoAnimation();
        }

        private void DoShoot(Vector3 intersectionPoint)
        {
            // Check if time delta between last shot and new shot is big enough (> cooldown):
            if (CurrentWorld.GetCurrentTimeInMilliseconds() - _lastShot > _cooldown)
            {
                // Generate new shot object:
                Shot s = new Shot();
                s.SetModel("KWSphere");
                s.SetScale(0.25f);
                s.SetColor(0, 0, 1);
                s.SetGlow(0, 0, 1, 0.25f);
                s.IsCollisionObject = true;
                s.SetPosition(GetCenterPointForAllHitboxes() + GetLookAtVector() * 0.25f); // Set shot slightly in front of player object
                s.TurnTowardsXYZ(intersectionPoint);                                       // Let it turn towards the mouse cursor pos

                CurrentWorld.AddGameObject(s);
                _lastShot = CurrentWorld.GetCurrentTimeInMilliseconds();
            }
        }

        private void DoCameraPosition(Vector2 m)
        {
            // Vector m stores the current mouse position delta between this and the last frame.
            // We need to multiply it by a magnitude of our choice (_degreesLeftRightPerFrame) to calculate
            // the amount of degrees that the camera has to rotate:
            _currentCameraRotationDegrees.X += m.X * _degreesLeftRightPerFrame;
            _currentCameraRotationDegrees.Y += m.Y * _degreesLeftRightPerFrame;
            if (_currentCameraRotationDegrees.Y < -75)                          // limit the up/down axis to the range [-75;2.5] degrees
                _currentCameraRotationDegrees.Y = -75;
            if (_currentCameraRotationDegrees.Y > 2.5f)
                _currentCameraRotationDegrees.Y = 2.5f;

            // This is not needed, but it moves the camera closer to the player as the camera position gets lower:
            // You may set both factors to (i.e.) 1 to disable this:
            float lav_factor = (0.00012f * (_currentCameraRotationDegrees.Y * _currentCameraRotationDegrees.Y) + 0.02099f * _currentCameraRotationDegrees.Y + 0.89190f);
            float lav_factor2 = _currentCameraRotationDegrees.Y >= -15 ? (_currentCameraRotationDegrees.Y + 15) / 20f : 0f;

            //lav_factor = 1;   // uncomment to disable varying camera distance 
            //lav_factor2 = 1;  // uncomment to disable varying camera distance 

            // The offsets are needed in order to prevent the crosshair from pointing directly onto the player object.
            // Both offsets are (later) applied to the SetCameraPosition() and SetCameraTarget() methods:
            Vector3 offset1 = HelperRotation.RotateVector(GetLookAtVector(), -90, Plane.Y) * 1 + GetLookAtVector() * 5 * lav_factor;
            Vector3 offset2 = HelperRotation.RotateVector(GetLookAtVector(), -90, Plane.Y) * 1 + GetLookAtVector() * 2 + _offsetVertical * 2 * lav_factor2;

            // This defines the point that the camera rotates around:
            // (The y-position should be the center of the player object's hitbox)
            Vector3 arcBallCenter = new Vector3(Position.X, GetCenterPointForAllHitboxes().Y, Position.Z);

            // Call helper method to determine the new camera position (arcball camera movement):
            Vector3 newCamPos = HelperRotation.CalculateRotationForArcBallCamera(
                arcBallCenter,                      // point to rotate around (pivot point)
                10f,                                // distance to the point
                _currentCameraRotationDegrees.X,    // rotation in degrees on left/right axis
                _currentCameraRotationDegrees.Y,    // rotation in degrees on up/down axis
                false,                              // invert left/right?
                false);                             // invert up/down?

            // Apply the new camera position to the world's camera object:
            CurrentWorld.SetCameraPosition(newCamPos + offset1);
            CurrentWorld.SetCameraTarget(new Vector3(Position.X, GetCenterPointForAllHitboxes().Y, Position.Z) + offset2);
        }

        private void DoStates()
        {
            if (_state == PlayerState.Jump)
            {
                MoveOffset(0, _momentum * KWEngine.DeltaTimeFactor, 0);
                _momentum -= _gravity * KWEngine.DeltaTimeFactor;
                if (_momentum < 0)
                {
                    _momentum = 0;
                    _state = PlayerState.Fall;
                }
            }
            else if (_state == PlayerState.Fall)
            {
                MoveOffset(0, _momentum * KWEngine.DeltaTimeFactor, 0);
                _momentum -= _gravity * KWEngine.DeltaTimeFactor;
            }
            else if (_state == PlayerState.OnFloor)
            {
                MoveOffset(0, -0.0001f, 0);
            }
        }
        private void DoCollisionDetection()
        {
            List<Intersection> collisionlist = GetIntersections();
            bool upCorrection = false;
            float maxYUpCorrection = 0;
            foreach (Intersection i in collisionlist)
            {
                if (i.Object is Shot)
                    continue;

                if (i.MTV.Y > maxYUpCorrection)
                    maxYUpCorrection = i.MTV.Y;

                MoveOffset(new Vector3(i.MTV.X, 0, i.MTV.Z));
                if (i.MTV.Y > 0)
                {
                    if (_state == PlayerState.OnFloor)
                    {
                        upCorrection = true;
                    }
                    else if (_state == PlayerState.Fall)
                    {
                        upCorrection = true;
                        _state = PlayerState.OnFloor;
                    }
                }
                else if (i.MTV.Y < 0 && Math.Abs(i.MTV.Y) > Math.Abs(i.MTV.X) && Math.Abs(i.MTV.Y) > Math.Abs(i.MTV.Z))
                {
                    if (_state == PlayerState.Jump)
                    {
                        _state = PlayerState.Fall;
                        _momentum = 0;
                        _percentage = 0.5f;
                    }
                }
            }
            MoveOffset(0, maxYUpCorrection, 0);

            if (_state == PlayerState.OnFloor && !upCorrection)
            {
                _state = PlayerState.Fall;
                _attacking = false;
                _momentum = 0;
                _percentage = 0.5f;
            }
        }
        private void DoAnimation()
        {
            if (this.HasAnimations)
            {
                if (_state == PlayerState.OnFloor)
                {
                    if (_running)
                    {
                        this.AnimationID = 2;
                        _percentage = (_percentage + 0.040f * KWEngine.DeltaTimeFactor) % 1.0f;
                    }
                    else if (_attacking)
                    {
                        this.AnimationID = 1;
                        _percentage = (_percentage + 0.045f * KWEngine.DeltaTimeFactor);
                    }
                    else
                    {
                        this.AnimationID = 0;
                        _percentage = (_percentage + 0.0025f * KWEngine.DeltaTimeFactor) % 1.0f;
                    }
                }
                else if (_state == PlayerState.Jump || _state == PlayerState.Fall)
                {
                    this.AnimationID = 3;
                    _percentage = (_percentage + 0.025f * KWEngine.DeltaTimeFactor) % 1.0f;
                }

                AnimationPercentage = _percentage;

            }
        }
    }
}
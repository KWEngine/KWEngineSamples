using KWEngine2;
using KWEngine2.Collision;
using KWEngine2.GameObjects;
using KWEngine2.Helper;
using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;

namespace _12_BoneAttachments
{
    internal class Player : GameObject
    {
        private enum MovementType { Idle, Walk, Attack };

        private float _speed = 0.1f;
        private float _animPercent = 0.0f;
        private MovementType _movementType = MovementType.Idle;
        private bool _mouseLeftReleased = true;
        private bool _spaceKeyReleased = true;

        public override void Act(KeyboardState ks, MouseState ms)
        {
            bool animationJustSwitched = false;

            if(ms.IsButtonUp(MouseButton.Left))
            {
                _mouseLeftReleased = true;
            }
            if (ks.IsKeyUp(Key.Space))
            {
                _spaceKeyReleased = true;
            }

            if (ms.IsButtonDown(MouseButton.Left))
            {
                _mouseLeftReleased = false;
                if(_movementType != MovementType.Attack)
                {
                    _movementType = MovementType.Attack;
                    animationJustSwitched = true;
                }
            }
            else
            {
                TurnTowardsXZ(GetMouseIntersectionPoint(ms, Plane.Y, 0, 16));
                _movementType = MovementType.Idle;

                if (ks.IsKeyDown(Key.W) && Position.Z > -5)
                {
                    MoveOffset(0, 0, -_speed * KWEngine.DeltaTimeFactor);
                    _movementType = MovementType.Walk;
                }
                if (ks.IsKeyDown(Key.S) && Position.Z < 5)
                {
                    MoveOffset(0, 0, _speed * KWEngine.DeltaTimeFactor);
                    _movementType = MovementType.Walk;
                }
                if (ks.IsKeyDown(Key.A) && Position.X > -5)
                {
                    MoveOffset(-_speed * KWEngine.DeltaTimeFactor, 0, 0);
                    _movementType = MovementType.Walk;
                }
                if (ks.IsKeyDown(Key.D) && Position.X < 5)
                {
                    MoveOffset(_speed * KWEngine.DeltaTimeFactor, 0, 0);
                    _movementType = MovementType.Walk;
                }
            }

            // Only pickup weapon, if there is no object attached to the player right now:
            if(ks.IsKeyDown(Key.Space) && _spaceKeyReleased && HasAttachedGameObjects)
            {
                _spaceKeyReleased = false;
                ThrowAwayObject();
            }
            

            // If the player has an intersection with a sword and the sword is not held by the player already
            // color the player object's outline red to show that the sword may be picked up now:
            List<Intersection> intersections = GetIntersections();
            foreach(Intersection i in intersections)
            {
                if(i.Object is Sword && HasAttachedGameObjects == false)
                {
                    SetColorOutline(1, 0, 0, 1);

                    // If space key is pressed, the player picks it up:
                    if(ks.IsKeyDown(Key.Space) && _spaceKeyReleased)
                    {
                        _spaceKeyReleased = false;
                        PickupObject(i.Object);
                        SetColorOutline(0, 0, 0, 0);
                    }
                }
                else
                {
                    SetColorOutline(0, 0, 0, 0);
                }
            }
            
            // This controls the player animations:
            DoAnimation(animationJustSwitched);
        }

        private void PickupObject(GameObject o)
        {
            AttachGameObjectToBone(o, "mixamorig:LeftHand");

            // Note: If the model has no dedicated weapon bone, you may need to choose any bone
            //       but this does not necessarily scale and rotate the attached object correctly.
            //       In this case, you can use SetScale() etc. to finetune the attachment.
            //       All parameters are not relative to the parent bone, not the actual world!
            o.SetScale(1.5f);           // Scale the weapon to 1.5x its size
            o.SetRotation(-45, 0, 90);  // Rotate the weapon in order for the Player object to hold it correctly
            o.SetPosition(0, 0.05f, 0); // Move the weapon slightly along the bone's direction (not needed)
        }

        private void ThrowAwayObject()
        {
            // Get the object that the player is holding right now:
            GameObject o = GetAttachedGameObjectForBone("mixamorig:LeftHand");

            // Detach any object that is held in the left hand:
            DetachGameObjectFromBone("mixamorig:LeftHand");

            // If there really was an object in the player's left hand:
            if(o != null)
            {
                // Place the sword object on the floor at the current player's position
                // and reset its scale:
                o.SetPosition(o.Position.X, 0, o.Position.Z);
                o.SetRotation(90, 90, 0);
                o.SetScale(1);
            }
        }

        private void DoAnimation(bool justSwitched = false)
        {
            if(_movementType == MovementType.Idle)
            {
                AnimationID = 0;
                _animPercent = (_animPercent + 0.005f * KWEngine.DeltaTimeFactor) % 1;
                AnimationPercentage = _animPercent;
            }
            else if(_movementType == MovementType.Walk)
            {
                AnimationID = 3;
                _animPercent = (_animPercent + 0.025f * KWEngine.DeltaTimeFactor) % 1;
                AnimationPercentage = _animPercent;
            }
            else if(_movementType == MovementType.Attack)
            {
                AnimationID = 7;
                if (justSwitched)
                {
                    _animPercent = 0;
                }
                
                _animPercent = _animPercent + 0.05f * KWEngine.DeltaTimeFactor;
                if (_animPercent >= 1)
                {
                    _movementType = MovementType.Idle;
                }
                AnimationPercentage = _animPercent;
            }
        }
    }
}

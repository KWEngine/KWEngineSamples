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
    class GameWorld : World
    {
        private const long COOLDOWN = 250;
        private long _timestampLastModeChange = 0;
        private bool _movementModeAxisAligned = true;

        private void SwitchMode(KeyboardState ks)
        {
            long currentTimestamp = GetCurrentTimeInMilliseconds();
            if (ks.IsKeyDown(Key.Space) && currentTimestamp - _timestampLastModeChange > COOLDOWN)
            {
                _movementModeAxisAligned = !_movementModeAxisAligned;
                _timestampLastModeChange = currentTimestamp;
            }
        }

        public bool IsModeAxisAligned()
        {
            return _movementModeAxisAligned;
        }

        public override void Act(KeyboardState ks, MouseState ms)
        {
            SwitchMode(ks);

            HUDObject hMode = GetHUDObjectByName("HUDMode");
            if (hMode != null)
            {
                if (_movementModeAxisAligned == true)
                {
                    hMode.SetText("movement: axis-aligned");
                }
                else
                {
                    hMode.SetText("movement: object-based");
                }
            }
        }

        public override void Prepare()
        {
            // Set background texture:
            SetTextureBackground(@".\textures\spacebackground.dds", 2, 2);

            // Load 3d player model:
            KWEngine.LoadModelFromFile("spaceship4", @".\models\spaceship4.obj");

            // Initialize player object:
            Player p = new Player();
            p.SetModel("spaceship4");
            p.SetRotation(-90, 180, 0); // rotate the object so that it faces upwards
            AddGameObject(p);

            // Place HUD objects:
            HUDObject hMode = new HUDObject(HUDObjectType.Text, CurrentWindow.Width - 640, 32);
            hMode.Name = "HUDMode";
            hMode.SetFont(FontFace.MajorMonoDisplay);
            AddHUDObject(hMode);

            HUDObject hHint1 = new HUDObject(HUDObjectType.Text, CurrentWindow.Width - 640, 64);
            hHint1.Name = "HUDHint1";
            hHint1.SetFont(FontFace.MajorMonoDisplay);
            hHint1.SetText("Switch modes with SPACE");
            AddHUDObject(hHint1);

            // Show current view directions of objects:
            DebugShowLookAtVectors = true;
        }
    }
}

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

namespace _05_BackgroundTextures
{
    internal class GameWorld : World
    {
        private long _modeChangeLastTimestamp = 0;
        private bool _mode2d = true;
        private HUDObject _hMode;

        public override void Act(KeyboardState ks, MouseState ms)
        {
            Vector2 mouseDeltaXY = CurrentWindow.GetMouseCursorMovement(ms);

            // Control camera:
            Vector3 currentCameraTarget = GetCameraTarget();
            currentCameraTarget.X += mouseDeltaXY.X;
            currentCameraTarget.Y += mouseDeltaXY.Y;

            Vector3 currentCameraPosition = GetCameraPosition();
            currentCameraPosition.X -= mouseDeltaXY.X;
            currentCameraPosition.Y -= mouseDeltaXY.Y;

            SetCameraPosition(currentCameraPosition);
            SetCameraTarget(currentCameraTarget);

            CurrentWindow.SetMouseCursorToWindowCenter();

            long currentTime = GetCurrentTimeInMilliseconds();
            if(ks.IsKeyDown(Key.Space) && currentTime - _modeChangeLastTimestamp >= 250)
            {
                if(_mode2d)
                {
                    SetTextureSkybox(@".\textures\skybox4.jpg");
                    _hMode.SetText("TEXTURE MODE: 3D");
                }
                else
                {
                    SetTextureBackground(@".\textures\spacebackground.dds", 2, 2);
                    _hMode.SetText("TEXTURE MODE: 2D");
                }
                _mode2d = !_mode2d;
                _modeChangeLastTimestamp = currentTime;
            }
        }

        public override void Prepare()
        {
            // Set Field-Of-View to 90°:
            FOV = 90;

            // Set background texture:
            SetTextureBackground(@".\textures\spacebackground.dds", 2, 2);

            // Load 3d player model:
            KWEngine.LoadModelFromFile("spaceship4", @".\models\spaceship4.obj");

            // Initialize player object:
            Player p = new Player();
            p.SetModel("spaceship4");
            p.SetScale(2);
            p.SetRotation(-90, 180, 0); // rotate the object so that it faces upwards
            AddGameObject(p);

            // Place HUD objects:
            _hMode = new HUDObject(HUDObjectType.Text, CurrentWindow.Width - 640, 32);
            _hMode.Name = "HUDMode";
            _hMode.SetFont(FontFace.MajorMonoDisplay);
            _hMode.SetText("TEXTURE MODE: 2D");
            AddHUDObject(_hMode);

            HUDObject hHint = new HUDObject(HUDObjectType.Text, CurrentWindow.Width - 640, 72);
            hHint.Name = "HUDHint";
            hHint.SetFont(FontFace.MajorMonoDisplay);
            hHint.SetText("(press SPACE to change)");
            AddHUDObject(hHint);

            HUDObject hHint2 = new HUDObject(HUDObjectType.Text, CurrentWindow.Width - 640, 140);
            hHint2.Name = "HUDHint2";
            hHint2.SetFont(FontFace.MajorMonoDisplay);
            hHint2.SetText("[move camera with mouse]");
            AddHUDObject(hHint2);

            CurrentWindow.CursorGrabbed = true;
            CurrentWindow.CursorVisible = false;
        }
    }
}

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
        private float _rotationX = 0;
        private float _rotationY = 0;
        private readonly Vector3 _pivot = Vector3.Zero;

        public override void Act(KeyboardState ks, MouseState ms)
        {
            // Get mouse movement for current frame:
            Vector2 mouseDeltaXY = CurrentWindow.GetMouseCursorMovement(ms);

            // Control camera:
            // (allow angles up to 89.9° on both axes)
            _rotationX = MathHelper.Clamp(_rotationX - mouseDeltaXY.X * 20 * KWEngine.DeltaTimeFactor, -89.9f, 89.9f);
            _rotationY = MathHelper.Clamp(_rotationY - mouseDeltaXY.Y * 20 * KWEngine.DeltaTimeFactor, -89.9f, 89.9f);
            
            // Rotate camera around the pivot point (0|0|0) at a distance of 25 units:
            Vector3 newCameraPos = HelperRotation.CalculateRotationForArcBallCamera(_pivot, 25, _rotationX, _rotationY);
            SetCameraPosition(newCameraPos);
 
            // use space bar to switch between background texture modes (2d / 3d):
            // (space bar may be pressed every 250ms)
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

            CurrentWindow.SetMouseCursorToWindowCenter();
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
            p.SetScale(3);
            p.SetRotation(-90, 180, 0); // rotate the object so that it faces upwards
            AddGameObject(p);

            // Place obstacles:
            Obstacle o1 = new Obstacle();
            o1.SetModel("KWCube");
            o1.SetGlow(1, 0, 0, 1);
            o1.SetColor(1, 0, 0);
            o1.SetPosition(-8, 3, -4);
            AddGameObject(o1);

            Obstacle o2 = new Obstacle();
            o2.SetModel("KWCube");
            o2.SetGlow(0, 1, 0, 1);
            o2.SetPosition(5, 3, 5);
            o2.SetColor(0, 1, 0);
            AddGameObject(o2);

            Obstacle o3 = new Obstacle();
            o3.SetModel("KWCube");
            o3.SetGlow(1, 1, 0, 1);
            o3.SetColor(1, 1, 0);
            o3.SetPosition(-3, -3, -3);
            AddGameObject(o3);

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

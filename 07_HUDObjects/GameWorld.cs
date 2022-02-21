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

namespace _07_HUDObjects
{
    internal class GameWorld : World
    {
        private Player _p;
        public override void Act(KeyboardState ks, MouseState ms)
        {
            if(ks.IsKeyDown(Key.Number1))
            {
                _p.SetPosition(-5, 0, 0);
            }
            else if(ks.IsKeyDown(Key.Number2))
            {
                _p.SetPosition(-0.1f, 0, 0);
            }
            else if (ks.IsKeyDown(Key.Number3))
            {
                _p.SetPosition(-0.15f, 0, 0);
            }
            else if (ks.IsKeyDown(Key.Number4))
            {
                _p.SetPosition(+0.1f, 0, 0);
            }
            else if (ks.IsKeyDown(Key.Number5))
            {
                _p.SetPosition(+0.15f, 0, 0);
            }
            else if (ks.IsKeyDown(Key.Number6))
            {
                _p.SetPosition(+4.15f, 0, 0);
            }
            else if (ks.IsKeyDown(Key.Number7))
            {
                _p.SetPosition(+14.15f, 0, 0);
            }
            else if (ks.IsKeyDown(Key.Number8))
            {
                _p.SetPosition(-4.15f, 0, 0);
            }
            else if (ks.IsKeyDown(Key.Number9))
            {
                _p.SetPosition(-14.15f, 0, 0);
            }
        }

        public override void Prepare()
        {
            // Set background texture:
            SetTextureBackground(@".\textures\spacebackground.dds", 2, 2);

            // Load 3d player model:
            KWEngine.LoadModelFromFile("spaceship4", @".\models\spaceship4.obj");

            // Initialize player object:
            _p = new Player();
            _p.SetModel("spaceship4");
            _p.SetRotation(-90, 180, 0); // rotate the object so that it faces upwards
            AddGameObject(_p);

            // Place HUD object:
            HUDObject hPosition = new HUDObject(
                HUDObjectType.Text,         // HUD object type (text or image)
                CurrentWindow.Width - 640,  // HUD object start position (margin left)
                32                          // HUD object start position (margin top)
                );
            hPosition.Name = "PlayerPosition";              // HUD object name (needed to find it later!)
            hPosition.CharacterSpreadFactor = 22;           // set width of each letter
            hPosition.SetFont(FontFace.MajorMonoDisplay);   // set font
            AddHUDObject(hPosition);                        // add HUD object to world

            // You can also images as HUD objects:
            /*
             * HUDObject hImage = new HUDObject(HUDObjectType.Image, 32, 32);
             * hImage.SetTexture(@".\textures\tiles.jpg");
             * hImage.SetScale(64, 64);
             * AddHUDObject(hImage);
             */
        }
    }
}

using KWEngine2;
using KWEngine2.Collision;
using KWEngine2.GameObjects;
using KWEngine2.Helper;
using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;


namespace _13_2DSpritesheets
{
    internal class GameWorld : World
    {
        public override void Act(KeyboardState ks, MouseState ms)
        {
            
        }

        public override void Prepare()
        {
            // First, set the projection to "orthographic". This eliminates perspective projection. Orthographic is needed for pure 2D.
            KWEngine.Projection = ProjectionType.Orthographic;

            // Next, for collision detection: Set the collision detection search radius to a value that is bigger than your objects' movement distances (per frame)
            KWEngine.SweepAndPruneToleranceWidth = 32;

            // Set the FOV to your window's current height to make a pixel perfect world coordinate system (2D):
            FOV = CurrentWindow.Height;

            // Set the world to be max 10000 pixels wide and high:
            WorldDistance = 10000;

            // No light sources, just set the ambient light to 100% on every channel:
            SetAmbientLight(1, 1, 1, 1);
            
            // Set simple white background texture:
            SetTextureBackground(@".\textures\white.bmp");


            // Initialize player object:
            Player p = new Player();
            p.SetModel("KWQuad");
            p.IsCollisionObject = true;
            p.SetPosition(0, 128, 0);
            p.SetScale(48, 64, 1);
            p.SetHitboxScale(1, 1, 1000); // Since sprite hitboxes are very thin, we need to multiply their z-size by a huge factor in order for collision detection to work properly.
            p.SetTexture(@".\textures\spritesheet.png");
            p.SetTextureRepeat(0.1f, 0.33333333f);
            AddGameObject(p);
            
           
            // Place the floor object:
            Floor f01 = new Floor();
            f01.SetModel("KWQuad");
            f01.IsCollisionObject = true;
            f01.SetScale(1024,16,1);
            f01.SetPosition(0, -256, 0);
            f01.SetColor(1, 1, 1);
            f01.SetTexture(@".\textures\iron_panel_albedo.dds");
            f01.SetTextureRepeat(16, 0.5f);
            AddGameObject(f01);



            // Place HUD objects:
            HUDObject hMode = new HUDObject(HUDObjectType.Text, 32, 32);
            hMode.Name = "Movement";
            hMode.SetFont(FontFace.NovaMono);
            hMode.SetColor(0, 0, 0, 1);
            hMode.CharacterSpreadFactor = 20;
            hMode.SetText("Movement: W S A D");
            AddHUDObject(hMode);
        }
    }
}

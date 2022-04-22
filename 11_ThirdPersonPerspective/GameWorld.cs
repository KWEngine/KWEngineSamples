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
    internal class GameWorld : World
    {
        public override void Act(KeyboardState ks, MouseState ms)
        {

        }

        public override void Prepare()
        {
            // ====================================================================== //
            // Prepare the scene by adding background images and placing the objects: //
            // ====================================================================== //
            
            SetTextureSkybox(@".\textures\skybox4.jpg");                                    // Set background texture.

            KWEngine.LoadModelFromFile("UBot", @".\models\ubot.fbx");                       // Load 3d player model.

            // Initialize player object:
            Player p = new Player();
            p.SetModel("UBot");
            p.IsCollisionObject = true;
            p.SetPosition(0, 0, 0);
            p.SetRotation(0, 180, 0);
            p.IsShadowCaster = true;
            AddGameObject(p);

            // Create a floor:
            Platform i01 = new Platform();
            i01.SetModel("KWCube");
            i01.Name = "Floor";
            i01.SetScale(50, 1, 50);
            i01.SetPosition(0, -0.5f, 0);
            i01.IsCollisionObject = true;
            i01.SetTexture(@".\textures\mud_albedo.dds");
            i01.SetTexture(@".\textures\mud_normal.dds", TextureType.Normal);
            i01.SetTexture(@".\textures\mud_roughness.dds", TextureType.Roughness);
            i01.SetTextureRepeat(3, 3);
            i01.IsShadowCaster = true;
            AddGameObject(i01);

            // Create a box for aim testing:
            Immovable i02 = new Immovable();
            i02.SetModel("KWCube");
            i02.Name = "Cube";
            i02.SetScale(4, 2, 4);
            i02.SetPosition(0, 1, -10);
            i02.IsCollisionObject = true;
            i02.SetTexture(@".\textures\iron_panel_albedo.dds");
            i02.SetTexture(@".\textures\iron_panel_normal.dds", TextureType.Normal);
            i02.SetTexture(@".\textures\iron_panel_metal.dds", TextureType.Metalness);
            i02.SetTexture(@".\textures\iron_panel_roughness.dds", TextureType.Roughness);
            i02.SetTextureRepeat(2, 2);
            i02.SetTextureRepeat(2, 1);
            i02.IsShadowCaster = true;
            AddGameObject(i02);

            LightObject sun = new LightObject(LightType.Sun, true);
            sun.SetPosition(100, 50, 100);
            sun.SetNearAndFarBounds(10, 250);
            sun.SetFOV(70);
            sun.SetColor(1, 1, 1, 3.5f);
            AddLightObject(sun);

            SetAmbientLight(1, 1, 1, 0.25f);                                                // Dim the ambient light to 25% intensity!
            SetTextureBackgroundBrightnessMultiplier(4);                                    // But the background texture should be multiplied by 4
                                                                                            //     to regain 100% background brightness.
            FOV = 100;                                                                      // Set the camera's field of view to 100°.


            // ====================================================================== //
            // Setup mouse cursor and HUD behaviour for third person:                 //
            // ====================================================================== //
            CurrentWindow.CursorGrabbed = true;                                             // Grab the cursor and hold it inside the window!
            CurrentWindow.CursorVisible = false;                                            // Set the operating system's cursor opacity to 0.

            HUDObject crosshair = new HUDObject(
                HUDObjectType.Image,                                                        // Create a crosshair image on HUD plane
                CurrentWindow.Width / 2,                                                    // and place it at 1/2 window width
                CurrentWindow.Height / 2                                                    // and 1/2 window height.
                );
            crosshair.SetTexture(@".\textures\crosshair.png");                              // Set the image.
            crosshair.SetScale(64, 64);                                                     // Set the image size on screen.
            crosshair.Name = "Crosshair";                                                   // Set name of crosshair object.
            AddHUDObject(crosshair);                                                        // Add the image to the HUD plane.

           
        }
    }
}

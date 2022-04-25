using KWEngine2;
using KWEngine2.Collision;
using KWEngine2.GameObjects;
using KWEngine2.Helper;
using OpenTK;
using OpenTK.Input;
using _12_BoneAttachments.Environment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _12_BoneAttachments
{
    internal class GameWorld : World
    {
        public override void Act(KeyboardState ks, MouseState ms)
        {
           
        }

        public override void Prepare()
        {
            // Set ambient light:
            SetAmbientLight(1, 1, 1, 0.25f);

            // Setup camera:
            SetCameraPosition(0, 10, 15);
            SetCameraTarget(0, 0, 0);

            // Setup main light:
            LightObject sun = new LightObject(LightType.Sun, true);
            sun.SetPosition(25, 25, 25);
            sun.SetTarget(0, 0, 0);
            sun.SetColor(1, 1, 1, 3);
            sun.SetNearAndFarBounds(5, 100);
            AddLightObject(sun);

            // Load 3D player model:
            KWEngine.LoadModelFromFile("Ortiz", @".\models\ortiz.fbx");

            // Initialize player object:
            Player p = new Player();
            p.SetModel("Ortiz");
            p.IsShadowCaster = true;
            p.IsCollisionObject = true;
            AddGameObject(p);

            // Floor:
            Floor f01 = new Floor();
            f01.SetModel("KWCube");
            f01.SetPosition(0, -0.5f, 0);
            f01.SetScale(20, 1, 20);
            f01.SetTexture(@".\textures\mud_albedo.dds");
            f01.SetTexture(@".\textures\mud_normal.dds", TextureType.Normal);
            f01.SetTexture(@".\textures\mud_roughness.dds", TextureType.Roughness);
            f01.SetTextureRepeat(2, 2);
            f01.IsShadowCaster = true;
            f01.IsCollisionObject = true;
            AddGameObject(f01);
        }
    }
}

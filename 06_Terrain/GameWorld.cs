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

namespace _06_Terrain
{
    internal class GameWorld : World
    {
        private Vector3 _camPosition = new Vector3(50, 35, 50);
        private Vector3 _pivot = new Vector3(0, 35, 0);
        private float _degrees = -45;
        public override void Act(KeyboardState ks, MouseState ms)
        {
            // make the camera rotate around a pivot point:
            Vector3 newCamPosition = HelperRotation.CalculateRotationAroundPointOnAxis(_pivot, 66f, _degrees, Plane.Y);
            SetCameraPosition(newCamPosition);

            _degrees = (_degrees - 0.2f * KWEngine.DeltaTimeFactor) % 360;
        }

        public override void Prepare()
        {
            // Built terrain model:
            KWEngine.BuildTerrainModel(
                "TerrainExample",               // model name (your choice!)
                @".\textures\heightmap.png",    // height map texture (don't go too high: 64px is more than enough!)
                @".\textures\mud_albedo.dds",   // standard texture
                50,                             // width of terrain model (x)
                2,                              // maximum height (white pixels will have this height, black pixels will have 0 height)
                50,                             // depth of terrain model (z)
                2,                              // texture repeat (left/right)
                2);                             // texture repeat (front/back)

            SetCameraPosition(_camPosition);
            SetCameraTarget(0, -5, 0);
            SetAmbientLight(1, 1, 1, 0.25f);

            LightObject sun = new LightObject(LightType.Sun, true);
            sun.SetPosition(50, 25, 0);
            sun.SetNearAndFarBounds(20, 100);
            sun.SetColor(1, 9f, 0.8f, 4);
            sun.SetFOVBiasCoefficient(0.000005f, 0.50f);
            AddLightObject(sun);

            // Add GameObject instance with previously built terrain model:
            Terrain t = new Terrain();
            t.SetModel("TerrainExample");
            t.SetTexture(@".\textures\mud_normal.dds", TextureType.Normal);         // add normal map (optional)
            t.SetTexture(@".\textures\mud_roughness.dds", TextureType.Roughness);   // add roughness map (optional)
            t.IsShadowCaster = true;
            AddGameObject(t);


            // Place some obstacles just because we can:
            Obstacle o1 = new Obstacle();
            o1.SetPosition(3, 2.25f, 2);
            o1.SetRotation(0, 30, 0);
            o1.SetScale(4);
            AddGameObject(o1);

            Obstacle o2 = new Obstacle();
            o2.SetPosition(20, 1.5f, -10);
            o2.SetRotation(0, 15, 0);
            o2.SetScale(3);
            AddGameObject(o2);
        }
    }
}

using KWEngine2;
using KWEngine2.GameObjects;
using KWEngine2.Helper;
using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _03_KWEngineCollisionDetection
{
    class GameWorld : World
    {
        private LightObject _light;
        private Vector3 _lightPosition = new Vector3(0, 5, 10);
        private float _lightOffsetX = 0;
        private float _lightOffsetY = 0;

        public override void Act(KeyboardState ks, MouseState ms)
        {
            // Animate the lights:
            _lightOffsetX = (_lightOffsetX + 0.01f * KWEngine.DeltaTimeFactor) % (float)(Math.PI * 2);
            _lightOffsetY = (_lightOffsetY + 0.03f * KWEngine.DeltaTimeFactor) % (float)(Math.PI * 2);
            float sinX = (float)Math.Sin(_lightOffsetX);
            float sinY = (float)Math.Sin(_lightOffsetY);
            _lightPosition.X = sinX * 10f;
            _lightPosition.Y = 2.5f + -sinY * 5f;
            _light.SetPosition(_lightPosition);
        }

        public override void Prepare()
        {
            KWEngine.LoadModelFromFile("ubot", @".\models\ubot.fbx");
            SetTextureBackground(@".\textures\spacebackground.dds", 2, 2);

            PlayerSimple p = new PlayerSimple();
            // PlayerComplex p = new PlayerComplex();       // use this instead for advanced player input handling
            p.SetModel("ubot");
            p.SetRotation(0, 90, 0);
            p.IsCollisionObject = true;
            p.IsShadowCaster = true;
            AddGameObject(p);

            Floor f01 = new Floor();
            f01.SetModel("KWCube");
            f01.SetPosition(0, -4.5f, 0);
            f01.SetScale(10, 1, 1);
            f01.IsCollisionObject = true;
            f01.SetTexture(@".\textures\pavement_06_albedo.dds", TextureType.Albedo);
            f01.SetTexture(@".\textures\pavement_06_normal.dds", TextureType.Normal);
            f01.SetTexture(@".\textures\pavement_06_roughness.dds", TextureType.Roughness);
            f01.SetTextureRepeat(5, 0.5f);
            f01.IsShadowCaster = true;
            AddGameObject(f01);

            Wall w01 = new Wall();
            w01.SetModel("KWCube");
            w01.SetPosition(-5.5f, 0.0f, 0);
            w01.SetScale(1, 10, 1);
            w01.IsCollisionObject = true;
            w01.SetTexture(@".\textures\scifi_tile_03_albedo.dds", TextureType.Albedo);
            w01.SetTexture(@".\textures\scifi_tile_03_normal.dds", TextureType.Normal);
            w01.SetTexture(@".\textures\scifi_tile_03_metallic.dds", TextureType.Metalness);
            w01.SetTexture(@".\textures\scifi_tile_03_roughness.dds", TextureType.Roughness);
            w01.SetTexture(@".\textures\scifi_tile_03_emissive.dds", TextureType.Emissive);
            w01.SetTextureRepeat(1, 10);
            w01.IsShadowCaster = true;
            AddGameObject(w01);

            Wall w02 = new Wall();
            w02.SetModel("KWCube");
            w02.SetPosition(5.5f, 0.0f, 0);
            w02.SetScale(1, 10, 1);
            w02.IsCollisionObject = true;
            w02.SetTexture(@".\textures\scifi_tile_03_albedo.dds", TextureType.Albedo);
            w02.SetTexture(@".\textures\scifi_tile_03_normal.dds", TextureType.Normal);
            w02.SetTexture(@".\textures\scifi_tile_03_metallic.dds", TextureType.Metalness);
            w02.SetTexture(@".\textures\scifi_tile_03_roughness.dds", TextureType.Roughness);
            w02.SetTexture(@".\textures\scifi_tile_03_emissive.dds", TextureType.Emissive);
            w02.SetTextureRepeat(1, 10);
            w02.IsShadowCaster = true;
            AddGameObject(w02);

            Wall w03 = new Wall();
            w03.SetModel("KWCube");
            w03.SetPosition(0, 0, -1);
            w03.SetScale(10, 10, 1);
            w03.IsCollisionObject = true;
            w03.SetTexture(@".\textures\scifi_tile_02_albedo.dds", TextureType.Albedo);
            w03.SetTexture(@".\textures\scifi_tile_02_normal.dds", TextureType.Normal);
            w03.SetTexture(@".\textures\scifi_tile_02_metallic.dds", TextureType.Metalness);
            w03.SetTexture(@".\textures\scifi_tile_02_roughness.dds", TextureType.Roughness);
            w03.SetTextureRepeat(5, 5);
            w03.IsShadowCaster = true;
            AddGameObject(w03);

            HelperAudio.SoundPreload(@".\sfx\jumpUp.ogg");
            HelperAudio.SoundPreload(@".\sfx\jumpLand.ogg");

            _light = new LightObject(LightType.Directional, true);
            _light.SetColor(0.25f, 1f, 0.25f, 4);
            _light.SetPosition(0, 5, 10);
            _light.SetNearAndFarBounds(5, 50);
            _light.SetTarget(0, -2.5f, 0);
            AddLightObject(_light);

            LightObject sun = new LightObject(LightType.Sun, true);
            sun.SetColor(1f, 0.5f, 1f, 2);
            sun.SetPosition(25, 25, 25);
            sun.SetTarget(0, 0, 0);
            sun.SetFOV(15);
            sun.SetNearAndFarBounds(20, 50);
            AddLightObject(sun);
        }
    }
}

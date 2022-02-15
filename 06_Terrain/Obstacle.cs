using KWEngine2;
using KWEngine2.GameObjects;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _06_Terrain
{
    class Obstacle : GameObject
    {
        public Obstacle()
        {
            this.SetModel("KWCube");
            this.IsShadowCaster = true;

            this.SetTexture(@".\textures\scifi_tile_03_albedo.dds", TextureType.Albedo);
            this.SetTexture(@".\textures\scifi_tile_03_normal.dds", TextureType.Normal);
            this.SetTexture(@".\textures\scifi_tile_03_metallic.dds", TextureType.Metalness);
            this.SetTexture(@".\textures\scifi_tile_03_roughness.dds", TextureType.Roughness);
            this.SetTexture(@".\textures\scifi_tile_03_emissive.dds", TextureType.Emissive);
        }
        public override void Act(KeyboardState ks, MouseState ms)
        {
        }
    }
}

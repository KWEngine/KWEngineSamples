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

namespace _09_Particles
{
    internal class GameWorld : World
    {
        private long _cooldown = 1000;
        private long _lastTimeStamp = 0;

        private readonly Vector3 _posExplosion1 = new Vector3(-8.0f,   -3.0f, 0f);
        private readonly Vector3 _posExplosion2 = new Vector3(-5.6f,   -3.5f, 0f);
        private readonly Vector3 _posExplosion3 = new Vector3(-3.3f,   -3.5f, 0f);
        private readonly Vector3 _posParticle1  = new Vector3(-1.3f,   -3.5f, 0f);
        private readonly Vector3 _posParticle2  = new Vector3(+0.95f,  -3.0f, 0f);
        private readonly Vector3 _posParticle3  = new Vector3(+3.1f,   -3.0f, 0f);
        private readonly Vector3 _posParticle4  = new Vector3(+5.5f,   -3.0f, 0f);
        private readonly Vector3 _posParticle5  = new Vector3(+7.7f,   -3.0f, 0f);

        private readonly Vector4 _clrExplosion1 = new Vector4(1, 1, 0, 1);
        private readonly Vector4 _clrExplosion2 = new Vector4(0, 1, 1, 1);
        private readonly Vector4 _clrExplosion3 = new Vector4(0, 1, 0, 1);

        public override void Act(KeyboardState ks, MouseState ms)
        {
            long currentTimestamp = GetCurrentTimeInMilliseconds();
            if(currentTimestamp - _lastTimeStamp >= _cooldown)
            {
                _lastTimeStamp = currentTimestamp;

                // Spawn explosions:
                Explosion ex1 = new Explosion(_posExplosion1, 32, 0.5f, 1.5f, 0.00075f * _cooldown, ExplosionType.Star, _clrExplosion1);
                ex1.SetAnimationAlgorithm(ExplosionAnimation.Spread);
                this.AddGameObject(ex1);

                Explosion ex2 = new Explosion(_posExplosion2, 16, 0.25f, 1.5f, 0.00075f * _cooldown, ExplosionType.Heart, _clrExplosion2);
                ex2.SetAnimationAlgorithm(ExplosionAnimation.WindUp);
                this.AddGameObject(ex2);

                Explosion ex3 = new Explosion(_posExplosion3, 64, 0.25f, 1.5f, 0.00075f * _cooldown, ExplosionType.Dollar, _clrExplosion3);
                ex3.SetAnimationAlgorithm(ExplosionAnimation.WhirlwindUp);
                this.AddGameObject(ex3);

                // Spawn fake particle effects:
                ParticleObject po1 = new ParticleObject(_posParticle1, new Vector3(2, 2, 2), ParticleType.BurstFire1);
                AddParticleObject(po1);

                ParticleObject po2 = new ParticleObject(_posParticle2, new Vector3(2, 2, 2), ParticleType.BurstOneUps);
                po2.SetColor(0, 1, 0, 1);
                AddParticleObject(po2);

                ParticleObject po3 = new ParticleObject(_posParticle3, new Vector3(2, 2, 2), ParticleType.BurstShield);
                po3.SetColor(0, 1, 1, 1);
                AddParticleObject(po3);

                ParticleObject po4 = new ParticleObject(_posParticle4, new Vector3(2, 2, 2), ParticleType.LoopSmoke2);
                po4.SetDuration(0.00075f * _cooldown);
                AddParticleObject(po4);

                ParticleObject po5 = new ParticleObject(_posParticle5, new Vector3(2, 2, 2), ParticleType.BurstTeleport2);
                AddParticleObject(po5);
            }
        }

        public override void Prepare()
        {
            // Set background texture:
            //SetTextureBackground(@".\textures\spacebackground.dds", 2, 2);

            // Place HUD objects:
            HUDObject hExpl1 = new HUDObject(HUDObjectType.Text, 8 + 16, CurrentWindow.Height - 64);
            hExpl1.Name = "Explosion #1";
            hExpl1.SetFont(FontFace.XanhMono);
            hExpl1.SetText("Explosion #1");
            hExpl1.SetScale(16, 16);
            hExpl1.CharacterSpreadFactor = 10;
            AddHUDObject(hExpl1);

            HUDObject hExpl2 = new HUDObject(HUDObjectType.Text, 168 + 16, CurrentWindow.Height - 64);
            hExpl2.Name = "Explosion #2";
            hExpl2.SetFont(FontFace.XanhMono);
            hExpl2.SetText("Explosion #2");
            hExpl2.SetScale(16, 16);
            hExpl2.CharacterSpreadFactor = 10;
            AddHUDObject(hExpl2);

            HUDObject hExpl3 = new HUDObject(HUDObjectType.Text, 328 + 16, CurrentWindow.Height - 64);
            hExpl3.Name = "Explosion #3";
            hExpl3.SetFont(FontFace.XanhMono);
            hExpl3.SetText("Explosion #3");
            hExpl3.SetScale(16, 16);
            hExpl3.CharacterSpreadFactor = 10;
            AddHUDObject(hExpl3);

            HUDObject hPart1 = new HUDObject(HUDObjectType.Text, 488 + 16, CurrentWindow.Height - 64);
            hPart1.Name = "Particle #1";
            hPart1.SetFont(FontFace.XanhMono);
            hPart1.SetText("Particle #1");
            hPart1.SetScale(16, 16);
            hPart1.CharacterSpreadFactor = 10;
            AddHUDObject(hPart1);

            HUDObject hPart2 = new HUDObject(HUDObjectType.Text, 648 + 16, CurrentWindow.Height - 64);
            hPart2.Name = "Particle #2";
            hPart2.SetFont(FontFace.XanhMono);
            hPart2.SetText("Particle #2");
            hPart2.SetScale(16, 16);
            hPart2.CharacterSpreadFactor = 10;
            AddHUDObject(hPart2);

            HUDObject hPart3 = new HUDObject(HUDObjectType.Text, 808 + 16, CurrentWindow.Height - 64);
            hPart3.Name = "Particle #3";
            hPart3.SetFont(FontFace.XanhMono);
            hPart3.SetText("Particle #3");
            hPart3.SetScale(16, 16);
            hPart3.CharacterSpreadFactor = 10;
            AddHUDObject(hPart3);

            HUDObject hPart4 = new HUDObject(HUDObjectType.Text, 968 + 16, CurrentWindow.Height - 64);
            hPart4.Name = "Particle #4";
            hPart4.SetFont(FontFace.XanhMono);
            hPart4.SetText("Particle #4");
            hPart4.SetScale(16, 16);
            hPart4.CharacterSpreadFactor = 10;
            AddHUDObject(hPart4);

            HUDObject hPart5 = new HUDObject(HUDObjectType.Text, 1128 + 16, CurrentWindow.Height - 64);
            hPart5.Name = "Particle #5";
            hPart5.SetFont(FontFace.XanhMono);
            hPart5.SetText("Particle #5");
            hPart5.SetScale(16, 16);
            hPart5.CharacterSpreadFactor = 10;
            AddHUDObject(hPart5);
        }
    }
}

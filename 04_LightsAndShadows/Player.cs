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

namespace _04_LightsAndShadows
{
    class Player : GameObject
    {
        private float _animationPercentage = 0;
        private int _animationId = 0;

        public override void Act(KeyboardState ks, MouseState ms)
        {
            if (HasAnimations)
            {
                AnimationID = _animationId;
                _animationPercentage = (_animationPercentage + 0.001f * KWEngine.DeltaTimeFactor) % 1f;
                AnimationPercentage = _animationPercentage;
            }
        }
    }
}

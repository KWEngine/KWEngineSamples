using KWEngine2;
using KWEngine2.Collision;
using KWEngine2.GameObjects;
using OpenTK;
using OpenTK.Input;

namespace _11_ThirdPersonPerspective
{
    class Shot : GameObject
    {
        private float _speed = 1f;
        private float _distance = 0;
        public override void Act(KeyboardState ks, MouseState ms)
        {
            float currentSpeed = _speed * KWEngine.DeltaTimeFactor;
            _distance += currentSpeed;
            Move(currentSpeed);

            Intersection i = GetIntersection();
            if (i != null)
            {
                Explosion ex = new Explosion(this.Position + GetLookAtVector() * (-currentSpeed * 0.5f), 8, 0.25f, 0.5f, 2f, ExplosionType.SphereRingY, new Vector4(0, 0.25f, 1f, 1));
                CurrentWorld.AddGameObject(ex);
                CurrentWorld.RemoveGameObject(this);
            }
            else if (_distance > 50)
            {
                CurrentWorld.RemoveGameObject(this);
            }
        }
    }
}
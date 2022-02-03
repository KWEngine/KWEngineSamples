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

namespace _03_CollisionDetection
{
    class GameWorld : World
    {
        private HUDObject _h1;

        public HUDObject GetH1()
        {
            return _h1;
        }

        public override void Act(KeyboardState ks, MouseState ms)
        {
         
        }

        public override void Prepare()
        {
            // Set background texture:
            SetTextureBackground(@".\textures\spacebackground.dds", 2, 2);

            // Load 3d player model:
            KWEngine.LoadModelFromFile("spaceship4", @".\models\spaceship4.obj");

            // Initialize player object:
            Player p = new Player();
            p.SetModel("spaceship4");
            p.SetRotation(-90, 180, 0); // rotate the object so that it faces upwards
            p.IsCollisionObject = true;
            AddGameObject(p);

            // Place some obstacles:
            Obstacle o1 = new Obstacle();
            o1.SetModel("KWSphere");
            o1.IsCollisionObject = true;
            o1.Name = "Obstacle #1";
            o1.SetPosition(-4, 0, 0);
            o1.SetColor(0, 1, 0);
            o1.SetScale(2);
            AddGameObject(o1);

            Obstacle o2 = new Obstacle();
            o2.SetModel("KWCube");
            o2.IsCollisionObject = true;
            o2.Name = "Obstacle #2";
            o2.SetPosition(4, 0, 0);
            o2.SetColor(0, 1, 1);
            o2.SetScale(2);
            o2.SetRotation(0, 0, 45);
            AddGameObject(o2);

            _h1 = new HUDObject(HUDObjectType.Text, 32, 32);
            _h1.SetText("Collider: none");
            _h1.SetFont(FontFace.MajorMonoDisplay);
            AddHUDObject(_h1);
        }
    }
}

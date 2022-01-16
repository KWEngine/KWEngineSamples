using KWEngine2;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02_KWEngineMouseInteractions
{
    class GameWorld : World
    {
        public override void Act(KeyboardState ks, MouseState ms)
        {

        }

        public override void Prepare()
        {
            KWEngine.LoadModelFromFile("ubot", @".\models\ubot.fbx");

            SetCameraPosition(25, 25, 25);
            SetCameraTarget(0, 0, 0);
            SetAmbientLight(1f, 1f, 1f, 0.8f);

            Player p = new Player();
            p.SetModel("ubot");
            AddGameObject(p);

            Floor f = new Floor();
            f.SetModel("KWCube");
            f.SetScale(30, 1, 30);
            f.SetPosition(0, -0.5f, 0);
            AddGameObject(f);

            Obstacle o1 = new Obstacle();
            o1.SetModel("KWCube");
            o1.SetScale(2);
            o1.SetPosition(5, 1, 5);
            o1.SetColor(1, 0, 0);
            AddGameObject(o1);

            Obstacle o2 = new Obstacle();
            o2.SetModel("KWCube");
            o2.SetScale(2);
            o2.SetPosition(-5, 1, 5);
            o2.SetColor(0, 0, 1);
            AddGameObject(o2);

            CursorSphere cs = new CursorSphere();
            cs.SetModel("KWSphere");
            cs.SetPosition(0, -2, 0);
            cs.SetColor(1, 1, 0);
            AddGameObject(cs);
        }
    }
}

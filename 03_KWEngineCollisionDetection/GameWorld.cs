﻿using KWEngine2;
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
        public override void Act(KeyboardState ks, MouseState ms)
        {

        }

        public override void Prepare()
        {
            KWEngine.LoadModelFromFile("ubot", @".\models\ubot.fbx");

            //SetCameraPosition(0, 0, 50);
         
            Player p = new Player();
            p.SetModel("ubot");
            p.SetScale(1);
            p.SetRotation(0, 90, 0);
            p.SetPosition(0, 0, 0);
            p.IsCollisionObject = true;
            AddGameObject(p);

            Floor f01 = new Floor();
            f01.SetModel("KWCube");
            f01.SetPosition(0, -4.5f, 0);
            f01.SetScale(10, 1, 1);
            f01.IsCollisionObject = true;
            AddGameObject(f01);

            Wall w01 = new Wall();
            w01.SetModel("KWCube");
            w01.SetPosition(-5.5f, 0.0f, 0);
            w01.SetScale(1, 10, 1);
            w01.IsCollisionObject = true;
            AddGameObject(w01);

            Wall w02 = new Wall();
            w02.SetModel("KWCube");
            w02.SetPosition(5.5f, 0.0f, 0);
            w02.SetScale(1, 10, 1);
            w02.IsCollisionObject = true;
            AddGameObject(w02);
        }
    }
}

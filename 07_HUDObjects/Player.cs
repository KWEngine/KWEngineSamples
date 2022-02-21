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

namespace _07_HUDObjects
{
    internal class Player : GameObject
    {
        private float _moveSpeed = 0.1f;

        public override void Act(KeyboardState ks, MouseState ms)
        {
            // Move along the world's x-/y-axes:
            if (ks.IsKeyDown(Key.Left) || ks.IsKeyDown(Key.A) )
            {
                if(Position.X > -7)
                    MoveOffset(-_moveSpeed * KWEngine.DeltaTimeFactor, 0.0f, 0.0f);
            }
            if (ks.IsKeyDown(Key.Right) || ks.IsKeyDown(Key.D))
            {
                if(Position.X < 7)
                    MoveOffset(_moveSpeed * KWEngine.DeltaTimeFactor, 0.0f, 0.0f);
            }
            if (ks.IsKeyDown(Key.Up) || ks.IsKeyDown(Key.W))
            {
                if(Position.Y < 4)
                    MoveOffset(0.0f, _moveSpeed * KWEngine.DeltaTimeFactor, 0.0f);
            }
            if (ks.IsKeyDown(Key.Down) || ks.IsKeyDown(Key.S))
            {
                if(Position.Y > -4)
                    MoveOffset(0.0f, -_moveSpeed * KWEngine.DeltaTimeFactor, 0.0f);
            }

            // Look for a HUD object with the name 'PlayerPosition':
            HUDObject hPosition = CurrentWorld.GetHUDObjectByName("PlayerPosition");

            // If the object was found...
            if(hPosition != null)
            {
                string xPosPadded = FormatPositionString(Math.Round(this.Position.X, 2).ToString());
                string yPosPadded = FormatPositionString(Math.Round(this.Position.Y, 2).ToString());

                // ...update its text with the player's position:
                hPosition.SetText("Position: ( " + xPosPadded + " | " + yPosPadded + " )"); 
            }
        }

        // Just a helper method to format the position string:
        // (this is not needed, but it looks a lot better)
        private string FormatPositionString(string posString)
        {
            int length = posString.Length;
            int index = posString.IndexOf(',');
            int indexSign = posString.IndexOf('-');

            int integers;
            int decimals;
            if(index >= 0)
            {
                integers = index - (indexSign >= 0 ? 1 : 0);
                decimals = length - index - 1;
            }
            else
            {
                integers = length - (indexSign >= 0 ? 1 : 0);
                decimals = 0;
            }

            string decimalPart;
            string integerPart;
            if(index >= 0)
            {
                decimalPart = posString.Substring(index+1).PadRight(3 - decimals, '0');
                integerPart = (indexSign >= 0 ? "-" : "+") + posString.Substring(indexSign >= 0 ? 1 : 0, integers).PadLeft(3 - integers, '0');
            }
            else
            {
                decimalPart = "00";
                integerPart = (indexSign >= 0 ? "-" : "+") + posString.Substring(indexSign >= 0 ? 1 : 0, integers).PadLeft(3 - integers, '0');
            }
            return integerPart + "," + decimalPart;
        }
    }
}

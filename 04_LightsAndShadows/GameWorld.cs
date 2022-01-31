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
    class GameWorld : World
    {
        private LightObject _sun;
        private float _sunDegrees = -45f;
        private LightObject _mouseLight;
        private Sphere _mouseLightSphere;

        public override void Act(KeyboardState ks, MouseState ms)
        {
            // SUN CONTROL:
            // Move sun around the scene:
            Vector3 newSunPosition = HelperRotation.CalculateRotationAroundPointOnAxis(
                                                        new Vector3(0, 20, 0),      // the point to rotate around (aka pivot)
                                                        20,                         // the radius to that point
                                                        _sunDegrees,                // the degrees of rotation
                                                        Plane.Y                     // the axis to rotate around
                                                        );
            _sun.SetPosition(newSunPosition);
            _sunDegrees = (_sunDegrees - 0.5f * KWEngine.DeltaTimeFactor) % 360f;   // this will cycle from 0 to 359 and then back to 0


            // RED LIGHT CONTROL:
            Vector3 mouseCursorPos = HelperIntersection.GetMouseIntersectionPoint(ms, Plane.Y, 3);
            _mouseLight.SetPosition(mouseCursorPos);
            _mouseLightSphere.SetPosition(mouseCursorPos);
        }

        public override void Prepare()
        {
            // Load custom model from model folder as "Nightshade":
            KWEngine.LoadModelFromFile("Nightshade", @".\models\Nightshade.fbx");

            // Initialize _sun attribute with a new LightObject instance of type "sun":
            _sun = new LightObject(LightType.Sun, true);
            _sun.SetPosition(20, 20, 20);
            _sun.SetFOV(25);                        // sun's field of view (the higher, the wider)
            _sun.SetColor(1, 1, 1, 2.5f);           // sunlight color and intensity (2.5f)
            _sun.SetNearAndFarBounds(10, 50);       // define the distance range (z-depth) that the sun 'sees'
            AddLightObject(_sun);
            //DebugShadowLight = _sun;              // optional: use to see the scene from the sun's perspective

            // Initialize another light object that follows the mouse cursor:
            _mouseLight = new LightObject(LightType.Point, true);
            _mouseLight.SetPosition(0, 5, 0);
            _mouseLight.SetColor(1, 0, 0, 5);
            _mouseLight.SetNearAndFarBounds(0.1f, 5);
            AddLightObject(_mouseLight);

            // Initialize sphere object that shows the mouse cursor's position:
            _mouseLightSphere = new Sphere();
            _mouseLightSphere.SetModel("KWSphere");
            _mouseLightSphere.SetGlow(1, 0.25f, 0.25f, 1);
            _mouseLightSphere.SetColor(1, 0.25f, 0.25f);
            _mouseLightSphere.SetScale(0.25f);
            _mouseLightSphere.IsShadowCaster = false;       // do not cast or receive shadows
            _mouseLightSphere.IsAffectedByLight = false;    // do not react to light sources
            AddGameObject(_mouseLightSphere);

            SetAmbientLight(1, 1, 1, 0.25f);        // determine the ambient light color (for pixels that are not seen by the sun)
            SetCameraPosition(0, 25, 25);

            Floor f01 = new Floor();
            f01.SetModel("KWCube");
            f01.SetTexture(@".\textures\tiles.jpg", TextureType.Albedo, CubeSide.All);              // regular texture file
            f01.SetTexture(@".\textures\tiles_normal.jpg", TextureType.Normal, CubeSide.All);       // (optional) normal map for custom light reflections
            f01.SetTexture(@".\textures\tiles_roughness.jpg", TextureType.Roughness, CubeSide.All); // (optional) roughness map for specular highlights
            f01.SetTextureRepeat(3, 3);                                                             // how many times the texture is tiled across the object?
            f01.SetScale(15, 0.2f, 15);                                                             
            f01.SetPosition(0, -0.1f, 0);
            f01.IsShadowCaster = true;                                                              // does the object cast and receive shadows? (default: false)
            AddGameObject(f01);

            Immovable i01 = new Immovable();
            i01.SetModel("KWSphere");
            i01.SetPosition(5, 2.5f, 5);
            i01.SetScale(5);
            i01.IsShadowCaster = true;                                                              // does the object cast and receive shadows? (default: false)
            AddGameObject(i01);

            Immovable i02 = new Immovable();
            i02.SetModel("Nightshade");
            i02.SetPosition(-2.5f, 0, -1.25f);
            i02.SetScale(2f);
            i02.IsShadowCaster = true;                                                              // does the object cast and receive shadows? (default: false)
            i02.AnimationID = 0;                                                                    // set the object's animation id (for animated models)
            i02.AnimationPercentage = 0;                                                            // set the object's animation frame (range always from 0 to 1)
            AddGameObject(i02);
        }
    }
}

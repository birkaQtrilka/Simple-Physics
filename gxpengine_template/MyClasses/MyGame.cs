using System;
using GXPEngine;
using gxpengine_template.MyClasses.SpacePartitioning;
using gxpengine_template.MyClasses.TankGame;

namespace gxpengine_template.MyClasses
{
    public class MyGame : Game
    {
        bool Approximate(float givenNumber, float to, float precission = 0.0001f)
        {
            return Mathf.Abs(givenNumber - to) <= precission;
        }
        static void Main()
        {
            new MyGame().Start();
        }

        public MyGame() : base(800, 600,false,false)
        {
           LoadScene();
        }
        void LoadScene()
        {
            //LateAddChild(new GameManager(game.width,game.height));
            LateAddChild(new Displayer(game.width,game.height));
            //UnitTests();
        }

        //tests here
        void UnitTests()
        {
            var v1 = new Vec2(4, 3);
            var v2 = v1;
            v1.Normalize();
            if (!(Approximate(v1.x, 0.8f) && Approximate(v1.y, 0.6f)))
                Console.WriteLine($"Normalize {v2}. Is {v1}, should be 0.80, 0.60");
            else
                Console.WriteLine($"Normalize {v2}: Is {v1}, correct");

            if (Approximate(v1.x, v2.x) && Approximate(v1.y, v2.x))
                Console.WriteLine($"The state of v1 has changed: false, should be true");
            else
                Console.WriteLine($"The state of v1 has changed: true, correct");

            v1 = new Vec2(4, 3);
            v2 = v1.Normalized();

            if (!(Approximate(v2.x, 0.8f) && Approximate(v2.y, 0.6f)))
                Console.WriteLine($"{v1} Normalized. Is {v2}, should be 0.80, 0.60");
            else
                Console.WriteLine($"{v1} Normalized: Is {v1}, correct");

            if (Approximate(v1.x, v2.x) && Approximate(v1.y, v2.x))
                Console.WriteLine($"The state of v1 has changed: true, should be false");
            else
                Console.WriteLine($"The state of v1 has changed: false, correct");

            float degToRad = Vec2.Deg2Rad(69);
            if (Approximate(degToRad, (69 / 180f) * Mathf.PI))
                Console.WriteLine("Deg2Rad: correct ");
            else 
                Console.WriteLine("Deg2Rad faulty: should be " + ((69 / 180f) * Mathf.PI) + ", is " + degToRad);

            float radToDeg = Vec2.Rad2Deg(1.6873294838247f);
            if (Approximate(radToDeg, (1.6873294838247f * 180) / Mathf.PI))
                Console.WriteLine("Rad2Deg: correct ");
            else 
                Console.WriteLine("Rad2Deg faulty: should be " + ((1.6873294838247f * 180) / Mathf.PI) + ", is " + radToDeg);

            Vec2 uVecDeg = Vec2.GetUnitVectorDeg(87);
            Vec2 expectedUVD = new Vec2(Mathf.Cos(87 / 180f * Mathf.PI), Mathf.Sin(87 / 180f * Mathf.PI));
            if (Approximate(uVecDeg.x, expectedUVD.x) && Approximate(uVecDeg.y, expectedUVD.y))
                Console.WriteLine("GetUnitVectorDeg: correct ");
            else 
                Console.WriteLine("GetUnitVectorDeg faulty: should be " + expectedUVD + ", is " + uVecDeg);

            Vec2 uVecRad = Vec2.GetUnitVectorRad(.866f);
            Vec2 expectedUVR = new Vec2(Mathf.Cos(.866f), Mathf.Sin(.866f));
            if (Approximate(uVecRad.x, expectedUVR.x) && Approximate(uVecRad.y, expectedUVR.y))
                Console.WriteLine("GetUnitVectorRad: correct ");
            else 
                Console.WriteLine("GetUnitVectorRad faulty: should be " + expectedUVR + ", is " + uVecRad);

            Vec2 random = Vec2.RandomUnitVector();
            if (Approximate(random.x * random.x + random.y * random.y, 1))
                Console.WriteLine("RandomUnitVector is normalized (ok)");
            else Console.WriteLine("RandomUnitVector faulty: result " + random + " is not normalized");

            for (int j = 0; j < 1000; j++)
            {
                random += Vec2.RandomUnitVector();
            }
            Console.WriteLine("RandomUnitVector has a " + (int)(100 - random.Length * .1f) + "% chance to be correctly distributed");

            v1 = new Vec2(1, 0);
            v1.SetAngleDegrees(90);
            if (!(Approximate(v1.x, 0) && Approximate(v1.y, 1f)))
                Console.WriteLine($"Set angle degrees to 90. Is {v1}, should be 0.00,1.00");
            else
                Console.WriteLine($"Set angle degrees to 90: Is {v1}, correct");

            v1.SetAngleRadians(Mathf.PI);
            if (!(Approximate(v1.x, -1) && Approximate(v1.y, 0)))
                Console.WriteLine($"Set angle Radians to PI. Is {v1}, should be -1.00, 0.00");
            else
                Console.WriteLine($"Set angle Radians to PI: Is {v1}, correct");

            v1.SetAngleRadians(Mathf.PI / 3);
            float result = v1.GetAngleRadians();
            if (!Approximate(result, Mathf.PI / 3))
                Console.WriteLine($"Get angle Radians to Mathf.PI/3. Is {result}, should be {Mathf.PI / 3}");
            else
                Console.WriteLine($"Set angle Radians to Mathf.PI/3: Is {result}, correct");

            v1.SetAngleDegrees(75);
            result = v1.GetAngleDegrees();
            if (!Approximate(result, 75))
                Console.WriteLine($"Get angle degrees to 180. Is {result}, should be 180");
            else
                Console.WriteLine($"Set angle degrees to 75: Is {result}, correct");

            v1 = new Vec2(1, 0);
            v1.RotateRadians(3 * Mathf.PI / 5);
            result = v1.GetAngleRadians();
            if (!Approximate(result, 3 * Mathf.PI / 5))
                Console.WriteLine($"Rotate 1,0 vector by 3 * Mathf.PI / 5 Radians. Is {result}, should be {3 * Mathf.PI / 5}");
            else
                Console.WriteLine($"Rotate 1,0 vector by 3 * Mathf.PI / 5 Radians: Is {result}, correct");
            v1 = new Vec2(1, 0);
            v1.RotateDegrees(32);
            result = v1.GetAngleDegrees();
            if (!Approximate(result, 32))
                Console.WriteLine($"Rotate 1,0 vector by 32 degrees. Is {result}, should be {32}");
            else
                Console.WriteLine($"Rotate 1,0 vector by 32 degrees: Is {result}, correct");

            Vec2 pivot = new Vec2(1, 2);
            v1 = new Vec2(1, 0);
            v1.RotateAroundDegrees(90, pivot);

            if (!(Approximate(v1.x, 3) && Approximate(v1.y, 2f)))
                Console.WriteLine($"Rotate around {pivot} point by 90 degrees. Is {v1}, should be 3.00, 2.00 ");
            else
                Console.WriteLine($"Rotate around {pivot} point by 90 degrees: Is {v1}, correct");

            pivot = new Vec2(1, 2);
            v1 = new Vec2(1, 0);
            v1.RotateAroundRadians(Mathf.PI / 2, pivot);

            if (!(Approximate(v1.x, 3) && Approximate(v1.y, 2f)))
                Console.WriteLine($"Rotate around {pivot} point by Mathf.PI / 2 radians. Is {v1}, should be 2.00, 1.00 ");
            else
                Console.WriteLine($"Rotate around {pivot} point by Mathf.PI / 2 radians: Is {v1}, correct");

            v1 = new Vec2(1, 8);
            v2 = new Vec2(7, 0);
            var normal = (v2 - v1).Normal();

            if (!(Approximate(normal.x, .8f) && Approximate(normal.y, .6f)))
                Console.WriteLine($"Normal of line with angle {(v2 - v1).GetAngleDegrees()} degrees. Is {normal}, should be 0.80, 0.60");
            else
                Console.WriteLine($"Normal of line {v2 - v1}: Is {normal}, correct");

            v1 = new Vec2 (8,6);
            v2 = new Vec2 (11,2);
            var dot = v1.Dot(v2);

            if (!Approximate(dot, 100))
                Console.WriteLine($"dot between {v1} and {v2}. Is {dot}, should be 100");
            else
                Console.WriteLine($"dot between {v1} and {v2}: Is {dot}, correct");

            v2 = v1;
            v1.RotateDegrees(90);
            dot = v1.Dot(v2);

            if (!Approximate(dot, 0))
                Console.WriteLine($"dot between {v1} and {v2}. Is {dot}, should be 0");
            else
                Console.WriteLine($"dot between {v1} and {v2}: Is {dot}, correct");

            normal = Vec2.GetUnitVectorDeg(45);
            v1 = new Vec2(0, -1);
            v2 = v1.Reflect(normal, 1);
            if (!(Approximate(v2.x, 1f) && Approximate(v2.y, 0f)))
                Console.WriteLine($"Reflect {v1} by normal {normal}. Is {v2}, should be 1.00 0.00");
            else
                Console.WriteLine($"Reflect {v1} by normal {normal}: Is {v2}, correct");

            v1 = new Vec2(8, 6);
            v2 = v1;
            var expected = Vec2.Deg2Rad(75);
            v1.RotateRadians(expected);

            result = v1.AngleBetween(v2);

            if (!Approximate(result, expected))
                Console.WriteLine($"Angle between {v1} and {v2}. Is {result}, should be {Vec2.Deg2Rad(75)}");
            else
                Console.WriteLine($"Angle between {v1} and {v2}: Is {result}, correct");
        }
    }
}

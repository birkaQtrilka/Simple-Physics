using System.Globalization;

namespace GXPEngine
{
    public struct Vec2
    {
        public float x;
        public float y;

        public float Length
        {
            get => Mathf.Sqrt(x * x + y * y);
        }

        public static Vec2 zero = new Vec2(0f, 0f);
        public static Vec2 one = new Vec2(1f, 1f);

        public static Vec2 up = new Vec2(0, 1f);
        public static Vec2 right = new Vec2(1f, 0);

        public Vec2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public Vec2 Normalized()
        {
            if (x == 0 && y == 0) return new Vec2();
            var l = Length;
            return new Vec2(x / l, y / l);
        }

        public void SetXY(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public void Normalize()
        {
            this = Normalized();
        }

        public void SetAngleDegrees(float deg)
        {
            float rad = Deg2Rad(deg);
            SetAngleRadians(rad);
        }

        public void SetAngleRadians(float rad)
        {
            float l = Length;
            x = l * Mathf.Cos(rad);
            y = l * Mathf.Sin(rad);
        }

        public float GetAngleRadians()
        {
            return Mathf.Atan2(y, x);
        }

        public float GetAngleDegrees()
        {
            return Rad2Deg(GetAngleRadians());
        }

        public void RotateRadians(float rads)
        {
            float cos = Mathf.Cos(rads);
            float sin = Mathf.Sin(rads);
            float xCopy = x;
            x = xCopy * cos - y * sin;
            y = xCopy * sin + y * cos;

        }

        public void RotateDegrees(float degrees)
        {
            float rads = Deg2Rad(degrees);
            RotateRadians(rads);
        }

        public void RotateAroundRadians(float rads, Vec2 point)
        {
            x -= point.x;
            y -= point.y;

            RotateRadians(rads);

            x += point.x;
            y += point.y;
        }

        public void RotateAroundDegrees(float degrees, Vec2 point)
        {
            RotateAroundRadians(Deg2Rad(degrees), point);
        }

        public static float Deg2Rad(float degrees)
        {
            return degrees * Mathf.PI / 180f;
        }

        public static float Rad2Deg(float rad)
        {
            return rad * 180f / Mathf.PI;
        }

        public static Vec2 GetUnitVectorDeg(float degrees)
        {
            float rads = Deg2Rad(degrees);//good?
            return new Vec2(Mathf.Cos(rads), Mathf.Sin(rads));
        }

        public static Vec2 GetUnitVectorRad(float rads)
        {
            return new Vec2(Mathf.Cos(rads), Mathf.Sin(rads));
        }

        public static Vec2 RandomUnitVector()
        {
            return GetUnitVectorRad(Utils.Random(0, 2 * Mathf.PI));
        }

        public float Dot(Vec2 other)
        {
            return x * other.x + y * other.y;
        }

        public Vec2 Normal()
        {
            return new Vec2(-y, x).Normalized();
        }

        public float AngleBetween(Vec2 other)
        {
            return Mathf.Acos(Dot(other) / (Length * other.Length));
        }

        public Vec2 Reflect(Vec2 normal, float bounciness)
        {
            return this - (1 + bounciness) * Dot(normal) * normal;
        }

        public override string ToString()
        {
            return string.Format(NumberFormatInfo.InvariantInfo, "{0:#,0.00}, {1:#,0.00}", x, y);
        }

        #region My additional methods
        public float DistanceTo(Vec2 pos)
        {
            return (pos - this).Length;
        }

        public float DistanceTo(float x, float y)
        {
            return new Vec2(x - this.x, y - this.y).Length;
        }

        public static Vec2 Lerp(Vec2 a, Vec2 b, float t)
        {
            return a + (b - a) * t;

        }

        public float ScalarProjectionOnto(Vec2 other, out Vec2 normalizedOther)
        {
            normalizedOther = other.Normalized();
            return Dot(normalizedOther);
        }

        public Vec2 GetReflectionRelativeTo(Vec2 vector)
        {
            return this - 2 * ScalarProjectionOnto(vector, out Vec2 normalizedOther) * normalizedOther;
        }

        #endregion

        #region Operator overloads
        //public static bool operator ==(Vec2 b1, Vec2 b2)
        //{
        //    return
        //}
        //public static bool operator !=(Vec2 b1, Vec2 b2)
        //{

        //}

        public static Vec2 operator +(Vec2 v1, Vec2 v2) =>
         new Vec2(v1.x + v2.x, v1.y + v2.y);


        public static Vec2 operator -(Vec2 v1) => 
            new Vec2(-v1.x, -v1.y);

        public static Vec2 operator -(Vec2 v1, Vec2 v2) => 
            new Vec2(v1.x - v2.x, v1.y - v2.y);

        public static Vec2 operator *(Vec2 v1, float s)
            => new Vec2(v1.x * s, v1.y * s);
        public static Vec2 operator /(Vec2 v1, float s)
                    => new Vec2(v1.x / s, v1.y / s);

        public static Vec2 operator *(float s, Vec2 v1)
            => new Vec2(v1.x * s, v1.y * s);

        #endregion


    }
}

using GXPEngine;
using System;
namespace Physics
{
    public class Circle : Collider
    {
        public float Radius { get; set; }

        public Circle(GameObject pOwner, Vec2 startPosition, float radius) : base(pOwner, startPosition)
        {
            Radius = radius;
        }

        public override CollisionInfo GetEarliestCollision(Collider other, Vec2 velocity)
        {
            if(other is Circle circle)
            {
                 return CircleCollision(circle, velocity);
            }
            else if(other is AngledLine line)
            {
                return LineCollision(line, velocity);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        CollisionInfo LineCollision(AngledLine line, Vec2 velocity)
        {
            Vec2 lineDir = (line.End - line.Start);
            Vec2 lineNormal = lineDir.Normal();

            Vec2 startLineToOldBall = position - line.Start;

            float distanceToLine = startLineToOldBall.Dot(lineNormal) - Radius;//distance from point on circumferance
            float wholeDistance = -velocity.Dot(lineNormal);
            float timeOfImpact;
            if (wholeDistance <= 0) return null; //opposite way facing with back

            if (distanceToLine >= 0)
                timeOfImpact = distanceToLine / wholeDistance;
            else if (distanceToLine > -Radius)//currently colliding
                timeOfImpact = 0;
            else//past the line
                return null;

            if (timeOfImpact > 1) return null;// in the future


            Vec2 pointOfImpact =  position + velocity * timeOfImpact;

            float projectionFromPOI = (pointOfImpact - line.Start).Dot(lineDir.Normalized());//newProjection = d from slides

            if (projectionFromPOI < 0 || projectionFromPOI > lineDir.Length ) return null;

            return new CollisionInfo(lineNormal, line.owner, timeOfImpact, pointOfImpact);
        }

        CollisionInfo CircleCollision(Circle other, Vec2 velocity)
        {
            Vec2 relativePosition = position - other.position;

            float c = Mathf.Pow(relativePosition.Length, 2) - Mathf.Pow(Radius + other.Radius, 2);
            float b = (2 * relativePosition).Dot(velocity);

            if (c < 0)
            {
                if (b >= 0) return null;

                Vec2 normalOfCol = (position - other.position).Normalized();
                return new CollisionInfo(normalOfCol, other.owner, 0, position);
            }

            float a = Mathf.Pow(velocity.Length, 2);

            if (a < 0.000001f) return null;


            float delta = b * b - 4 * a * c;

            if (delta < 0) return null;
            
            float timeOfImpact = (-b - Mathf.Sqrt(delta)) / (2 * a);

            if (timeOfImpact < 0 || timeOfImpact >= 1) return null;

            Vec2 poi = position + velocity * timeOfImpact; // oldPos
            Vec2 normal = (poi - other.position).Normalized();
            return new CollisionInfo(normal, other.owner, timeOfImpact, poi);
        }

        public override bool Overlaps(Collider other)
        {
            if (other is Circle circle)
            {
                 return circle.position.DistanceTo(position) < Radius + circle.Radius;
            }
            else if(other is AngledLine line)
            {
                Vec2 startToCircle = position - line.Start;
                Vec2 lineDir = (line.End - line.Start);
                Vec2 lineNormal = lineDir.Normal();
                float distanceToLine = Mathf.Abs(startToCircle.Dot(lineNormal));

                float lineProjection = startToCircle.Dot(lineDir.Normalized());
                return distanceToLine < Radius && lineProjection >= 0 && lineProjection <= lineDir.Length;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        
    }
}

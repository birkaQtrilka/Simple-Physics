using GXPEngine;
namespace Physics {

	/// <summary>
	/// A superclass for all shapes in our physics engine (like circles, lines, AABBs, ...)
	/// </summary>
	public abstract class Collider 
	{
		public GameObject owner { get; }
		public Vec2 position { get; set; }

		public Collider(GameObject pOwner, Vec2 startPosition)
		{
			owner=pOwner;
			position=startPosition;
		}
		public abstract CollisionInfo GetEarliestCollision(Collider other, Vec2 velocity);

		public abstract bool Overlaps(Collider other);
		
	}
}

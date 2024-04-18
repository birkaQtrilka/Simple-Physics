using GXPEngine;
using System.Collections.Generic;

namespace Physics {
	// This class holds our own colliders - similar to the GXPEngine's CollisionManager, but
	//  we can put any shape here.
	// Using MoveUntilCollision, you can move colliders (even those that aren't registered in this class!),
	//  while checking against collisions with the registered colliders. 
	public class ColliderManager {
		public static ColliderManager main {
			get {
				if (_main==null) {
					_main=new ColliderManager();
				}
				return _main;
			}
		}
		static ColliderManager _main;

		List<Collider> solidColliders;
		List<Collider> triggerColliders;

		public ColliderManager() {
			solidColliders=new List<Collider>();
			triggerColliders=new List<Collider>();
		}

		public void AddSolidCollider(Collider col) {
            solidColliders.Add(col);
		}

		public void RemoveSolidCollider(Collider col) {

            solidColliders.Remove(col);
		}

		public void AddTriggerCollider(Collider col) {
			triggerColliders.Add(col);
		}

		public void RemoveTriggerCollider(Collider col) {

            triggerColliders.Remove(col);
		}

		// Note: feel free to do something smart here like space partitioning, to improve efficiency:

		public CollisionInfo MoveUntilCollision(Collider col, Vec2 velocity) {
			CollisionInfo firstCollision = null;
			foreach (Collider other in solidColliders) {
				if (other!=col) {
					CollisionInfo colInfo = col.GetEarliestCollision(other, velocity);
					if (colInfo!=null && colInfo.timeOfImpact<1) {
						if (firstCollision==null || firstCollision.timeOfImpact>colInfo.timeOfImpact) {
							firstCollision=colInfo;
						}
					}
				}
			}
			return firstCollision;
		}

		//public List<Collider> GetTriggerOverlaps(Collider col) {
		//	List<Collider> overlaps = new List<Collider>();
		//	foreach (Collider other in triggerColliders) {
		//		if (other!=col && col.Overlaps(other)) {
		//			overlaps.Add(other);
		//		}
		//	}
		//	return overlaps;
		//}

  //      public List<Collider> GetSolidOverlaps(Collider col)
  //      {
  //          List<Collider> overlaps = new List<Collider>();
  //          foreach (Collider other in solidColliders)
  //          {
  //              if (other != col && col.Overlaps(other))
  //              {
  //                  overlaps.Add(other);
  //              }
  //          }
  //          return overlaps;
  //      }

        public List<Collider> GetOverlaps(Collider col)
        {
            List<Collider> overlaps = new List<Collider>();
            foreach (Collider other in triggerColliders)
            {
                if (other != col && col.Overlaps(other))
                {
                    overlaps.Add(other);
                }
            }
            foreach (Collider other in solidColliders)
            {
                if (other != col && col.Overlaps(other))
                {
                    overlaps.Add(other);
                }
            }
            return overlaps;
        }
    }
}
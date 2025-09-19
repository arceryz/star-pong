using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace StarPong.Framework
{
	/// <summary>
	/// Applies collision and overlap detection to objects in the scene tree.
	/// Only objects in the scene tree (being updated) are tracked.
	/// </summary>
	public class Physics
	{
		public static Physics Instance;
		Dictionary<CollisionObject, List<CollisionObject>> ActiveCollisions = new();

		public Physics()
		{
			Instance = this;
		}

		public void Update(float delta)
		{
			List<GameObject> objects = SceneTree.Instance.GetObjectsInGroup("physics");
			Debug.WriteLine(objects.Count);

			// Very inefficient collision scheme that does not take into account the
			// local position, nor sorting with a quadtree.
			for (int i = 0; i < objects.Count; i++)
			{
				for (int j = i + 1; j < objects.Count; j++)
				{
					CollisionObject cobj1 = objects[i] as CollisionObject;
					CollisionObject cobj2 = objects[j] as CollisionObject;
					if (!cobj1.CollisionEnabled || !cobj2.CollisionEnabled) continue;

					bool colliding = cobj1.GetBoundingRect().IsOverlapping(cobj2.GetBoundingRect());
					bool previouslyColliding = GetCollidingObjects(cobj1).Contains(cobj2);

					if (!previouslyColliding && colliding)
					{
						// TODO: Implement position and normal.
						cobj1.OnCollision(Vector2.Zero, Vector2.Zero, cobj2);
						cobj2.OnCollision(Vector2.Zero, Vector2.Zero, cobj1);

						AddCollision(cobj1, cobj2);
						AddCollision(cobj2, cobj1);
					}
					else if (previouslyColliding && !colliding)
					{
						RemoveCollision(cobj1, cobj2);
						RemoveCollision(cobj2, cobj1);
					}
				}
			}
		}

		public List<CollisionObject> GetCollidingObjects(CollisionObject cobj)
		{
			if (ActiveCollisions.ContainsKey(cobj))
			{
				return ActiveCollisions[cobj];
			}
			return new();
		}

		void AddCollision(CollisionObject cobj1, CollisionObject cobj2)
		{
			// If this is the first time this object is colliding,
			// we must also hook its behavior for when it leaves the tree.
			if (!ActiveCollisions.ContainsKey(cobj1))
			{
				cobj1.TreeExited += () => OnObjectTreeExited(cobj1);
				ActiveCollisions[cobj1] = new([cobj2]);
			}
			else
			{
				ActiveCollisions[cobj1].Add(cobj2);
			}
		}

		void RemoveCollision(CollisionObject cobj1, CollisionObject cobj2)
		{
			ActiveCollisions[cobj1].Remove(cobj2);
			ActiveCollisions[cobj2].Remove(cobj1);
		}

		/// <summary>
		/// When an object exits the tree, we want it to unregister from any collisions.
		/// Any active collisions will be marked as finished.
		/// </summary>
		/// <param name="cobj"></param>
		void OnObjectTreeExited(CollisionObject cobj)
		{
			if (ActiveCollisions.ContainsKey(cobj))
			{
				List<CollisionObject> activeColliders = ActiveCollisions[cobj];
				foreach (CollisionObject other in activeColliders)
				{
					ActiveCollisions[other].Remove(cobj);
				}
				ActiveCollisions.Remove(cobj);
			}
		}
	}
}

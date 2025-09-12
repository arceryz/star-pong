using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StarPong.Source.Framework
{
	public class GameObjectList
	{
		public List<GameObject> objects = new();
		public List<CollisionObject> collisionObjects = new();

		public GameObjectList()
		{
			
		}

		public void Add(GameObject obj)
		{
			objects.Add(obj);
			if (obj is CollisionObject cobj)
			{
				collisionObjects.Add(cobj);
			}
		}

		public void Draw(SpriteBatch batch)
		{
			foreach (GameObject obj in objects)
			{
				if (!obj.IsDestroyed)
				{
					obj.Draw(batch);
				}
			}
			foreach (GameObject obj in objects)
			{
				if (!obj.IsDestroyed)
				{
					obj.DebugDraw(batch);
				}
			}
		}

		public void Update(float delta)
		{
			// We can safely remove objects because we create a copy of the object list.
			foreach (GameObject obj in objects.ToList())
			{
				if (obj.IsDestroyed)
				{
					objects.Remove(obj);
					if (obj is CollisionObject cobj) {
						collisionObjects.Remove(cobj);
					}
				}
				else
				{
					obj.Update(delta);
				}
			}
		}

		public void CollideWith(GameObjectList with)
		{
			foreach (CollisionObject obj1 in collisionObjects)
			{
				foreach (CollisionObject obj2 in with.collisionObjects)
				{
					if (obj1.IsDestroyed || obj2.IsDestroyed 
						|| !obj1.CollisionEnabled || !obj2.CollisionEnabled
						|| obj1 == obj2) continue;
					if (obj1.GetBoundingRect().IsOverlapping(obj2.GetBoundingRect()))
					{
						obj1.OnCollision(obj2.Position, Vector2.Zero, obj2);
						obj2.OnCollision(obj1.Position, Vector2.Zero, obj1);
					}
				}
			}
		}
	}
}

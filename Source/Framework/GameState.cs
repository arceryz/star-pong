using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StarPong.Source.Framework
{
	public class GameState
	{
		public GameState() { }

		// To be overriden.
		public virtual void Initialize() { }
		public virtual void Update(float delta) { }
		public virtual void Draw(SpriteBatch batch) { }

		public void HandleCollisions(List<CollisionObject> objects)
		{
			for (int i = 0; i < objects.Count; i++)
			{
				for (int j = i + 1; j < objects.Count; j++)
				{
					CollisionObject coll1 = objects[i];
					CollisionObject coll2 = objects[j];
					if (coll1.IsColliding(coll2))
					{
						coll1.OnCollision(Vector2.Zero, Vector2.Zero, coll2);
						coll2.OnCollision(Vector2.Zero, Vector2.Zero, coll1);
					}
				}
			}
		}
	}
}

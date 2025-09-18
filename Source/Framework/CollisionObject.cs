using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace StarPong.Framework
{
	public class CollisionObject: GameObject
	{
		public Rect2 CollisionRect = Rect2.Zero;
		public Vector2 Velocity = Vector2.Zero;
		public Color DebugColor = Color.Lime;
		public bool CollisionEnabled = true;

		public CollisionObject()
		{
			AddToGroup("physics");
		}

		public virtual void OnCollision(Vector2 pos, Vector2 normal, CollisionObject other) { }

		public Rect2 GetBoundingRect()
		{
			return CollisionRect.Translated(GlobalPosition);
		}

		public bool IsMouseHovering()
		{
			Vector2 mpos = Input.GetMousePosition();
			Rect2 rect = CollisionRect.Translated(Position);
			return rect.ContainsPoint(new Vector2(mpos.X, mpos.Y));
		}

		public override void DebugDraw(SpriteBatch spriteBatch)
		{
			if (CollisionEnabled)
			{
				Engine.DebugDrawRect(GetBoundingRect(), DebugColor);
			}
		}

		/// <summary>
		/// Moves the object according to its velocity.
		/// </summary>
		/// <param name="delta"></param>
		public override void Update(float delta)
		{
			Position += Velocity * delta;
		}
	}
}

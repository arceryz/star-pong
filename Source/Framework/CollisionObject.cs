using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace StarPong.Source.Framework
{
	public class CollisionObject: GameObject
	{
		public Rect2 CollisionRect = Rect2.Zero;
		public Vector2 Velocity = Vector2.Zero;
		public Color DebugColor = Color.Lime;
		public bool CollisionEnabled = true;

		public virtual void OnCollision(Vector2 pos, Vector2 normal, CollisionObject other) { }

		public Rect2 GetBoundingRect() => CollisionRect.Translated(Position);
		public bool IsMouseHovering()
		{
			MouseState ms = Mouse.GetState();
			Rect2 rect = CollisionRect.Translated(Position);
			return rect.ContainsPoint(new Vector2(ms.Position.X, ms.Position.Y));
		}

		public override void DebugDraw(SpriteBatch spriteBatch)
		{
			if (CollisionEnabled)
			{
				Engine.Instance.DebugDrawRect(GetBoundingRect(), DebugColor);
			}
		}
	}
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StarPong.Source.Framework
{
	public class GameObject
	{
		public Vector2 Position = Vector2.Zero;
		public Color Color = Color.White;
		public bool IsDestroyed { get; private set; } = false;

		public virtual void Update(float delta) { }
		public virtual void DebugDraw(SpriteBatch spriteBatch) { }
		public virtual void Draw(SpriteBatch batch) {}
		public void Destroy() => IsDestroyed = true;
	}
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StarPong.Source.Framework
{
	public class GameObject
	{
		public Vector2 Position = Vector2.Zero;
		public Color Color = Color.White;

		public virtual void Update(float delta) { }
		public virtual void Draw(SpriteBatch batch) { }
	}
}

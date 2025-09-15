using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StarPong.Framework
{
	public class Label: GameObject
	{
		public string Text;
		public SpriteFont Font;
		public Color Color = Color.White;

		public Label(string text, SpriteFont font )
		{
			this.Text = text;
			this.Font = font;
		}

		public override void Draw(SpriteBatch batch)
		{
			DrawString(batch, Font, Text, GlobalPosition, Color);
		}
	}
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StarPong.Framework
{
	public class Label: GameObject
	{
		public string Text;
		public ImageFont Font;
		public float Scale = 1.0f;

		public Label(ImageFont font, string text="", float scale=1)
		{
			this.Text = text;
			this.Font = font;
			this.Scale = scale;
		}

		public override void Draw(SpriteBatch batch)
		{
			Font.DrawString(batch, GlobalPosition, Text, GlobalDrawZ, Scale);
		}
	}
}

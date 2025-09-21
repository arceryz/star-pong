using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StarPong.Framework
{
	/// <summary>
	/// Labels display a piece of text.
	/// </summary>
	public class Label: GameObject
	{
		public string Text;
		public ImageFont Font;
		public SpriteFont SFont;
		public float Scale = 1.0f;
		public Color Color = Color.White;

		public Label(ImageFont font, string text="", float scale=1, SpriteFont sfont=null)
		{
			this.Text = text;
			this.Font = font;
			this.Scale = scale;
			this.SFont = sfont;
		}

		public override void Draw(SpriteBatch batch)
		{
			if (SFont != null)
			{
				Vector2 center = SFont.MeasureString(Text) * 0.5f;
				batch.DrawString(SFont, Text, GlobalPosition, Color, 0, center, Scale, SpriteEffects.None, 0);
			}
			else
			{
				Font.DrawString(batch, GlobalPosition, Text, Scale);
			}
		}
	}
}

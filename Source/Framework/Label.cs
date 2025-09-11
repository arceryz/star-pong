using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace StarPong.Source.Framework
{
	public class Label: GameObject
	{
		static SpriteFont font;
		public static void LoadContent(ContentManager content)
		{
			font = content.Load<SpriteFont>("title_font");
		}

		public string Text;

		public Label(string _text, Color _color, Vector2 _position)
		{
			Text = _text;
			Position = _position;
			Color = _color;
		}

		public override void Draw(SpriteBatch batch)
		{
			Vector2 size = font.MeasureString(Text);
			batch.DrawString(font, Text, Position - 0.5f * size, Color, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 1);
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Pong.Source
{
	public class Label
	{
		static SpriteFont font;
		public static void LoadContent(ContentManager content)
		{
			font = content.Load<SpriteFont>("title_font");
		}

		string text;
		public Vector2 Position;
		Color color;

		public Label(string _text, Color _color, Vector2 _position)
		{
			text = _text;
			Position = _position;
			color = _color;
		}

		public void Draw(SpriteBatch batch)
		{
			Vector2 size = font.MeasureString(text);
			batch.DrawString(font, text, Position - 0.5f * size, color);
		}

		public void Update(float delta)
		{

		}
	}
}

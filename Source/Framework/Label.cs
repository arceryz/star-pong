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
		Color color;

		public Label(string text, Color color, Vector2 position)
		{
			this.Text = text;
			this.Position = position;
			this.color = color;
		}

		public override void Draw(SpriteBatch batch)
		{
			DrawString(batch, font, Text, Position, color);
		}
	}
}

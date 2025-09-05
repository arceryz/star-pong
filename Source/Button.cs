using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Pong.Source
{
	public class Button
	{
		static Texture2D buttonGlossPressed;
		static Texture2D buttonGloss;
		static SpriteFont font;

		public static void LoadContent(ContentManager content)
		{
			buttonGloss = content.Load<Texture2D>("button_rectangle_gloss");
			buttonGlossPressed = content.Load<Texture2D>("button_rectangle_depth_gloss");
			font = content.Load<SpriteFont>("button_font");
		}

		string text;
		Vector2 position;
		Color color;
		float scale;

		public Button(string _text, Color _color, Vector2 _position)
		{
			text = _text;	
			position = _position;
			color = _color;
		}

		public void Update(float delta)
		{

		}

		public void Draw(SpriteBatch batch)
		{
			batch.Draw(buttonGloss, position - new Vector2(buttonGloss.Width, buttonGloss.Height) * 0.5f, Color.White);

			Vector2 size = font.MeasureString(text);
			batch.DrawString(font, text, position - 0.5f * size, color);
		}
	}
}

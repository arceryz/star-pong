using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace StarPong.Source.Framework
{
	public class Button: CollisionObject
	{
		static Texture2D buttonGlossDepth;
		static Texture2D buttonGloss;
		static SpriteFont font;

		public static void LoadContent(ContentManager content)
		{
			buttonGloss = content.Load<Texture2D>("button_rectangle_gloss");
			buttonGlossDepth = content.Load<Texture2D>("button_rectangle_depth_gloss");
			font = content.Load<SpriteFont>("button_font");
		}

		public Action Pressed;
		public string Text;
		bool isPressed = false;

		public Button(string _text, Color _color, Vector2 _position)
		{
			Text = _text;	
			Position = _position;
			Color = _color;
			CollisionRect = new Rect2(buttonGloss.Bounds).Centered();
		}

		public override void Update(float delta)
		{
			MouseState state = Mouse.GetState();
			if (IsMouseHovering())
			{
				if (state.LeftButton == ButtonState.Pressed)
				{
					isPressed = true;
				}
				if (isPressed && state.LeftButton == ButtonState.Released)
				{
					isPressed = false;
					Pressed?.Invoke();
				}
			}
			else
			{
				isPressed = false;
			}
		}

		public override void Draw(SpriteBatch batch)
		{
			Texture2D tex = isPressed ? buttonGloss : buttonGlossDepth;
			batch.Draw(tex, Position - new Vector2(buttonGloss.Width, buttonGloss.Height) * 0.5f, Color.White);

			Vector2 size = font.MeasureString(Text);
			batch.DrawString(font, Text, Position - 0.5f * size, Color);
		}
	}
}

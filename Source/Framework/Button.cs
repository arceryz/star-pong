using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace StarPong.Source.Framework
{
	public class Button: CollisionObject
	{
		static Texture2D selectionArrowsTex;
		static SpriteFont font;

		public static void LoadContent(ContentManager content)
		{
			selectionArrowsTex = content.Load<Texture2D>("UI/Selection_Arrows");
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
			CollisionRect = new Rect2(selectionArrowsTex.Bounds).Centered();
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
			if (IsMouseHovering())
			{
				batch.Draw(selectionArrowsTex, Utility.CenterToTex(Position, selectionArrowsTex), Color.White);
			}

			Vector2 size = font.MeasureString(Text);
			batch.DrawString(font, Text, Position - 0.5f * size, Color);
		}
	}
}

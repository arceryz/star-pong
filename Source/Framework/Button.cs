using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace StarPong.Framework
{
	public class Button: CollisionObject
	{
		public Action Pressed;

		public string Text;
		public Color Color = Color.White;
		public SpriteFont Font;
		public Texture2D SelectionTexture;

		bool isPressed = false;

		public Button(string text, SpriteFont font, Texture2D selectionTexture)
		{
			this.Text = text;
			this.Font = font;
			this.SelectionTexture = selectionTexture;
			this.CollisionRect = new Rect2(selectionTexture.Bounds).Centered();
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
				DrawTexture(batch, SelectionTexture, GlobalPosition, Color.White);
			}
			DrawString(batch, Font, Text, Position, Color);
		}
	}
}

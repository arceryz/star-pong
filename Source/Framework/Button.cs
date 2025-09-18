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
		public ImageFont Font;
		public Texture2D SelectionTexture;
		public float FontScale;
		public float FlickerInterval = 0;

		float flickerTimer = 0;
		bool isPressed = false;

		public Button(ImageFont font, string text, Texture2D selectionTexture, float fontScale=1)
		{
			this.FontScale = fontScale;
			this.Text = text;
			this.Font = font;
			this.SelectionTexture = selectionTexture;
			this.CollisionRect = new Rect2(selectionTexture.Bounds).Centered();
		}

		public Button SetFlicker(float interval)
		{
			FlickerInterval = interval;
			return this;
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

			if (FlickerInterval > 0)
			{
				flickerTimer += delta;
				if (flickerTimer > 2 * FlickerInterval)
				{
					flickerTimer = 0;
				}
			}
		}

		public override void Draw(SpriteBatch batch)
		{
			if (IsMouseHovering())
			{
				DrawTexture(batch, SelectionTexture, GlobalPosition, Color.White);
			}
			if (flickerTimer < FlickerInterval)
			{
				Font.DrawString(batch, GlobalPosition, Text, FontScale);
			}
		}
	}
}

using System;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace StarPong.Framework
{
	/// <summary>
	/// The button is self-explanatory. It can be clicked and has text from an image font.
	/// It uses the collision rectangle for mouse hover detection, but is not really a collision object.
	/// </summary>
	public class Button: CollisionObject
	{
		public Action Pressed;

		public string Text;
		public ImageFont Font;
		public float FontScale;
		public float FlickerInterval = 0;

		Sprite selector;
		float flickerTimer = 0;
		bool isPressed = false;

		SoundEffectInstance clickSFX;
		SoundEffectInstance hoverSFX;

		public Button(ImageFont font, string text, float fontScale=1)
		{
			this.FontScale = fontScale;
			this.Text = text;
			this.Font = font;

			selector = new Sprite(Engine.Load<Texture2D>(Assets.Textures.UI_SelectionArrows), 5, 1);
			selector.Scale = 4;
			selector.AddAnimation("default", 10, 0, 0, 5, true);
			selector.Play("default");
			AddChild(this.selector);

			CollisionRect = selector.FrameSize.Centered().Scaled(selector.Scale, selector.Scale);

			clickSFX = Engine.Load<SoundEffect>(Assets.Sounds.UI_Button_Click).CreateInstance();
			hoverSFX = Engine.Load<SoundEffect>(Assets.Sounds.UI_Button_Hover).CreateInstance();
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
					clickSFX.Stop();
					clickSFX.Play();
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
			if (!selector.Visible && IsMouseHovering())
			{
				hoverSFX.Stop();
				hoverSFX.Play();
			}
			selector.Visible = IsMouseHovering();

			if (flickerTimer < FlickerInterval || FlickerInterval == 0)
			{
				Font.DrawString(batch, GlobalPosition, Text, FontScale);
			}
		}
	}
}

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
	/// <summary>
	/// A number that flies off in a direction and stops while flickering.
	/// Then self-destructs. Useful for damage and energy indicators.
	/// </summary>
	public class FlickerNumber: GameObject
	{
		public static SpriteFont PixelFont;
		public static SpriteFont SmallPixelFont;

		public static void LoadContent(ContentManager content)
		{
			PixelFont = content.Load<SpriteFont>("UI/Pixel");
			SmallPixelFont = content.Load<SpriteFont>("UI/SmallPixel");
		}

		public static Color[] CriticalPal = [Color.Red, Color.Orange, Color.White];
		public static Color[] GreyPal = [Color.Gray, Color.DarkGray, Color.WhiteSmoke];
		public static Color[] YellowPal = [Color.Yellow, Color.White, Color.Orange];
		public static Color[] GreenPal = [Color.Green, Color.Lime, Color.LimeGreen];

		public string Text;
		Color[] palette;
		int colorIndex = 0;
		float flickerTimer = 0;
		float flickerInterval = 0.1f;
		float lifeTime = 1.0f;
		Vector2 velocity = Vector2.Zero;
		float damp = 0;
		public SpriteFont Font = PixelFont;

		public FlickerNumber(string _text, float _interval, float _lifetime, Vector2 _vel, float _damp, Color[] _palette)
		{
			Text = _text;
			palette = _palette;
			flickerInterval = _interval;
			velocity = _vel;
			damp = _damp;
			lifeTime = _lifetime;
			colorIndex = Utility.RandInt32() % palette.Length;
		}
		public override void Update(float delta)
		{
			Position += delta * velocity;
			velocity = velocity * Math.Max(0, 1.0f - damp * delta);

			flickerTimer += delta;
			if (flickerTimer > flickerInterval)
			{
				colorIndex = (colorIndex + 1) % palette.Length;
				flickerTimer = 0;
			}

			lifeTime -= delta;
			if (lifeTime < 0)
			{
				Destroy();
			}
		}

		public override void Draw(SpriteBatch batch)
		{
			batch.DrawString(PixelFont, Text, Position - 0.5f * PixelFont.MeasureString(Text), palette[colorIndex]);
		}
	}
}

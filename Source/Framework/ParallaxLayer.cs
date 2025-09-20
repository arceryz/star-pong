using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StarPong.Framework
{
	public class ParallaxLayer: GameObject
	{
		float speed = 1.0f;
		Texture2D texture;
		public Color Color = Color.White;

		int[] slides = new int[2];
		float stepAccumulator = 0;

		public ParallaxLayer(Texture2D tex, float _speed)
		{
			speed = _speed;
			texture = tex;

			slides[0] = 0;
			slides[1] = texture.Width;
		}

		public override void Update(float delta)
		{
			// Accumulate sub-pixel values to avoid artefacts on the boundary.
			stepAccumulator += delta * speed;
			int step = (int)Math.Floor(stepAccumulator);
			stepAccumulator -= step;

			for (int i = 0; i < slides.Length; i++)
			{
				slides[i] -= step;
			}
			for (int i = 0; i < slides.Length; i++)
			{
				int s = slides[i];
				if (s <= - texture.Width)
				{
					int nextSlide = slides[(i + 1) % slides.Length];
					s = nextSlide + texture.Width;
				}
				slides[i] = s;
			}
		}

		public override void Draw(SpriteBatch batch)
		{
			foreach (float x in slides)
			{
				DrawTexture(batch, texture, new Vector2(x, 0), Color, false, false);
			}
		}
	}
}

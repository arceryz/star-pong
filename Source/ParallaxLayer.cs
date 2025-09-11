using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using StarPong.Source.Framework;

namespace StarPong.Source
{
	public class ParallaxLayer: GameObject
	{
		public static Texture2D Stars;
		public static Texture2D Asteroids_Mid;
		public static Texture2D Asteroids_Close;

		public static void LoadContent(ContentManager content)
		{
			Stars = content.Load<Texture2D>("Background/BG_Stars");
			Asteroids_Mid = content.Load<Texture2D>("Background/BG_Asteroids_Mid");
			Asteroids_Close = content.Load<Texture2D>("Background/BG_Asteroids_Far");
		}

		float speed = 1.0f;
		Texture2D texture;

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
				batch.Draw(texture, new Vector2(x, 0), Color.White);
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using StarPong.Source.Framework;

namespace StarPong.Source
{
	public class Mothership: CollisionObject
	{
		static Vector2 swayOffset = new Vector2(-100, -100);
		static float swayStrength = 35.0f;

		static Texture2D motherBlueTex;
		static Texture2D motherRedTex;

		public static void LoadContent(ContentManager content)
		{
			motherBlueTex = content.Load<Texture2D>("Mothership/Mother_Blue");
			motherRedTex = content.Load<Texture2D>("Mothership/Mother_Red");
			
		}

		Team side;
		Texture2D shipTexture;
		FastNoiseLite noise;

		public Mothership(Team _side)
		{
			side = _side;
			shipTexture = side == Team.Blue ? motherBlueTex : motherRedTex;

			noise = new FastNoiseLite(Utility.RandInt32());
			noise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
			noise.SetFrequency(0.25f);
		}

		public override void Update(float delta)
		{
			base.Update(delta);
			float sw = shipTexture.Bounds.Width;

			if (side == Team.Blue)
			{
				Position = swayOffset;
			}
			else
			{
				Position.X = swayOffset.X + Engine.Instance.ScreenWidth - 75;
				Position.Y = swayOffset.Y;
			}

			Position.X += noise.GetNoise(Engine.Instance.Time, 0) * swayStrength;
			Position.Y += noise.GetNoise(Engine.Instance.Time, 1000) * swayStrength;
		}

		public override void Draw(SpriteBatch batch)
		{
			batch.Draw(shipTexture, Position, null, Color.White);
		}
	}
}

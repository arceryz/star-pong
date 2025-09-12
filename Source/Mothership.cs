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
		static Texture2D motherBlueTex;
		static Texture2D motherRedTex;

		public static void LoadContent(ContentManager content)
		{
			motherBlueTex = content.Load<Texture2D>("Mothership/Mother_Blue");
			motherRedTex = content.Load<Texture2D>("Mothership/Mother_Red");
			
		}

		public enum HullStatus
		{
			Strong,
			Damaged,
			Critical
		}

		static Vector2 swayOffset = new Vector2(-100, -100);
		static float swayStrength = 35.0f;

		public int HullPoints = 300;
		HullStatus hullStatus = HullStatus.Strong;

		public Action Destroyed;
		public Team Side;
		Texture2D shipTexture;
		FastNoiseLite noise;

		public Mothership(Team _side)
		{
			Side = _side;
			shipTexture = Side == Team.Blue ? motherBlueTex : motherRedTex;

			noise = new FastNoiseLite(Utility.RandInt32());
			noise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
			noise.SetFrequency(0.25f);

			CollisionRect = new Rect2(shipTexture.Bounds).Scaled(0.3f, 1.0f);
 		}

		public override void Update(float delta)
		{
			base.Update(delta);
			float sw = shipTexture.Bounds.Width;

			if (Side == Team.Blue)
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
			SpriteEffects eff = Side == Team.Red ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			batch.Draw(shipTexture, Position, null, Color.White, 0, Vector2.Zero, 1.0f, eff, 0);
		}

		public void StartDestruction()
		{
			Destroyed?.Invoke();
		}

		public void TakeDamage(int dmg, Vector2 pos)
		{
			HullPoints -= dmg;
			if (HullPoints <= 0)
			{
				StartDestruction();
				return;
			}

			// Transitions to call explosions.
			if (HullPoints < 200 && hullStatus == HullStatus.Strong)
			{
				hullStatus = HullStatus.Damaged;
				// Strong to damaged.
			}
			if (HullPoints < 100 && hullStatus == HullStatus.Damaged)
			{
				hullStatus = HullStatus.Critical;
				// Critical to damaged.
			}
		}

		public override void OnCollision(Vector2 pos, Vector2 normal, CollisionObject other)
		{
			if (other is Bullet bul && bul.Player.Side != Side)
			{
				TakeDamage(1, pos);
			}
			if (other is Bomb)
			{
				TakeDamage(100, pos);
			}
		}
	}
}

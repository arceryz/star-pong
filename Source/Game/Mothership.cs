using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StarPong.Framework;

namespace StarPong.Game
{
	public class Mothership: CollisionObject, IDamageable
	{
		public enum HullStatus
		{
			Strong,
			Damaged,
			Critical
		}

		const float swayStrength = 50.0f;
		const float swaySpeed = 1.0f;

		public int Health { get; set; } = 300;
		public Team Team { get; set; }
		public Action Exploded;

		HullStatus hullStatus = HullStatus.Strong;
		Texture2D texture;
		FastNoiseLite noise;

		public Mothership(Team team)
		{
			this.Team = team;
			if (team == Team.Blue)
			{
				texture = Engine.Load<Texture2D>(AssetPaths.Texture.Blue_Mothership);
			}
			else
			{
				Flip = true;
				texture = Engine.Load<Texture2D>(AssetPaths.Texture.Red_Mothership);
			}

			noise = new FastNoiseLite(Utility.RandInt32());
			noise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
			noise.SetFrequency(0.25f);
			CollisionRect = new Rect2(texture.Bounds).Centered().Scaled(0.3f, 1.0f);
 		}

		public override void Update(float delta)
		{
			float sign = Flip ? 1 : -1;
			Position = Engine.GetAnchor(sign, 0, sign * -35);
			Position.X += noise.GetNoise(Engine.Time * swaySpeed, 0) * swayStrength;
			Position.Y += noise.GetNoise(-Engine.Time * swaySpeed, 1000) * swayStrength;
		}

		public override void Draw(SpriteBatch batch)
		{
			DrawTexture(batch, texture, GlobalPosition, Color.White, Flip);
		}

		public void Explode()
		{
			Exploded?.Invoke();
		}

		public void TakeDamage(int dmg, Vector2 loc)
		{
			Health -= dmg;
			if (Health <= 0)
			{
				Explode();
				return;
			}

			// Transitions to call explosions.
			if (Health < 200 && hullStatus == HullStatus.Strong)
			{
				hullStatus = HullStatus.Damaged;
				// Strong to damaged.
			}
			if (Health < 100 && hullStatus == HullStatus.Damaged)
			{
				hullStatus = HullStatus.Critical;
				// Critical to damaged.
			}
		}
	}
}

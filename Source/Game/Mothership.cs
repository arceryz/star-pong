using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StarPong.Framework;

namespace StarPong.Game
{
	public class Mothership: CollisionObject, IDamageable
	{
		public enum HullStatusEnum
		{
			Strong = 3,
			Damaged = 2,
			Critical = 1,
			Destroyed = 0
		}

		const float swayStrength = 50.0f;
		const float swaySpeed = 1.0f;

		public int Health { get; private set; } = 100;
		public Team Team { get; private set; }
		public Action Exploded;
		public HullStatusEnum HullStatus { get; private set; } = HullStatusEnum.Strong;

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

			while (Health <= 0)
			{
				// Reduce hull status by one and restore health until all damage is resolved.
				// Just in case >100 dmg is dealt in one tick.
				if (HullStatus > 0) HullStatus--;
				Health += 100;

				if (HullStatus == HullStatusEnum.Damaged)
				{
				}
				if (HullStatus == HullStatusEnum.Critical)
				{
				}
				if (HullStatus == HullStatusEnum.Destroyed)
				{
					Explode();
					return;
				}
			}
		}
	}
}

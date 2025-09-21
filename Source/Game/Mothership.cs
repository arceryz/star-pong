using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using StarPong.Framework;
using StarPong.Scenes;

namespace StarPong.Game
{
	/// <summary>
	/// Motherships linger on the sides of the screen and serve as objectives to
	/// destroy and win the game. They explode spectacularly when breaking a stage of hull.
	/// </summary>
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
		public Action HullStatusChanged;
		public HullStatusEnum HullStatus { get; private set; } = HullStatusEnum.Strong;

		bool hasAlarmGone = false;
		float alarmTimer = 0;
		SoundEffectInstance alarmSFX;
		Texture2D texture;
		FastNoiseLite noise;

		public Mothership(Team team)
		{
			this.Team = team;
			if (team == Team.Blue)
			{
				texture = Engine.Load<Texture2D>(Assets.Textures.Blue_Mothership);
			}
			else
			{
				Flip = true;
				texture = Engine.Load<Texture2D>(Assets.Textures.Red_Mothership);
			}

			noise = new FastNoiseLite(Utility.RandInt32());
			noise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
			noise.SetFrequency(0.25f);
			CollisionRect = new Rect2(texture.Bounds).Centered().Scaled(0.3f, 1.0f);

			alarmSFX = Engine.Load<SoundEffect>(Assets.Sounds.Mother_Alarm).CreateInstance();
		}

		public override void Update(float delta)
		{
			if (HullStatus == HullStatusEnum.Critical)
			{
				alarmTimer += delta;
				if (!hasAlarmGone && alarmTimer > 1.0f)
				{
					alarmSFX.Play();
					hasAlarmGone = true;
				}
			}

			if (HullStatus != HullStatusEnum.Destroyed)
			{
				float sign = Flip ? 1 : -1;
				Position = Engine.GetAnchor(sign, 0, sign * -35);
				Position.X += noise.GetNoise(Engine.Time * swaySpeed, 0) * swayStrength;
				Position.Y += noise.GetNoise(-Engine.Time * swaySpeed, 1000) * swayStrength;
			}
		}

		public override void Draw(SpriteBatch batch)
		{
			DrawTexture(batch, texture, GlobalPosition, Color.White, Flip);
		}

		public int GetTotalHealth()
		{
			return MathHelper.Max(Health, 0) + MathHelper.Max((int)HullStatus-1,0) * 100;
		}

		public void TakeDamage(int dmg, Vector2 loc)
		{
			if (PlayingScene.IsGameFinished) return;
			Health -= dmg;
			while (Health <= 0 && HullStatus != HullStatusEnum.Destroyed)
			{
				// Reduce hull status by one and restore health until all damage is resolved.
				// Just in case >100 dmg is dealt in one tick.
				if (HullStatus > 0) HullStatus--;

				Rect2 rect = CollisionRect.Scaled(1.0f, 0.8f).Translated(ToGlobalDir(new Vector2(100, 0)));
				if (HullStatus == HullStatusEnum.Damaged || HullStatus == HullStatusEnum.Critical)
				{
					ChainExplosionFX exp = new ChainExplosionFX(rect, 1.5f, 0.3f, 0.5f, true);
					AddChild(exp);
					Health += 100;
				}
				if (HullStatus == HullStatusEnum.Destroyed)
				{
					ChainExplosionFX exp = new ChainExplosionFX(rect, 3.0f, 0.15f, 1.0f, true);
					exp.TreeExited += () => Exploded?.Invoke();
					AddChild(exp);
				}

				HullStatusChanged?.Invoke();
			}
		}
	}
}

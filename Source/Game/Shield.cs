using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using StarPong.Framework;

namespace StarPong.Game
{
	public class Shield: CollisionObject, IDamageable
	{
		public Action BulletHit;
		public Team Team { get; private set; }
		public int Health { get; private set; } = 0;
		public bool IsActive { get { return CollisionEnabled; } }

		Sprite sprite;
		SoundEffectInstance activateSFX;
		SoundEffectInstance deactivateSFX;
		SoundEffectInstance runningSFX;
		SoundEffectInstance deflectSFX;

		public Shield(Team team)
		{
			this.Team = team;
			Texture2D tex;
			if (Team == Team.Blue)
			{
				tex = Engine.Load<Texture2D>(Assets.Textures.Blue_Shield);
			}
			else
			{
				tex = Engine.Load<Texture2D>(Assets.Textures.Red_Shield);
			}

			sprite = new Sprite(tex, 4, 3);
			sprite.AddAnimation("activate", 8, 0, 0, 4, false);
			sprite.AddAnimation("deactivate", 8, 0, 1, 4, false);
			sprite.AddAnimation("running", 0, 0, 2, 1, true);
			sprite.RotationDeg = Team == Team.Blue ? 90 : -90;
			sprite.Scale = 2;
			sprite.AnimationFinished += OnAnimationFinished;
			AddChild(sprite);

			CollisionRect = new Rect2(-4, 0, 16, sprite.FrameSize.Width * 1.5f).Centered();
			CollisionEnabled = false;

			// Create instances to avoid duplicate sounds from occuring.
			// The regular SoundEffect pools instances internally, not what we want.
			activateSFX = Engine.Load<SoundEffect>(Assets.Sounds.Shield_Activate).CreateInstance();
			deactivateSFX = Engine.Load<SoundEffect>(Assets.Sounds.Shield_Deactivate).CreateInstance();
			runningSFX = Engine.Load<SoundEffect>(Assets.Sounds.Shield_Loop).CreateInstance();
			runningSFX.IsLooped = true;
			deflectSFX = Engine.Load<SoundEffect>(Assets.Sounds.Shield_Deflect).CreateInstance();
		}

		public override void ExitTree()
		{
			runningSFX.Stop();
		}

		public void Activate()
		{
			CollisionEnabled = true;
			Visible = true;
			sprite.Play("activate");
			activateSFX.Play();
		}

		public void Deactivate()
		{
			CollisionEnabled = false;
			sprite.Play("deactivate");
			deactivateSFX.Play();
			runningSFX.Stop();
		}

		public void OnAnimationFinished()
		{
			if (sprite.CurrentAnimation == "activate")
			{
				sprite.Play("running");
				runningSFX.Play();
			}
			if (sprite.CurrentAnimation == "deactivate") Visible = false;
		}

		public override void OnCollision(Vector2 pos, Vector2 normal, CollisionObject other)
		{
			if (other is IDamageable dmg && dmg.Team != Team && other is Bullet)
			{
				BulletHit?.Invoke();
			}

			// For the bomb we will reflect it around a spherical-ish normal.
			if (other is Bomb bomb)
			{
				// Ignore collision if the bomb is behind the shield.
				if (bomb.Velocity.X > 0 && bomb.GlobalPosition.X > GlobalPosition.X ||
					bomb.Velocity.X < 0 && bomb.GlobalPosition.X < GlobalPosition.X)
				{
					return;
				}

				Vector2 norm = (bomb.GlobalPosition - GlobalPosition) / CollisionRect.Height;

				// Make the normal flatter by adding extra 1 to the X direction,
				// also make it more curved near the edges by squaring the Y.
				norm.X += -2.5f * Utility.Sign(bomb.Velocity.X);
				norm.Y = MathHelper.Clamp(norm.Y, -1, 1);
				norm.Y *= Utility.Sign(norm.Y) * norm.Y;
				norm.Normalize();

				bomb.Velocity = Vector2.Reflect(bomb.Velocity, norm);
				deflectSFX.Play();
			}
		}
	}
}

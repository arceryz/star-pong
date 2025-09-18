using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
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
			if (other is Bomb bomb)
			{
				bomb.Velocity.X *= -1;
				deflectSFX.Play();
			}
		}
	}
}

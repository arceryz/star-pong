using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
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

		public Shield(Team team)
		{
			this.Team = team;
			Texture2D tex;
			if (Team == Team.Blue)
			{
				tex = Engine.Load<Texture2D>(AssetPaths.Texture.Blue_Shield);
			}
			else
			{
				tex = Engine.Load<Texture2D>(AssetPaths.Texture.Red_Shield);
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
		}

		public void Activate()
		{
			CollisionEnabled = true;
			Visible = true;
			sprite.Play("activate");
		}

		public void Deactivate()
		{
			CollisionEnabled = false;
			sprite.Play("deactivate");
		}

		public void OnAnimationFinished()
		{
			if (sprite.CurrentAnimation == "activate") sprite.Play("running");
			if (sprite.CurrentAnimation == "deactivate") Visible = false;
		}

		public override void OnCollision(Vector2 pos, Vector2 normal, CollisionObject other)
		{
			if (other is IDamageable dmg && dmg.Team != Team && other is Bullet)
			{
				BulletHit?.Invoke();
			}
		}
	}
}

using System;
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
		Texture2D texture;

		public Shield(Team team)
		{
			this.Team = team;
			if (Team == Team.Blue)
			{
				texture = Engine.Load<Texture2D>(AssetPaths.Texture.Blue_Shield);
			}
			else
			{
				Flip = true;
				texture = Engine.Load<Texture2D>(AssetPaths.Texture.Red_Shield);
			}

			CollisionRect = new Rect2(texture.Bounds).Centered();
			CollisionEnabled = false;
		}

		public override void Draw(SpriteBatch batch)
		{
			if (CollisionEnabled)
			{
				DrawTexture(batch, texture, GlobalPosition, Color.White, Flip);
			}
		}

		public void Activate()
		{
			CollisionEnabled = true;
		}

		public void Deactivate()
		{
			CollisionEnabled = false;
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

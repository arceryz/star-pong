using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using StarPong.Framework;

namespace StarPong.Game
{
	public class Bullet: CollisionObject, IDamageable
	{
		const float speed = 500.0f;

		public Team Team { get; private set; }
		public int Health { get; private set; } = 1;
		Texture2D texture;

		public Bullet(Team team, Vector2 dir)
		{
			this.Team = team;
			if (team == Team.Blue)
			{
				texture = Engine.Load<Texture2D>(AssetPaths.Texture.Blue_Bullet);
			}
			else
			{
				Flip = true;
				texture = Engine.Load<Texture2D>(AssetPaths.Texture.Red_Bullet);
			}

			Velocity = dir * speed;
			CollisionRect = new Rect2(texture.Bounds).Centered();
			OverridePosition = true;
		}

		public override void Draw(SpriteBatch batch)
		{
			DrawTexture(batch, texture, GlobalPosition, Color.White, Flip);
		}

		public override void OnCollision(Vector2 pos, Vector2 normal, CollisionObject other)
		{
			if (other is IDamageable dmgable && dmgable.Team != Team &&
				!(other is Bomb))
			{
				dmgable.TakeDamage(1, GlobalPosition);
				QueueFree();
			}
		}
	}
}

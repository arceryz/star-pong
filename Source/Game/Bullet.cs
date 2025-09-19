using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using StarPong.Framework;
using StarPong.Scenes;

namespace StarPong.Game
{
	public class Bullet: CollisionObject, IDamageable
	{
		const float speed = 500.0f;

		public Team Team { get; private set; }
		public int Health { get; private set; } = 1;
		Texture2D texture;
		SoundEffect fireSFX;
		SoundEffect impactSFX;

		public Bullet(Team team, Vector2 dir)
		{
			this.Team = team;
			if (team == Team.Blue)
			{
				texture = Engine.Load<Texture2D>(Assets.Textures.Blue_Bullet);
			}
			else
			{
				Flip = true;
				texture = Engine.Load<Texture2D>(Assets.Textures.Red_Bullet);
			}

			Velocity = dir * speed;
			CollisionRect = new Rect2(texture.Bounds).Centered().Scaled(2.0f, 2.0f);
			OverridePosition = true;

			fireSFX = Engine.Load<SoundEffect>(Assets.Sounds.Bullet_Fired);
			fireSFX.Play();
			impactSFX = Engine.Load<SoundEffect>(Assets.Sounds.Bullet_Impact);
		}

		public override void Draw(SpriteBatch batch)
		{
			DrawTexture(batch, texture, GlobalPosition, Color.White, Flip);
		}

		public override void OnCollision(Vector2 pos, Vector2 normal, CollisionObject other)
		{
			// When a bullet collides we want the target object to take 1 damage and
			// to play a sound effect and a screen shake.
			// We can decide here which objects can collide with this bullet.
			if (other is IDamageable dmgable && dmgable.Team != Team &&
				!(other is Bomb))
			{
				int dmg = 1;
				if (other is Mothership) dmg = 3;
				if (PlayingScene.IsGameFinished) dmg = 0;

				Debug.WriteLine($"Dealing {dmg} to {other}");

				dmgable.TakeDamage(dmg, GlobalPosition);
				impactSFX.Play();

				ExplosionFX explosion = new ExplosionFX(ExplosionType.Small);
				explosion.OverridePosition = true;
				explosion.Position = GlobalPosition;
				Parent.AddChild(explosion);

				QueueFree();
			}
		}
	}
}

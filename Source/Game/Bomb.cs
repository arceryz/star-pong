using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using StarPong.Framework;

namespace StarPong.Game
{
	public class Bomb: CollisionObject, IDamageable
	{
		const float spawnSpread = 45.0f;
		const float speed = 200.0f;

		public Team Team { get; private set; } = Team.Neutral;
		public int Health { get; private set; } = 999;
		Sprite sprite;

		public Bomb()
		{
			Texture2D bombSheet = Engine.Load<Texture2D>(Assets.Textures.Bomb);
			sprite = new Sprite(bombSheet, 4, 1);
			sprite.AddAnimation("default", 4, 0, 0, 4);
			sprite.Play("default");
			AddChild(sprite);

			CollisionRect = sprite.FrameSize.Centered().Scaled(0.5f, 0.5f);

			// Set the position to the center of the screen.
			Position = new Vector2(Engine.GameWidth / 2.0f, Engine.GameHeight / 2.0f);

			// Set the velocity to a random angle between +- spread and random direction.
			float spread = MathHelper.ToRadians(spawnSpread);
			float rotateAngle = Utility.RandRange(-spread, spread);

			Velocity = new Vector2(speed, 0.0f);
			Velocity.Rotate(rotateAngle);
			Velocity *= Utility.RandBool() ? -1.0f : 1.0f;
		}

		public override void Update(float delta)
		{
			base.Update(delta);

			// Handle reflection and reset when out of bounds on either side.
			Rect2 bounding = GetBoundingRect();

			if (bounding.Y < 0) // Upper wall
			{
				Position.Y = bounding.Height / 2;
				Velocity.Y *= -1;
			}
			if (bounding.Y + bounding.Height > Engine.GameHeight)
			{
				Velocity.Y *= -1;
				Position.Y = Engine.GameHeight - bounding.Height / 2;
			}
		}

		public void Explode()
		{
			ExplosionFX explosion = new ExplosionFX(ExplosionType.Big);
			explosion.DrawLayer = 3;
			Parent.AddChild(explosion);
			explosion.Position = Position;
			Engine.AddCameraShake(200);
			QueueFree();
		}

		public override void OnCollision(Vector2 pos, Vector2 normal, CollisionObject other)
		{
			if (other is Shield)
			{
				Velocity.X *= -1;
			}
			if (other is Mothership mother)
			{
				mother.TakeDamage(100, Position);
				Explode();
			}
		}
	}
}

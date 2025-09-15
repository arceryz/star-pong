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

		public Team Team { get; set; } = Team.Neutral;
		public int Health { get; set; } = 999;
		Texture2D texture;

		public Bomb()
		{
			texture = Engine.Load<Texture2D>(AssetPaths.Texture.Bomb);
			CollisionRect = new Rect2(texture.Bounds);

			// Set the position to the center of the screen.
			Position = new Vector2(Engine.ScreenWidth / 2.0f, Engine.ScreenHeight / 2.0f);

			// Set the velocity to a random angle between +- spread and random direction.
			float spread = MathHelper.ToRadians(spawnSpread);
			float rotateAngle = Utility.RandRange(-spread, spread);

			Velocity = new Vector2(1.0f, 0.0f);
			Velocity.Rotate(rotateAngle);
			Velocity *= Utility.RandBool() ? -1.0f : 1.0f;
		}

		public override void Update(float delta)
		{
			base.Update(delta);

			// Handle reflection and reset when out of bounds on either side.
			if (Position.Y < 0) // Upper wall
			{
				Position.Y = 0;
				Velocity.Y *= -1;
			}
			if (Position.Y > Engine.ScreenHeight - texture.Height)
			{
				Velocity.Y *= -1;
				Position.Y = Engine.ScreenHeight - texture.Height;
			}
		}

		public override void Draw(SpriteBatch batch)
		{
			DrawTexture(batch, texture, GlobalPosition, Color.White);
		}

		public void Explode()
		{
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

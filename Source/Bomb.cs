using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using StarPong.Source.Framework;

namespace StarPong.Source
{
	public class Bomb: CollisionObject
	{
		static Texture2D ballTex;

		public static void LoadContent(ContentManager content)
		{
			ballTex = content.Load<Texture2D>("bal");
		}

		const float initialVelocitySpread = 45.0f;
		const float ballSpeed = 200.0f;

		public Bomb()
		{
			Reset();
			CollisionRect = new Rect2(ballTex.Bounds);
		}

		public override void Update(float delta)
		{
			Position += ballSpeed * Velocity * delta;

			// Handle reflection and reset when out of bounds on either side.
			if (Position.Y < 0) // Upper wall
			{
				Position.Y = 0;
				Velocity.Y *= -1;
			}
			if (Position.Y > Engine.Instance.ScreenHeight - ballTex.Height)
			{
				Velocity.Y *= -1;
				Position.Y = Engine.Instance.ScreenHeight - ballTex.Height;
			}
		}

		public override void Draw(SpriteBatch batch)
		{
			batch.Draw(ballTex, Position, Color.White);
		}

		public void Reset()
		{
			// Set the position to the center of the screen.
			Position = new Vector2(Engine.Instance.ScreenWidth / 2.0f, Engine.Instance.ScreenHeight / 2.0f);

			// Set the velocity to a random angle between +- spread and random direction.
			float spread = MathHelper.ToRadians(initialVelocitySpread);
			float rotateAngle = Utility.RandRange(-spread, spread);

			Velocity = new Vector2(1.0f, 0.0f);
			Velocity.Rotate(rotateAngle);
			Velocity *= Utility.RandBool() ? -1.0f : 1.0f;
		}

		public override void OnCollision(Vector2 pos, Vector2 normal, CollisionObject other)
		{
			if (other is Shield)
			{
				Velocity.X *= -1;
			}
			if (other is Mothership)
			{
				Reset();
			}
		}
	}
}

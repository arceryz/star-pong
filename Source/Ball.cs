using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Pong.Source
{
	public class Ball: ICollideable
	{
		public delegate void PaddleGoalHit(bool side);
		static Texture2D ballTexture;

		// Parameters.
		const float initialVelocitySpread = 45.0f;
		const float ballSpeed = 200.0f;

		public static void LoadContent(ContentManager content)
		{
			ballTexture = content.Load<Texture2D>("bal");
		}

		Vector2 position;
		Vector2 velocity;

		public Ball()
		{
			Reset();
		}

		public void Update(float delta)
		{
			position += ballSpeed * velocity * delta;

			// Handle reflection and reset when out of bounds on either side.
			if (position.Y < 0) // Upper wall
			{
				position.Y = 0;
				velocity.Y *= -1;
			}
			if (position.Y > Pong.ScreenHeight - ballTexture.Height)
			{
				velocity.Y *= -1;
				position.Y = Pong.ScreenHeight - ballTexture.Height;
			}
		}

		public void Draw(SpriteBatch batch)
		{
			batch.Draw(ballTexture, position, Color.White);
		}

		public void Reset()
		{
			// Set the position to the center of the screen.
			position = new Vector2(Pong.ScreenWidth / 2.0f, Pong.ScreenHeight / 2.0f);

			// Set the velocity to a random angle between +- spread and random direction.
			float spread = MathHelper.ToRadians(initialVelocitySpread);
			float rotateAngle = Utility.RandRange(-spread, spread);

			velocity = new Vector2(1.0f, 0.0f);
			velocity.Rotate(rotateAngle);
			velocity *= Utility.RandBool() ? -1.0f : 1.0f;
		}

		// Collideable interface.
		public Rect2 GetCollisionRect() => new Rect2(ballTexture.Bounds);
		public void OnCollision(Vector2 pos, Vector2 normal, ICollideable other)
		{
			velocity.X *= -1;
		}

		public Vector2 GetPosition() => position;
	}
}

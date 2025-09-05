using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Pong.Source.GameStates
{
	public class PlayingState: GameState
	{
		// Game elements
		Paddle paddle1;
		Paddle paddle2;
		Ball ball;

		public PlayingState()
		{
			paddle1 = new Paddle(Paddle.Side.Left);
			paddle2 = new Paddle(Paddle.Side.Right);
			ball = new Ball();
		}

		public override void Initialize()
		{
			ball.Reset();
		}

		public override void Update(float delta)
		{
			paddle1.Update(delta);
			paddle2.Update(delta);
			ball.Update(delta);

			// Detect collision between ball and paddle, and then notify the ball.
			// Can be optimised using quad tree but there is practically no gains here.
			// Our loop evaluates each pair only once.
			ICollideable[] collisionObjects = [paddle1, paddle2, ball];
			for (int i = 0; i < collisionObjects.Length; i++)
			{
				for (int j = i + 1; j < collisionObjects.Length; j++)
				{
					ICollideable coll1 = collisionObjects[i];
					ICollideable coll2 = collisionObjects[j];
					if (coll1.IsColliding(coll2))
					{
						coll1.OnCollision(Vector2.Zero, Vector2.Zero, coll2);
						coll2.OnCollision(Vector2.Zero, Vector2.Zero, coll1);
					}
				}
			}
		}

		public override void Draw(SpriteBatch batch)
		{
			// Map elements.
			batch.Draw(Pong.MapLineTexture, 
				new Vector2(Pong.ScreenWidth / 2.0f - Pong.MapLineTexture.Width * 0.5f, 0), Color.White);

			// Game elements.
			paddle1.Draw(batch);
			paddle2.Draw(batch);
			ball.Draw(batch);
		}
	}
}

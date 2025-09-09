using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using StarPong.Source.Framework;

namespace StarPong.Source.GameStates
{
	public class PlayingState: GameState
	{
		// Game elements
		List<CollisionObject> collideObjects;
		Ball ball;

		public override void Initialize()
		{
			collideObjects = new();
			collideObjects.Add(new Paddle(Paddle.Side.Left));
			collideObjects.Add(new Paddle(Paddle.Side.Right));

			ball = new Ball();
			ball.Reset();
			collideObjects.Add(ball);
		}

		public override void Update(float delta)
		{
			foreach (GameObject obj in collideObjects) obj.Update(delta);
			HandleCollisions(collideObjects);
		}

		public override void Draw(SpriteBatch batch)
		{
			// Map elements.
			batch.Draw(Engine.MapLineTexture, 
				new Vector2(Engine.Instance.ScreenWidth / 2.0f - Engine.MapLineTexture.Width * 0.5f, 0), Color.White);

			// Game elements.
			foreach (GameObject obj in collideObjects) obj.Draw(batch);
		}
	}
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
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
		List<GameObject> uiObjects;
		Ball ball;
		Label scoreLabel;

		// Score
		Rect2 leftGoal;
		Rect2 rightGoal;
		int scoreLeft;
		int scoreRight;

		public override void Initialize()
		{
			uiObjects = new();
			scoreLabel = new Label("", Color.Black, Engine.Instance.GetAnchor(0, -1, 0, 32));
			SetScoreText();
			uiObjects.Add(scoreLabel);

			collideObjects = new();
			collideObjects.Add(new Paddle(Paddle.Side.Left));
			collideObjects.Add(new Paddle(Paddle.Side.Right));

			ball = new Ball();
			ball.Reset();
			collideObjects.Add(ball);

			scoreLeft = 0;
			scoreRight = 0;

			int goalWidth = 300;
			leftGoal = new Rect2(-goalWidth, 0, goalWidth, Engine.Instance.ScreenHeight);
			rightGoal = new Rect2(Engine.Instance.ScreenWidth, 0, goalWidth, Engine.Instance.ScreenHeight);
		}

		public override void Update(float delta)
		{
			foreach (GameObject obj in collideObjects) obj.Update(delta);
			foreach (GameObject obj in uiObjects) obj.Update(delta);
			HandleCollisions(collideObjects);

			if (ball.IsOverlapping(leftGoal)) ScorePoint(Paddle.Side.Right);
			if (ball.IsOverlapping(rightGoal)) ScorePoint(Paddle.Side.Left);
		}

		public override void Draw(SpriteBatch batch)
		{
			// Map elements.
			batch.Draw(Engine.MapLineTexture, 
				new Vector2(Engine.Instance.ScreenWidth / 2.0f - Engine.MapLineTexture.Width * 0.5f, 0), Color.White);

			// Game elements.
			foreach (GameObject obj in collideObjects) obj.Draw(batch);
			foreach (GameObject obj in uiObjects) obj.Draw(batch);
		}

		public void ScorePoint(Paddle.Side side)
		{
			if (side == Paddle.Side.Right) scoreRight++;
			else if (side == Paddle.Side.Left) scoreLeft++;
			ball.Reset();
			SetScoreText();
		}

		public void SetScoreText()
		{
			scoreLabel.Text = $"{scoreLeft} - {scoreRight}";
		}
	}
}

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
		Rect2 blueGoal;
		Rect2 redGoal;
		int scoreBlue;
		int scoreRed;

		public override void Initialize()
		{
			uiObjects = new();
			scoreLabel = new Label("", Color.Black, Engine.Instance.GetAnchor(0, -1, 0, 32));
			SetScoreText();
			uiObjects.Add(scoreLabel);

			collideObjects = new();
			collideObjects.Add(new Mothership(Team.Blue));
			collideObjects.Add(new Mothership(Team.Red));
			collideObjects.Add(new Player(Team.Blue));
			collideObjects.Add(new Player(Team.Red));

			ball = new Ball();
			ball.Reset();
			collideObjects.Add(ball);

			scoreBlue = 0;
			scoreRed = 0;

			int goalWidth = 300;
			blueGoal = new Rect2(-goalWidth, 0, goalWidth, Engine.Instance.ScreenHeight);
			redGoal = new Rect2(Engine.Instance.ScreenWidth, 0, goalWidth, Engine.Instance.ScreenHeight);
		}

		public override void Update(float delta)
		{
			foreach (GameObject obj in collideObjects) obj.Update(delta);
			foreach (GameObject obj in uiObjects) obj.Update(delta);
			HandleCollisions(collideObjects);

			if (ball.IsOverlapping(blueGoal)) ScorePoint(Team.Red);
			if (ball.IsOverlapping(redGoal)) ScorePoint(Team.Blue);
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

		public void ScorePoint(Team side)
		{
			if (side == Team.Blue) scoreBlue++;
			else if (side == Team.Red) scoreRed++;
			ball.Reset();
			SetScoreText();
		}

		public void SetScoreText()
		{
			scoreLabel.Text = $"{scoreBlue}  {scoreRed}";
		}
	}
}

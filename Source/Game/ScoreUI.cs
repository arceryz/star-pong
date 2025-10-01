using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StarPong.Framework;
using StarPong.Scenes;

namespace StarPong.Game
{
	/// <summary>
	/// This class draws the health points of the mothership as a score indicator.
	/// It also displays the game running time.
	/// </summary>
	public class ScoreUI: GameObject
	{
		Mothership mother1;
		Mothership mother2;

		Label stageLabel;
		Label healthLabel;
		Label timeLabel;

		public ScoreUI(Mothership mother1, Mothership mother2)
		{
			this.mother1 = mother1;
			this.mother2 = mother2;
			Position = Engine.GetAnchor(0, -1, 0, 45);

			stageLabel = new Label(Engine.Load<ImageFont>(Assets.Fonts.Gyruss_Gold), "", 4);
			healthLabel = new Label(Engine.Load<ImageFont>(Assets.Fonts.Gyruss_Grey), "", 2);
			healthLabel.Position = new Vector2(0, 30);

			timeLabel = new Label(Engine.Load<ImageFont>(Assets.Fonts.Gyruss_Bronze), "", 4);
			timeLabel.OverridePosition = true;
			timeLabel.Position = Engine.GetAnchor(0, 1, 0, -30);

			AddChild(stageLabel);
			//AddChild(healthLabel);
			AddChild(timeLabel);
		}

		public override void Draw(SpriteBatch batch)
		{
			if (PlayingScene.IsGameFinished)
			{
				stageLabel.Visible = false;
				healthLabel.Visible = false;
			}
			stageLabel.Text = $"{(int)mother1.GetTotalHealth()} - {(int)mother2.GetTotalHealth()}";
			healthLabel.Text = $"{mother1.Health} - {mother2.Health}";
			timeLabel.Text = $"{(int)PlayingScene.GameRunningTime}";
		} 
	}
}

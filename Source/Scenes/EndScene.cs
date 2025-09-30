using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using StarPong.Framework;
using StarPong.Game;

namespace StarPong.Scenes
{
	public class EndScene: GameObject
	{
		public EndScene() { }

		public override void EnterTree()
		{
			//***********************************************//
			// Assets
			//***********************************************//
			Texture2D stars = Engine.Load<Texture2D>(Assets.Textures.BG_Stars);
			ImageFont gyrussGold = Engine.Load<ImageFont>(Assets.Fonts.Gyruss_Gold);
			ImageFont gyrussGrey = Engine.Load<ImageFont>(Assets.Fonts.Gyruss_Grey);


			//***********************************************//
			// Objects
			//***********************************************//
			Team winner = PlayingScene.LastDamagedTeam == Team.Blue ? Team.Red : Team.Blue;
			Label titleLabel = new Label(gyrussGold, $"{winner.ToString().ToLower()} wins", 6);
			titleLabel.Position = Engine.GetAnchor(0, -0.5f, 0, 0);

			Label scoreLabel = new Label(gyrussGold, $"{PlayingScene.FinalScore1} - {PlayingScene.FinalScore2}", 4);
			scoreLabel.Position = titleLabel.Position + new Vector2(0, 75);

			Button playButton = new Button(gyrussGrey, "play again", 2);
			playButton.Pressed += () => Engine.ChangeScene(SceneName.PlayingScene);
			playButton.Position = Engine.GetAnchor(0, 0);

			Button returnButton = new Button(gyrussGrey, "return", 2);
			returnButton.Pressed += () => Engine.ChangeScene(SceneName.MenuScene);
			returnButton.Position = Engine.GetAnchor(0, 0, 0, 100);

			playButton.GrabFocus();
			playButton.FocusDown = returnButton;
			returnButton.FocusUp = playButton;

			ParallaxLayer bg = new ParallaxLayer(stars, 100.0f);

			MediaPlayer.Play(Engine.Load<Song>(Assets.Songs.Menu2));
			MediaPlayer.Volume = 0.5f;
			MediaPlayer.IsRepeating = true;


			//***********************************************//
			// Hierarchy
			//***********************************************//
			AddChild(bg);
			AddChild(titleLabel);
			AddChild(scoreLabel);
			AddChild(playButton);
			AddChild(returnButton);
		}
	}
}

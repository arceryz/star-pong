using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using StarPong.Framework;

namespace StarPong.Scenes
{
	public class MenuScene: GameObject
	{
		float timer = 0;
		Label authorLabel;

		public MenuScene() { }
		public override void EnterTree()
		{
			//***********************************************//
			// Assets
			//***********************************************//
			Texture2D stars = Engine.Load<Texture2D>(Assets.Textures.BG_Stars);
			Texture2D planets = Engine.Load<Texture2D>(Assets.Textures.BG_Planets);
			ImageFont gyrussGold = Engine.Load<ImageFont>(Assets.Fonts.Gyruss_Gold);
			ImageFont gyrussGrey = Engine.Load<ImageFont>(Assets.Fonts.Gyruss_Grey);


			//***********************************************//
			// Objects
			//***********************************************//
			Label titleLabel = new Label(gyrussGold, "star-pong", 6);
			titleLabel.Position = Engine.GetAnchor(0, -0.3f, 0, 0);

			Button playButton = new Button(gyrussGrey, "click here to play", 2)
				.SetFlicker(0.5f);
			playButton.Position = Engine.GetAnchor(0, 0);
			playButton.Pressed += () => Engine.ChangeScene(SceneName.SettingsScene);

			ParallaxLayer bg1 = new ParallaxLayer(stars, 50.0f);
			ParallaxLayer bg2 = new ParallaxLayer(planets, 150.0f);
			bg2.Color = Color.Gray;

			authorLabel = new Label(null, "Created by Timothy van der Valk", 1, Engine.DebugFont);
			authorLabel.Position = Engine.GetAnchor(0, 1, 0, -80);

			Label infoLabel = new Label(null, "(Z) Toggle debug shapes | (T) Toggle tree view | (Esc) Quit | (F) Fullscreen | (Shift-R) Main menu", 1, Engine.DebugFont);
			infoLabel.Position = authorLabel.Position + new Vector2(0, 30);

			MediaPlayer.Play(Engine.Load<Song>(Assets.Songs.Menu1));


			//***********************************************//
			// Hierarchy
			//***********************************************//
			AddChild(bg1);
			AddChild(bg2);
			AddChild(titleLabel);
			AddChild(playButton);
			AddChild(authorLabel);
			AddChild(infoLabel);
		}

		public override void Update(float delta)
		{
			timer += delta;
			if (timer > 0.1f)
			{
				timer = 0;
				Color[] colors = { Color.Red, Color.Orange, Color.Magenta, Color.LightBlue, Color.Cyan, Color.White, Color.Green, Color.Lime, Color.Yellow };
				authorLabel.Color = colors[Utility.RandInt32() % colors.Length];
			}

			if (Input.IsSequencePressed("toggle_secret"))
			{
				Engine.ChangeScene(SceneName.SecretScene);
			}
		}
	}
}

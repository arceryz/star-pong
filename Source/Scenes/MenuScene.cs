﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using StarPong.Framework;

namespace StarPong.Scenes
{
	public class MenuScene: GameObject
	{
		public MenuScene() { }
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
			Label titleLabel = new Label(gyrussGold, "star-pong", 6);
			titleLabel.Position = Engine.GetAnchor(0, -0.3f, 0, 0);

			Button playButton = new Button(gyrussGrey, "click here to play", 2)
				.SetFlicker(0.5f);
			playButton.Position = Engine.GetAnchor(0, 0);
			playButton.Pressed += () => Engine.ChangeScene(SceneName.PlayingScene);

			ParallaxLayer bg = new ParallaxLayer(stars, 100.0f);

			MediaPlayer.Play(Engine.Load<Song>(Assets.Songs.Menu1));
			MediaPlayer.Volume = 0.5f;
			MediaPlayer.IsRepeating = true;

			//***********************************************//
			// Hierarchy
			//***********************************************//
			AddChild(bg);
			AddChild(titleLabel);
			AddChild(playButton);
		}

		public override void Update(float delta)
		{
			if (Input.IsActionPressed("toggle_secret"))
			{
				Engine.ChangeScene(SceneName.SecretScene);
			}
		}
	}
}

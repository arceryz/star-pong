using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using StarPong.Framework;
using StarPong.Game;

namespace StarPong.Scenes
{
	public class MenuScene: GameObject
	{
		public MenuScene()
		{
			//***********************************************//
			// Assets
			//***********************************************//
			Texture2D selTex = Engine.Load<Texture2D>(Assets.Textures.UI_SelectionArrows);
			Texture2D stars = Engine.Load<Texture2D>(Assets.Textures.BG_Stars);
			ImageFont gyrussGold = Engine.Load<ImageFont>(Assets.Fonts.Gyruss_Gold);
			ImageFont gyrussGrey = Engine.Load<ImageFont>(Assets.Fonts.Gyruss_Grey);


			//***********************************************//
			// Objects
			//***********************************************//
			Label titleLabel = new Label(gyrussGold, "star-pong", 6);
			titleLabel.Position = Engine.GetAnchor(0, -0.3f, 0, 0);

			Button playButton = new Button(gyrussGrey, "click here to play", selTex, 2)
				.SetFlicker(0.5f);
			playButton.Position = Engine.GetAnchor(0, 0);
			playButton.Pressed += _OnPlayPressed;

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

		public void _OnPlayPressed()
		{
			Engine.ChangeScene(SceneName.PlayingScene);
		}
	}
}

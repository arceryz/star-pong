using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StarPong.Framework;
using StarPong.Game;

namespace StarPong.Scenes
{
	public class MenuScene: GameObject
	{
		public MenuScene()
		{
			// Assets.
			ImageFont gyrussGold = new ImageFont(AssetPaths.Font.Gyruss_Gold, "abcdefghijklmnopqrstuvwxyz0123456789-");
			ImageFont gyrussGrey = new ImageFont(AssetPaths.Font.Gyruss_Grey, "abcdefghijklmnopqrstuvwxyz0123456789-");
			Texture2D selTex = Engine.Load<Texture2D>(AssetPaths.Texture.UI_SelectionArrows);
			Texture2D stars = Engine.Load<Texture2D>(AssetPaths.Texture.BG_Stars);

			// UI belongs to a higher layer than the background.
			GameObject ui = new GameObject();
			ui.DrawLayer = 1;

			Label titleLabel = new Label(gyrussGold, "star-pong", 6);
			titleLabel.Position = Engine.GetAnchor(0, -0.3f, 0, 0);

			Button playButton = new Button(gyrussGrey, "click here to play", selTex, 2)
				.SetFlicker(0.5f);
			playButton.Position = Engine.GetAnchor(0, 0);
			playButton.Pressed += _OnPlayPressed;

			// Background (layer=0).
			ParallaxLayer bg = new ParallaxLayer(stars, 100.0f);

			// Construct scene hierarchy.
			AddChild(ui);
				ui.AddChild(titleLabel);
				ui.AddChild(playButton);
			AddChild(bg);
		}

		public void _OnPlayPressed()
		{
			Engine.ChangeScene(SceneName.PlayingScene);
		}
	}
}

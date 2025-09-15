using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StarPong.Framework;

namespace StarPong.Scenes
{
	public class MenuScene: GameObject
	{

		public MenuScene()
		{
			// Assets.
			SpriteFont pixelFont = Engine.Load<SpriteFont>(AssetPaths.Font.UI_Pixel);
			Texture2D selTex = Engine.Load<Texture2D>(AssetPaths.Texture.UI_ButtonSelection);
			Texture2D stars = Engine.Load<Texture2D>(AssetPaths.Texture.BG_Stars);

			// UI belongs to a higher layer than the background.
			GameObject ui = new GameObject();
			ui.DrawLayer = 1;
			AddChild(ui);

			Label titleLabel = new Label("STAR-Pong", pixelFont);
			titleLabel.Color = Color.Blue;
			titleLabel.Position = Engine.GetAnchor(0, -0.2f, 0, 0);
			ui.AddChild(titleLabel);

			Button playButton = new Button("Play", pixelFont, selTex);
			playButton.Position = Engine.GetAnchor(0, 0);
			playButton.Pressed += _OnPlayPressed;
			ui.AddChild(playButton);

			// Background (layer=0).
			ParallaxLayer bg = new ParallaxLayer(stars, 100.0f);
			AddChild(bg);
		}

		public void _OnPlayPressed()
		{
			Engine.ChangeScene(SceneName.PlayingScene);
		}
	}
}

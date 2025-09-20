using Microsoft.Xna.Framework.Graphics;
using StarPong.Framework;

namespace StarPong.Scenes
{
	public class SecretScene: GameObject
	{
		public SecretScene() { }

		public override void EnterTree()
		{
			//***********************************************//
			// Assets
			//***********************************************//
			Texture2D stars = Engine.Load<Texture2D>(Assets.Textures.BG_Stars);
			Texture2D cat = Engine.Load<Texture2D>(Assets.Textures.Cat);
			Texture2D selTex = Engine.Load<Texture2D>(Assets.Textures.UI_SelectionArrows);

			//***********************************************//
			// Objects
			//***********************************************//
			ParallaxLayer bg = new ParallaxLayer(stars, 100.0f);
			Sprite catSprite = new Sprite(cat, 1, 1);
			catSprite.Position = Engine.GetAnchor(0, 0, 0, -50);
			catSprite.Scale = 0.25f;

			Button returnButton = new Button(Engine.Load<ImageFont>(Assets.Fonts.Gyruss_Grey), "love u", selTex, 4);
			returnButton.Position = Engine.GetAnchor(0, 0, 0, 200);
			returnButton.Pressed += () => Engine.ChangeScene(SceneName.MenuScene);

			//***********************************************//
			// Hierarchy
			//***********************************************//
			AddChild(bg);
			AddChild(catSprite);
			AddChild(returnButton);
		}
	}
}

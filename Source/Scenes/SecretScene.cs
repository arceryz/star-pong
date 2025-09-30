using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
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


			//***********************************************//
			// Objects
			//***********************************************//
			ParallaxLayer bg = new ParallaxLayer(stars, 100.0f);
			Sprite catSprite = new Sprite(cat, 1, 1);
			catSprite.Position = Engine.GetAnchor(0, 0, 0, -50);
			catSprite.Scale = 0.25f;

			Label label = new Label(Engine.Load<ImageFont>(Assets.Fonts.Gyruss_Gold), "created 09-21-25", 4);
			label.Position = Engine.GetAnchor(0, 0, 0, 200);

			Button returnButton = new Button(Engine.Load<ImageFont>(Assets.Fonts.Gyruss_Grey), "love u xxx", 4);
			returnButton.Position = Engine.GetAnchor(0, 1, 0, -100);
			returnButton.GrabFocus();
			returnButton.Pressed += () => Engine.ChangeScene(SceneName.MenuScene);


			//***********************************************//
			// Hierarchy
			//***********************************************//
			AddChild(bg);
			AddChild(catSprite);
			AddChild(label);
			AddChild(returnButton);
		}
	}
}

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
	public class MenuScene: GameObject
	{

		public override void Initialize()
		{
			// Assets.
			SpriteFont buttonFont = Engine.Load<SpriteFont>(AssetPaths.Font.UI_Pixel);
			Texture2D buttonSel = Engine.Load<Texture2D>(AssetPaths.Texture.UI_ButtonSelection);

			// UI belongs to a higher layer than the background.
			GameObject uiLayer = new GameObject();
			uiLayer.DrawLayer = 1;
			AddChild(uiLayer);

			uiLayer.AddChild(new Label("STAR-Pong", Color.Gray, Engine.Instance.GetAnchor(0, -0.2f, 4, 4)));
			uiLayer.AddChild(new Label("STAR-Pong", Color.Blue, Engine.Instance.GetAnchor(0, -0.2f, 0, 0)));

			Button playButton = new Button("Play", buttonFont, buttonSel);
			playButton.Position = Engine.Instance.GetAnchor(0, 0);
			playButton.Pressed += _OnPlayPressed;
			uiLayer.AddChild(playButton);

			// Background (layer=0).
			AddChild(new ParallaxLayer(ParallaxLayer.Stars, 100.0f));
		}

		public void _OnPlayPressed()
		{
			
		}
	}
}

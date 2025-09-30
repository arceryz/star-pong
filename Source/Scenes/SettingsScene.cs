using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using StarPong.Framework;
using StarPong.Game;
using StarPong.Scenes;

namespace StarPong.Scenes
{
	public class SettingsScene: GameObject
	{
		public static bool BotEnabled = false;
		public static bool TurboModeEnabled = false;
		public static bool ShakeDisabled = false;

		Label teamNames;
		Label control1;
		Label control2;
		Label control3;
		Button botButton;
		Button turboButton;
		Button shakeButton;

		public SettingsScene() { }

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
			teamNames = new Label(gyrussGold, "", 4);
			control1 = new Label(gyrussGrey, "", 2);
			control2 = new Label(gyrussGrey, "", 2);
			control3 = new Label(gyrussGrey, "", 2);
			teamNames.Position = Engine.GetAnchor(0, 0, 0, -170);
			control1.Position = teamNames.Position + new Vector2(0, 60);
			control2.Position = control1.Position + new Vector2(0, 40);
			control3.Position = control1.Position + new Vector2(0, 80);

			botButton = new Button(gyrussGold, "", 2);
			botButton.Pressed += () => {
				BotEnabled = !BotEnabled;
				UpdateUI(); 
			};
			botButton.Position = control3.Position + new Vector2(0, 70);

			turboButton = new Button(gyrussGold, "", 2);
			turboButton.Pressed += () =>
			{
				TurboModeEnabled = !TurboModeEnabled;
				UpdateUI();
			};
			turboButton.Position = botButton.Position + new Vector2(0, 65);

			shakeButton = new Button(gyrussGold, "", 2);
			shakeButton.Position = turboButton.Position + new Vector2(0, 65);
			shakeButton.Pressed += () =>
			{
				ShakeDisabled = !ShakeDisabled;
				UpdateUI();
			};

			UpdateUI();

			Button playButton = new Button(gyrussGold, "play", 4);
			playButton.Position = Engine.GetAnchor(0, 1, 0, -100);
			playButton.Pressed += () => Engine.ChangeScene(SceneName.PlayingScene);
			
			// Set up focus.
			botButton.FocusDown = turboButton;
			turboButton.FocusUp = botButton;
			turboButton.FocusDown = shakeButton;
			shakeButton.FocusUp = turboButton;
			shakeButton.FocusDown = playButton;
			playButton.FocusUp = shakeButton;
			playButton.GrabFocus();

			ParallaxLayer bg = new ParallaxLayer(stars, 50.0f);

			//***********************************************//
			// Hierarchy
			//***********************************************//
			AddChild(bg);
			AddChild(teamNames);
			AddChild(control1);
			AddChild(control2);
			AddChild(control3);
			AddChild(botButton);
			AddChild(turboButton);
			AddChild(shakeButton);
			AddChild(playButton);
		}
		
		public void UpdateUI()
		{
			if (BotEnabled)
			{
				teamNames.Text = "           blue   red-bot  ";
				control1.Text = "ship movement        w-s              bot      ";
				control2.Text = "shoot                 c               bot      ";
				control3.Text = "toggle shield         v               bot      ";
				botButton.Text = " bot    enabled ";
			}
			else
			{
				teamNames.Text = "           blue     red    ";
				control1.Text = "ship movement        w-s         up-down arrows";
				control2.Text = "shoot                 c                o       ";
				control3.Text = "toggle shield         v                p       ";
				botButton.Text = " bot    disabled";
			}

			turboButton.Text = TurboModeEnabled ? "turbo   enabled " : "turbo   disabled";
			shakeButton.Text = !ShakeDisabled ?   "shake   enabled " : "shake   disabled";
		}
	}
}

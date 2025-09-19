using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using StarPong.Framework;
using StarPong.Game;
using System.Diagnostics;

namespace StarPong.Scenes
{
	public class PlayingScene: GameObject
	{
		public static float GameRunningTime = 0;
		public static bool IsCriticalPhase = false;

		public PlayingScene()
		{
			//***********************************************//
			// Assets
			//***********************************************//
			Texture2D stars = Engine.Load<Texture2D>(Assets.Textures.BG_Stars);
			Texture2D asteroids1 = Engine.Load<Texture2D>(Assets.Textures.BG_Asteroids_Close);
			Texture2D asteroids2 = Engine.Load<Texture2D>(Assets.Textures.BG_Asteroids_Mid);


			//***********************************************//
			// Objects
			//***********************************************//
			GameObject game = new GameObject();

			Mothership mother1 = new Mothership(Team.Blue);
			Mothership mother2 = new Mothership(Team.Red);
			mother1.HullStatusChanged += () => OnMotherHullStatusChanged(mother1);
			mother2.HullStatusChanged += () => OnMotherHullStatusChanged(mother2);

			Player player1 = new Player(Team.Blue);
			Player player2 = new Player(Team.Red);
		
			Bomb bomb = new Bomb();

			// Background
			ParallaxLayer bg1 = new ParallaxLayer(stars, 150.0f);
			ParallaxLayer bg2 = new ParallaxLayer(asteroids2, 250.0f);
			ParallaxLayer bg3 = new ParallaxLayer(asteroids1, 200.0f);
		
			// UI
			PlayerUI playerui_1 = new PlayerUI(player1);
			PlayerUI playerui_2 = new PlayerUI(player2);
			ScoreUI scoreui = new ScoreUI(mother1, mother2);

			MediaPlayer.Play(Engine.Load<Song>(Assets.Songs.Battle_Normal));
			MediaPlayer.Volume = 0.5f;
			MediaPlayer.IsRepeating = true;


			//***********************************************//
			// Hierarchy
			// The draw priority is given by the order in this list (farther > earlier) as well as the
			// individual draw layer that can be set for each object. Currently, all these objects have the same layer=0.
			//***********************************************//
			AddChild(bg1);
			AddChild(bg2);
			AddChild(bg3);

			AddChild(game);
				game.AddChild(mother1);
				game.AddChild(mother2);
				game.AddChild(player1);
				game.AddChild(player2);
				game.AddChild(bomb);
			
			AddChild(playerui_1);
			AddChild(playerui_2);
			AddChild(scoreui);
		}

		public override void Update(float delta)
		{
			GameRunningTime += delta;
		}

		void OnMotherHullStatusChanged(Mothership mother)
		{
			if (mother.HullStatus == Mothership.HullStatusEnum.Critical && !IsCriticalPhase)
			{
				MediaPlayer.Play(Engine.Load<Song>(Assets.Songs.Battle_Critical));
				IsCriticalPhase = true;
			}
		}
	}
}

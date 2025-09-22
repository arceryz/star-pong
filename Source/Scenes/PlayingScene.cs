using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using StarPong.Framework;
using StarPong.Game;

namespace StarPong.Scenes
{
	public class PlayingScene: GameObject
	{
		public static float GameRunningTime;
		public static bool IsCriticalPhase;
		public static bool IsGameFinished;
		public static int FinalScore1;
		public static int FinalScore2;
		public static Team LastDamagedTeam;

		Mothership mother1;
		Mothership mother2;

		public PlayingScene() 
		{
			GameRunningTime = 0;
			IsCriticalPhase = false;
			IsGameFinished = false;
			LastDamagedTeam = Team.Neutral;
		}

		public override void EnterTree()
		{
			//***********************************************//
			// Assets
			//***********************************************//
			Texture2D stars = Engine.Load<Texture2D>(Assets.Textures.BG_Stars);
			Texture2D asteroids = Engine.Load<Texture2D>(Assets.Textures.BG_Asteroids);
			Texture2D planets = Engine.Load<Texture2D>(Assets.Textures.BG_Planets);


			//***********************************************//
			// Objects
			//***********************************************//
			GameObject game = new GameObject();

			mother1 = new Mothership(Team.Blue);
			mother2 = new Mothership(Team.Red);
			mother1.HullStatusChanged += () => OnMotherHullStatusChanged(mother1);
			mother2.HullStatusChanged += () => OnMotherHullStatusChanged(mother2);

			Player player1 = new Player(Team.Blue);
			Player player2 = new Player(Team.Red);
		
			Bomb bomb = new Bomb();

			// Background
			float turboFactor = SettingsScene.TurboModeEnabled ? 2.0f : 1.0f;
			ParallaxLayer bg1 = new ParallaxLayer(stars, 50.0f * turboFactor);
			ParallaxLayer bg2 = new ParallaxLayer(planets, 150.0f * turboFactor);
			bg2.Color = Color.Gray;
			ParallaxLayer bg3 = new ParallaxLayer(asteroids, 300.0f * turboFactor);
		
			// UI
			PlayerUI playerui_1 = new PlayerUI(player1);
			PlayerUI playerui_2 = new PlayerUI(player2);
			ScoreUI scoreui = new ScoreUI(mother1, mother2);

			MediaPlayer.Play(Engine.Load<Song>(Assets.Songs.Battle_Normal));


			//***********************************************//
			// Hierarchy
			// The draw priority is given by the order in this list (farther > earlier) as well as the
			// individual draw layer that can be set for each object. Currently, all these objects have the same layer=0.
			//***********************************************//
			AddChild(bg1);
			AddChild(bg2);

			AddChild(game);
				game.AddChild(mother1);
				game.AddChild(mother2);
				game.AddChild(player1);
				game.AddChild(player2);
				game.AddChild(bomb);
			AddChild(bg3);

			AddChild(playerui_1);
			AddChild(playerui_2);
			AddChild(scoreui);
		}

		public override void Update(float delta)
		{
			if (!IsGameFinished) GameRunningTime += delta;
		}

		void OnMotherHullStatusChanged(Mothership mother)
		{
			if (IsGameFinished)
			{
				return;
			}

			LastDamagedTeam = mother.Team;
			if (mother.HullStatus == Mothership.HullStatusEnum.Critical && !IsCriticalPhase)
			{
				MediaPlayer.Stop();
				MediaPlayer.Play(Engine.Load<Song>(Assets.Songs.Battle_Critical));
				IsCriticalPhase = true;
			}

			// Finish the game.
			if (mother.HullStatus == Mothership.HullStatusEnum.Destroyed)
			{
				mother.Exploded += OnMotherExploded;
				IsGameFinished = true;
				FinalScore1 = mother1.GetTotalHealth();
				FinalScore2 = mother2.GetTotalHealth();
				MediaPlayer.Stop();
			}
		}

		void OnMotherExploded()
		{
			Engine.ChangeScene(SceneName.EndScene);
		}
	}
}

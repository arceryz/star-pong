using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using StarPong.Framework;
using StarPong.Game;
using System.Diagnostics;

namespace StarPong.Scenes
{
	public class PlayingScene: GameObject
	{
		Mothership mother1;
		Mothership mother2;

		Player player1;
		Player player2;

		PlayerInfoBar info1;
		PlayerInfoBar info2;

		public PlayingScene()
		{
			Texture2D stars = Engine.Load<Texture2D>(AssetPaths.Texture.BG_Stars);
			Texture2D asteroids1 = Engine.Load<Texture2D>(AssetPaths.Texture.BG_Asteroids_Close);
			Texture2D asteroids2 = Engine.Load<Texture2D>(AssetPaths.Texture.BG_Asteroids_Mid);

			// Game
			GameObject game = new GameObject();
			game.DrawLayer = 10;
			AddChild(game);

			mother1 = new Mothership(Team.Blue);
			mother2 = new Mothership(Team.Red);
			game.AddChild(mother1);
			game.AddChild(mother2);

			player1 = new Player(Team.Blue);
			player2 = new Player(Team.Red);
			player1.DrawLayer = 1;
			player2.DrawLayer = 1;
			game.AddChild(player1);
			game.AddChild(player2);

			Bomb bomb = new Bomb();
			bomb.DrawLayer = 20;
			game.AddChild(bomb);

			// Background
			GameObject bg = new GameObject();
			AddChild(bg);

			ParallaxLayer par1 = new ParallaxLayer(stars, 150.0f);
			ParallaxLayer par2 = new ParallaxLayer(asteroids2, 250.0f);
			ParallaxLayer par3 = new ParallaxLayer(asteroids1, 200.0f);
			par1.DrawLayer = 1;
			par2.DrawLayer = 2;
			par3.DrawLayer = 3;
			bg.AddChild(par1);
			bg.AddChild(par2);
			bg.AddChild(par3);

			// UI
			GameObject ui = new GameObject();
			ui.DrawLayer = 30;
			AddChild(ui);

			info1 = new PlayerInfoBar(player1);
			info2 = new PlayerInfoBar(player2);
			ui.AddChild(info1);
			ui.AddChild(info2);

			// Play music.
			MediaPlayer.Play(Engine.Load<Song>(AssetPaths.Song.Battle1_Normal));
			MediaPlayer.IsRepeating = true;
		}
	}
}

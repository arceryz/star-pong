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
			// Assets
			Texture2D stars = Engine.Load<Texture2D>(AssetPaths.Texture.BG_Stars);
			Texture2D asteroids1 = Engine.Load<Texture2D>(AssetPaths.Texture.BG_Asteroids_Close);
			Texture2D asteroids2 = Engine.Load<Texture2D>(AssetPaths.Texture.BG_Asteroids_Mid);

			// Game
			GameObject game = new GameObject();

			mother1 = new Mothership(Team.Blue);
			mother2 = new Mothership(Team.Red);

			player1 = new Player(Team.Blue);
			player2 = new Player(Team.Red);
		
			Bomb bomb = new Bomb();

			// Background
			ParallaxLayer bg1 = new ParallaxLayer(stars, 150.0f);
			ParallaxLayer bg2 = new ParallaxLayer(asteroids2, 250.0f);
			ParallaxLayer bg3 = new ParallaxLayer(asteroids1, 200.0f);
		
			// UI
			info1 = new PlayerInfoBar(player1);
			info2 = new PlayerInfoBar(player2);

			// Play music.
			MediaPlayer.Play(Engine.Load<Song>(AssetPaths.Song.Battle1_Normal));
			MediaPlayer.IsRepeating = true;

			// Set up hierarchy.
			// The draw priority is given by the order in this list (farther > earlier) as well as the
			// individual draw layer that can be set for each object. Currently, all these objects have the same layer=0.
			AddChild(bg1);
			AddChild(bg2);
			AddChild(bg3);

			AddChild(game);
				game.AddChild(mother1);
				game.AddChild(mother2);
				game.AddChild(player1);
				game.AddChild(player2);
				game.AddChild(bomb);
			
			AddChild(info1);
			AddChild(info2);
		}
	}
}

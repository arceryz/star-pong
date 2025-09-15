using Microsoft.Xna.Framework.Graphics;
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

		public PlayingScene()
		{
			Texture2D stars = Engine.Load<Texture2D>(AssetPaths.Texture.BG_Stars);
			Texture2D asteroids1 = Engine.Load<Texture2D>(AssetPaths.Texture.BG_Asteroids_Close);
			Texture2D asteroids2 = Engine.Load<Texture2D>(AssetPaths.Texture.BG_Asteroids_Mid);

			// UI
			GameObject bg = new GameObject();
			AddChild(bg);

			bg.AddChild(new ParallaxLayer(stars, 150.0f));
			bg.AddChild(new ParallaxLayer(asteroids2, 250.0f));
			bg.AddChild(new ParallaxLayer(asteroids1, 200.0f));

			// Game
			GameObject game = new GameObject();
			game.DrawLayer = 1;
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
		}
	}
}

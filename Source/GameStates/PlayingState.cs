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
	public class PlayingState: GameState
	{
		GameObjectList shipLayer;
		GameObjectList uiLayer;
		GameObjectList bgLayer;
		GameObjectList bulletLayer;

		// This is the ball.
		Bomb bomb;
		Mothership blueMother;
		Mothership redMother;
		Player bluePlayer;
		Player redPlayer;

		public override void Initialize()
		{
			shipLayer = new();
			bulletLayer = new();
			uiLayer = new();
			bgLayer = new();

			// Background
			bgLayer.Add(new ParallaxLayer(ParallaxLayer.Stars, 100.0f));
			bgLayer.Add(new ParallaxLayer(ParallaxLayer.Asteroids_Mid, 150.0f));
			bgLayer.Add(new ParallaxLayer(ParallaxLayer.Asteroids_Close, 200.0f));

			// Game
			blueMother = new Mothership(Team.Blue);
			blueMother.Destroyed += () => OnMothershipDestroyed(blueMother);
			shipLayer.Add(blueMother);

			redMother = new Mothership(Team.Red);
			redMother.Destroyed += () => OnMothershipDestroyed(redMother);
			shipLayer.Add(redMother);

			bluePlayer = new Player(Team.Blue, bulletLayer, shipLayer);
			shipLayer.Add(bluePlayer);

			redPlayer = new Player(Team.Red, bulletLayer, shipLayer);
			shipLayer.Add(redPlayer);

			bomb = new Bomb();
			bomb.Reset();
			bulletLayer.Add(bomb);
		}

		public override void Update(float delta)
		{
			bgLayer.Update(delta);
			shipLayer.Update(delta);
			bulletLayer.Update(delta);
			shipLayer.CollideWith(bulletLayer);
			bulletLayer.CollideWith(bulletLayer);
		}

		public override void Draw(SpriteBatch batch)
		{
			bgLayer.Draw(batch);
			shipLayer.Draw(batch);
			bulletLayer.Draw(batch);
			uiLayer.Draw(batch);
		}

		public void OnMothershipDestroyed(Mothership ship)
		{
			Engine.Instance.ChangeState(Engine.GameStateEnum.EndState);
		}
	}
}

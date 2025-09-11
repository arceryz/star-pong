using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using StarPong.Source.Framework;

namespace StarPong.Source.GameStates
{
	public class EndState: GameState
	{
		GameObjectList uiLayer;

		public override void Initialize()
		{
			uiLayer = new();

			Button playButton = new Button("Play again", Color.White, Engine.Instance.GetAnchor(0, 0, 0, -50));
			playButton.Pressed += _OnPlayAgainPressed;
			uiLayer.Add(playButton);

			Button backButton = new Button("Back", Color.White, Engine.Instance.GetAnchor(0, 0, 0, 50));
			backButton.Pressed += _OnBackPressed;
			uiLayer.Add(backButton);
		}

		public override void Update(float delta)
		{
			uiLayer.Update(delta);
		}

		public override void Draw(SpriteBatch batch)
		{
			uiLayer.Draw(batch);
		}

		public void _OnBackPressed()
		{
			Engine.Instance.ChangeState(Engine.GameStateEnum.MenuState);
		}

		public void _OnPlayAgainPressed()
		{
			Engine.Instance.ChangeState(Engine.GameStateEnum.PlayingState);
		}
	}
}

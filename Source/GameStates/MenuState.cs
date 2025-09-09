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
	public class MenuState: GameState
	{
		List<GameObject> uiLayer = new();

		public override void Initialize()
		{
			uiLayer.Add(new Label("STAR-Pong", Color.Gray, Engine.Instance.GetAnchor(0, -0.2f, 4, 4)));
			uiLayer.Add(new Label("STAR-Pong", Color.Blue, Engine.Instance.GetAnchor(0, -0.2f, 0, 0)));

			Button playButton = new Button("Play", Color.Black, Engine.Instance.GetAnchor(0, 0));
			playButton.Pressed += _OnPlayPressed;
			uiLayer.Add(playButton);
		}

		public override void Update(float delta)
		{
			foreach (GameObject obj in uiLayer) obj.Update(delta);
		}

		public override void Draw(SpriteBatch batch)
		{
			foreach (GameObject obj in uiLayer) obj.Draw(batch);
		}

		public void _OnPlayPressed()
		{
			Engine.Instance.ChangeState(Engine.GameStateEnum.PlayingState);
		}
	}
}

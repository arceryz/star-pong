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
			uiLayer.Add(new Label("STAR-Pong", Color.Gray, new Vector2(Engine.Instance.ScreenWidth / 2.0f + 4, Engine.Instance.ScreenHeight / 3.0f + 4)));
			uiLayer.Add(new Label("STAR-Pong", Color.Blue, new Vector2(Engine.Instance.ScreenWidth / 2.0f, Engine.Instance.ScreenHeight / 3.0f)));

			Button playButton = new Button("Play", Color.Black, new Vector2(Engine.Instance.ScreenWidth / 2.0f, Engine.Instance.ScreenHeight / 2.0f));
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

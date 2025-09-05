using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Pong.Source.GameStates
{
	public class MenuState: GameState
	{
		Label titleLabel;
		Label titleLabelBack;
		Button playButton;

		public MenuState()
		{
			titleLabel = new Label("ASTER-PONG", Color.Blue, new Vector2(Pong.ScreenWidth / 2.0f, Pong.ScreenHeight / 3.0f));
			titleLabelBack = new Label("ASTER-PONG", Color.Gray, new Vector2(Pong.ScreenWidth / 2.0f + 4, Pong.ScreenHeight / 3.0f + 4));
			playButton = new Button("Play", Color.Black, new Vector2(Pong.ScreenWidth / 2.0f, Pong.ScreenHeight / 2.0f));
		}

		public override void Update(float delta)
		{
			titleLabel.Update(delta);
			titleLabelBack.Update(delta);
			playButton.Update(delta);
		}

		public override void Draw(SpriteBatch batch)
		{
			titleLabelBack.Draw(batch);
			titleLabel.Draw(batch);
			playButton.Draw(batch);
		}
	}
}

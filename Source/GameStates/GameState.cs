using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Pong.Source.GameStates
{
	public class GameState
	{
		public GameState() { }
		public virtual void Initialize() { }
		public virtual void Update(float delta) { }
		public virtual void Draw(SpriteBatch batch) { }
	}
}

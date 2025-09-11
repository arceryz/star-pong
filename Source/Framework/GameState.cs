using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StarPong.Source.Framework
{
	public class GameState
	{
		public GameState() { }

		// To be overriden.
		public virtual void Initialize() { }
		public virtual void Update(float delta) { }
		public virtual void Draw(SpriteBatch batch) { }
	}
}

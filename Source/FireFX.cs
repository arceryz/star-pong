using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using StarPong.Source.Framework;

namespace StarPong.Source
{
	public class FireFX: GameObject
	{
		static Texture2D fireSmallTex;

		public static void LoadContent(ContentManager content)
		{
			fireSmallTex = content.Load<Texture2D>("FX/Fire_Small");
		}

		Sprite sprite;

		public FireFX()
		{
			sprite = new Sprite(fireSmallTex, 8, 1);
			sprite.AddAnimation("fire", 4, 0, 0, 8);
			sprite.Play("fire");
		}

		public override void Update(float delta)
		{
			sprite.Position = Position;
			sprite.Update(delta);
		}

		public override void Draw(SpriteBatch batch)
		{
			sprite.Draw(batch);
		}
	}
}

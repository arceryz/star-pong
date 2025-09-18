using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using StarPong.Framework;

namespace StarPong.Game
{
	public enum FireType
	{
		Small,
		Big
	}

	public class FireFX: GameObject
	{
		Sprite sprite;

		public FireFX(FireType type)
		{
			if (type == FireType.Big)
			{
				Texture2D tex = Engine.Load<Texture2D>(Assets.Textures.FireFX_Big);
				sprite = new Sprite(tex, 4, 1);
				sprite.AddAnimation("fire", 4, 0, 0, 4);
			}
			else
			{
				Texture2D tex = Engine.Load<Texture2D>(Assets.Textures.FireFX_Small);
				sprite = new Sprite(tex, 8, 1);
				sprite.Scale = 2;
				sprite.AddAnimation("fire", 8, 0, 0, 8);
			}
			sprite.Play("fire");
			AddChild(sprite);
		}
	}
}

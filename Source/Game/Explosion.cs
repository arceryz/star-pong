using Microsoft.Xna.Framework.Graphics;
using StarPong.Framework;

namespace StarPong.Game
{
	public enum ExplosionType
	{
		Small,
		Big
	}

	public class Explosion: GameObject
	{
		Sprite sprite;

		public Explosion(ExplosionType type)
		{
			if (type == ExplosionType.Big)
			{
				Texture2D tex = Engine.Load<Texture2D>(AssetPaths.Texture.Explosion_Big);
				sprite = new Sprite(tex, 4, 1);
				sprite.AddAnimation("explode", 8, 0, 0, 4, false);
			}
			else if (type == ExplosionType.Small)
			{
				Texture2D tex = Engine.Load<Texture2D>(AssetPaths.Texture.Explosion_Small);
				sprite = new Sprite(tex, 4, 1);
				sprite.AddAnimation("explode", 8, 0, 0, 4, false);
			}

			sprite.AnimationFinished += () => QueueFree();
			sprite.Play("explode");
			AddChild(sprite);
		}
	}
}

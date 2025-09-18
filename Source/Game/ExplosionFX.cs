using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using StarPong.Framework;

namespace StarPong.Game
{
	public enum ExplosionType
	{
		Small,
		Big
	}

	public class ExplosionFX: GameObject
	{
		Sprite sprite;
		int repeatCount;

		public ExplosionFX(ExplosionType type)
		{
			if (type == ExplosionType.Big)
			{
				Texture2D tex = Engine.Load<Texture2D>(Assets.Textures.ExplosionFX_Big);
				sprite = new Sprite(tex, 4, 1);
				sprite.AddAnimation("explode", 8, 0, 0, 4, false);
				sprite.Scale = 2;
				repeatCount = 3;

				SoundEffect sfx = Engine.Load<SoundEffect>(Assets.Sounds.Explosion);
				sfx.Play();
			}
			else if (type == ExplosionType.Small)
			{
				Texture2D tex = Engine.Load<Texture2D>(Assets.Textures.ExplosionFX_Small);
				sprite = new Sprite(tex, 4, 1);
				sprite.AddAnimation("explode", 8, 0, 0, 4, false);
				sprite.Scale = 1;
				repeatCount = 3;
			}

			sprite.AnimationFinished += OnAnimationFinished;
			sprite.Play("explode");
			AddChild(sprite);
		}

		void OnAnimationFinished()
		{
			if (repeatCount == 0)
			{
				QueueFree();
				return;
			}
			repeatCount--;
			sprite.Play("explode");
		}
	}
}

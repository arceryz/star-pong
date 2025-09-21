using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using StarPong.Framework;
using StarPong.Scenes;

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

		public ExplosionFX(ExplosionType type, int repeatCount=3, bool allSound=false)
		{
			if (type == ExplosionType.Big)
			{
				Texture2D tex = Engine.Load<Texture2D>(Assets.Textures.ExplosionFX_Big);
				sprite = new Sprite(tex, 4, 1);
				sprite.AddAnimation("explode", 8, 0, 0, 4, false);
				sprite.Scale = 2;
				Engine.AddCameraShake(100);
			}
			else if (type == ExplosionType.Small)
			{
				Texture2D tex = Engine.Load<Texture2D>(Assets.Textures.ExplosionFX_Small);
				sprite = new Sprite(tex, 4, 1);
				sprite.AddAnimation("explode", 8, 0, 0, 4, false);
				sprite.Scale = 1;
				Engine.AddCameraShake(SettingsScene.TurboModeEnabled ? 1: 5);
			}

			if (allSound || type == ExplosionType.Big)
			{
				SoundEffect sfx = Engine.Load<SoundEffect>(Assets.Sounds.Explosion);
				sfx.Play(0.7f, 0, 0);
			}

			this.repeatCount = repeatCount;
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

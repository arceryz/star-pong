using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StarPong.Framework
{
	/// <summary>
	/// A sprite that can play animations from a spritesheet.
	/// </summary>
	public class Sprite: GameObject
	{
		protected class Animation
		{
			public int Fps = 0;
			public Rectangle[] Frames;
			
			public Animation(int fps, Rectangle[] sourceRectangles)
			{
				this.Fps = fps;
				this.Frames = sourceRectangles;
			}
		}

		public string CurrentAnimation { get; private set; } = "default";

		Dictionary<string, Animation> animations = new();
		Animation animation;
		int frameIndex = 0;
		float frameTimer = 0;

		Texture2D spriteSheet;
		int slicesX;
		int slicesY;

		public Sprite(Texture2D spriteSheet, int slicesX, int slicesY)
		{
			this.spriteSheet = spriteSheet;
			this.slicesX = slicesX;
			this.slicesY = slicesY;
		}

		public override void Update(float delta)
		{
			if (animation == null) return;
			if (animation.Fps == 0) return;

			frameTimer += delta;
			if (frameTimer > 1.0f / animation.Fps)
			{
				frameTimer = 0;
				frameIndex = (frameIndex + 1) % animation.Frames.Length;
			}
		}

		public override void Draw(SpriteBatch batch)
		{
			DrawTexture(batch, spriteSheet, GlobalPosition, Color.White, Flip);
		}

		public void AddAnimation(string name, int fps, int sliceX, int sliceY, int frameCount)
		{
			int sw = spriteSheet.Width / slicesX;
			int sh = spriteSheet.Height / slicesY;

			Rectangle[] rects = new Rectangle[frameCount];
			for (int i = 0; i < frameCount; i++)
			{
				rects[i] = new Rectangle(sw * (sliceX + i), sh * sliceY, sw, sh);
			}

			Animation anim = new(fps, rects);
			animations[name] = anim;
		}

		public void Play(string name)
		{
			CurrentAnimation = name;
			animation = animations[name];
		}
	}
}

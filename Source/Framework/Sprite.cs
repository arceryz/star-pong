using System;
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
			public bool Loop = true;
			public int Fps = 0;
			public Rectangle[] Frames;
			
			public Animation(int fps, Rectangle[] sourceRectangles, bool loop)
			{
				this.Loop = loop;
				this.Fps = fps;
				this.Frames = sourceRectangles;
			}
		}

		public string CurrentAnimation { get; private set; } = "";
		public Action AnimationFinished;
		public Rect2 FrameSize = Rect2.Zero;
		public int FrameIndex = 0;
		public float RotationDeg = 0;
		public float Scale = 1;

		Dictionary<string, Animation> animations = new();
		Animation animation;
		float frameTimer = 0;

		Texture2D spriteSheet;
		int sliceCountX;
		int sliceCountY;

		public Sprite(Texture2D spriteSheet, int sliceCountX, int sliceCountY)
		{
			this.spriteSheet = spriteSheet;
			this.sliceCountX = sliceCountX;
			this.sliceCountY = sliceCountY;
			FrameSize = new Rect2(0, 0, spriteSheet.Width / sliceCountX, spriteSheet.Height / sliceCountY);
		}

		public override void Update(float delta)
		{
			if (animation == null) return;
			if (animation.Fps == 0) return;

			frameTimer += delta;
			if (frameTimer > 1.0f / animation.Fps)
			{
				frameTimer = 0;
				FrameIndex = (FrameIndex + 1) % animation.Frames.Length;
				if (FrameIndex == 0 && !animation.Loop)
				{
					Stop();
					AnimationFinished?.Invoke();
				}
			}
		}

		public override void Draw(SpriteBatch batch)
		{
			if (animation != null)
			{
				DrawTexture(batch, spriteSheet, GlobalPosition, animation.Frames[FrameIndex], Color.White, Flip, true, RotationDeg, Scale);
			}
			if (sliceCountX == 1 && sliceCountY == 1)
			{
				DrawTexture(batch, spriteSheet, GlobalPosition, Color.White, Flip, true, RotationDeg, Scale);
			}
		}

		public void AddAnimation(string name, int fps, int sliceX, int sliceY, int frameCount, bool loop=true)
		{
			int sw = spriteSheet.Width / sliceCountX;
			int sh = spriteSheet.Height / sliceCountY;

			Rectangle[] rects = new Rectangle[frameCount];
			for (int i = 0; i < frameCount; i++)
			{
				rects[i] = new Rectangle(sw * (sliceX + i), sh * sliceY, sw, sh);
			}

			Animation anim = new(fps, rects, loop);
			animations[name] = anim;
		}

		public void Play(string name, bool restart=true)
		{
			if (CurrentAnimation != name || restart)
			{
				CurrentAnimation = name;
				animation = animations[name];
				FrameIndex = 0;
				frameTimer = 0;
			}
		}

		public void Stop()
		{
			animation = null;
		}
	}
}

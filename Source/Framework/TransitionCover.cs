using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace StarPong.Framework
{
	/// <summary>
	/// Plays a transition on the full screen and sends a signal when
	/// the transition has been completed.
	/// </summary>
	public class TransitionCover: GameObject
	{
		public Action Finished;
		bool finished = false;

		Color color;
		float timer = 0;
		float duration = 0;
		float waitTime = 0;
		int stepCount = 0;
		int step = 0;
		bool fadeIn;

		SoundEffectInstance sfx;

		public TransitionCover(Color color, int stepCount, float duration, float waitTime, bool fadeIn=true)
		{
			this.color = color;
			this.stepCount = stepCount;
			this.duration = duration;
			this.waitTime = waitTime;
			this.fadeIn = fadeIn;

			if (fadeIn)
			{
				sfx = Engine.Load<SoundEffect>(Assets.Sounds.UI_Scene_Transition).CreateInstance();
				sfx.Play();
			}
		}

		public override void Update(float delta)
		{
			timer += delta;
			if (step < stepCount)
			{
				if (timer > duration / stepCount)
				{
					timer = 0;
					step++;
				}
			}
			else if (!finished)
			{
				if (timer > waitTime)
				{
					finished = true;
					Finished?.Invoke();
					QueueFree();
				}
			}
		}

		public override void Draw(SpriteBatch batch)
		{
			float a = 255.0f * (float)step / stepCount;
			if (!fadeIn) a = 255 - a;
			Primitives2D.FillRectangle(batch, Engine.Viewport.ToRectangle(), color with { A = (byte)a });
		}
	}
}

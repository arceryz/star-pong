using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StarPong.Framework
{
	public static class Utility
	{
		public static Random RNG = new();

		public static float RandRange(float from, float to)
		{
			return from + (to - from) * (float)RNG.NextSingle();
		}

		public static bool RandBool()
		{
			return 0 == RNG.Next(0, 2);
		}

		public static int RandInt32()
		{
			return (int)(RNG.NextInt64() % Int32.MaxValue);
		}

		public static Vector2 TexCenter(Texture2D tex)
		{
			return new Vector2(tex.Bounds.Width, tex.Bounds.Height) / 2;
		}

		public static Vector2 RandUnit2()
		{
			Vector2 vec = new Vector2(1, 0);
			vec.Rotate(RandRange(0, 2 * MathF.PI));
			return vec;
		}

		public static float Sign(float x)
		{
			return x > 0 ? 1 : (x < 0 ? -1 : 0);
		}
	}
}

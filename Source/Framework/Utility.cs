using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StarPong.Source.Framework
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

		public static Vector2 CenterToTex(Vector2 vec, Texture2D tex)
		{
			return new Vector2(vec.X - tex.Bounds.Width * 0.5f, vec.Y - tex.Bounds.Height * 0.5f);
		}
	}
}

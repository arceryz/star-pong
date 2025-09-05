using System;
using System.Drawing;
using System.Numerics;

namespace Pong.Source
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
	}
}

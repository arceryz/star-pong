using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarPong.Source
{
	public static class CollisionBit
	{
		public const int TeamRed = 1 << 0;
		public const int TeamBlue = 1 << 1;
		public const int Bomb = 1 << 2;
		public const int Debris = 1 << 3;
	}
}

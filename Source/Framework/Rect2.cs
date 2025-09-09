using Microsoft.Xna.Framework;

namespace StarPong.Source.Framework
{
	public class Rect2
	{
		public static Rect2 Zero = new Rect2(0, 0, 0, 0);
		public static bool IsIntervalOverlapping(float a, float b, float x, float y)
		{
			return (a <= x && x <= b) || (a <= y && y <= b) || (x <= a && a <= y) || (x <= b && b <= y);
		}

		public float X, Y, Width, Height;

		public Rect2(float x, float y, float width, float height)
		{
			X = x;
			Y = y;
			Width = width;
			Height = height;
		}

		public Rect2(Rectangle rect)
		{
			X = rect.X;
			Y = rect.Y;
			Width = rect.Width;
			Height = rect.Height;
		}

		public Rect2 Translated(Vector2 trans)
		{
			return new Rect2(X + trans.X, Y + trans.Y, Width, Height);
		}

		public Rect2 Centered()
		{
			return new Rect2(X - Width / 2, Y - Height / 2, Width, Height);
		}

		public bool ContainsPoint(Vector2 point)
		{
			return X <= point.X && point.X <= X + Width && Y <= point.Y && point.Y <= Y + Height;
		}
		public bool IsOverlapping(Rect2 other)
		{
			return IsIntervalOverlapping(X, X + Width, other.X, other.X + other.Width)
				&& IsIntervalOverlapping(Y, Y + Height, other.Y, other.Y + other.Height);
		}

		public Vector2 GetTL() => new Vector2(X, Y);
		public Vector2 GetTR() => new Vector2(X + Width, Y);
		public Vector2 GetBL() => new Vector2(X, Y + Height);
		public Vector2 GetBR() => new Vector2(X + Width, Y + Height);

		public override string ToString()
		{
			return $"[{(int)X}:{(int)Y}+{(int)Width}:{(int)Height}]";
		}
	}
}

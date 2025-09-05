using Microsoft.Xna.Framework;

namespace Pong.Source
{
	public interface ICollideable
	{
		public Vector2 GetPosition();
		public Rect2 GetCollisionRect();
		public void OnCollision(Vector2 pos, Vector2 normal, ICollideable other);

		public bool IsColliding(ICollideable other)
		{
			Rect2 rect = GetCollisionRect().Translated(GetPosition());
			Rect2 rect2 = other.GetCollisionRect().Translated(other.GetPosition());
			return rect.IsOverlapping(rect2);
		}
	}
}

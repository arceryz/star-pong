using Microsoft.Xna.Framework;

namespace StarPong.Game
{
	public enum Team
	{
		Neutral,
		Blue, 
		Red
	}

	internal interface IDamageable
	{
		public Team Team { get; }
		public int Health { get; }
		public void TakeDamage(int amount, Vector2 where) { }
	}
}

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
		public Team Team { get; set; }
		public int Health { get; set; }
		public void TakeDamage(int amount, Vector2 where) { }
	}
}

using Microsoft.Xna.Framework;

namespace StarPong.Game
{
	public enum Team
	{
		Neutral,
		Blue, 
		Red
	}

	/// <summary>
	/// I use this interface to streamline damage handling and prevent
	/// the same team from hitting itself. Also exposes health and damage functions.
	/// </summary>
	internal interface IDamageable
	{
		public Team Team { get; }
		public int Health { get; }
		public void TakeDamage(int amount, Vector2 where) { }
	}
}

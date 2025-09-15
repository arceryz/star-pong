using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using StarPong.Framework;

namespace StarPong.Game
{
	public class Shield: CollisionObject, IDamageable
	{
		const int shieldHealth = 3;

		public Team Team { get; set; }
		public int Health { get; set; } = 0;
		public bool IsActive { get { return CollisionEnabled; } }
		Texture2D texture;

		public Shield(Team team)
		{
			this.Team = team;
			if (Team == Team.Blue)
			{
				texture = Engine.Load<Texture2D>(AssetPaths.Texture.Shield_Blue);
			}
			else
			{
				Flip = true;
				texture = Engine.Load<Texture2D>(AssetPaths.Texture.Shield_Red);
			}

			CollisionRect = new Rect2(texture.Bounds).Centered();
			CollisionEnabled = false;
		}

		public override void Draw(SpriteBatch batch)
		{
			if (CollisionEnabled)
			{
				DrawTexture(batch, texture, GlobalPosition, Color.White, Flip);
			}
		}

		public void TakeDamage(int amount, Vector2 loc)
		{
			Health -= amount;
			if (Health <= 0)
			{
				Deactivate();
			}
		}

		public void Activate()
		{
			CollisionEnabled = true;
			Health = shieldHealth;
		}

		public void Deactivate()
		{
			CollisionEnabled = false;
		}
	}
}

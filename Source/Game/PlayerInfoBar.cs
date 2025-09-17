using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StarPong.Framework;

namespace StarPong.Game
{
	public class PlayerInfoBar: GameObject
	{
		const float offset = 32.0f;
		const float energyPipEnergyQuantity = 10;

		Player player;
		Texture2D portraitTex;
		Texture2D healthPipTex;
		Texture2D energyPipTex;

		public PlayerInfoBar(Player player)
		{
			if (player.Team == Team.Blue)
			{
				portraitTex = Engine.Load<Texture2D>(AssetPaths.Texture.UI_Blue_Portrait);
				Position = Engine.GetAnchor(-1, -1, offset, offset);
			}
			else
			{
				portraitTex = Engine.Load<Texture2D>(AssetPaths.Texture.UI_Red_Portrait);
				Position = Engine.GetAnchor(1, -1, -offset, offset);
			}
			healthPipTex = Engine.Load<Texture2D>(AssetPaths.Texture.UI_HealthPip);
			energyPipTex = Engine.Load<Texture2D>(AssetPaths.Texture.UI_EnergyPip);

			this.player = player;
		}

		public override void Draw(SpriteBatch batch)
		{
			if (player.Health == 1 && Engine.Time % 0.1f < 0.05f || player.Health > 1)
			{
				// Draw health pips next to each other.
				// Keep in mind the direction based on team
				int dir = player.Team == Team.Blue ? 1 : -1;
				Vector2 healthPipOffset = new Vector2(56 * dir, -14);
				Vector2 energyPipOffset = healthPipOffset + new Vector2(-18 * dir, 24);
				for (int i = 0; i < player.Health; i++)
				{
					DrawTexture(batch, healthPipTex, GlobalPosition + healthPipOffset + new Vector2(i * 30 * dir, 0), Color.White, dir == -1, true);
				}
				for (int i = 0; i < player.Energy / energyPipEnergyQuantity; i++)
				{
					DrawTexture(batch, energyPipTex, GlobalPosition + energyPipOffset + new Vector2(i * 8 * dir, 0), Color.White, false, true);
				}

				DrawTexture(batch, portraitTex, GlobalPosition, Color.White, false);
			}

			if (player.Health <= 0)
			{
				DrawTexture(batch, portraitTex, GlobalPosition, Color.Gray, false);
			}
		}
	}
}

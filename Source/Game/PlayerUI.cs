using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StarPong.Framework;
using StarPong.Scenes;

namespace StarPong.Game
{
	/// <summary>
	/// Draws the portrait, health and energy of a player vessel.
	/// Displays timer when the player is destroyed.
	/// </summary>
	public class PlayerUI: GameObject
	{
		public const float RespawnLabelOffset = 85;
		public const float HOffset = 120;
		public const float VOffset = 42;
		public const float EnergyPipEnergyQuantity = 10;

		Player player;
		Texture2D portraitTex;
		Texture2D healthPipTex;
		Texture2D energyPipTex;
		Label respawnLabel;

		public PlayerUI(Player player)
		{
			respawnLabel = new Label(Engine.Load<ImageFont>(Assets.Fonts.Gyruss_Bronze), "", 4);
			AddChild(respawnLabel);

			if (player.Team == Team.Blue)
			{
				portraitTex = Engine.Load<Texture2D>(Assets.Textures.UI_Blue_Portrait);
				Position = Engine.GetAnchor(0, -1, -HOffset + 2, VOffset);
				respawnLabel.Position = new Vector2(-RespawnLabelOffset, 0);
			}
			else
			{
				portraitTex = Engine.Load<Texture2D>(Assets.Textures.UI_Red_Portrait);
				Position = Engine.GetAnchor(0, -1, HOffset + 2, VOffset);
				respawnLabel.Position = new Vector2(RespawnLabelOffset, 0);
			}
			healthPipTex = Engine.Load<Texture2D>(Assets.Textures.UI_HealthPip);
			energyPipTex = Engine.Load<Texture2D>(Assets.Textures.UI_EnergyPip);

			this.player = player;
		}

		public override void Draw(SpriteBatch batch)
		{
			if (PlayingScene.IsGameFinished)
			{
				respawnLabel.Visible = false;
				return;
			}
			if (player.Health == 1 && Engine.Time % 0.1f < 0.05f || player.Health > 1)
			{
				// Draw health pips next to each other.
				// Keep in mind the direction based on team
				int dir = player.Team == Team.Blue ? -1 : 1;
				Vector2 healthPipOffset = new Vector2(56 * dir, -14);
				Vector2 energyPipOffset = healthPipOffset + new Vector2(-18 * dir, 24);
				for (int i = 0; i < player.Health; i++)
				{
					DrawTexture(batch, healthPipTex, GlobalPosition + healthPipOffset + new Vector2(i * 30 * dir, 0), Color.White, dir == -1, true);
				}
				for (int i = 0; i < player.Energy / EnergyPipEnergyQuantity; i++)
				{
					DrawTexture(batch, energyPipTex, GlobalPosition + energyPipOffset + new Vector2(i * 8 * dir, 0), Color.White, false, true);
				}

				DrawTexture(batch, portraitTex, GlobalPosition, Color.White, false);
			}

			if (player.Health <= 0)
			{
				DrawTexture(batch, portraitTex, GlobalPosition, Color.Gray, false);
				respawnLabel.Text = $"{3-(int)player.RespawnWaitTimer}";
				respawnLabel.Visible = true;
			}

			if (player.State == Player.StateEnum.RespawnWait)
			{
				respawnLabel.Text = $"{3 - (int)player.RespawnWaitTimer}";
				respawnLabel.Visible = true;
			}
			else respawnLabel.Visible = false;

		}
	}
}

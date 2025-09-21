using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StarPong.Framework;
using StarPong.Scenes;

namespace StarPong.Game
{
	/// <summary>
	/// The bomb collides with the border of the screens and deals a ton of damage
	/// to motherships on contact. It spawns in the middle of the screen and flies
	/// to the side that has not been hit previously.
	/// </summary>
	public class Bomb: CollisionObject, IDamageable
	{
		public Team Team { get; private set; } = Team.Neutral;
		public int Health { get; private set; } = 999;
		public bool IsActive = false;

		const float spawnSpread = 45.0f;
		const float speed = 200.0f;
		const float criticalSpeed = 250.0f;
		const float waitingDuration = 3.0f;
		const float waitingTickInterval = 0.5f;

		Sprite sprite;
		Label waitLabel;
		float waitingTimer = 0;

		public Bomb()
		{
			Texture2D bombSheet = Engine.Load<Texture2D>(Assets.Textures.Bomb);
			sprite = new Sprite(bombSheet, 4, 1);
			sprite.AddAnimation("default", 8, 0, 0, 4);
			sprite.Play("default");
			AddChild(sprite);

			CollisionRect = sprite.FrameSize.Centered().Scaled(0.7f, 0.9f);

			waitLabel = new Label(Engine.Load<ImageFont>(Assets.Fonts.Gyruss_Bronze), "3", 4);
			waitLabel.Position = new Vector2(0, -sprite.FrameSize.Height);
			AddChild(waitLabel);

			AddToGroup("bomb");
			Reset();
		}

		public override void Update(float delta)
		{
			if (waitingTimer < waitingDuration)
			{
				IsActive = false;

				waitingTimer += delta;
				sprite.Visible = (int)(waitingTimer / waitingTickInterval) % 2 == 0;
				waitLabel.Text = $"{waitingDuration - (int)waitingTimer}";

				if (waitingTimer > waitingDuration)
				{
					waitLabel.Visible = false;
					sprite.Visible = true;

					// Set the velocity to a random angle between +- spread and random direction.
					float spread = MathHelper.ToRadians(spawnSpread);
					float rotateAngle = Utility.RandRange(-spread, spread);

					Velocity = new Vector2(PlayingScene.IsCriticalPhase ? criticalSpeed: speed, 0.0f);
					if (SettingsScene.TurboModeEnabled)
						Velocity *= 1.5f;
					Velocity.Rotate(rotateAngle);

					float dir = 0;
					if (PlayingScene.LastDamagedTeam == Team.Neutral)
						dir = Utility.RandBool() ? 1.0f : -1.0f;
					else
						dir = PlayingScene.LastDamagedTeam == Team.Blue ? 1.0f : -1.0f;
					Velocity *= dir;
				}
				return;
			}

			IsActive = true;

			// Apply movement.
			base.Update(delta);

			// Handle reflection and reset when out of bounds on either side.
			Rect2 bounding = GetBoundingRect();

			if (bounding.Y < 0) // Upper wall
			{
				Position.Y = bounding.Height / 2 + 1;
				Velocity.Y *= -1;
			}
			if (bounding.Y + bounding.Height > Engine.GameHeight)
			{
				Velocity.Y *= -1;
				Position.Y = Engine.GameHeight - bounding.Height / 2 - 1;
			}
		}

		public void Reset()
		{
			Position = Engine.GetAnchor(0, 0);
			waitingTimer = 0;
			waitLabel.Visible = true;
		}

		public void Explode()
		{
			ExplosionFX explosion = new ExplosionFX(ExplosionType.Big);
			explosion.DrawLayer = 3;
			explosion.Position = Position;
			Parent.AddChild(explosion);

			if (!PlayingScene.IsGameFinished)
				Reset();
			else
				Visible = false;
		}

		public override void OnCollision(Vector2 pos, Vector2 normal, CollisionObject other)
		{
			if (other is Mothership mother)
			{
				mother.TakeDamage(100, Position);
				Explode();
			}
		}
	}
}

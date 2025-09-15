using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StarPong.Framework;

namespace StarPong.Game
{
	public class Player: CollisionObject, IDamageable
    {
		public enum InputAction
        {
            MoveUp,
            MoveDown,
            Shoot,
            RaiseShield,
        }

        static Dictionary<InputAction, Keys> inputMappingLeft = new()
        {
            { InputAction.MoveUp, Keys.W },
            { InputAction.MoveDown, Keys.S },
            { InputAction.Shoot, Keys.C },
            { InputAction.RaiseShield, Keys.V },
        };

		static Dictionary<InputAction, Keys> inputMappingRight = new()
		{
			{ InputAction.MoveUp, Keys.Up },
			{ InputAction.MoveDown, Keys.Down },
            { InputAction.Shoot, Keys.O },
            { InputAction.RaiseShield, Keys.P }
		};

        const float strafeSpeed = 200.0f;
        const float horizontalOffset = 125.0f;
        const float verticalOffset = 10.0f;
        const float shootCooldownTime = 0.25f;
        const float shieldDistance = 50.0f;

        public Team Team { get; set; }
        public int Health { get; set; } = 3;

        Texture2D texture;
        Dictionary<InputAction, Keys> inputMapping;
        float shootCooldown = 0;
        Shield shield;

        public Player(Team team)
        {
            this.Team = team;
			if (Team == Team.Blue)
            {
                texture = Engine.Load<Texture2D>(AssetPaths.Texture.Player_Blue);
                inputMapping = inputMappingLeft;
            }
            else
            {
                Flip = true;
				texture = Engine.Load<Texture2D>(AssetPaths.Texture.Player_Red);
				inputMapping = inputMappingRight;
            }

			CollisionRect = new Rect2(texture.Bounds).Centered().Scaled(0.7f, 0.6f);

			shield = new Shield(Team);
            shield.Position = ToGlobalDir(new Vector2(shieldDistance, 0));
			AddChild(shield);

            Position.Y = Engine.ScreenHeight / 2.0f;
		}

        public override void Update(float delta)
        {
            Velocity = Vector2.Zero;
			if (Input.IsKeyHeld(inputMapping[InputAction.MoveUp]))
            {
                Velocity = new Vector2(0, -strafeSpeed);
            }
            else if (Input.IsKeyHeld(inputMapping[InputAction.MoveDown]))
            {
                Velocity = new Vector2(0, +strafeSpeed);
            }

            base.Update(delta);
			float sw = texture.Bounds.Width;
            float sh = texture.Bounds.Height;

			Position.Y = MathHelper.Clamp(Position.Y, verticalOffset + sh/2, Engine.ScreenHeight - sh/2 - verticalOffset);
            Position.X = Team == Team.Blue ? horizontalOffset - sw/2 : Engine.ScreenWidth - horizontalOffset;

            // Shooting.
            if (shootCooldown > 0) shootCooldown -= delta;
            if (Input.IsKeyHeld(inputMapping[InputAction.Shoot]) && shootCooldown <= 0)
            {
                shootCooldown = shootCooldownTime;

                Bullet bullet = new Bullet(Team, ToGlobalDir(new Vector2(1, 0)));
                bullet.Position = ToGlobal(new Vector2(texture.Width, 0));
                AddChild(bullet);
			}

			// Shielding.
			if (shield.IsActive && Input.IsKeyPressed(inputMapping[InputAction.RaiseShield]))
			{
				shield.Deactivate();
			}
			else if (Input.IsKeyPressed(inputMapping[InputAction.RaiseShield]))
            {
                shield.Activate();
            }
        }

        public override void Draw(SpriteBatch batch)
        {
            DrawTexture(batch, texture, GlobalPosition, Color.White, Flip);
        }

        public void Explode()
        {
            return;
        }

        public void TakeDamage(int dmg, Vector2 pos)
        {
			Health -= dmg;
            if (Health <= 0)
            {
                Explode();
                return;
            }
		}
    }
}

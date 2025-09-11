using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StarPong.Source.Framework;

namespace StarPong.Source
{
	public enum Team
	{
		Blue, Red
	}

	public class Player: CollisionObject
    {
		static Texture2D shipBlueTex;
		static Texture2D shipRedTex;

		public static void LoadContent(ContentManager content)
		{
			shipBlueTex = content.Load<Texture2D>("Player/Ship_Blue");
			shipRedTex = content.Load<Texture2D>("Player/Ship_Red");
		}

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

        public Team Side { get; private set; }
        Texture2D shipTexture;
        Dictionary<InputAction, Keys> inputMapping;

        GameObjectList bulletList;
        float shootCooldown = 0;
        int health = 3;

        public Shield Shield;

        public Player(Team _side, GameObjectList _bulletList, GameObjectList _shieldList)
        {
            bulletList = _bulletList;
            Side = _side;
            if (Side == Team.Blue)
            {
                shipTexture = shipBlueTex;
                inputMapping = inputMappingLeft;
            }
            else
            {
                shipTexture = shipRedTex;
                inputMapping = inputMappingRight;
            }

            CollisionRect = new Rect2(shipTexture.Bounds).Scaled(0.7f, 0.6f);
            Shield = new Shield(Side);
            _shieldList.Add(Shield);
        }

        public override void Update(float delta)
        {
            // Query keyboard state for each of the player, then compute the
            // total step in pixels using delta. 
            Vector2 step = Vector2.Zero;

            if      (Input.IsKeyHeld(inputMapping[InputAction.MoveUp]))    step += new Vector2(0, -strafeSpeed);
            else if (Input.IsKeyHeld(inputMapping[InputAction.MoveDown]))  step += new Vector2(0, +strafeSpeed);

            Position += step * delta;
			float sw = shipTexture.Bounds.Width;
            float sh = shipTexture.Bounds.Height;

			Position.Y = MathHelper.Clamp(Position.Y, verticalOffset, Engine.Instance.ScreenHeight - sh - verticalOffset);
            Position.X = Side == Team.Blue ? horizontalOffset - sw : Engine.Instance.ScreenWidth - horizontalOffset;

            if (shootCooldown > 0) shootCooldown -= delta;
            if (Input.IsKeyHeld(inputMapping[InputAction.Shoot]) && shootCooldown <= 0)
            {
                shootCooldown = shootCooldownTime;

                Vector2 bulletPos = Position;
                Vector2 bulletDirection = Vector2.Zero;

                if (Side == Team.Blue)
                {
                    bulletDirection = Vector2.UnitX;
                    bulletPos += new Vector2(shipTexture.Width, shipTexture.Height / 2);
                }
                if (Side == Team.Red)
                {
                    bulletDirection = -Vector2.UnitX;
                    bulletPos += new Vector2(0, shipTexture.Height / 2);
                }

                bulletList.Add(new Bullet(bulletPos, bulletDirection, this));
			}

			// Shielding.
			if (Shield.IsActive && Input.IsKeyPressed(inputMapping[InputAction.RaiseShield]))
			{
				Shield.Deactivate();
			}
			else if (Input.IsKeyPressed(inputMapping[InputAction.RaiseShield]))
            {
                Shield.Activate();
            }

            Shield.Position = Position;
            if (Side == Team.Blue) Shield.Position += new Vector2(shipTexture.Width + shieldDistance, shipTexture.Height / 2);
            else Shield.Position += new Vector2(-shieldDistance, shipTexture.Height / 2);

			Shield.Update(delta);
        }

        public override void Draw(SpriteBatch batch)
        {
            SpriteEffects eff = Side == Team.Blue ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            batch.Draw(shipTexture, Position, null, Color.White, 0, Vector2.Zero, 1.0f, eff, 0);
            Shield.Draw(batch);
        }

        public void TakeDamage(int dmg)
        {
            health -= 1;
            if (health <= 0)
            {
                Debug.WriteLine("Player died");
            }
        }

		public override void OnCollision(Vector2 pos, Vector2 normal, CollisionObject other)
		{
			if (other is Bullet bullet)
            {
                if (bullet.Player != this)
                {
                    TakeDamage(1);
                }
            }
		}
    }
}

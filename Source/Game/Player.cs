using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StarPong.Framework;
using StarPong.Scenes;

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

        public const float ShootCooldownTime = 0.25f;
        public const float EnergyRegenCooldownTime = 0.5f;
        public const float ShieldDistance = 100.0f;

        public const float TotalEnergy = 100;
        public const float EnergyRegenSec = 30;
        public const float FastEnergyRegenWaitTime = 1;
		public const float FastEnergyRegenSec = 60;
		public const float BulletEnergyCost = 10;
		public const float ShieldActivateMinEnergy = 20;
		public const float ShieldActivateEnergyCost = 10;
        public const float ShieldEnergyCostSec = 30;
        public const float ShieldBulletAbsorbCost = 10;

        public Team Team { get; private set; }
        public int Health { get; private set; } = 3;
        public float Energy = 100;

		const float strafeSpeed = 200.0f;
		const float horizontalOffset = 50.0f;
		const float verticalOffset = 0.0f;

		float shootCooldown = 0;
		float energyRegenCooldown = 0;

		Texture2D texture;
        Dictionary<InputAction, Keys> inputMapping;
        Shield shield;
        Sprite muzzleFlash;

        Vector2 bulletSpawnOrigin = Vector2.Zero;

        public Player(Team team)
        {
            this.Team = team;
			if (Team == Team.Blue)
            {
                texture = Engine.Load<Texture2D>(Assets.Textures.Blue_Player);
                inputMapping = inputMappingLeft;
            }
            else
            {
                Flip = true;
				texture = Engine.Load<Texture2D>(Assets.Textures.Red_Player);
				inputMapping = inputMappingRight;
            }

			CollisionRect = new Rect2(texture.Bounds).Centered().Scaled(0.7f, 0.6f);

			shield = new Shield(Team);
            shield.Position = ToGlobalDir(new Vector2(ShieldDistance, 0));
            shield.BulletHit += OnShieldBulletHit;
			AddChild(shield);

            Position.Y = Engine.GameHeight / 2.0f;
            bulletSpawnOrigin = new Vector2(texture.Width - 60, 0);

			muzzleFlash = new Sprite(Engine.Load<Texture2D>(Assets.Textures.MuzzleFlash), 3, 1);
            muzzleFlash.AddAnimation("flash", 12, 0, 0, 3, false);
			muzzleFlash.Position = ToGlobalDir(bulletSpawnOrigin);
            muzzleFlash.DrawLayer = 1;
            AddChild(muzzleFlash);
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
			float sw = CollisionRect.Width;
            float sh = CollisionRect.Height;

			Position.Y = MathHelper.Clamp(Position.Y, verticalOffset + sh/2, Engine.GameHeight - sh/2 - verticalOffset);
            Position.X = Team == Team.Blue ? horizontalOffset + sw/2 : Engine.GameWidth - horizontalOffset - sw/2;

            // Prevent other functions when the game is finished.
			if (PlayingScene.IsGameFinished)
            {
                return;
            }

			// Shooting.
			if (shootCooldown > 0) shootCooldown -= delta;
            if (Input.IsKeyHeld(inputMapping[InputAction.Shoot]) && shootCooldown <= 0 && Energy >= BulletEnergyCost)
            {
                Energy -= BulletEnergyCost;
				shootCooldown = ShootCooldownTime;
                energyRegenCooldown = EnergyRegenCooldownTime;

                Bullet bullet = new Bullet(Team, ToGlobalDir(new Vector2(1, 0)));
                bullet.DrawLayer = 1;
                bullet.Position = ToGlobal(bulletSpawnOrigin);
                AddChild(bullet);

                muzzleFlash.Play("flash");
			}

			// Shielding.
			if ((shield.IsActive && Input.IsKeyPressed(inputMapping[InputAction.RaiseShield])) || Energy <= 0)
			{
				shield.Deactivate();
			}
			else if (Input.IsKeyPressed(inputMapping[InputAction.RaiseShield]) && Energy >= ShieldActivateMinEnergy)
            {
                Energy -= ShieldActivateEnergyCost;
				shield.Activate();
            }
            if (shield.IsActive)
            {
                energyRegenCooldown = EnergyRegenCooldownTime;
				Energy -= ShieldEnergyCostSec * delta;
            }

            // Energy regen.
            energyRegenCooldown -= delta;
            if (energyRegenCooldown < 0)
            {
                Energy += (energyRegenCooldown < -FastEnergyRegenWaitTime ? FastEnergyRegenSec: EnergyRegenSec) * delta;
            }
			Energy = MathHelper.Clamp(Energy, 0, TotalEnergy);
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
            // Spawn some fire for every hit by a bullet,
            // helps as an additional health indicator.
			FireFX fire = new FireFX(FireType.Small);
            fire.Position = CollisionRect.GetRandomPoint();
			AddChild(fire);

			Health -= dmg;
			if (Health <= 0)
            {
                Explode();
                return;
            }
		}

        public void OnShieldBulletHit()
        {
            Energy -= ShieldBulletAbsorbCost;
        }
    }
}

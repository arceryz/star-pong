using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StarPong.Framework;
using StarPong.Scenes;

namespace StarPong.Game
{
	public class Player: CollisionObject, IDamageable
    {
        public enum StateEnum
        {
            Playing,
			Exploding,
			RespawnWait,
            Spawning
        }

		public enum InputAction
        {
            MoveUp,
            MoveDown,
            Shoot,
            RaiseShield,
        }

		#region Settings

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

		public const float SpawningFlickerInterval = 0.05f;
        public const float RespawnWaitTime = 3.0f;
		public const float SpawnTime = 1.0f;
		public const float SpawnDistance = 100.0f;

		public const float TotalEnergy = 100;
        public const float EnergyRegenSec = 30;
        public const float FastEnergyRegenWaitTime = 1;
		public const float FastEnergyRegenSec = 60;
		public const float BulletEnergyCost = 10;
		public const float ShieldActivateMinEnergy = 20;
		public const float ShieldActivateEnergyCost = 10;
        public const float ShieldEnergyCostSec = 30;
        public const float ShieldBulletAbsorbCost = 10;

		public const float StrafeSpeed = 200.0f;
		public const float HorizontalOffset = 100.0f;
		public const float VerticalOffset = 0.0f;

		#endregion

		public Team Team { get; private set; }
        public int Health { get; private set; } = 3;
        public float Energy = 100;
        public float RespawnWaitTimer { get; private set; } = 0;
		public StateEnum State { get; private set; } = StateEnum.Playing;
		public int DeathCount { get; private set; } = 0;

		float spawnFlickerTimer = 0;
		float spawnTimer = 0;
		float shootCooldown = 0;
		float energyRegenCooldown = 0;
		float moveDirection = 0;
		Vector2 bulletSpawnOrigin = Vector2.Zero;
		Dictionary<InputAction, Keys> inputMapping;

        Shield shield;
        Sprite muzzleFlash;
        Sprite ship;

        public Player(Team team)
        {
            this.Team = team;
            Texture2D texture;
			if (Team == Team.Blue)
            {
                texture = Engine.Load<Texture2D>(Assets.Textures.Blue_Player);
                inputMapping = inputMappingLeft;
            }
            else
            {
                Flip = true;
				texture = Engine.Load<Texture2D>(Assets.Textures.Red_Player);

				// Use a bot for team red if the setting is enabled.
				// Bot is used when input mapping is not set.
				if (!SettingsScene.BotEnabled) inputMapping = inputMappingRight;
            }

            ship = new Sprite(texture, 3, 3);
			ship.RotationDeg = Flip ? -90 : 90;
			ship.AddAnimation(Flip ? "fly_down": "fly_up", 9, 0, 0, 3);
			ship.AddAnimation(Flip ? "fly_up": "fly_down", 9, 0, 1, 3);
			ship.AddAnimation("fly_forward", 0, 0, 2, 1, false);
			ship.Play("fly_forward");
            AddChild(ship);

			shield = new Shield(Team);
            shield.Position = ToGlobalDir(new Vector2(ShieldDistance, 0));
            shield.BulletHit += OnShieldBulletHit;
			AddChild(shield);

			bulletSpawnOrigin = new Vector2(ship.FrameSize.Width - 60, 0);
			CollisionRect = ship.FrameSize.Centered().Scaled(0.7f, 0.6f);

			muzzleFlash = new Sprite(Engine.Load<Texture2D>(Assets.Textures.MuzzleFlash), 3, 1);
            muzzleFlash.AddAnimation("flash", 12, 0, 0, 3, false);
			muzzleFlash.Position = ToGlobalDir(bulletSpawnOrigin);
            muzzleFlash.DrawLayer = 1;
            AddChild(muzzleFlash);

			StartSpawning();
		}

		#region Update
		public override void Update(float delta)
        {
			if (State == StateEnum.Playing)
			{
				if (inputMapping != null) PlayerControlUpdate();
				else BotControlUpdate();

				UpdateMovement(delta);
				if (!PlayingScene.IsGameFinished) UpdateCombat(delta);
			}
			else if (State == StateEnum.RespawnWait)
			{
				RespawnWaitTimer += delta;
				if (RespawnWaitTimer > RespawnWaitTime) StartSpawning();
			}
			else if (State == StateEnum.Spawning)
			{
				UpdateMovement(delta);

				if (DeathCount > 0) spawnFlickerTimer += delta;
				if (spawnFlickerTimer > SpawningFlickerInterval)
				{
					spawnFlickerTimer = 0;
					Visible = !Visible;
				}

				spawnTimer += delta;
				if (spawnTimer > SpawnTime) StartPlaying();
			}
		}

        void UpdateMovement(float delta)
        {
			Velocity = new Vector2(0, moveDirection * StrafeSpeed);

			base.Update(delta);
			float sw = CollisionRect.Width;
			float sh = CollisionRect.Height;

			Position.Y = MathHelper.Clamp(Position.Y, VerticalOffset + sh / 2, Engine.GameHeight - sh / 2 - VerticalOffset);
			float targetX = Team == Team.Blue ? HorizontalOffset + sw / 2 : Engine.GameWidth - HorizontalOffset - sw / 2;
			if (State != StateEnum.Spawning) Position.X = targetX;
			else
			{
				float startX = Team == Team.Blue ? -SpawnDistance : Engine.GameWidth + SpawnDistance;
				Position.X = MathHelper.Lerp(startX, targetX, spawnTimer / SpawnTime);
			}

			if (moveDirection > 0) ship.Play("fly_down", false);
			else if (moveDirection == 0) ship.Play("fly_forward", false);
			else if (moveDirection < 0) ship.Play("fly_up", false);
		}
		
        void UpdateCombat(float delta)
        {
			// Shooting.
			if (shootCooldown > 0) shootCooldown -= delta;

			// Shielding.
			if (shield.IsActive)
			{
				energyRegenCooldown = EnergyRegenCooldownTime;
				Energy -= ShieldEnergyCostSec * delta;
				if (Energy <= 0) shield.Deactivate();
			}

			// Energy regen.
			energyRegenCooldown -= delta;
			if (energyRegenCooldown < 0)
			{
				Energy += (energyRegenCooldown < -FastEnergyRegenWaitTime ? FastEnergyRegenSec : EnergyRegenSec) * delta;
			}
			Energy = MathHelper.Clamp(Energy, 0, TotalEnergy);
		}
		#endregion

		#region Controls and Bot

		void Shoot()
		{
			if (shootCooldown <= 0 && Energy >= BulletEnergyCost)
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
		}

		void ToggleShield()
		{
			if (shield.IsActive)
			{
				shield.Deactivate();
			}
			else if (Energy >= ShieldActivateMinEnergy)
			{
				Energy -= ShieldActivateEnergyCost;
				shield.Activate();
			}
		}

		void PlayerControlUpdate()
		{
			if (Input.IsKeyHeld(inputMapping[InputAction.MoveUp])) moveDirection = -1;
			else if (Input.IsKeyHeld(inputMapping[InputAction.MoveDown])) moveDirection = 1;
			else moveDirection = 0;

			if (Input.IsKeyPressed(inputMapping[InputAction.RaiseShield])) ToggleShield();
			if (Input.IsKeyPressed(inputMapping[InputAction.Shoot])) Shoot();
		}

		void BotControlUpdate()
		{

		}
		#endregion

		#region State Transitions
		public void StartPlaying()
		{
			State = StateEnum.Playing;
			Visible = true;
		}

		public void StartSpawning()
		{
			Visible = true;
			CollisionEnabled = true;
			spawnTimer = 0;
			spawnFlickerTimer = 0;
			State = StateEnum.Spawning;
			Health = 3;
			foreach (GameObject child in Children)
			{
				if (child is FireFX) child.QueueFree();
			}

			Position.Y = Engine.GameHeight / 2;
			UpdateMovement(0);
		}

		public void StartRespawnWait()
		{
			Visible = false;
			RespawnWaitTimer = 0;
			State = StateEnum.RespawnWait;
			CollisionEnabled = false;
		}

		public void StartExploding()
        {
            ChainExplosionFX exp = new ChainExplosionFX(CollisionRect, 1.0f, 0.25f, 0);
			exp.TreeExited += () => StartRespawnWait();
            AddChild(exp);
            State = StateEnum.Exploding;
			DeathCount++;
        }
#endregion

		#region Damage
		public void TakeDamage(int dmg, Vector2 pos)
        {
			if (State != StateEnum.Playing) return;

            // Spawn some fire for every hit by a bullet,
            // helps as an additional health indicator.
			FireFX fire = new FireFX(FireType.Small);
            fire.Position = CollisionRect.GetRandomPoint();
			AddChild(fire);

			Health -= dmg;
			if (Health <= 0)
            {
				StartExploding();
                return;
            }
		}

        public void OnShieldBulletHit()
        {
            Energy -= ShieldBulletAbsorbCost;
        }
		#endregion
	}
}

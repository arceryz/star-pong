using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StarPong.Framework;
using StarPong.Scenes;

namespace StarPong.Game
{
	/// <summary>
	/// The player can shoot, raise and shield and move. It's objective is to protect
	/// the mothership behind it and take down the enemy mothership and player.
	/// If no control scheme is provided, a bot will be used instead.
	/// This bot simply reflects the bomb if it comes close, and spams bullets on the
	/// enemy if the bomb is no longer a threat.
	/// </summary>
	public class Player: CollisionObject, IDamageable
    {
		#region Settings

        public const float ShootCooldownTime = 0.25f;
        public const float EnergyRegenCooldownTime = 0.5f;
        public const float ShieldDistance = 100.0f;

		public const float SpawningFlickerInterval = 0.05f;
        public const float RespawnWaitTime = 3.0f;
		public const float SpawnTime = 1.0f;
		public const float SpawnDistance = 100.0f;

        public const float EnergyRegenSec = 40;
        public const float FastEnergyRegenWaitTime = 0.1f;
		public const float FastEnergyRegenSec = 80;
		public const float BulletEnergyCost = 10;
		public const float ShieldActivateMinEnergy = 20;
		public const float ShieldActivateEnergyCost = 10;
        public const float ShieldEnergyCostSec = 20;
        public const float ShieldBulletAbsorbCost = 10;

		public const float StrafeSpeed = 200.0f;
		public const float HorizontalOffset = 100.0f;
		public const float VerticalOffset = 0.0f;

		#endregion

		public enum StateEnum
		{
			Playing,
			Exploding,
			RespawnWait,
			Spawning
		}

		public Team Team { get; private set; }
        public int Health { get; private set; } = 3;
        public float Energy = 0;
        public float RespawnWaitTimer { get; private set; } = 0;
		public StateEnum State { get; private set; } = StateEnum.Playing;
		public int DeathCount { get; private set; } = 0;
		public int ShootCount { get; private set; } = 0;

		int maxEnergy = 100;
		float spawnFlickerTimer = 0;
		float spawnTimer = 0;
		float shootCooldown = 0;
		float energyRegenCooldown = 0;
		float moveDirection = 0;
		Vector2 bulletSpawnOrigin = Vector2.Zero;

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
            }
            else
            {
                Flip = true;
				texture = Engine.Load<Texture2D>(Assets.Textures.Red_Player);
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

			AddToGroup("players");
			StartSpawning();
		}

		#region Update
		public override void Update(float delta)
        {
			if (State == StateEnum.Playing)
			{
				if (SettingsScene.BotEnabled && Team == Team.Red) BotControlUpdate(delta);
				else PlayerControlUpdate(delta);

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
			Velocity = new Vector2(0, moveDirection * StrafeSpeed) * (SettingsScene.TurboModeEnabled ? 1.5f : 1.0f);

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
			}

			// Energy regen.
			float turboFactor = SettingsScene.TurboModeEnabled ? 1.5f : 1.0f;
			energyRegenCooldown -= delta * turboFactor;
			if (energyRegenCooldown < 0)
			{
				Energy += turboFactor * (energyRegenCooldown < -FastEnergyRegenWaitTime ? FastEnergyRegenSec : EnergyRegenSec) * delta;
			}
			Energy = MathHelper.Clamp(Energy, 0, maxEnergy);
		}
		#endregion

		#region Controls and Bot

		bool Shoot()
		{
			if (PlayingScene.IsGameFinished) return false;

			float turboFactor = SettingsScene.TurboModeEnabled ? 0.5f : 1.0f;
			if (shootCooldown <= 0 && Energy >= BulletEnergyCost)
			{
				Energy -= BulletEnergyCost;
				shootCooldown = ShootCooldownTime * turboFactor;
				energyRegenCooldown = EnergyRegenCooldownTime * turboFactor;

				Bullet bullet = new Bullet(Team, ToGlobalDir(new Vector2(1, 0)));
				bullet.DrawLayer = 1;
				if (SettingsScene.TurboModeEnabled)
				{
					bullet.Position = ToGlobal(bulletSpawnOrigin + new Vector2(0, ShootCount % 2 == 0 ? -15: 15));
				}
				else
				{
					bullet.Position = ToGlobal(bulletSpawnOrigin);
				}
				AddChild(bullet);

				ShootCount++;
				muzzleFlash.Play("flash");
				return true;
			}
			return false;
		}

		void RaiseShield()
		{
			if (shield.IsActive)
			{
				if (Energy > 0) shield.GivePower();
			}
			else
			{
				if (Energy >= ShieldActivateMinEnergy)
				{
					Energy -= ShieldActivateEnergyCost;
					shield.GivePower();
				}
			}
		}

		void PlayerControlUpdate(float delta)
		{
			string prefix = Team == Team.Blue ? "player0_" : "player1_";
			if (Input.IsActionHeld(prefix + "move_up")) moveDirection = -1;
			else if (Input.IsActionHeld(prefix + "move_down")) moveDirection = 1;
			else moveDirection = 0;
			if (Input.IsActionHeld(prefix + "shield")) RaiseShield();
			if (Input.IsActionHeld(prefix + "shoot")) Shoot();
		}

		// BOT SETTINGS
		const float botMoveStartDist = 30;
		const float botMoveEndDist = 2;
		const float botShootingDist = 50;
		const float botFrenzyMoveDelayMax = 0.5f;
		const float botShootingDelayMax = 0.5f;

		float botShootingTimer = 0;
		float botShootingDelay = 0;
		float botFrenzyMoveDelay = 0;
		float botFrenzyTimer = 0;
		float botTargetOffset = 0;
		bool botIsMoving = false;
		bool botFrenzyMovement = false;

		void BotControlUpdate(float delta)
		{

			Bomb bomb = (Bomb)SceneTree.GetObjectsInGroup("bomb")[0];

			List<GameObject> players = SceneTree.GetObjectsInGroup("players");
			Player enemyPlayer = (Player)(players[0] == this ? players[1] : players[0]);

			// Computation of blackboard values.
			//
			// This value gives the X for which we must really start dealing
			// with the bomb (moving towards it, toggling shield).
			float bombCriticalX = shield.GlobalPosition.X - 100;

			// If bomb proximity is 1 then the bomb is on our side.
			// If 0, then on the enemy side and we will shoot.
			// Also 0 if the bomb is moving away from us.
			float bombProximity = MathHelper.Clamp((bomb.GlobalPosition.X - enemyPlayer.GlobalPosition.X) / (bombCriticalX - enemyPlayer.GlobalPosition.X), 0, 1);
			if (!bomb.IsActive || bomb.Velocity.X < 0) bombProximity = 0;

			float selfY = GlobalPosition.Y;
			float bombY = bomb.GlobalPosition.Y;
			float enemyY = enemyPlayer.GlobalPosition.Y;
			float enemyDist = MathF.Abs(enemyY - selfY);
			float targetY = MathHelper.Lerp(enemyY, bombY, bombProximity) + botTargetOffset;
			float targetDist = MathF.Abs(targetY - selfY);
			float desiredMoveDirection = Utility.Sign(targetY - selfY);

			float turboBulletFactor = SettingsScene.TurboModeEnabled ? 0.2f : 1.0f;
			float turboFrenzyFactor = SettingsScene.TurboModeEnabled ? 0.2f : 1.0f;

			// Moving behavior.
			//
			// Start moving when sufficiently far away and move until close enough.
			// This prevents micro-movements when there is little change.
			moveDirection = 0;
			if (botIsMoving)
			{
				moveDirection = desiredMoveDirection;
				if (targetDist < botMoveEndDist)
				{
					botIsMoving = false;
				}
			}
			else
			{
				if (botFrenzyMovement)
				{
					botFrenzyTimer += delta;
					if (botFrenzyTimer > botFrenzyMoveDelay)
					{
						botFrenzyTimer = 0;
						botTargetOffset = Utility.RandRange(-1,1) * 70 * (targetY < selfY ? -1 : 1);
						botIsMoving = true;
					}
				}
				else
				{
					botTargetOffset = 0;
					if (targetDist > botMoveStartDist)
					{
						botIsMoving = true;
					}
				}
			}

			// Shielding behavior.
			//
			// Start activating the shield when the proximity is sufficiently high.
			// Lowering this reduces the efficiency of the bot.
			if (bombProximity > 0.9)
			{
				RaiseShield();
			}

			// Shooting behavior.
			//
			// If the bomb is not close, we will try shooting the enemy if in range.
			// Go into frenzy movement when in shooting range.
			if (bombProximity < 0.8 && enemyDist < botShootingDist)
			{
				if (!botFrenzyMovement)
				{
					botFrenzyMovement = true;
					botFrenzyMoveDelay = Utility.Rand01() * botFrenzyMoveDelayMax * turboFrenzyFactor;
				}

				botShootingTimer += delta;
				if (botShootingTimer > botShootingDelay)
				{
					if (Shoot())
					{
						botShootingTimer = 0;
						botShootingDelay = Utility.Rand01() * botShootingDelayMax * turboBulletFactor;
					}
				}
			}
			else
			{
				botFrenzyMovement = false;
			}
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
			Health = SettingsScene.TurboModeEnabled ? 5 : 3;
			maxEnergy = SettingsScene.TurboModeEnabled ? 150 : 110;
			Energy = maxEnergy;
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

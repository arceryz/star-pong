using System.Diagnostics;
using StarPong.Framework;

namespace StarPong.Game
{
	/// <summary>
	/// This explosion combines the single explosion and applies it many
	/// times across a region for a chain effect.
	/// It can also spawn fire effects at the explosion points, which then
	/// linger on the parent. Used by the mothership to indicate damaged hull.
	/// </summary>
	public class ChainExplosionFX: GameObject
	{
		Rect2 target;
		float timer = 0;
		float duration = 0;
		float rate = 0;
		float spawnTimer = 0;
		float bigRatio = 0.5f;
		bool spawnFire = false;

		ExplosionFX lastExplosion;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="target">Target rectangle in relative coordinates</param>
		/// <param name="duration"></param>
		/// <param name="rate"></param>
		public ChainExplosionFX(Rect2 target, float duration, float rate, float bigRatio, bool spawnFire=false)
		{
			this.duration = duration;
			this.rate = rate;
			this.target = target;
			this.bigRatio = bigRatio;
			this.spawnFire = spawnFire;
			Engine.AddCameraShake(bigRatio * 1000);
		}

		public override void Update(float delta)
		{
			timer += delta;
			if (timer < duration)
			{
				spawnTimer += delta;
				if (spawnTimer > rate)
				{
					spawnTimer = 0;

					GameObject parent = this;
					ExplosionType type = Utility.Rand01() < bigRatio ? ExplosionType.Big : ExplosionType.Small;
					ExplosionFX exp = new ExplosionFX(type, 3, true);

					if (spawnFire)
					{
						FireFX fire = new FireFX(type == ExplosionType.Small ? FireType.Small : FireType.Big);
						fire.Position = target.GetRandomPoint();
						Parent.AddChild(fire);
						parent = fire;
					}
					else exp.Position = target.GetRandomPoint();

					parent.AddChild(exp);
					lastExplosion = exp;
				}
			}
			else
			{
				if (lastExplosion.IsDeleted)
				{
					QueueFree();
				}
			}
		}
	}
}

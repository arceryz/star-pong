using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using StarPong.Source.Framework;

namespace StarPong.Source
{
	public class Shield: CollisionObject
	{
		static Texture2D shieldBlueTex;
		static Texture2D shieldRedTex;

		public static void LoadContent(ContentManager content)
		{
			shieldBlueTex = content.Load<Texture2D>("Player/Shield_Blue");
			shieldRedTex = content.Load<Texture2D>("Player/Shield_Red");
		}

		Texture2D shieldTex;
		public Team Side;
		public bool IsActive { get; private set; } = false;

		public Shield(Team _side)
		{
			Side = _side;
			shieldTex = Side == Team.Blue ? shieldBlueTex : shieldRedTex;
			CollisionRect = new Rect2(shieldTex.Bounds).Centered();
			CollisionEnabled = false;
		}

		public override void Update(float delta)
		{
			
		}

		public override void Draw(SpriteBatch batch)
		{
			if (IsActive)
			{
				SpriteEffects eff = Side == Team.Blue ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
				batch.Draw(shieldTex, Utility.CenterToTex(Position, shieldTex), null, Color.White, 0, Vector2.Zero, 1.0f, eff, 0);
			}
		}

		public override void OnCollision(Vector2 pos, Vector2 normal, CollisionObject other)
		{
			base.OnCollision(pos, normal, other);
		}

		public void Activate()
		{
			IsActive = true;
			CollisionEnabled = true;
		}

		public void Deactivate()
		{
			IsActive = false;
			CollisionEnabled = false;
		}
	}
}

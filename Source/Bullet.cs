using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using StarPong.Source.Framework;

namespace StarPong.Source
{
	public class Bullet: CollisionObject
	{
		static Texture2D bulletRedTex;
		static Texture2D bulletBlueTex;

		public static void LoadContent(ContentManager content)
		{
			bulletRedTex = content.Load<Texture2D>("Player/Bullet_Red");
			bulletBlueTex = content.Load<Texture2D>("Player/Bullet_Blue");
		}

		const float bulletSpeed = 500.0f;

		Texture2D bulletTex;
		Vector2 velocity = Vector2.Zero;
		public Player Player;

		public Bullet(Vector2 _position, Vector2 _direction, Player _player)
		{
			Player = _player;
			Position = _position;
			velocity = _direction * bulletSpeed;
			bulletTex = Player.Side == Team.Blue ? bulletBlueTex : bulletRedTex;
			CollisionRect = new Rect2(bulletTex.Bounds).Centered();
		}

		public override void Update(float delta)
		{
			Position += velocity * delta;
		}

		public override void Draw(SpriteBatch batch)
		{
			SpriteEffects eff = velocity.X < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			batch.Draw(bulletTex, Utility.CenterToTex(Position, bulletTex), null, Color.White, 0, Vector2.Zero, 1.0f, eff, 0);
		}

		public override void OnCollision(Vector2 pos, Vector2 normal, CollisionObject other)
		{
			if ((other is Shield shield && shield.Side != Player.Side) ||
				(other is Mothership mother && mother.Side != Player.Side) ||
				(other is Bullet && other != this))
			{
				Destroy();
			}
		}
	}
}

using System;
using System.Collections.Generic;
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

        public enum InputAction
        {
            MoveUp,
            MoveDown,
            MoveLeft,
            MoveRight,
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

        static Texture2D shipBlueTex;
        static Texture2D shipRedTex;

        const float strafeSpeed = 200.0f;
        const float horizontalOffset = 125.0f;
        const float verticalOffset = 10.0f;

        public static void LoadContent(ContentManager content)
        {
            shipBlueTex = content.Load<Texture2D>("Player/Ship_Blue");
            shipRedTex = content.Load<Texture2D>("Player/Ship_Red");
        }

        Team side;
        Texture2D shipTexture;
        Dictionary<InputAction, Keys> inputMapping;


        public Player(Team _side)
        {
            side = _side;
            if (side == Team.Blue)
            {
                shipTexture = shipBlueTex;
                inputMapping = inputMappingLeft;
            }
            else
            {
                shipTexture = shipRedTex;
                inputMapping = inputMappingRight;
            }

            CollisionRect = new Rect2(shipTexture.Bounds);
        }

        public override void Update(float delta)
        {
            // Query keyboard state for each of the player, then compute the
            // total step in pixels using delta. 
            KeyboardState kb = Keyboard.GetState();
            Vector2 step = Vector2.Zero;

            if      (kb.IsKeyDown(inputMapping[InputAction.MoveUp]))    step += new Vector2(0, -strafeSpeed);
            else if (kb.IsKeyDown(inputMapping[InputAction.MoveDown]))  step += new Vector2(0, +strafeSpeed);

            Position += step * delta;
			float sw = shipTexture.Bounds.Width;
            float sh = shipTexture.Bounds.Height;

			Position.Y = MathHelper.Clamp(Position.Y, verticalOffset, Engine.Instance.ScreenHeight - sh - verticalOffset);
            Position.X = side == Team.Blue ? horizontalOffset - sw : Engine.Instance.ScreenWidth - horizontalOffset;

        }

        public override void Draw(SpriteBatch batch)
        {
            SpriteEffects eff = side == Team.Blue ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            batch.Draw(shipTexture, Position, null, Color.White, 0, Vector2.Zero, 1.0f, eff, 0);
        }
    }
}

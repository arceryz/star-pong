using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StarPong.Source.Framework;

namespace StarPong.Source
{
    public class Paddle: CollisionObject
    {
        public enum Side
        {
            Left, Right
        }

        static Texture2D paddleTextureRed;
        static Texture2D paddleTextureBlue;

        const float paddleSpeed = 200.0f;
        const float paddleOffset = 10.0f;

        public static void LoadContent(ContentManager content)
        {
            paddleTextureRed = content.Load<Texture2D>("rodeSpeler");
            paddleTextureBlue = content.Load<Texture2D>("blauweSpeler");
        }

        float height = 0.0f;
        Side side;

        public Paddle(Side _side)
        {
            side = _side;
            CollisionRect = new Rect2(paddleTextureBlue.Bounds);
        }

        public override void Update(float delta)
        {
            // Query keyboard state for each of the player, then compute the
            // total step in pixels using delta. 
            KeyboardState kb = Keyboard.GetState();
            float spd = delta * paddleSpeed;
            float step = 0.0f;

            if (side == Side.Left) {
                if (kb.IsKeyDown(Keys.W)) step -= spd;
                if (kb.IsKeyDown(Keys.S)) step += spd;
            }
            else if (side == Side.Right) {
                if (kb.IsKeyDown(Keys.Up)) step -= spd;
                if (kb.IsKeyDown(Keys.Down)) step += spd;
            }

            // Ensure height remains clamped to the screen dimensions.
            height = MathHelper.Clamp(height + step, 0.0f, Engine.Instance.ScreenHeight - paddleTextureRed.Height);
        }

        public override void Draw(SpriteBatch batch)
        {
            Position.Y = height;
            Texture2D texture = paddleTextureRed;

            if (side == Side.Left)
            {
                texture = paddleTextureBlue;
                Position.X = paddleOffset;
            }
            else if (side == Side.Right)
            {
                Position.X = Engine.Instance.ScreenWidth - paddleTextureBlue.Width - paddleOffset;
            }

            batch.Draw(texture, Position, Color.White);
        }
    }
}

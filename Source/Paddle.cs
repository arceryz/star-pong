using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pong.Source
{
    public class Paddle: ICollideable
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
        Vector2 position = Vector2.Zero;
        Side side;

        public Paddle(Side _side)
        {
            side = _side;
        }

        public void Update(float delta)
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
            height = MathHelper.Clamp(height + step, 0.0f, Pong.ScreenHeight - paddleTextureRed.Height);
        }

        public void Draw(SpriteBatch batch)
        {
            position.Y = height;
            Texture2D texture = paddleTextureRed;

            if (side == Side.Left)
            {
                texture = paddleTextureBlue;
                position.X = paddleOffset;
            }
            else if (side == Side.Right)
            {
                position.X = Pong.ScreenWidth - paddleTextureBlue.Width - paddleOffset;
            }

            batch.Draw(texture, position, Color.White);
        }

        // ICollideable interface
        public Rect2 GetCollisionRect() => new Rect2(paddleTextureBlue.Bounds);
        public void OnCollision(Vector2 pos, Vector2 normal, ICollideable other) { }
        public Vector2 GetPosition() => position;
    }
}

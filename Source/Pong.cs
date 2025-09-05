using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pong.Source;
using Pong.Source.GameStates;
using System.Collections.Generic;

namespace Pong
{
    public class Pong : Game
    {
        // If ever becomes dynamic/resizeable window, then change here.
        // For now assume constant.
        public const int ScreenWidth = 1280;
        public const int ScreenHeight = 720;

		// Assets not in any classes.
		public static Texture2D MapLineTexture;

		// Game states.
		public enum GameStateEnum
		{
			MenuState,
			PlayingState,
		}
        static GameStateEnum activeState;
        static Dictionary<GameStateEnum, GameState> gameStates = new();

        public static void ChangeState(GameStateEnum state)
        {
            activeState = state;
            gameStates[state].Initialize();
        }

        // Runtime part.
		GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

		public Pong()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = ScreenWidth;
            graphics.PreferredBackBufferHeight = ScreenHeight;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            gameStates[GameStateEnum.MenuState] = new MenuState();
            gameStates[GameStateEnum.PlayingState] = new PlayingState();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Loading of the assets.
            Paddle.LoadContent(Content);
            Ball.LoadContent(Content);
            Button.LoadContent(Content);
            Label.LoadContent(Content);
            MapLineTexture = Content.Load<Texture2D>("pongMapLine");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

			gameStates[activeState].Update(delta);
			base.Update(gameTime);
		}

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            // Drawing of the game.
            spriteBatch.Begin();
            gameStates[activeState].Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

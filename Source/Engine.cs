using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StarPong.Source;
using StarPong.Source.GameStates;
using StarPong.Source.Framework;
using System.Collections.Generic;
using System.ComponentModel;

namespace StarPong
{
    public class Engine : Game
    {
        public static Engine Instance;
		public static Texture2D MapLineTexture;

		public int ScreenWidth = 1280;
        public int ScreenHeight = 720;

        public float Time = 0;

		// Game states.
		public enum GameStateEnum
		{
			MenuState,
			PlayingState,
		}
        GameStateEnum activeState;
        Dictionary<GameStateEnum, GameState> gameStates = new();

        // Internal.
		GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

		public Engine()
        {
            Instance = this;
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = ScreenWidth;
            graphics.PreferredBackBufferHeight = ScreenHeight;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
			base.Initialize();
			gameStates[GameStateEnum.MenuState] = new MenuState();
            gameStates[GameStateEnum.PlayingState] = new PlayingState();
			ChangeState(GameStateEnum.MenuState);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
			MapLineTexture = Content.Load<Texture2D>("pongMapLine");

			Player.LoadContent(Content);
            Mothership.LoadContent(Content);
            Ball.LoadContent(Content);
            Button.LoadContent(Content);
            Label.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            Time = (float)gameTime.TotalGameTime.TotalSeconds;
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

			gameStates[activeState].Update(delta);
		}

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            // Drawing of the game.
            spriteBatch.Begin();
            gameStates[activeState].Draw(spriteBatch);
            spriteBatch.End();
        }

		public void ChangeState(GameStateEnum state)
		{
			activeState = state;
			gameStates[state].Initialize();
		}

        public Vector2 GetAnchor(float xs, float ys, float xo = 0, float yo = 0)
        {
            return new Vector2(ScreenWidth * (xs + 1) / 2 + xo, ScreenHeight * (ys + 1) / 2 + yo);
        }
	}
}

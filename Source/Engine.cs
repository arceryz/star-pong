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

		public int ScreenWidth = 1280;
        public int ScreenHeight = 720;

        public float Time = 0;
        public bool EnableDebugDraw = false;

        public Input input = new();

		// Game states.
		public enum GameStateEnum
		{
			MenuState,
			PlayingState,
            EndState,
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
            gameStates[GameStateEnum.EndState] = new EndState();
			ChangeState(GameStateEnum.MenuState);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

			Player.LoadContent(Content);
            Mothership.LoadContent(Content);
            Bomb.LoadContent(Content);
            Button.LoadContent(Content);
            Label.LoadContent(Content);
            Bullet.LoadContent(Content);
            Shield.LoadContent(Content);
            ParallaxLayer.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            input.Update();
            if (Input.IsKeyPressed(Keys.Z))
            {
                EnableDebugDraw = !EnableDebugDraw;
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            Time = (float)gameTime.TotalGameTime.TotalSeconds;
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

			gameStates[activeState].Update(delta);
		}

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // Drawing of the game.
            spriteBatch.Begin();
            gameStates[activeState].Draw(spriteBatch);
            spriteBatch.End();
        }

        public void DebugDrawRect(Rect2 rect, Color color)
        {
            if (EnableDebugDraw)
            {
                Primitives2D.DrawRectangle(spriteBatch, new Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height), color);
            }
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

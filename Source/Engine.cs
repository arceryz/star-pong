using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StarPong.Source;
using StarPong.Source.GameStates;
using StarPong.Source.Framework;
using System.Collections.Generic;
using System.Net.Mime;

namespace StarPong
{
	public enum SceneName
	{
		MenuScene,
		PlayingScene,
		EndScene,
	}

	public class Engine : Game
    {
        public static Engine Instance;

		public int ScreenWidth = 1280;
        public int ScreenHeight = 720;

        public float Time = 0;
        public bool EnableDebugDraw = false;

        SceneName activeScene;
        Dictionary<SceneName, GameObject> scenes = new();

        // Internal.
		GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
		Input input = new();
        SceneTree sceneTree = new();

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
			scenes[SceneName.MenuScene] = new MenuScene();
			ChangeScene(SceneName.MenuScene);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            input.Update();
            if (Input.IsKeyPressed(Keys.Z))
            {
                EnableDebugDraw = !EnableDebugDraw;
            }
            if (Input.IsKeyHeld(Keys.Escape))
            {
                Exit();
            }

            Time = (float)gameTime.TotalGameTime.TotalSeconds;
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            sceneTree.Update(delta);
		}

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.FrontToBack);
            sceneTree.Draw(spriteBatch);
            spriteBatch.End();
        }

        public void DebugDrawRect(Rect2 rect, Color color)
        {
            if (EnableDebugDraw)
            {
                Primitives2D.DrawRectangle(spriteBatch, new Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height), color);
            }
        }

		public void ChangeScene(SceneName scene)
		{
			activeScene = scene;
            sceneTree.SetRoot(scenes[scene]);
		}

        public Vector2 GetAnchor(float xs, float ys, float xo = 0, float yo = 0)
        {
            return new Vector2(ScreenWidth * (xs + 1) / 2 + xo, ScreenHeight * (ys + 1) / 2 + yo);
        }

        public static T Load<T>(string path)
        {
            return Instance.Content.Load<T>(path);
        }
	}
}

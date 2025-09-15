using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StarPong.Framework;
using StarPong.Scenes;

namespace StarPong
{
	public enum SceneName
	{
		MenuScene,
		PlayingScene,
		EndScene,
	}

	public class Engine : Microsoft.Xna.Framework.Game
    {
        public static Engine Instance;

		public static int ScreenWidth = 1280;
        public static int ScreenHeight = 720;

        public static float Time = 0;
        public static bool EnableDebugDraw = false;

        public static SceneName ActiveScene { get; private set; }

		// Internal.
		GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
		Input input = new();
        SceneTree sceneTree = new();
        Physics physics = new();

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
			ChangeScene(SceneName.MenuScene);
		}

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
		}

        protected override void Update(GameTime gameTime)
        {
			Time = (float)gameTime.TotalGameTime.TotalSeconds;
			float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

			input.Update();
            if (Input.IsKeyPressed(Keys.Z))
            {
                EnableDebugDraw = !EnableDebugDraw;
            }
            if (Input.IsKeyHeld(Keys.Escape))
            {
                Exit();
            }

            physics.Update(delta);
            sceneTree.Update(delta);
		}

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.FrontToBack);
            sceneTree.Draw(spriteBatch);
            spriteBatch.End();
        }

        public static void DebugDrawRect(Rect2 rect, Color color)
        {
            if (EnableDebugDraw)
            {
                Primitives2D.DrawRectangle(Instance.spriteBatch, new Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height), color);
            }
        }

		public static void ChangeScene(SceneName scene)
		{
            if (SceneTree.Instance.Root != null)
            {
                SceneTree.Instance.Root.QueueFree();
            }
			ActiveScene = scene;

            GameObject obj = null;
            if (scene == SceneName.MenuScene) obj = new MenuScene();
            else if (scene == SceneName.PlayingScene) obj = new PlayingScene();
            Instance.sceneTree.SetRoot(obj);
		}

        public static Vector2 GetAnchor(float xs, float ys, float xo = 0, float yo = 0)
        {
            return new Vector2(Engine.ScreenWidth * (xs + 1) / 2 + xo, Engine.ScreenHeight * (ys + 1) / 2 + yo);
        }

        public static T Load<T>(string path)
        {
            return Instance.Content.Load<T>(path);
        }
	}
}

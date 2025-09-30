using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.NetworkInformation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using StarPong.Framework;
using StarPong.Scenes;

namespace StarPong
{
	public enum SceneName
	{
		MenuScene,
		PlayingScene,
		EndScene,
		SecretScene,
		SettingsScene,
	}

	/// <summary>
	/// Manages engine-specific global state for the game, game loop and transition logic.
	/// Loads custom assets and provides utility functions.
	/// </summary>
	public class Engine : Microsoft.Xna.Framework.Game
    {
        public static Engine Instance;
		public static int GameWidth = 1280;
        public static int GameHeight = 720;
        public static float Time = 0;
        public static bool EnableDebugDraw = false;
        public static bool EnableTreeDraw = false;
        public static SceneName ActiveScene { get; private set; }
        public static SpriteFont DebugFont;
		public static Dictionary<string, Object> CustomAssets = new();
        public static Rect2 Viewport = Rect2.Zero;
		public static bool IsTransitioning { get; private set; } = false;
	
		const float cameraShakeMax = 30;
		const float cameraShakeFalloff = 5;
        static float cameraShake = 0;

		// Internal.
		GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
		Input input = new();
        SceneTree sceneTree = new();
        Physics physics = new();
        DrawSorter drawSorter = new();
        RenderTarget2D renderTarget;
		RenderTarget2D effectTarget;
		Effect crtEffect;
		SoundEffectInstance transitionSFX;

        #region Core
        public Engine()
        {
            Instance = this;

            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = GameWidth;
            graphics.PreferredBackBufferHeight = GameHeight;
            Window.AllowUserResizing = true;

			Content.RootDirectory = "Content";
            IsMouseVisible = true;
		}

        protected override void Initialize()
        {
			base.Initialize();
			ChangeScene(SceneName.MenuScene);

			MediaPlayer.Volume = 0.5f;
			MediaPlayer.IsRepeating = true;

			// Input Setup.
			Input.AddSequence("toggle_secret",   new InputSequence([Keys.B, Keys.E, Keys.A, Keys.U]));

			Input.AddAction("player0_move_up",   new InputAction([new IEKey(Keys.W),    new IEButton(Buttons.DPadUp, 0), new IEButton(Buttons.LeftThumbstickUp, 0)]));
			Input.AddAction("player0_move_down", new InputAction([new IEKey(Keys.S),    new IEButton(Buttons.DPadDown, 0), new IEButton(Buttons.LeftThumbstickDown, 0)]));
			Input.AddAction("player0_shoot",     new InputAction([new IEKey(Keys.C),    new IEButton(Buttons.LeftShoulder, 0), new IEButton(Buttons.RightShoulder, 0)]));
			Input.AddAction("player0_shield",    new InputAction([new IEKey(Keys.V),    new IEButton(Buttons.LeftTrigger, 0), new IEButton(Buttons.RightTrigger, 0)]));

			Input.AddAction("player1_move_up",   new InputAction([new IEKey(Keys.Up),   new IEButton(Buttons.DPadUp, 1), new IEButton(Buttons.LeftThumbstickUp, 1)]));
			Input.AddAction("player1_move_down", new InputAction([new IEKey(Keys.Down), new IEButton(Buttons.DPadDown, 1), new IEButton(Buttons.LeftThumbstickDown, 1)]));
			Input.AddAction("player1_shoot",     new InputAction([new IEKey(Keys.O),    new IEButton(Buttons.LeftShoulder, 1), new IEButton(Buttons.RightShoulder, 1)]));
			Input.AddAction("player1_shield",    new InputAction([new IEKey(Keys.P),    new IEButton(Buttons.LeftTrigger, 1), new IEButton(Buttons.RightTrigger, 1)]));
		}

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
			renderTarget = new RenderTarget2D(GraphicsDevice, GameWidth, GameHeight, false, SurfaceFormat.Color, DepthFormat.None);
			effectTarget = new RenderTarget2D(GraphicsDevice, GameWidth, GameHeight, false, SurfaceFormat.Color, DepthFormat.None);

			crtEffect = Load<Effect>(Assets.Shaders.CRT);
			DebugFont = Load<SpriteFont>(Assets.Fonts.Debug_Kobe_TTF);
            CustomAssets[Assets.Fonts.Gyruss_Gold] = new ImageFont(Assets.Fonts.Gyruss_Gold, "abcdefghijklmnopqrstuvwxyz0123456789-");
            CustomAssets[Assets.Fonts.Gyruss_Grey] = new ImageFont(Assets.Fonts.Gyruss_Grey, "abcdefghijklmnopqrstuvwxyz0123456789-");
			CustomAssets[Assets.Fonts.Gyruss_Bronze] = new ImageFont(Assets.Fonts.Gyruss_Bronze, "abcdefghijklmnopqrstuvwxyz0123456789-");
		}

        protected override void Update(GameTime gameTime)
        {
			Time = (float)gameTime.TotalGameTime.TotalSeconds;
			float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

			input.Update();
            if (Input.IsKeyPressed(Keys.Z)) EnableDebugDraw = !EnableDebugDraw;
            if (Input.IsKeyHeld(Keys.Escape)) Exit();
            if (Input.IsKeyPressed(Keys.T)) EnableTreeDraw = !EnableTreeDraw;
			if (Input.IsKeyPressed(Keys.F)) graphics.ToggleFullScreen();
			if (Input.IsKeyPressed(Keys.R) && Input.IsKeyHeld(Keys.LeftShift)) Engine.ChangeScene(SceneName.MenuScene);

			physics.Update(delta);
			sceneTree.Update(delta);
			sceneTree.UpdateTransform();

            cameraShake *= MathHelper.Max(1.0f - cameraShakeFalloff*delta, 0);
		}

        protected override void Draw(GameTime gameTime)
        {
			ComputeViewport();

			//***********************************************//
			// 1. Render the game itself.
			//***********************************************//
			GraphicsDevice.SetRenderTarget(renderTarget);
			GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Deferred, samplerState: SamplerState.PointClamp);
			drawSorter.ResetLayers();
            sceneTree.QueueDrawObjects();
			drawSorter.DrawLayers(spriteBatch);
			if (EnableDebugDraw) drawSorter.DebugDrawLayers(spriteBatch);

			spriteBatch.End();

			//***********************************************//
			// 2. Apply the CRT effect.

			// We have to use a seperate render target for the effect to
			// avoid graphical glitches from reading/writing the same texture.
			//***********************************************//
			GraphicsDevice.SetRenderTarget(effectTarget);
			GraphicsDevice.Clear(Color.Black);
			crtEffect.Parameters["Time"]?.SetValue((float)gameTime.TotalGameTime.TotalSeconds);
			crtEffect.Parameters["Shake"]?.SetValue(Utility.RandUnit2() * MathHelper.Min(cameraShakeMax, cameraShake) * 0.001f);
			spriteBatch.Begin(SpriteSortMode.Deferred, samplerState: SamplerState.PointClamp, effect: crtEffect);
			spriteBatch.Draw(renderTarget, new Rectangle(0, 0, 1280, 720), Color.White);
			spriteBatch.End();

			//***********************************************//
			// 3. Render the debug overlay on top.
			//***********************************************//
			spriteBatch.Begin(SpriteSortMode.Deferred, samplerState: SamplerState.PointClamp);
			GraphicsDevice.SetRenderTarget(effectTarget);
			if (EnableTreeDraw) sceneTree.DebugDrawTree(spriteBatch);
			spriteBatch.End();

			//***********************************************//
			// 4. Draw the render target to the screen.
			//***********************************************//
			GraphicsDevice.SetRenderTarget(null);
			GraphicsDevice.Clear(Color.Black);
			spriteBatch.Begin(SpriteSortMode.Deferred, samplerState: SamplerState.PointClamp);
			spriteBatch.Draw(effectTarget, Viewport.ToRectangle(), Color.White);
			spriteBatch.End();
		}
		#endregion

		#region Utility

		public static void DebugDrawRect(Rect2 rect, Color color)
        {
            Primitives2D.DrawRectangle(Instance.spriteBatch, new Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height), color);
        }

		public static void ChangeScene(SceneName scene)
		{
			if (IsTransitioning) return;

            GameObject obj = null;
			if (scene == SceneName.MenuScene) obj = new MenuScene();
			else if (scene == SceneName.EndScene) obj = new EndScene();
			else if (scene == SceneName.PlayingScene) obj = new PlayingScene();
			else if (scene == SceneName.SecretScene) obj = new SecretScene();
			else if (scene == SceneName.SettingsScene) obj = new SettingsScene();

			if (SceneTree.Instance.Root != null)
			{
				TransitionCover trans = new TransitionCover(Color.Black, 7, 1, 0.2f);
				trans.DrawLayer = DrawSorter.TopLayer;
				trans.Finished += () =>
				{
					ActiveScene = scene;
					TransitionCover trans2 = new TransitionCover(Color.Black, 4, 0.5f, 0, false);
					SceneTree.Instance.SetRoot(obj);
					obj.AddChild(trans2);
				};
				SceneTree.Instance.Root.AddChild(trans);
			}
			else
			{
				SceneTree.Instance.SetRoot(obj);
			}
		}

        public static Vector2 GetAnchor(float xs, float ys, float xo = 0, float yo = 0)
        {
            return new Vector2(Engine.GameWidth * (xs + 1) / 2 + xo, Engine.GameHeight * (ys + 1) / 2 + yo);
        }

        /// <summary>
        /// Load an asset from the content pipeline or a custom asset loaded by us.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T Load<T>(string path)
        {
            if (CustomAssets.ContainsKey(path))
            {
                return (T)CustomAssets[path];
			}
			return Instance.Content.Load<T>(path);
        }
        #endregion

        #region Viewport
        public void ComputeViewport()
        {
			// Calculate scaling rectangle to maintain aspect ratio.
			// By setting the scale to the smallest of the two ratios, we ensure the game fits on the screen.
			// Black bars will be present on the sides that don't fit.
			float windowWidth = GraphicsDevice.PresentationParameters.BackBufferWidth;
			float windowHeight = GraphicsDevice.PresentationParameters.BackBufferHeight;
			float scale = Math.Min(windowWidth / GameWidth, windowHeight / GameHeight);

			// The viewport is centered on the screen.
			int vpWidth = (int)(GameWidth * scale);
			int vpHeight = (int)(GameHeight * scale);
			int vpX = (int)((windowWidth - vpWidth) / 2);
			int vpY = (int)((windowHeight - vpHeight) / 2);
			Viewport = new Rect2(vpX, vpY, vpWidth, vpHeight);
		}

        public static void AddCameraShake(float strength)
        {
            Engine.cameraShake += strength;
        }

        #endregion
    }
}

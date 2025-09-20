using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace StarPong.Framework
{
	/// <summary>
	/// This class records key presses within a single frame and keeps
	/// track of sequences of key presses, called input actions.
	/// Inspired by the Godot Engine Input class.
	/// </summary>
	public class Input
	{
		class InputAction
		{
			public bool Activated = false;
			Keys[] keys;
			int currentIndex = 0;

			public InputAction(Keys[] keys)
			{
				this.keys = keys;
			}

			public void Update()
			{
				if (Input.IsAnyKeyPressed())
				{
					Keys nextKey = keys[currentIndex];
					if (Input.IsKeyPressed(nextKey))
					{
						currentIndex++;
						if (currentIndex == keys.Length)
						{
							Activated = true;
							currentIndex = 0;
						}
					}
					else currentIndex = 0;
				}
			}
		}

		static Input Instance;

		KeyboardState prevState = new();
		KeyboardState currentState = new();
		Dictionary<string, InputAction> inputActions = new();

		public Input() 
		{
			Instance = this;
		}

		public void Update()
		{
			prevState = currentState;
			currentState = Keyboard.GetState();
			foreach (InputAction combination in inputActions.Values)
			{
				combination.Activated = false;
				combination.Update();
			}
		}

		public static bool IsKeyPressed(Keys key)
		{
			return !Instance.prevState.IsKeyDown(key) && Instance.currentState.IsKeyDown(key);
		}
		
		public static bool IsKeyHeld(Keys key)
		{
			return Instance.currentState.IsKeyDown(key);
		}

		public static bool IsAnyKeyPressed()
		{
			return Instance.currentState.GetPressedKeyCount() > 0 && Instance.prevState.GetPressedKeyCount() == 0;
		}

		public static bool IsActionPressed(string name)
		{
			if (Instance.inputActions.ContainsKey(name))
			{
				return Instance.inputActions[name].Activated;
			}
			return false;
		}

		public static Vector2 GetMousePosition()
		{
			MouseState ms = Mouse.GetState();
			Vector2 pos = new Vector2(ms.Position.X, ms.Position.Y);
			Vector2 orig = Engine.Viewport.GetTL();

			// Take relative coordinates to the true game window, then rescale to game width.
			Vector2 mpos = Vector2.Zero;
			mpos.X = (pos.X - orig.X) / Engine.Viewport.Width * Engine.GameWidth;
			mpos.Y = (pos.Y - orig.Y) / Engine.Viewport.Height * Engine.GameHeight;

			return mpos;
		}

		public static void AddAction(string name, Keys[] keys)
		{
			if (!Instance.inputActions.ContainsKey(name))
			{
				InputAction ia = new InputAction(keys);
				Instance.inputActions.Add(name, ia);
			}
		}
	}
}

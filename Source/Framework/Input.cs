using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Transactions;

namespace StarPong.Framework
{
	public class InputAction
	{
		InputEvent[] events;
		bool isHeld = false;
		bool isPressed = false;

		public InputAction(InputEvent[] events)
		{
			this.events = events;
		}

		public void Update()
		{
			bool held = false;
			foreach (InputEvent ie in events)
			{
				if (ie.IsHeld())
				{
					held = true;
					break;
				}
			}

			isPressed = !isHeld && held;
			isHeld = held;
		}

		public bool IsHeld() => isHeld;
		public bool IsPressed() => isPressed;
	}

	public class InputSequence
	{
		public bool Activated = false;
		Keys[] keys;
		int currentIndex = 0;

		public InputSequence(Keys[] keys)
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

	public enum MouseButton
	{
		Left,
		Right,
		Middle,
	}

	/// <summary>
	/// This class records key presses within a single frame and keeps
	/// track of sequences of key presses, called input actions.
	/// Inspired by the Godot Engine Input class.
	/// </summary>
	public class Input
	{
		static Input Instance;
		public static bool UsingGamepad { get; private set; } = false;
		public static bool UsingMouse { get; private set; } = false;
		public static Action InputMethodChanged;

		const int MaxGamepads = 2;
		KeyboardState prevKeyState = new();
		KeyboardState currentKeyState = new();
		MouseState prevMouseState = new();
		MouseState currentMouseState = new();
		GamePadState[] prevGamepadStates = new GamePadState[MaxGamepads];
		GamePadState[] currentGamepadStates = new GamePadState[MaxGamepads];

		Dictionary<string, InputSequence> inputSequences = new();
		Dictionary<string, InputAction> actions = new();

		public Input() 
		{
			Instance = this;
		}

		public void Update()
		{
			prevKeyState = currentKeyState;
			currentKeyState = Keyboard.GetState();
			prevMouseState = currentMouseState;
			currentMouseState = Mouse.GetState();

			foreach (InputSequence combination in inputSequences.Values)
			{
				combination.Activated = false;
				combination.Update();
			}
			foreach (InputAction action in actions.Values)
			{
				action.Update();
			}
			for (int i = 0; i < MaxGamepads; i++)
			{
				prevGamepadStates[i] = currentGamepadStates[i];
				currentGamepadStates[i] = GamePad.GetState(i);
			}

			if (IsAnyGamepadButtonPressed())
			{
				UsingGamepad = true;
				InputMethodChanged?.Invoke();
			}
			if (IsAnyKeyPressed())
			{
				UsingGamepad = false;
				InputMethodChanged?.Invoke();
			}

			UsingMouse = prevMouseState.Position != currentMouseState.Position;
			if (UsingMouse) UsingGamepad = false;
		}

		#region Input Queries
		public static bool IsKeyPressed(Keys key)
		{
			return !Instance.prevKeyState.IsKeyDown(key) && Instance.currentKeyState.IsKeyDown(key);
		}
		
		public static bool IsKeyHeld(Keys key)
		{
			return Instance.currentKeyState.IsKeyDown(key);
		}

		public static bool IsAnyKeyPressed()
		{
			return Instance.currentKeyState.GetPressedKeyCount() > 0 && Instance.prevKeyState.GetPressedKeyCount() == 0;
		}

		public static bool IsAnyGamepadButtonPressed()
		{
			for (int i = 0; i < MaxGamepads; i++)
			{
				GamePadState st = Instance.currentGamepadStates[i];
				if (st.IsConnected)
				{
					foreach (Buttons b in Enum.GetValues(typeof(Buttons)))
					{
						if (st.IsButtonDown(b)) return true;
					}
				}
			}
			return false;
		}

		public static bool IsGamepadButtonPressed(Buttons button, int gamepadIndex = 0)
		{
			if (gamepadIndex < 0 || gamepadIndex >= MaxGamepads) return false;
			return !Instance.prevGamepadStates[gamepadIndex].IsButtonDown(button) && Instance.currentGamepadStates[gamepadIndex].IsButtonDown(button);
		}

		public static bool IsGamepadButtonHeld(Buttons button, int gamepadIndex = 0)
		{
			if (gamepadIndex < 0 || gamepadIndex >= MaxGamepads) return false;
			return Instance.currentGamepadStates[gamepadIndex].IsButtonDown(button);
		}

		public static bool IsActionHeld(string name)
		{
			if (Instance.actions.ContainsKey(name))
			{
				return Instance.actions[name].IsHeld();
			}
			return false;
		}

		public static bool IsActionPressed(string name)
		{
			if (Instance.actions.ContainsKey(name))
			{
				return Instance.actions[name].IsPressed();
			}
			return false;
		}

		public static bool IsSequencePressed(string name)
		{
			if (Instance.inputSequences.ContainsKey(name))
			{
				return Instance.inputSequences[name].Activated;
			}
			return false;
		}

		public static bool IsMouseButtonHeld(MouseButton button, bool prev=false)
		{
			MouseState ms = prev ? Instance.prevMouseState : Instance.currentMouseState;
			if (button == MouseButton.Left && ms.LeftButton == ButtonState.Pressed) return true;
			else if (button == MouseButton.Right && ms.RightButton == ButtonState.Pressed) return true;
			else if (button == MouseButton.Middle && ms.MiddleButton == ButtonState.Pressed) return true;
			return false;
		}

		public static bool IsMouseButtonPressed(MouseButton button)
		{
			return IsMouseButtonHeld(button) && !IsMouseButtonHeld(button, true);
		}

		public static Vector2 GetMousePosition()
		{
			MouseState ms = Instance.currentMouseState;
			Vector2 pos = new Vector2(ms.Position.X, ms.Position.Y);
			Vector2 orig = Engine.Viewport.GetTL();

			// Take relative coordinates to the true game window, then rescale to game width.
			Vector2 mpos = Vector2.Zero;
			mpos.X = (pos.X - orig.X) / Engine.Viewport.Width * Engine.GameWidth;
			mpos.Y = (pos.Y - orig.Y) / Engine.Viewport.Height * Engine.GameHeight;

			return mpos;
		}
		#endregion

		#region Setup

		public static void AddAction(string name, InputAction action)
		{
			if (!Instance.actions.ContainsKey(name))
			{
				Instance.actions.Add(name, action);
			}
		}	

		public static void AddSequence(string name, InputSequence seq)
		{
			if (!Instance.inputSequences.ContainsKey(name))
			{
				Instance.inputSequences.Add(name, seq);
			}
		}
		#endregion
	}
}

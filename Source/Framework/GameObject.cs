using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StarPong.Framework
{
	public class GameObject
	{
		public GameObject Parent { get; private set; }
		public List<GameObject> Children { get; private set; } = new();
		public List<string> Groups { get; private set; } = new();

		public SceneTree Tree;
		public bool OverridePosition = false;
		public Vector2 Position = Vector2.Zero;
		public Vector2 GlobalPosition = Vector2.Zero;
		public int GlobalDrawLayer { get; private set; } = 0;
		// This value must be between 0 and 1, and MonoGame will sort highest on top with FrontToBack.
		public float GlobalDrawZ => 1.0f - 1.0f / (1.0f + GlobalDrawLayer);
		public int DrawLayer = 0;
		public bool Flip = false;

		public virtual void EnterTree() { }
		public virtual void Update(float delta) { }
		public virtual void DebugDraw(SpriteBatch spriteBatch) { }
		public virtual void Draw(SpriteBatch batch) {}

		public void UpdateHierarchy(float delta)
		{
			if (Parent != null && !OverridePosition)
			{
				GlobalPosition = Position + Parent.GlobalPosition;
			}
			else
			{
				GlobalPosition = Position;
			}

			Update(delta);
			foreach (GameObject child in Children)
			{
				child.UpdateHierarchy(delta);
			}
		}

		public void DrawHierarchy(SpriteBatch batch)
		{
			if (Parent != null)
			{
				GlobalDrawLayer = DrawLayer + Parent.GlobalDrawLayer;
			}
			else
			{
				GlobalDrawLayer = DrawLayer;
			}

			Draw(batch);
			DebugDraw(batch);
			foreach (GameObject child in Children)
			{
				child.DrawHierarchy(batch);
			}
		}

		/// <summary>
		/// Sets up a game object and its children with the scene tree.
		/// Calls EnterTree()
		/// </summary>
		/// <param name="tree"></param>
		public void InitializeHierarchy(SceneTree tree)
		{
			foreach (string group in Groups)
			{
				tree.AddObjectToGroup(this, group);
			}
			Tree = tree;
			EnterTree();
			foreach (GameObject child in Children)
			{
				if (child.Tree == null)
				{
					child.InitializeHierarchy(tree);
				}
			}
		}

		public void QueueFree()
		{
			SceneTree.Instance.QueueFree(this);
			foreach (GameObject child in Children)
			{
				child.QueueFree();
			}
		}

		public void AddChild(GameObject child)
		{
			Children.Add(child);
			child.Parent = this;
			if (Tree != null)
			{
				child.InitializeHierarchy(Tree);
			}
		}

		public void AddToGroup(string group)
		{
			Groups.Add(group);
		}

		public void DrawTexture(SpriteBatch batch, Texture2D texture, Vector2 position, Color color, bool flip=false, bool center=true)
		{
			Vector2 pos = center ? Utility.CenterToTex(position, texture) : position;
			SpriteEffects eff = flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			batch.Draw(texture, pos, null, color, 0, Vector2.Zero, 1.0f, eff, GlobalDrawZ);
		}

		public void DrawTexture(SpriteBatch batch, Texture2D texture, Vector2 position, Rectangle sourceRect, Color color, bool flip = false, bool center = true)
		{
			Vector2 pos = center ? position - new Vector2(sourceRect.Width, sourceRect.Height) / 2: position;
			SpriteEffects eff = flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			batch.Draw(texture, pos, sourceRect, Color.White, 0, Vector2.Zero, 1, eff, GlobalDrawZ);
		}

		public void DrawString(SpriteBatch batch, SpriteFont font, string text, Vector2 position, Color color, bool centered=true)
		{
			Vector2 offset = centered ? 0.5f * font.MeasureString(text) : Vector2.Zero;
			batch.DrawString(font, text, Position - offset, color, 0, Vector2.Zero, 1.0f, SpriteEffects.None, GlobalDrawZ);
		}

		public Vector2 ToGlobal(Vector2 pos)
		{
			return ToGlobalDir(pos) + Position;
		}

		public Vector2 ToGlobalDir(Vector2 vec)
		{
			return Flip ? new Vector2(-vec.X, vec.Y) : vec;
		}
	}
}

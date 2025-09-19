using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StarPong.Game;

namespace StarPong.Framework
{
	public class GameObject
	{
		public GameObject Parent { get; private set; }
		public List<GameObject> Children { get; private set; } = new();
		public List<string> Groups { get; private set; } = new();
		public Guid Id { get; private set; }
		public SceneTree Tree;
		public bool IsDeleted { get; private set; }

		public bool OverridePosition = false;
		public Vector2 Position = Vector2.Zero;
		public Vector2 GlobalPosition = Vector2.Zero;

		public int GlobalDrawLayer { get; private set; } = 0;
		public int DrawLayer = 0;
		public bool Visible = true;
		public bool Flip = false;

		public Action TreeExited;

		public GameObject()
		{
			Id = Guid.NewGuid();
		}

		// The core callbacks are only for use by the inherited classes.
		// The functionality underlying for each game object is in the
		// hierarchy-class of functions, who also update children.
		public virtual void ExitTree() { }
		public virtual void EnterTree() { }
		public virtual void Update(float delta) { }
		public virtual void DebugDraw(SpriteBatch spriteBatch) { }
		public virtual void Draw(SpriteBatch batch) {}

		#region Hierarchy Methods
		/// <summary>
		/// Update all objects in this hierarchy. This can create new objects,
		/// but those are not included in the update.
		/// Their position will be updated by a subsequent call to UpdateTransformHierarchy().
		/// </summary>
		/// <param name="delta"></param>
		public void UpdateHierarchy(float delta)
		{
			Update(delta);
			foreach (GameObject child in Children.ToList())
			{
				child.UpdateHierarchy(delta);
			}
		}

		/// <summary>
		/// Updates the global transforms of this object and all its children.
		/// </summary>
		public void UpdateTransformHierarchy()
		{
			if (Parent != null && !OverridePosition)
				GlobalPosition = Position + Parent.GlobalPosition;
			else
				GlobalPosition = Position;
			foreach (GameObject child in Children.ToList())
			{
				child.UpdateTransformHierarchy();
			}
		}

		/// <summary>
		/// Queues draw calls for this object and all its children.
		/// Due to the nature of the draw sorter, later calls are drawn on top,
		/// but draw layers are still respected.
		/// </summary>
		public void QueueDrawHierarchy()
		{
			if (Parent != null)
				GlobalDrawLayer = DrawLayer + Parent.GlobalDrawLayer;
			else
				GlobalDrawLayer = DrawLayer;
			if (Visible)
			{
				DrawSorter.Instance.QueueDrawObject(this);

				foreach (GameObject child in Children.ToList())
				{
					child.QueueDrawHierarchy();
				}
			}
		}

		/// <summary>
		/// Sets up a game object and its children with the scene tree.
		/// Calls EnterTree()
		/// </summary>
		/// <param name="tree"></param>
		public void EnterTreeHierarchy(SceneTree tree)
		{
			foreach (string group in Groups)
			{
				tree.AddObjectToGroup(this, group);
			}
			Tree = tree;
			EnterTree();
			foreach (GameObject child in Children.ToList())
			{
				if (child.Tree == null)
				{
					child.EnterTreeHierarchy(tree);
				}
			}
		}

		/// <summary>
		/// Notifies the game object and all its children that they left the tree.
		/// </summary>
		public void ExitTreeHierarchy()
		{
			Tree = null;
			foreach (string group in Groups)
			{
				SceneTree.Instance.RemoveObjectFromGroup(this, group);
			}
			ExitTree();
			TreeExited?.Invoke();
			foreach (GameObject child in Children.ToList())
			{
				child.ExitTreeHierarchy();
			}
		}
		#endregion

		#region Scene Tree
		public void QueueFree()
		{
			IsDeleted = true;
			SceneTree.Instance.QueueFree(this);
		}

		/// <summary>
		/// Adds an object as a child, and if part of the scene tree, also
		/// initializes the child by calling EnterTree.
		/// </summary>
		/// <param name="child"></param>
		public void AddChild(GameObject child)
		{
			Children.Add(child);
			child.Parent = this;
			if (Tree != null)
			{
				child.EnterTreeHierarchy(Tree);
			}
			child.UpdateTransformHierarchy();
		}

		/// <summary>
		/// Removes the child from this parent. This will cause it to leave the tree
		/// and calls the necessary callbacks for cleanup. It will leave any assigned groups.
		/// </summary>
		/// <param name="child"></param>
		public void RemoveChild(GameObject child)
		{
			Children.Remove(child);
			child.ExitTreeHierarchy();
		}

		public void AddToGroup(string group)
		{
			Groups.Add(group);
		}

		#endregion

		#region Utilities
		public void DrawTexture(SpriteBatch batch, Texture2D texture, Vector2 position, Color color, bool flip=false, bool center=true, float rotDeg=0, float scale=1)
		{
			Vector2 texCenter = Utility.TexCenter(texture);
			Vector2 pos = center ? position - texCenter : position;
			SpriteEffects eff = flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			batch.Draw(texture, pos + texCenter, null, color, MathHelper.ToRadians(rotDeg), texCenter, scale, eff, 0);
		}

		public void DrawTexture(SpriteBatch batch, Texture2D texture, Vector2 position, Rectangle sourceRect, Color color, bool flip = false, bool center = true, float rotDeg=0, float scale=1)
		{
			Vector2 texCenter = new Vector2(sourceRect.Width, sourceRect.Height) / 2;
			Vector2 pos = center ? position - texCenter: position;
			SpriteEffects eff = flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			batch.Draw(texture, pos + texCenter, sourceRect, Color.White, MathHelper.ToRadians(rotDeg), texCenter, scale, eff, 0);
		}

		public Vector2 ToGlobal(Vector2 pos)
		{
			return ToGlobalDir(pos) + Position;
		}

		public Vector2 ToGlobalDir(Vector2 vec)
		{
			return Flip ? new Vector2(-vec.X, vec.Y) : vec;
		}
		#endregion
	}
}

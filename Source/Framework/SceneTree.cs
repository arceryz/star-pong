using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks.Dataflow;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StarPong.Framework
{
	/// <summary>
	/// The scene tree manages the game object hierarchy, frees the objects
	/// when necessary and facilitates the main game loop.
	/// </summary>
	public class SceneTree
	{
		public static SceneTree Instance;

		List<GameObject> freeQueue = new();
		public GameObject Root { get; private set; }
		Dictionary<string, List<GameObject>> groups = new();

		public SceneTree()
		{
			Instance = this;
		}

		#region Game Objects
		public void UpdateTransform()
		{
			if (Root != null)
			{
				Root.UpdateTransformHierarchy();
			}
		}

		public void Update(float delta)
		{
			if (Root != null)
			{
				Root.UpdateHierarchy(delta);
			}
			foreach (GameObject obj in freeQueue.ToList())
			{
				obj.Parent?.RemoveChild(obj);
			}
			freeQueue = new();
		}

		public void QueueDrawObjects()
		{
			if (Root != null)
			{
				Root.QueueDrawHierarchy();
			}
		}

		public void QueueFree(GameObject obj)
		{
			freeQueue.Add(obj);
		}

		public void SetRoot(GameObject obj)
		{
			Root = obj;
			Root.InitializeHierarchy(this);
		}
		#endregion

		#region Groups
		public void AddObjectToGroup(GameObject obj, string group)
		{
			if (!groups.ContainsKey(group))
			{
				groups[group] = new();
			}
			groups[group].Add(obj);
		}

		public void RemoveObjectFromGroup(GameObject obj, string group)
		{
			if (groups.ContainsKey(group))
			{
				groups[group].Remove(obj);
			}
		}

		public List<GameObject> GetObjectsInGroup(string group)
		{
			if (groups.ContainsKey(group))
			{
				return groups[group];
			}
			return new();
		}
		#endregion

		#region Fun Visualisation

		const int treeX = 10;
		const int treeY = 10;
		const int treeSpacingDepth = 40;
		const int treeSpacingYIndex = 20;
		static Color treePanelColor = Color.Black with { A = 220 };
		static Color treeColor = Color.White;

		/// <summary>
		/// Draws a literal tree to indicate the parent/child relationships of the active objects.
		/// For funsies and for debugging purposes.
		/// </summary>
		/// <param name="batch"></param>
		/// 
		public void DebugDrawTree(SpriteBatch batch)
		{
			if (Root != null)
			{
				int yindex = DrawTreeRecur(Root, batch, 0, 0, false);

				Rectangle rect = new Rectangle(treeX, treeY, 500 + 10, (yindex+1) * treeSpacingYIndex + 10);
				Primitives2D.FillRectangle(batch, rect, treePanelColor);
				Primitives2D.DrawRectangle(batch, rect, treeColor);

				DrawTreeRecur(Root, batch, 0, 0, true);
			}
		}

		int DrawTreeRecur(GameObject obj, SpriteBatch batch, int yindex, int depth, bool draw=true)
		{
			Vector2 position = new Vector2(treeX+10, treeY+5) + new Vector2(depth * treeSpacingDepth, yindex * treeSpacingYIndex);
			if (draw) batch.DrawString(Engine.DebugFont, obj.GetType().Name, position, treeColor);

			int baseY = yindex;
			float y1 = 1;
			float y2 = 1.5f;
			foreach (GameObject child in obj.Children)
			{
				Vector2 vpos1 = position + new Vector2(5, y1 * treeSpacingYIndex);
				Vector2 vpos2 = position + new Vector2(5, y2 * treeSpacingYIndex);
				Primitives2D.DrawLine(batch, vpos1, vpos2, treeColor);

				Vector2 hpos = vpos2 + new Vector2(treeSpacingDepth-10, 0);
				Primitives2D.DrawLine(batch, vpos2, hpos, treeColor);

				yindex = DrawTreeRecur(child, batch, yindex + 1, depth + 1);
				y1 = y2;
				y2 = (yindex - baseY) + 1.5f;
				
			}
			return yindex;
		}
		#endregion
	}
}

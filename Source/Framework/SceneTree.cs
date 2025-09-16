using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

		public void Draw(SpriteBatch batch)
		{
			if (Root != null)
			{
				Root.DrawHierarchy(batch);
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
	}
}

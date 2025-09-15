using System.Collections.Generic;
using System.Diagnostics;
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

		List<GameObject> freeQueue;
		public GameObject Root { get; private set; }
		Dictionary<string, List<GameObject>> groups = new();

		public SceneTree()
		{
			Instance = this;
		}

		public void Update(float delta)
		{
			freeQueue = new();
			if (Root != null)
			{
				Root.UpdateHierarchy(delta);
			}
			foreach (GameObject obj in freeQueue)
			{
				Debug.WriteLine($"Freeing object {obj}");
				obj.Parent?.Children.Remove(obj);
			}
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
			foreach (string group in obj.Groups)
			{
				RemoveObjectFromGroup(obj, group);
			}
			freeQueue.Add(obj);
		}

		public void SetRoot(GameObject obj)
		{
			Root = obj;
			Root.InitializeHierarchy(this);
		}

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
	}
}

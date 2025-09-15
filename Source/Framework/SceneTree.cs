using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace StarPong.Source.Framework
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
				// Delete from parent's list.
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
			freeQueue.Add(obj);
		}

		public void SetRoot(GameObject obj)
		{
			Root = obj;
			Root.Initialize();
		}
	}
}

using System;
using System.Collections.Generic;
using Carbine.Graphics;
using Carbine.Input;
using Carbine.Scenes.Transitions;

namespace Carbine.Scenes
{
	// Token: 0x02000054 RID: 84
	public class SceneManager
	{
		// Token: 0x17000092 RID: 146
		// (get) Token: 0x0600025A RID: 602 RVA: 0x0000D4DB File Offset: 0x0000B6DB
		public static SceneManager Instance
		{
			get
			{
				if (SceneManager.instance == null)
				{
					SceneManager.instance = new SceneManager();
				}
				return SceneManager.instance;
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x0600025B RID: 603 RVA: 0x0000D4F3 File Offset: 0x0000B6F3
		// (set) Token: 0x0600025C RID: 604 RVA: 0x0000D4FB File Offset: 0x0000B6FB
		public ITransition Transition
		{
			get
			{
				return this.transition;
			}
			set
			{
				this.transition = value;
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x0600025D RID: 605 RVA: 0x0000D504 File Offset: 0x0000B704
		public bool IsTransitioning
		{
			get
			{
				return this.state == SceneManager.State.Transition;
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x0600025E RID: 606 RVA: 0x0000D50F File Offset: 0x0000B70F
		public bool IsEmpty
		{
			get
			{
				return this.isEmpty;
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x0600025F RID: 607 RVA: 0x0000D517 File Offset: 0x0000B717
		// (set) Token: 0x06000260 RID: 608 RVA: 0x0000D51F File Offset: 0x0000B71F
		public bool CompositeMode
		{
			get
			{
				return this.isCompositeMode;
			}
			set
			{
				this.isCompositeMode = value;
			}
		}

		// Token: 0x06000261 RID: 609 RVA: 0x0000D528 File Offset: 0x0000B728
		private SceneManager()
		{
			this.scenes = new SceneManager.SceneStack();
			this.isEmpty = true;
			this.transition = new InstantTransition();
			this.state = SceneManager.State.Scene;
		}

		// Token: 0x06000262 RID: 610 RVA: 0x0000D554 File Offset: 0x0000B754
		public void Push(Scene newScene)
		{
			this.Push(newScene, false);
		}

		// Token: 0x06000263 RID: 611 RVA: 0x0000D560 File Offset: 0x0000B760
		public void Push(Scene newScene, bool swap)
		{
			if (this.scenes.Count > 0)
			{
				this.previousScene = (swap ? this.scenes.Pop() : this.scenes.Peek());
				this.popped = swap;
			}
			this.scenes.Push(newScene);
			this.SetupTransition();
			this.isEmpty = false;
		}

		// Token: 0x06000264 RID: 612 RVA: 0x0000D5BC File Offset: 0x0000B7BC
		public Scene Pop()
		{
			if (this.scenes.Count > 0)
			{
				Scene result = this.scenes.Pop();
				this.popped = true;
				if (this.scenes.Count > 0)
				{
					this.scenes.Peek();
					this.SetupTransition();
				}
				else
				{
					this.isEmpty = true;
				}
				this.previousScene = result;
				return result;
			}
			throw new EmptySceneStackException();
		}

		// Token: 0x06000265 RID: 613 RVA: 0x0000D621 File Offset: 0x0000B821
		private void SetupTransition()
		{
			this.transition.Reset();
			this.state = SceneManager.State.Transition;
			InputManager.Instance.Enabled = false;
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0000D640 File Offset: 0x0000B840
		public Scene Peek()
		{
			if (this.scenes.Count > 0)
			{
				return this.scenes.Peek();
			}
			throw new EmptySceneStackException();
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000D664 File Offset: 0x0000B864
		public void Clear()
		{
			Scene scene = this.scenes.Peek();
			while (this.scenes.Count > 0)
			{
				Scene scene2 = this.scenes.Pop();
				if (scene2 == scene)
				{
					scene2.Unfocus();
				}
				scene2.Unload();
			}
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000D6A9 File Offset: 0x0000B8A9
		public void Update()
		{
			this.UpdateScene();
			if (this.state == SceneManager.State.Transition)
			{
				this.UpdateTransition();
			}
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0000D6C0 File Offset: 0x0000B8C0
		private void UpdateScene()
		{
			if (this.scenes.Count > 0)
			{
				Scene scene = this.scenes.Peek();
				scene.Update();
				return;
			}
			throw new EmptySceneStackException();
		}

		// Token: 0x0600026A RID: 618 RVA: 0x0000D6F4 File Offset: 0x0000B8F4
		private void UpdateTransition()
		{
			if (!this.newSceneShown && this.transition.ShowNewScene)
			{
				if (this.previousScene != null)
				{
					if (this.popped)
					{
						this.previousScene.Unfocus();
						this.previousScene.Unload();
						this.previousScene.Dispose();
						this.popped = false;
					}
					else
					{
						this.previousScene.Unfocus();
					}
				}
				Scene scene = this.scenes.Peek();
				scene.Focus();
				this.previousScene = null;
				this.newSceneShown = true;
			}
			if (!this.transition.IsComplete)
			{
				this.transition.Update();
				if (!this.transition.Blocking && this.previousScene != null)
				{
					this.previousScene.Update();
				}
				if (this.transition.Progress > 0.5f && !this.cleanupFlag)
				{
					TextureManager.Instance.Purge();
					GC.Collect();
					this.cleanupFlag = true;
					return;
				}
			}
			else
			{
				this.state = SceneManager.State.Scene;
				this.newSceneShown = false;
				InputManager.Instance.Enabled = true;
				this.cleanupFlag = false;
			}
		}

		// Token: 0x0600026B RID: 619 RVA: 0x0000D804 File Offset: 0x0000BA04
		public void AbortTransition()
		{
			if (this.state == SceneManager.State.Transition)
			{
				if (this.previousScene != null)
				{
					this.previousScene.Unfocus();
					this.previousScene.Unload();
					this.previousScene.Dispose();
					this.previousScene = null;
				}
				if (!this.newSceneShown)
				{
					Scene scene = this.scenes.Peek();
					scene.Focus();
				}
				this.state = SceneManager.State.Scene;
				this.newSceneShown = false;
				InputManager.Instance.Enabled = true;
				this.cleanupFlag = false;
			}
		}

		// Token: 0x0600026C RID: 620 RVA: 0x0000D884 File Offset: 0x0000BA84
		private void CompositeDraw()
		{
			int count = this.scenes.Count;
			for (int i = 0; i < count - 1; i++)
			{
				if (this.scenes[i + 1].DrawBehind)
				{
					this.scenes[i].Draw();
				}
			}
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000D8D4 File Offset: 0x0000BAD4
		public void Draw()
		{
			if (this.scenes.Count > 0)
			{
				if (this.transition.ShowNewScene)
				{
					if (this.isCompositeMode)
					{
						this.CompositeDraw();
					}
					Scene scene = this.scenes.Peek();
					scene.Draw();
				}
				else if (this.previousScene != null)
				{
					this.previousScene.Draw();
				}
				if (!this.transition.IsComplete)
				{
					this.transition.Draw();
				}
			}
		}

		// Token: 0x040001DA RID: 474
		private static SceneManager instance;

		// Token: 0x040001DB RID: 475
		private SceneManager.State state;

		// Token: 0x040001DC RID: 476
		private SceneManager.SceneStack scenes;

		// Token: 0x040001DD RID: 477
		private Scene previousScene;

		// Token: 0x040001DE RID: 478
		private ITransition transition;

		// Token: 0x040001DF RID: 479
		private bool isEmpty;

		// Token: 0x040001E0 RID: 480
		private bool popped;

		// Token: 0x040001E1 RID: 481
		private bool newSceneShown;

		// Token: 0x040001E2 RID: 482
		private bool cleanupFlag;

		// Token: 0x040001E3 RID: 483
		private bool isCompositeMode;

		// Token: 0x02000055 RID: 85
		private enum State
		{
			// Token: 0x040001E5 RID: 485
			Scene,
			// Token: 0x040001E6 RID: 486
			Transition
		}

		// Token: 0x02000056 RID: 86
		private class SceneStack
		{
			// Token: 0x17000097 RID: 151
			public Scene this[int i]
			{
				get
				{
					return this.list[i];
				}
			}

			// Token: 0x17000098 RID: 152
			// (get) Token: 0x06000270 RID: 624 RVA: 0x0000D959 File Offset: 0x0000BB59
			public int Count
			{
				get
				{
					return this.list.Count;
				}
			}

			// Token: 0x06000271 RID: 625 RVA: 0x0000D966 File Offset: 0x0000BB66
			public SceneStack()
			{
				this.list = new List<Scene>();
			}

			// Token: 0x06000272 RID: 626 RVA: 0x0000D979 File Offset: 0x0000BB79
			public void Clear()
			{
				this.list.Clear();
			}

			// Token: 0x06000273 RID: 627 RVA: 0x0000D986 File Offset: 0x0000BB86
			public void Push(Scene scene)
			{
				this.list.Add(scene);
			}

			// Token: 0x06000274 RID: 628 RVA: 0x0000D994 File Offset: 0x0000BB94
			public Scene Peek()
			{
				return this.Peek(0);
			}

			// Token: 0x06000275 RID: 629 RVA: 0x0000D99D File Offset: 0x0000BB9D
			public Scene Peek(int i)
			{
				if (i < 0 || i >= this.list.Count)
				{
					return null;
				}
				return this.list[this.list.Count - i - 1];
			}

			// Token: 0x06000276 RID: 630 RVA: 0x0000D9D0 File Offset: 0x0000BBD0
			public Scene Pop()
			{
				Scene result = null;
				if (this.list.Count > 0)
				{
					result = this.list[this.list.Count - 1];
					this.list.RemoveAt(this.list.Count - 1);
				}
				return result;
			}

			// Token: 0x040001E7 RID: 487
			private List<Scene> list;
		}
	}
}

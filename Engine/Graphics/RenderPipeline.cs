using System;
using System.Collections.Generic;
using SFML.Graphics;

namespace Carbine.Graphics
{
	// Token: 0x0200002A RID: 42
	public class RenderPipeline
	{
		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000175 RID: 373 RVA: 0x0000750F File Offset: 0x0000570F
		public RenderTarget Target
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x06000176 RID: 374 RVA: 0x00007518 File Offset: 0x00005718
		public RenderPipeline(RenderTarget target)
		{
			this.target = target;
			this.renderables = new List<Renderable>();
			this.renderablesToAdd = new Stack<Renderable>();
			this.renderablesToRemove = new Stack<Renderable>();
			this.uids = new Dictionary<Renderable, int>();
			this.depthCompare = new RenderPipeline.RenderableComparer(this);
			this.viewRect = default(FloatRect);
			this.rendRect = default(FloatRect);
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00007582 File Offset: 0x00005782
		public void Add(Renderable renderable)
		{
			if (!this.renderables.Contains(renderable))
			{
				this.renderablesToAdd.Push(renderable);
				return;
			}
			Console.WriteLine("Tried to add renderable that already exists in the RenderPipeline.");
		}

		// Token: 0x06000178 RID: 376 RVA: 0x000075AC File Offset: 0x000057AC
		public void AddAll<T>(IList<T> addRenderables) where T : Renderable
		{
			int count = addRenderables.Count;
			for (int i = 0; i < count; i++)
			{
				this.Add(addRenderables[i]);
			}
		}

		// Token: 0x06000179 RID: 377 RVA: 0x000075DE File Offset: 0x000057DE
		public void Remove(Renderable renderable)
		{
			if (renderable != null)
			{
				this.renderablesToRemove.Push(renderable);
			}
		}

		// Token: 0x0600017A RID: 378 RVA: 0x000075EF File Offset: 0x000057EF
		public void Update(Renderable renderable)
		{
			this.needToSort = true;
		}

		// Token: 0x0600017B RID: 379 RVA: 0x000075F8 File Offset: 0x000057F8
		private void DoAdditions()
		{
			while (this.renderablesToAdd.Count > 0)
			{
				Renderable renderable = this.renderablesToAdd.Pop();
				this.renderables.Add(renderable);
				this.uids.Add(renderable, this.rendCount);
				this.needToSort = true;
				this.rendCount++;
			}
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00007654 File Offset: 0x00005854
		private void DoRemovals()
		{
			while (this.renderablesToRemove.Count > 0)
			{
				Renderable renderable = this.renderablesToRemove.Pop();
				this.renderables.Remove(renderable);
				this.uids.Remove(renderable);
			}
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00007698 File Offset: 0x00005898
		public void Each(Action<Renderable> forEachFunc)
		{
			int count = this.renderables.Count;
			for (int i = 0; i < count; i++)
			{
				forEachFunc(this.renderables[i]);
			}
		}

		// Token: 0x0600017E RID: 382 RVA: 0x000076CF File Offset: 0x000058CF
		public void Clear()
		{
			this.Clear(true);
		}

		// Token: 0x0600017F RID: 383 RVA: 0x000076D8 File Offset: 0x000058D8
		public void Clear(bool dispose)
		{
			this.renderablesToRemove.Clear();
			if (dispose)
			{
				foreach (Renderable renderable in this.renderables)
				{
					this.renderables[0].Dispose();
				}
			}
			this.renderables.Clear();
			if (dispose)
			{
				while (this.renderablesToAdd.Count > 0)
				{
					this.renderablesToAdd.Pop().Dispose();
				}
			}
			this.renderablesToAdd.Clear();
		}

		// Token: 0x06000180 RID: 384 RVA: 0x0000777C File Offset: 0x0000597C
		public void Draw()
		{
			this.DoAdditions();
			this.DoRemovals();
			if (this.needToSort)
			{
				this.renderables.Sort(this.depthCompare);
				this.needToSort = false;
			}
			View view = this.target.GetView();
			this.viewRect.Left = view.Center.X - view.Size.X / 2f;
			this.viewRect.Top = view.Center.Y - view.Size.Y / 2f;
			this.viewRect.Width = view.Size.X;
			this.viewRect.Height = view.Size.Y;
			int count = this.renderables.Count;
			for (int i = 0; i < count; i++)
			{
				Renderable renderable = this.renderables[i];
				if (renderable.Visible)
				{
					this.rendRect.Left = renderable.Position.X - renderable.Origin.X;
					this.rendRect.Top = renderable.Position.Y - renderable.Origin.Y;
					this.rendRect.Width = renderable.Size.X;
					this.rendRect.Height = renderable.Size.Y;
					if (this.rendRect.Intersects(this.viewRect))
					{
						renderable.Draw(this.target);
					}
				}
			}
		}

		// Token: 0x040000D7 RID: 215
		private RenderTarget target;

		// Token: 0x040000D8 RID: 216
		private List<Renderable> renderables;

		// Token: 0x040000D9 RID: 217
		private Stack<Renderable> renderablesToAdd;

		// Token: 0x040000DA RID: 218
		private Stack<Renderable> renderablesToRemove;

		// Token: 0x040000DB RID: 219
		private bool needToSort;

		// Token: 0x040000DC RID: 220
		private RenderPipeline.RenderableComparer depthCompare;

		// Token: 0x040000DD RID: 221
		private Dictionary<Renderable, int> uids;

		// Token: 0x040000DE RID: 222
		private int rendCount;

		// Token: 0x040000DF RID: 223
		private FloatRect viewRect;

		// Token: 0x040000E0 RID: 224
		private FloatRect rendRect;

		// Token: 0x0200002B RID: 43
		private class RenderableComparer : IComparer<Renderable>
		{
			// Token: 0x06000181 RID: 385 RVA: 0x00007902 File Offset: 0x00005B02
			public RenderableComparer(RenderPipeline pipeline)
			{
				this.pipeline = pipeline;
			}

			// Token: 0x06000182 RID: 386 RVA: 0x00007914 File Offset: 0x00005B14
			public int Compare(Renderable x, Renderable y)
			{
				if (x.Depth != y.Depth)
				{
					return x.Depth - y.Depth;
				}
				return this.pipeline.uids[y] - this.pipeline.uids[x];
			}

			// Token: 0x040000E1 RID: 225
			private RenderPipeline pipeline;
		}
	}
}

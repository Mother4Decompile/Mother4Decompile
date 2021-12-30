using System;
using System.Collections.Generic;
using Carbine.Graphics;
using Carbine.GUI;
using Carbine.Input;
using Mother4.Data;
using SFML.Graphics;
using SFML.System;

namespace Mother4.GUI
{
	// Token: 0x02000042 RID: 66
	internal abstract class MenuPanel : Renderable
	{
		// Token: 0x0600015B RID: 347 RVA: 0x000093FF File Offset: 0x000075FF
		public MenuPanel(Vector2f position, Vector2f size, int depth, WindowBox.Style style, uint flavor)
		{
			this.Initialize(position, size, depth, style, flavor);
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00009414 File Offset: 0x00007614
		public MenuPanel(Vector2f position, Vector2f size, int depth)
		{
			this.Initialize(position, size, depth, Settings.WindowStyle, Settings.WindowFlavor);
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00009430 File Offset: 0x00007630
		private void Initialize(Vector2f position, Vector2f size, int depth, WindowBox.Style style, uint flavor)
		{
			this.position = position;
			this.size = size;
			this.depth = depth;
			this.controls = new List<Renderable>();
			this.window = new WindowBox(Settings.WindowStyle, Settings.WindowFlavor, this.position, this.size + MenuPanel.BORDER_OFFSET * 2f, this.depth);
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00009498 File Offset: 0x00007698
		public void Add(Renderable control)
		{
			this.controls.Add(control);
			control.Position += this.position + MenuPanel.BORDER_OFFSET;
			control.Depth += this.depth;
		}

		// Token: 0x0600015F RID: 351 RVA: 0x000094E5 File Offset: 0x000076E5
		public void Remove(Renderable control)
		{
			this.controls.Remove(control);
		}

		// Token: 0x06000160 RID: 352
		public abstract void AxisPressed(Vector2f axis);

		// Token: 0x06000161 RID: 353
		public abstract object ButtonPressed(Button button);

		// Token: 0x06000162 RID: 354
		public abstract void Focus();

		// Token: 0x06000163 RID: 355
		public abstract void Unfocus();

		// Token: 0x06000164 RID: 356 RVA: 0x000094F4 File Offset: 0x000076F4
		public override void Draw(RenderTarget target)
		{
			for (int i = 0; i < this.controls.Count; i++)
			{
				if (this.controls[i].Visible && this.controls[i].Depth < this.window.Depth)
				{
					this.controls[i].Draw(target);
				}
			}
			this.window.Draw(target);
			for (int j = 0; j < this.controls.Count; j++)
			{
				if (this.controls[j].Visible && this.controls[j].Depth >= this.window.Depth)
				{
					this.controls[j].Draw(target);
				}
			}
		}

		// Token: 0x06000165 RID: 357 RVA: 0x000095C0 File Offset: 0x000077C0
		protected override void Dispose(bool disposing)
		{
			if (!this.disposed && disposing)
			{
				foreach (Renderable renderable in this.controls)
				{
					renderable.Dispose();
				}
			}
			this.disposed = true;
		}

		// Token: 0x04000259 RID: 601
		private static readonly Vector2f BORDER_OFFSET = new Vector2f(8f, 8f);

		// Token: 0x0400025A RID: 602
		private List<Renderable> controls;

		// Token: 0x0400025B RID: 603
		private WindowBox window;
	}
}

using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace Carbine.Graphics
{
	// Token: 0x02000026 RID: 38
	internal class GraphicGroup : Graphic
	{
		// Token: 0x17000051 RID: 81
		public Graphic this[int index]
		{
			get
			{
				return this.graphics[index];
			}
		}

		// Token: 0x06000145 RID: 325 RVA: 0x000066F5 File Offset: 0x000048F5
		public GraphicGroup(Vector2f position, Vector2f origin, int depth)
		{
			this.graphics = new List<Graphic>();
			this.Position = position;
			this.Origin = origin;
			this.Depth = depth;
			this.Size = new Vector2f(0f, 0f);
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00006732 File Offset: 0x00004932
		public void Add(Graphic graphic)
		{
			this.graphics.Add(graphic);
			this.FindSize();
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00006746 File Offset: 0x00004946
		public void Remove(Graphic graphic)
		{
			this.graphics.Remove(graphic);
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00006755 File Offset: 0x00004955
		private void FindSize()
		{
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00006758 File Offset: 0x00004958
		public override void Draw(RenderTarget target)
		{
			foreach (Graphic graphic in this.graphics)
			{
				graphic.Draw(target);
			}
		}

		// Token: 0x0600014A RID: 330 RVA: 0x000067AC File Offset: 0x000049AC
		protected override void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				foreach (Graphic graphic in this.graphics)
				{
					graphic.Dispose();
				}
			}
			this.disposed = true;
		}

		// Token: 0x040000B9 RID: 185
		private List<Graphic> graphics;
	}
}

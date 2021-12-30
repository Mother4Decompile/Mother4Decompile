using System;
using Carbine.Graphics;
using Mother4.Data;
using SFML.System;

namespace Mother4.Battle.UI
{
	// Token: 0x02000075 RID: 117
	internal class YouWon : IDisposable
	{
		// Token: 0x0600027F RID: 639 RVA: 0x0000FC90 File Offset: 0x0000DE90
		public YouWon(RenderPipeline pipeline)
		{
			this.pipeline = pipeline;
			this.youWon = new IndexedColorGraphic(Paths.GRAPHICS + "youwon.dat", "youwon", YouWon.POSITION, 2147450980);
			this.pipeline.Add(this.youWon);
			this.frameCount = (this.youWon.Texture as IndexedTexture).PaletteCount - 1U;
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0000FD04 File Offset: 0x0000DF04
		~YouWon()
		{
			this.Dispose(false);
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0000FD34 File Offset: 0x0000DF34
		public void Update()
		{
			if (this.frame < this.frameCount)
			{
				this.frameTimer++;
				if (this.frameTimer > 4)
				{
					this.frame += 1U;
					this.youWon.CurrentPalette = this.frame;
					this.frameTimer = 0;
				}
			}
		}

		// Token: 0x06000282 RID: 642 RVA: 0x0000FD8C File Offset: 0x0000DF8C
		public void Remove()
		{
			this.pipeline.Remove(this.youWon);
		}

		// Token: 0x06000283 RID: 643 RVA: 0x0000FD9F File Offset: 0x0000DF9F
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000284 RID: 644 RVA: 0x0000FDAE File Offset: 0x0000DFAE
		protected void Dispose(bool disposing)
		{
			if (!this.disposed && disposing)
			{
				this.youWon.Dispose();
			}
			this.disposed = true;
		}

		// Token: 0x040003E3 RID: 995
		private const int DEPTH = 2147450980;

		// Token: 0x040003E4 RID: 996
		private const int FRAME_DELAY = 4;

		// Token: 0x040003E5 RID: 997
		private static Vector2f POSITION = new Vector2f(160f, 18f);

		// Token: 0x040003E6 RID: 998
		private bool disposed;

		// Token: 0x040003E7 RID: 999
		private RenderPipeline pipeline;

		// Token: 0x040003E8 RID: 1000
		private IndexedColorGraphic youWon;

		// Token: 0x040003E9 RID: 1001
		private uint frameCount;

		// Token: 0x040003EA RID: 1002
		private uint frame;

		// Token: 0x040003EB RID: 1003
		private int frameTimer;
	}
}

using System;
using SFML.Graphics;

namespace Carbine.Graphics
{
	// Token: 0x02000024 RID: 36
	public class FullColorTexture : ICarbineTexture, IDisposable
	{
		// Token: 0x1700004B RID: 75
		// (get) Token: 0x0600012C RID: 300 RVA: 0x000062AF File Offset: 0x000044AF
		public Texture Image
		{
			get
			{
				return this.imageTex;
			}
		}

		// Token: 0x0600012D RID: 301 RVA: 0x000062B7 File Offset: 0x000044B7
		public FullColorTexture(Image image)
		{
			this.imageTex = new Texture(image);
		}

		// Token: 0x0600012E RID: 302 RVA: 0x000062CB File Offset: 0x000044CB
		public FullColorTexture(Texture tex)
		{
			this.imageTex = new Texture(tex);
		}

		// Token: 0x0600012F RID: 303 RVA: 0x000062E0 File Offset: 0x000044E0
		~FullColorTexture()
		{
			this.Dispose(false);
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00006310 File Offset: 0x00004510
		public virtual void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000131 RID: 305 RVA: 0x0000631F File Offset: 0x0000451F
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed && disposing)
			{
				this.imageTex.Dispose();
			}
			this.disposed = true;
		}

		// Token: 0x040000B1 RID: 177
		private Texture imageTex;

		// Token: 0x040000B2 RID: 178
		private bool disposed;
	}
}

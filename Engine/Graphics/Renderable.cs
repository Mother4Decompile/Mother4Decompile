using System;
using SFML.Graphics;
using SFML.System;

namespace Carbine.Graphics
{
	// Token: 0x0200001F RID: 31
	public abstract class Renderable : IDisposable
	{
		// Token: 0x0600010C RID: 268 RVA: 0x00006114 File Offset: 0x00004314
		~Renderable()
		{
			this.Dispose(false);
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x0600010D RID: 269 RVA: 0x00006144 File Offset: 0x00004344
		// (set) Token: 0x0600010E RID: 270 RVA: 0x0000614C File Offset: 0x0000434C
		public virtual Vector2f Position
		{
			get
			{
				return this.position;
			}
			set
			{
				this.position = value;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x0600010F RID: 271 RVA: 0x00006155 File Offset: 0x00004355
		// (set) Token: 0x06000110 RID: 272 RVA: 0x0000615D File Offset: 0x0000435D
		public virtual Vector2f Origin
		{
			get
			{
				return this.origin;
			}
			set
			{
				this.origin = value;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000111 RID: 273 RVA: 0x00006166 File Offset: 0x00004366
		// (set) Token: 0x06000112 RID: 274 RVA: 0x0000616E File Offset: 0x0000436E
		public virtual Vector2f Size
		{
			get
			{
				return this.size;
			}
			set
			{
				this.size = value;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000113 RID: 275 RVA: 0x00006177 File Offset: 0x00004377
		// (set) Token: 0x06000114 RID: 276 RVA: 0x0000617F File Offset: 0x0000437F
		public virtual int Depth
		{
			get
			{
				return this.depth;
			}
			set
			{
				this.depth = value;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000115 RID: 277 RVA: 0x00006188 File Offset: 0x00004388
		// (set) Token: 0x06000116 RID: 278 RVA: 0x00006190 File Offset: 0x00004390
		public virtual bool Visible
		{
			get
			{
				return this.visible;
			}
			set
			{
				this.visible = value;
			}
		}

		// Token: 0x06000117 RID: 279
		public abstract void Draw(RenderTarget target);

		// Token: 0x06000118 RID: 280 RVA: 0x00006199 File Offset: 0x00004399
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
			}
			this.disposed = true;
		}

		// Token: 0x06000119 RID: 281 RVA: 0x000061AC File Offset: 0x000043AC
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0400009F RID: 159
		protected Vector2f position;

		// Token: 0x040000A0 RID: 160
		protected Vector2f origin;

		// Token: 0x040000A1 RID: 161
		protected Vector2f size;

		// Token: 0x040000A2 RID: 162
		protected int depth;

		// Token: 0x040000A3 RID: 163
		protected bool visible = true;

		// Token: 0x040000A4 RID: 164
		protected bool disposed;
	}
}

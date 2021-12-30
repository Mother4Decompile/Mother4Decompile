using System;
using SFML.System;

namespace Carbine.Actors
{
	// Token: 0x02000002 RID: 2
	public abstract class Actor : IDisposable
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		// (set) Token: 0x06000002 RID: 2 RVA: 0x00002058 File Offset: 0x00000258
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

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000003 RID: 3 RVA: 0x00002061 File Offset: 0x00000261
		public virtual Vector2f Velocity
		{
			get
			{
				return this.velocity;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000004 RID: 4 RVA: 0x00002069 File Offset: 0x00000269
		// (set) Token: 0x06000005 RID: 5 RVA: 0x00002071 File Offset: 0x00000271
		public virtual float ZOffset
		{
			get
			{
				return this.zOffset;
			}
			set
			{
				this.zOffset = value;
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x0000207C File Offset: 0x0000027C
		~Actor()
		{
			this.Dispose(false);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000020AC File Offset: 0x000002AC
		public virtual void Input()
		{
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000020AE File Offset: 0x000002AE
		public virtual void Update()
		{
			this.position += this.velocity;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000020C7 File Offset: 0x000002C7
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000020D6 File Offset: 0x000002D6
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
			}
			this.disposed = true;
		}

		// Token: 0x04000001 RID: 1
		protected Vector2f position;

		// Token: 0x04000002 RID: 2
		protected float zOffset;

		// Token: 0x04000003 RID: 3
		protected Vector2f velocity;

		// Token: 0x04000004 RID: 4
		protected bool disposed;
	}
}

using System;

namespace Carbine.Scenes
{
	// Token: 0x02000052 RID: 82
	public abstract class Scene : IDisposable
	{
		// Token: 0x17000091 RID: 145
		// (get) Token: 0x0600024A RID: 586 RVA: 0x0000D1F1 File Offset: 0x0000B3F1
		// (set) Token: 0x0600024B RID: 587 RVA: 0x0000D1F9 File Offset: 0x0000B3F9
		public bool DrawBehind
		{
			get
			{
				return this.drawBehind;
			}
			set
			{
				this.drawBehind = value;
			}
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000D204 File Offset: 0x0000B404
		~Scene()
		{
			this.Dispose(false);
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0000D234 File Offset: 0x0000B434
		public virtual void Focus()
		{
		}

		// Token: 0x0600024E RID: 590 RVA: 0x0000D236 File Offset: 0x0000B436
		public virtual void Unfocus()
		{
		}

		// Token: 0x0600024F RID: 591 RVA: 0x0000D238 File Offset: 0x0000B438
		public virtual void Unload()
		{
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0000D23A File Offset: 0x0000B43A
		public virtual void Update()
		{
		}

		// Token: 0x06000251 RID: 593 RVA: 0x0000D23C File Offset: 0x0000B43C
		public virtual void Draw()
		{
		}

		// Token: 0x06000252 RID: 594 RVA: 0x0000D23E File Offset: 0x0000B43E
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000253 RID: 595 RVA: 0x0000D24D File Offset: 0x0000B44D
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
			}
			this.disposed = true;
		}

		// Token: 0x040001D3 RID: 467
		protected bool disposed;

		// Token: 0x040001D4 RID: 468
		private bool drawBehind;
	}
}

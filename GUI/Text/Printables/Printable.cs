using System;
using Carbine.Graphics;

namespace Mother4.GUI.Text.Printables
{
	// Token: 0x02000049 RID: 73
	internal abstract class Printable : Renderable
	{
		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060001AF RID: 431 RVA: 0x0000AC4B File Offset: 0x00008E4B
		public bool Complete
		{
			get
			{
				return this.isDone;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060001B0 RID: 432 RVA: 0x0000AC53 File Offset: 0x00008E53
		public bool Removable
		{
			get
			{
				return this.isRemovable;
			}
		}

		// Token: 0x060001B1 RID: 433
		public abstract void Update();

		// Token: 0x0400028B RID: 651
		protected bool isDone;

		// Token: 0x0400028C RID: 652
		protected bool isRemovable;
	}
}

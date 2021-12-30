using System;

namespace Carbine.Scenes.Transitions
{
	// Token: 0x02000059 RID: 89
	public class InstantTransition : ITransition
	{
		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000288 RID: 648 RVA: 0x0000DD57 File Offset: 0x0000BF57
		public bool IsComplete
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000289 RID: 649 RVA: 0x0000DD5A File Offset: 0x0000BF5A
		public float Progress
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x0600028A RID: 650 RVA: 0x0000DD61 File Offset: 0x0000BF61
		public bool ShowNewScene
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x0600028B RID: 651 RVA: 0x0000DD64 File Offset: 0x0000BF64
		// (set) Token: 0x0600028C RID: 652 RVA: 0x0000DD6C File Offset: 0x0000BF6C
		public bool Blocking { get; set; }

		// Token: 0x0600028D RID: 653 RVA: 0x0000DD75 File Offset: 0x0000BF75
		public void Update()
		{
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000DD77 File Offset: 0x0000BF77
		public void Draw()
		{
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0000DD79 File Offset: 0x0000BF79
		public void Reset()
		{
		}
	}
}

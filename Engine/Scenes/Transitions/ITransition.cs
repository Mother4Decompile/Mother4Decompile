using System;

namespace Carbine.Scenes.Transitions
{
	// Token: 0x02000057 RID: 87
	public interface ITransition
	{
		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000277 RID: 631
		bool IsComplete { get; }

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000278 RID: 632
		float Progress { get; }

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000279 RID: 633
		bool ShowNewScene { get; }

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x0600027A RID: 634
		// (set) Token: 0x0600027B RID: 635
		bool Blocking { get; set; }

		// Token: 0x0600027C RID: 636
		void Update();

		// Token: 0x0600027D RID: 637
		void Draw();

		// Token: 0x0600027E RID: 638
		void Reset();
	}
}

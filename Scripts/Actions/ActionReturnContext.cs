using System;
using System.Collections.Generic;

namespace Mother4.Scripts.Actions
{
	// Token: 0x0200011A RID: 282
	internal struct ActionReturnContext
	{
		// Token: 0x17000113 RID: 275
		// (get) Token: 0x060006D1 RID: 1745 RVA: 0x0002B9A3 File Offset: 0x00029BA3
		// (set) Token: 0x060006D2 RID: 1746 RVA: 0x0002B9AB File Offset: 0x00029BAB
		public ScriptExecutor.WaitType Wait { get; set; }

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x060006D3 RID: 1747 RVA: 0x0002B9B4 File Offset: 0x00029BB4
		// (set) Token: 0x060006D4 RID: 1748 RVA: 0x0002B9BC File Offset: 0x00029BBC
		public Dictionary<string, object> Data { get; set; }
	}
}

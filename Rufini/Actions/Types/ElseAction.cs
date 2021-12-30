using System;
using System.Collections.Generic;
using Mother4.Scripts;
using Mother4.Scripts.Actions;

namespace Rufini.Actions.Types
{
	// Token: 0x02000133 RID: 307
	internal class ElseAction : RufiniAction
	{
		// Token: 0x06000714 RID: 1812 RVA: 0x0002D3CC File Offset: 0x0002B5CC
		public ElseAction()
		{
			this.paramList = new List<ActionParam>();
		}

		// Token: 0x06000715 RID: 1813 RVA: 0x0002D3E0 File Offset: 0x0002B5E0
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			context.Executor.JumpToElseOrEndIf();
			return default(ActionReturnContext);
		}
	}
}

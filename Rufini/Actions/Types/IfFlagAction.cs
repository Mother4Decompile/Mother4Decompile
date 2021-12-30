using System;
using System.Collections.Generic;
using Carbine.Flags;
using Mother4.Scripts;
using Mother4.Scripts.Actions;

namespace Rufini.Actions.Types
{
	// Token: 0x02000142 RID: 322
	internal class IfFlagAction : RufiniAction
	{
		// Token: 0x0600073B RID: 1851 RVA: 0x0002EEF4 File Offset: 0x0002D0F4
		public IfFlagAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "id",
					Type = typeof(int)
				},
				new ActionParam
				{
					Name = "val",
					Type = typeof(bool)
				}
			};
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x0002EF64 File Offset: 0x0002D164
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			int value = base.GetValue<int>("id");
			bool value2 = base.GetValue<bool>("val");
			bool flag = FlagManager.Instance[value];
			if (value2 != flag)
			{
				context.Executor.JumpToElseOrEndIf();
			}
			return default(ActionReturnContext);
		}
	}
}

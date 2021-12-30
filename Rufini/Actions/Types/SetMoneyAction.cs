using System;
using System.Collections.Generic;
using Carbine.Flags;
using Mother4.Scripts;
using Mother4.Scripts.Actions;

namespace Rufini.Actions.Types
{
	// Token: 0x02000159 RID: 345
	internal class SetMoneyAction : RufiniAction
	{
		// Token: 0x06000772 RID: 1906 RVA: 0x00030A98 File Offset: 0x0002EC98
		public SetMoneyAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "val",
					Type = typeof(int)
				}
			};
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x00030AE0 File Offset: 0x0002ECE0
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			int value = base.GetValue<int>("val");
			ValueManager.Instance[1] = value;
			return default(ActionReturnContext);
		}
	}
}

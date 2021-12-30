using System;
using System.Collections.Generic;
using Carbine.Flags;
using Mother4.Scripts;
using Mother4.Scripts.Actions;

namespace Rufini.Actions.Types
{
	// Token: 0x02000120 RID: 288
	internal class AddMoneyAction : RufiniAction
	{
		// Token: 0x060006E5 RID: 1765 RVA: 0x0002BDE4 File Offset: 0x00029FE4
		public AddMoneyAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "val",
					Type = typeof(int)
				},
				new ActionParam
				{
					Name = "msg",
					Type = typeof(bool)
				}
			};
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x0002BE54 File Offset: 0x0002A054
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			int value = base.GetValue<int>("val");
			base.GetValue<bool>("msg");
			ValueManager instance;
			(instance = ValueManager.Instance)[1] = instance[1] + value;
			return default(ActionReturnContext);
		}
	}
}

using System;
using System.Collections.Generic;
using Carbine.Flags;
using Mother4.Scripts;
using Mother4.Scripts.Actions;
using Mother4.Scripts.Actions.ParamTypes;

namespace Rufini.Actions.Types
{
	// Token: 0x02000162 RID: 354
	internal class TimeAction : RufiniAction
	{
		// Token: 0x06000787 RID: 1927 RVA: 0x0003143C File Offset: 0x0002F63C
		public TimeAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "time",
					Type = typeof(RufiniOption)
				}
			};
		}

		// Token: 0x06000788 RID: 1928 RVA: 0x00031484 File Offset: 0x0002F684
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			RufiniOption value = base.GetValue<RufiniOption>("time");
			FlagManager.Instance[1] = (value.Option != 0);
			return default(ActionReturnContext);
		}
	}
}

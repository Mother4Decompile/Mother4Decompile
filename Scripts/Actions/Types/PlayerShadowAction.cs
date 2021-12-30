using System;
using System.Collections.Generic;

namespace Mother4.Scripts.Actions.Types
{
	// Token: 0x0200014E RID: 334
	internal class PlayerShadowAction : RufiniAction
	{
		// Token: 0x06000756 RID: 1878 RVA: 0x0002FDF8 File Offset: 0x0002DFF8
		public PlayerShadowAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "shw",
					Type = typeof(bool)
				}
			};
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x0002FE40 File Offset: 0x0002E040
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			bool value = base.GetValue<bool>("shw");
			context.Player.SetShadow(value);
			return default(ActionReturnContext);
		}
	}
}

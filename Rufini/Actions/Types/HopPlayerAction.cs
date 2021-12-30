using System;
using System.Collections.Generic;
using Mother4.Scripts;
using Mother4.Scripts.Actions;

namespace Rufini.Actions.Types
{
	// Token: 0x02000140 RID: 320
	internal class HopPlayerAction : RufiniAction
	{
		// Token: 0x06000737 RID: 1847 RVA: 0x0002EE10 File Offset: 0x0002D010
		public HopPlayerAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "h",
					Type = typeof(int)
				},
				new ActionParam
				{
					Name = "col",
					Type = typeof(bool)
				}
			};
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x0002EE80 File Offset: 0x0002D080
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			base.GetValue<string>("name");
			int value = base.GetValue<int>("h");
			base.GetValue<bool>("col");
			context.Player.HopFactor = (float)value;
			return default(ActionReturnContext);
		}
	}
}

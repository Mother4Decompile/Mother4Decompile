using System;
using System.Collections.Generic;
using Mother4.Scripts;
using Mother4.Scripts.Actions;
using Mother4.Scripts.Actions.ParamTypes;

namespace Rufini.Actions.Types
{
	// Token: 0x02000153 RID: 339
	internal class ScreenEffectAction : RufiniAction
	{
		// Token: 0x06000763 RID: 1891 RVA: 0x000303D8 File Offset: 0x0002E5D8
		public ScreenEffectAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "eff",
					Type = typeof(RufiniOption)
				}
			};
		}

		// Token: 0x06000764 RID: 1892 RVA: 0x00030420 File Offset: 0x0002E620
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			Console.WriteLine("NOT IMPLEMENTED - BUG DAVE");
			return default(ActionReturnContext);
		}
	}
}

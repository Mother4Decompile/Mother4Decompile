using System;
using System.Collections.Generic;
using Mother4.Scripts;
using Mother4.Scripts.Actions;
using Mother4.Scripts.Actions.ParamTypes;

namespace Rufini.Actions.Types
{
	// Token: 0x0200013D RID: 317
	internal class ExecutionModeAction : RufiniAction
	{
		// Token: 0x06000731 RID: 1841 RVA: 0x0002EA28 File Offset: 0x0002CC28
		public ExecutionModeAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "mode",
					Type = typeof(RufiniOption)
				}
			};
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x0002EA70 File Offset: 0x0002CC70
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			Console.WriteLine("NOT IMPLEMENTED - BUG DAVE");
			return default(ActionReturnContext);
		}
	}
}

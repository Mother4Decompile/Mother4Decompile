using System;
using System.Collections.Generic;
using Mother4.Scripts;
using Mother4.Scripts.Actions;
using Mother4.Scripts.Actions.ParamTypes;

namespace Rufini.Actions.Types
{
	// Token: 0x02000143 RID: 323
	internal class IfReturnAction : RufiniAction
	{
		// Token: 0x0600073D RID: 1853 RVA: 0x0002EFB0 File Offset: 0x0002D1B0
		public IfReturnAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "op",
					Type = typeof(RufiniOption)
				},
				new ActionParam
				{
					Name = "val",
					Type = typeof(int)
				}
			};
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x0002F020 File Offset: 0x0002D220
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			Console.WriteLine("NOT IMPLEMENTED - BUG DAVE");
			return default(ActionReturnContext);
		}
	}
}

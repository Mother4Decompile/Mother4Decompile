using System;
using System.Collections.Generic;
using Mother4.Scripts;
using Mother4.Scripts.Actions;
using Mother4.Scripts.Actions.ParamTypes;

namespace Rufini.Actions.Types
{
	// Token: 0x02000146 RID: 326
	internal class ItemAddAction : RufiniAction
	{
		// Token: 0x06000744 RID: 1860 RVA: 0x0002F2F4 File Offset: 0x0002D4F4
		public ItemAddAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "inv",
					Type = typeof(RufiniOption)
				},
				new ActionParam
				{
					Name = "item",
					Type = typeof(int)
				},
				new ActionParam
				{
					Name = "msg",
					Type = typeof(bool)
				},
				new ActionParam
				{
					Name = "sfx",
					Type = typeof(bool)
				}
			};
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x0002F3B8 File Offset: 0x0002D5B8
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			Console.WriteLine("NOT IMPLEMENTED - BUG DAVE");
			return default(ActionReturnContext);
		}
	}
}

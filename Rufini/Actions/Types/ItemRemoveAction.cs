using System;
using System.Collections.Generic;
using Mother4.Scripts;
using Mother4.Scripts.Actions;
using Mother4.Scripts.Actions.ParamTypes;

namespace Rufini.Actions.Types
{
	// Token: 0x02000147 RID: 327
	internal class ItemRemoveAction : RufiniAction
	{
		// Token: 0x06000746 RID: 1862 RVA: 0x0002F3D8 File Offset: 0x0002D5D8
		public ItemRemoveAction()
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

		// Token: 0x06000747 RID: 1863 RVA: 0x0002F49C File Offset: 0x0002D69C
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			Console.WriteLine("NOT IMPLEMENTED - BUG DAVE");
			return default(ActionReturnContext);
		}
	}
}

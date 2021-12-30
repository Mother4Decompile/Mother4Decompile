using System;
using System.Collections.Generic;
using Carbine.Flags;
using Mother4.Scripts;
using Mother4.Scripts.Actions;

namespace Rufini.Actions.Types
{
	// Token: 0x02000167 RID: 359
	internal class ValueSetAction : RufiniAction
	{
		// Token: 0x06000791 RID: 1937 RVA: 0x00031788 File Offset: 0x0002F988
		public ValueSetAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "id",
					Type = typeof(int)
				},
				new ActionParam
				{
					Name = "val",
					Type = typeof(int)
				}
			};
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x000317F8 File Offset: 0x0002F9F8
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			int value = base.GetValue<int>("id");
			int value2 = base.GetValue<int>("val");
			ValueManager.Instance[value] = value2;
			return default(ActionReturnContext);
		}
	}
}

using System;
using System.Collections.Generic;
using Carbine.Flags;
using Mother4.Scripts;
using Mother4.Scripts.Actions;

namespace Rufini.Actions.Types
{
	// Token: 0x02000165 RID: 357
	internal class ValueAddAction : RufiniAction
	{
		// Token: 0x0600078D RID: 1933 RVA: 0x000315D4 File Offset: 0x0002F7D4
		public ValueAddAction()
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

		// Token: 0x0600078E RID: 1934 RVA: 0x00031644 File Offset: 0x0002F844
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			int value = base.GetValue<int>("id");
			int value2 = base.GetValue<int>("val");
			ValueManager instance;
			int index;
			(instance = ValueManager.Instance)[index = value] = instance[index] + value2;
			return default(ActionReturnContext);
		}
	}
}

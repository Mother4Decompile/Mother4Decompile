using System;
using System.Collections.Generic;
using Carbine.Flags;
using Mother4.Scripts;
using Mother4.Scripts.Actions;
using Mother4.Scripts.Actions.ParamTypes;

namespace Rufini.Actions.Types
{
	// Token: 0x02000144 RID: 324
	internal class IfValueAction : RufiniAction
	{
		// Token: 0x0600073F RID: 1855 RVA: 0x0002F040 File Offset: 0x0002D240
		public IfValueAction()
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

		// Token: 0x06000740 RID: 1856 RVA: 0x0002F0D8 File Offset: 0x0002D2D8
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			int value = base.GetValue<int>("id");
			int value2 = base.GetValue<int>("op");
			int value3 = base.GetValue<int>("val");
			int num = ValueManager.Instance[value];
			bool[] array = new bool[]
			{
				value3 == num,
				value3 != num,
				value3 <= num,
				value3 >= num,
				value3 < num,
				value3 > num
			};
			if (!array[value2])
			{
				context.Executor.JumpToElseOrEndIf();
			}
			return default(ActionReturnContext);
		}
	}
}

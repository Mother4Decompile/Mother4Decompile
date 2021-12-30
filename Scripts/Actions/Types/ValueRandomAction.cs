using System;
using System.Collections.Generic;
using Carbine;
using Carbine.Flags;

namespace Mother4.Scripts.Actions.Types
{
	// Token: 0x02000166 RID: 358
	internal class ValueRandomAction : RufiniAction
	{
		// Token: 0x0600078F RID: 1935 RVA: 0x0003168C File Offset: 0x0002F88C
		public ValueRandomAction()
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
					Name = "min",
					Type = typeof(int)
				},
				new ActionParam
				{
					Name = "max",
					Type = typeof(int)
				}
			};
		}

		// Token: 0x06000790 RID: 1936 RVA: 0x00031724 File Offset: 0x0002F924
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			int value = base.GetValue<int>("id");
			int num = base.GetValue<int>("min");
			int num2 = base.GetValue<int>("max");
			if (num2 < num)
			{
				num = num2;
				num2 = num;
			}
			int maxValue = num2 - num;
			int value2 = num + Engine.Random.Next(maxValue);
			ValueManager.Instance[value] = value2;
			return default(ActionReturnContext);
		}
	}
}

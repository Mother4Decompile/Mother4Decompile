using System;
using System.Collections.Generic;

namespace Mother4.Scripts.Actions.Types
{
	// Token: 0x02000128 RID: 296
	internal class SetPlayerAnimationLoopCountAction : RufiniAction
	{
		// Token: 0x060006F9 RID: 1785 RVA: 0x0002C6F4 File Offset: 0x0002A8F4
		public SetPlayerAnimationLoopCountAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "lc",
					Type = typeof(int)
				}
			};
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x0002C73C File Offset: 0x0002A93C
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			int value = base.GetValue<int>("lc");
			if (context.Player != null)
			{
				context.Player.SetAnimationLoopCount(value);
			}
			return default(ActionReturnContext);
		}
	}
}

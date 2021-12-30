using System;
using System.Collections.Generic;
using Mother4.Actors.NPCs;

namespace Mother4.Scripts.Actions.Types
{
	// Token: 0x02000127 RID: 295
	internal class SetNPCAnimationLoopCountAction : RufiniAction
	{
		// Token: 0x060006F7 RID: 1783 RVA: 0x0002C644 File Offset: 0x0002A844
		public SetNPCAnimationLoopCountAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "name",
					Type = typeof(string)
				},
				new ActionParam
				{
					Name = "lc",
					Type = typeof(int)
				}
			};
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x0002C6B4 File Offset: 0x0002A8B4
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			string value = base.GetValue<string>("name");
			int value2 = base.GetValue<int>("lc");
			NPC npcByName = context.GetNpcByName(value);
			if (npcByName != null)
			{
				npcByName.SetAnimationLoopCount(value2);
			}
			return default(ActionReturnContext);
		}
	}
}

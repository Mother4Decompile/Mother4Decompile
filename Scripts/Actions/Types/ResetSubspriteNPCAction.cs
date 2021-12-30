using System;
using System.Collections.Generic;
using Carbine.Actors;
using Mother4.Actors.NPCs;

namespace Mother4.Scripts.Actions.Types
{
	// Token: 0x02000124 RID: 292
	internal class ResetSubspriteNPCAction : RufiniAction
	{
		// Token: 0x060006F1 RID: 1777 RVA: 0x0002C468 File Offset: 0x0002A668
		public ResetSubspriteNPCAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "name",
					Type = typeof(string)
				}
			};
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x0002C4DC File Offset: 0x0002A6DC
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			string name = base.GetValue<string>("name");
			NPC npc = (NPC)context.ActorManager.Find((Actor x) => x is NPC && ((NPC)x).Name == name);
			if (npc != null)
			{
				npc.ClearOverrideSubsprite();
			}
			return default(ActionReturnContext);
		}
	}
}

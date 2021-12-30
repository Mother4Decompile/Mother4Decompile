using System;
using System.Collections.Generic;
using Carbine.Actors;
using Mother4.Actors.NPCs;
using Mother4.Scripts;
using Mother4.Scripts.Actions;

namespace Rufini.Actions.Types
{
	// Token: 0x0200013F RID: 319
	internal class HopNPCAction : RufiniAction
	{
		// Token: 0x06000735 RID: 1845 RVA: 0x0002ECDC File Offset: 0x0002CEDC
		public HopNPCAction()
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
					Name = "h",
					Type = typeof(int)
				},
				new ActionParam
				{
					Name = "col",
					Type = typeof(bool)
				}
			};
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x0002EDA0 File Offset: 0x0002CFA0
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			string name = base.GetValue<string>("name");
			int value = base.GetValue<int>("h");
			base.GetValue<bool>("col");
			NPC npc = (NPC)context.ActorManager.Find((Actor n) => n is NPC && ((NPC)n).Name == name);
			if (npc != null)
			{
				npc.HopFactor = (float)value;
			}
			return default(ActionReturnContext);
		}
	}
}

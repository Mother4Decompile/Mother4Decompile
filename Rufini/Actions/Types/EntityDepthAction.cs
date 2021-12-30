using System;
using System.Collections.Generic;
using Carbine.Actors;
using Mother4.Actors.NPCs;
using Mother4.Scripts;
using Mother4.Scripts.Actions;

namespace Rufini.Actions.Types
{
	// Token: 0x02000139 RID: 313
	internal class EntityDepthAction : RufiniAction
	{
		// Token: 0x06000727 RID: 1831 RVA: 0x0002E0D4 File Offset: 0x0002C2D4
		public EntityDepthAction()
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
					Name = "dpt",
					Type = typeof(int)
				},
				new ActionParam
				{
					Name = "rel",
					Type = typeof(bool)
				},
				new ActionParam
				{
					Name = "rst",
					Type = typeof(bool)
				}
			};
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x0002E1C4 File Offset: 0x0002C3C4
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			ActionReturnContext result = default(ActionReturnContext);
			string name = base.GetValue<string>("name");
			int value = base.GetValue<int>("dpt");
			bool value2 = base.GetValue<bool>("rel");
			bool value3 = base.GetValue<bool>("rst");
			NPC npc = (NPC)context.ActorManager.Find((Actor n) => n is NPC && ((NPC)n).Name == name);
			if (npc != null)
			{
				if (!value3)
				{
					int newDepth = value2 ? (npc.Depth + value) : value;
					npc.ForceDepth(newDepth);
				}
				else
				{
					npc.ResetDepth();
				}
			}
			return result;
		}
	}
}

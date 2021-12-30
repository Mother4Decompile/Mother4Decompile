using System;
using System.Collections.Generic;
using Carbine.Actors;
using Mother4.Actors.NPCs;
using Mother4.Scripts;
using Mother4.Scripts.Actions;

namespace Rufini.Actions.Types
{
	// Token: 0x02000137 RID: 311
	internal class EntityDeleteAction : RufiniAction
	{
		// Token: 0x06000721 RID: 1825 RVA: 0x0002DD38 File Offset: 0x0002BF38
		public EntityDeleteAction()
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

		// Token: 0x06000722 RID: 1826 RVA: 0x0002DDAC File Offset: 0x0002BFAC
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			string name = base.GetValue<string>("name");
			NPC npc = (NPC)context.ActorManager.Find((Actor x) => x is NPC && ((NPC)x).Name == name);
			if (npc != null)
			{
				context.ActorManager.Remove(npc);
				context.CollisionManager.Remove(npc);
			}
			return default(ActionReturnContext);
		}
	}
}

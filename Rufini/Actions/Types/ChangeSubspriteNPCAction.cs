using System;
using System.Collections.Generic;
using Carbine.Actors;
using Mother4.Actors.NPCs;
using Mother4.Scripts;
using Mother4.Scripts.Actions;

namespace Rufini.Actions.Types
{
	// Token: 0x02000131 RID: 305
	internal class ChangeSubspriteNPCAction : RufiniAction
	{
		// Token: 0x06000710 RID: 1808 RVA: 0x0002D230 File Offset: 0x0002B430
		public ChangeSubspriteNPCAction()
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
					Name = "sub",
					Type = typeof(string)
				}
			};
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x0002D2CC File Offset: 0x0002B4CC
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			string name = base.GetValue<string>("name");
			string value = base.GetValue<string>("sub");
			NPC npc = (NPC)context.ActorManager.Find((Actor x) => x is NPC && ((NPC)x).Name == name);
			if (npc != null)
			{
				if (value.Length > 0)
				{
					npc.OverrideSubsprite(value);
				}
				else
				{
					npc.ClearOverrideSubsprite();
				}
			}
			return default(ActionReturnContext);
		}
	}
}

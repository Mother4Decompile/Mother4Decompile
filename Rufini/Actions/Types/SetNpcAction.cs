using System;
using System.Collections.Generic;
using Carbine.Actors;
using Mother4.Actors.NPCs;
using Mother4.Scripts;
using Mother4.Scripts.Actions;

namespace Rufini.Actions.Types
{
	// Token: 0x0200015B RID: 347
	internal class SetNpcAction : RufiniAction
	{
		// Token: 0x06000776 RID: 1910 RVA: 0x00030BB4 File Offset: 0x0002EDB4
		public SetNpcAction()
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
					Name = "talk",
					Type = typeof(bool)
				}
			};
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x00030C50 File Offset: 0x0002EE50
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			string name = base.GetValue<string>("name");
			base.GetValue<bool>("talk");
			context.ActiveNPC = null;
			if (name != null && name.Length > 0)
			{
				NPC npc = (NPC)context.ActorManager.Find((Actor n) => n is NPC && ((NPC)n).Name == name);
				if (npc != null)
				{
					context.ActiveNPC = npc;
				}
			}
			return default(ActionReturnContext);
		}
	}
}

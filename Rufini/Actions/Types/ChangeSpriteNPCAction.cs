using System;
using System.Collections.Generic;
using System.IO;
using Carbine.Actors;
using Mother4.Actors.NPCs;
using Mother4.Data;
using Mother4.Scripts;
using Mother4.Scripts.Actions;

namespace Rufini.Actions.Types
{
	// Token: 0x0200012F RID: 303
	internal class ChangeSpriteNPCAction : RufiniAction
	{
		// Token: 0x0600070C RID: 1804 RVA: 0x0002CFE8 File Offset: 0x0002B1E8
		public ChangeSpriteNPCAction()
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
					Name = "spr",
					Type = typeof(string)
				},
				new ActionParam
				{
					Name = "sub",
					Type = typeof(string)
				}
			};
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x0002D0AC File Offset: 0x0002B2AC
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			string name = base.GetValue<string>("name");
			string value = base.GetValue<string>("spr");
			string value2 = base.GetValue<string>("sub");
			NPC npc = (NPC)context.ActorManager.Find((Actor x) => x is NPC && ((NPC)x).Name == name);
			if (npc != null && value.Length > 0)
			{
				string text = Paths.GRAPHICS + value + ".dat";
				if (File.Exists(text))
				{
					npc.ChangeSprite(text, value2);
				}
				else
				{
					Console.WriteLine("Sprite file \"{0}\" does not exist.", text);
				}
			}
			return default(ActionReturnContext);
		}
	}
}

using System;
using System.Collections.Generic;
using Carbine.Actors;
using Carbine.Graphics;
using Mother4.Actors.NPCs;
using Mother4.Scripts;
using Mother4.Scripts.Actions;

namespace Rufini.Actions.Types
{
	// Token: 0x0200012D RID: 301
	internal class CameraNPCAction : RufiniAction
	{
		// Token: 0x06000706 RID: 1798 RVA: 0x0002CD38 File Offset: 0x0002AF38
		public CameraNPCAction()
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
					Name = "spd",
					Type = typeof(float)
				},
				new ActionParam
				{
					Name = "blk",
					Type = typeof(bool)
				}
			};
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x0002CDFC File Offset: 0x0002AFFC
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			ActionReturnContext result = default(ActionReturnContext);
			string name = base.GetValue<string>("name");
			float value = base.GetValue<float>("spd");
			bool value2 = base.GetValue<bool>("blk");
			if (!string.IsNullOrWhiteSpace(name))
			{
				NPC npc = (NPC)context.ActorManager.Find((Actor x) => x is NPC && ((NPC)x).Name == name);
				if (npc != null)
				{
					ViewManager.Instance.MoveTo(npc, value);
					if (value2)
					{
						this.context = context;
						ViewManager.Instance.OnMoveToComplete += this.OnMoveToComplete;
						result.Wait = ScriptExecutor.WaitType.Event;
					}
				}
			}
			else
			{
				ViewManager.Instance.FollowActor = null;
			}
			return result;
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x0002CEBE File Offset: 0x0002B0BE
		private void OnMoveToComplete(ViewManager sender)
		{
			ViewManager.Instance.OnMoveToComplete -= this.OnMoveToComplete;
			this.context.Executor.Continue();
		}

		// Token: 0x0400092C RID: 2348
		private ExecutionContext context;
	}
}

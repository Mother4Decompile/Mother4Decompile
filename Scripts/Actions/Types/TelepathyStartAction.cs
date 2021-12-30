using System;
using System.Collections.Generic;
using Carbine.Flags;
using Mother4.Actors;
using Mother4.GUI;

namespace Mother4.Scripts.Actions.Types
{
	// Token: 0x02000160 RID: 352
	internal class TelepathyStartAction : RufiniAction
	{
		// Token: 0x06000780 RID: 1920 RVA: 0x00031195 File Offset: 0x0002F395
		public TelepathyStartAction()
		{
			this.paramList = new List<ActionParam>();
		}

		// Token: 0x06000781 RID: 1921 RVA: 0x000311A8 File Offset: 0x0002F3A8
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			Console.WriteLine("It's telepathy time!");
			FlagManager.Instance[4] = true;
			this.context = context;
			if (this.context.Player != null)
			{
				this.context.Player.Telepathize();
				this.context.Player.OnTelepathyAnimationComplete += this.player_OnTelepathyAnimationComplete;
			}
			return new ActionReturnContext
			{
				Wait = ScriptExecutor.WaitType.Event
			};
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x0003121C File Offset: 0x0002F41C
		private void player_OnTelepathyAnimationComplete(Player player)
		{
			player.OnTelepathyAnimationComplete -= this.player_OnTelepathyAnimationComplete;
			if (this.context.CheckedNPC != null)
			{
				this.context.CheckedNPC.Telepathize();
				if (this.context.TextBox is OverworldTextBox)
				{
					((OverworldTextBox)this.context.TextBox).SetDimmer(0.5f);
				}
			}
			this.context.Executor.Continue();
			this.context = null;
		}

		// Token: 0x04000959 RID: 2393
		private ExecutionContext context;
	}
}

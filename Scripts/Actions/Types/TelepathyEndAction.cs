using System;
using System.Collections.Generic;
using Carbine.Flags;
using Mother4.GUI;

namespace Mother4.Scripts.Actions.Types
{
	// Token: 0x0200015F RID: 351
	internal class TelepathyEndAction : RufiniAction
	{
		// Token: 0x0600077E RID: 1918 RVA: 0x0003111F File Offset: 0x0002F31F
		public TelepathyEndAction()
		{
			this.paramList = new List<ActionParam>();
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x00031134 File Offset: 0x0002F334
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			Console.WriteLine("Telepathy time is over!");
			FlagManager.Instance[4] = false;
			if (context.CheckedNPC != null)
			{
				context.CheckedNPC.Untelepathize();
				if (context.TextBox is OverworldTextBox)
				{
					((OverworldTextBox)context.TextBox).SetDimmer(0f);
				}
			}
			return default(ActionReturnContext);
		}
	}
}

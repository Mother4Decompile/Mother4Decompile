using System;
using System.Collections.Generic;
using Mother4.GUI;
using Mother4.Scripts;
using Mother4.Scripts.Actions;
using Mother4.Scripts.Actions.ParamTypes;

namespace Rufini.Actions.Types
{
	// Token: 0x0200015A RID: 346
	internal class SetNametagAction : RufiniAction
	{
		// Token: 0x06000774 RID: 1908 RVA: 0x00030B10 File Offset: 0x0002ED10
		public SetNametagAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "text",
					Type = typeof(RufiniString)
				}
			};
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x00030B58 File Offset: 0x0002ED58
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			RufiniString value = base.GetValue<RufiniString>("text");
			if (context.TextBox is OverworldTextBox)
			{
				((OverworldTextBox)context.TextBox).Nametag = ((value.Names.Length > 0) ? value.Value : string.Empty);
			}
			return default(ActionReturnContext);
		}
	}
}

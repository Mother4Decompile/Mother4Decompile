using System;
using System.Collections.Generic;

namespace Mother4.Scripts.Actions.Types
{
	// Token: 0x02000164 RID: 356
	internal class ToggleTextboxAction : RufiniAction
	{
		// Token: 0x0600078B RID: 1931 RVA: 0x0003154C File Offset: 0x0002F74C
		public ToggleTextboxAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "text",
					Type = typeof(bool)
				}
			};
		}

		// Token: 0x0600078C RID: 1932 RVA: 0x00031594 File Offset: 0x0002F794
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			bool value = base.GetValue<bool>("text");
			if (value)
			{
				context.TextBox.Show();
			}
			else
			{
				context.TextBox.Hide();
			}
			return default(ActionReturnContext);
		}
	}
}

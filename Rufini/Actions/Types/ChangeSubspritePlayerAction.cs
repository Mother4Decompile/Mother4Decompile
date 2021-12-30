using System;
using System.Collections.Generic;
using Mother4.Scripts;
using Mother4.Scripts.Actions;

namespace Rufini.Actions.Types
{
	// Token: 0x02000132 RID: 306
	internal class ChangeSubspritePlayerAction : RufiniAction
	{
		// Token: 0x06000712 RID: 1810 RVA: 0x0002D340 File Offset: 0x0002B540
		public ChangeSubspritePlayerAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "sub",
					Type = typeof(string)
				}
			};
		}

		// Token: 0x06000713 RID: 1811 RVA: 0x0002D388 File Offset: 0x0002B588
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			string value = base.GetValue<string>("sub");
			if (value.Length > 0)
			{
				context.Player.OverrideSubsprite(value);
			}
			else
			{
				context.Player.ClearOverrideSubsprite();
			}
			return default(ActionReturnContext);
		}
	}
}

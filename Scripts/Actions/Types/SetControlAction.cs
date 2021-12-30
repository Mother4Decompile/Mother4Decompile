using System;
using System.Collections.Generic;
using Carbine.Input;
using Mother4.Scripts.Actions.ParamTypes;

namespace Mother4.Scripts.Actions.Types
{
	// Token: 0x02000126 RID: 294
	internal class SetControlAction : RufiniAction
	{
		// Token: 0x060006F5 RID: 1781 RVA: 0x0002C55C File Offset: 0x0002A75C
		public SetControlAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "mode",
					Type = typeof(RufiniOption)
				}
			};
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x0002C5A4 File Offset: 0x0002A7A4
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			switch (base.GetValue<RufiniOption>("mode").Option)
			{
			case 1:
				if (context.Player != null)
				{
					context.Player.InputLocked = true;
				}
				InputManager.Instance.Enabled = true;
				break;
			case 2:
				if (context.Player != null)
				{
					context.Player.InputLocked = false;
				}
				InputManager.Instance.Enabled = false;
				break;
			default:
				if (context.Player != null)
				{
					context.Player.InputLocked = false;
				}
				InputManager.Instance.Enabled = false;
				break;
			}
			return default(ActionReturnContext);
		}

		// Token: 0x0400091D RID: 2333
		private const int DIALOGUE_CONTROL = 1;

		// Token: 0x0400091E RID: 2334
		private const int NO_CONTROL = 2;
	}
}

using System;

namespace Mother4.Scripts.Actions.Types
{
	// Token: 0x02000125 RID: 293
	internal class ResetSubspritePlayerAction : RufiniAction
	{
		// Token: 0x060006F3 RID: 1779 RVA: 0x0002C530 File Offset: 0x0002A730
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			context.Player.ClearOverrideSubsprite();
			return default(ActionReturnContext);
		}
	}
}

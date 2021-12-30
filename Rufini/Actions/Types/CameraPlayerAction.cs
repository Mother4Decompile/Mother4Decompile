using System;
using System.Collections.Generic;
using Carbine.Graphics;
using Mother4.Scripts;
using Mother4.Scripts.Actions;

namespace Rufini.Actions.Types
{
	// Token: 0x0200012E RID: 302
	internal class CameraPlayerAction : RufiniAction
	{
		// Token: 0x06000709 RID: 1801 RVA: 0x0002CEE8 File Offset: 0x0002B0E8
		public CameraPlayerAction()
		{
			this.paramList = new List<ActionParam>
			{
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

		// Token: 0x0600070A RID: 1802 RVA: 0x0002CF58 File Offset: 0x0002B158
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			ActionReturnContext result = default(ActionReturnContext);
			float value = base.GetValue<float>("spd");
			bool value2 = base.GetValue<bool>("blk");
			ViewManager.Instance.MoveTo(context.Player, value);
			if (value2)
			{
				this.context = context;
				ViewManager.Instance.OnMoveToComplete += this.OnMoveToComplete;
				result.Wait = ScriptExecutor.WaitType.Event;
			}
			return result;
		}

		// Token: 0x0600070B RID: 1803 RVA: 0x0002CFBF File Offset: 0x0002B1BF
		private void OnMoveToComplete(ViewManager sender)
		{
			ViewManager.Instance.OnMoveToComplete -= this.OnMoveToComplete;
			this.context.Executor.Continue();
		}

		// Token: 0x0400092D RID: 2349
		private ExecutionContext context;
	}
}

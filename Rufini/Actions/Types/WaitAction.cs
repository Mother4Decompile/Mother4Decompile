using System;
using System.Collections.Generic;
using Carbine.Utility;
using Mother4.Scripts;
using Mother4.Scripts.Actions;

namespace Rufini.Actions.Types
{
	// Token: 0x02000168 RID: 360
	internal class WaitAction : RufiniAction
	{
		// Token: 0x06000793 RID: 1939 RVA: 0x00031834 File Offset: 0x0002FA34
		public WaitAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "wait",
					Type = typeof(int)
				}
			};
		}

		// Token: 0x06000794 RID: 1940 RVA: 0x0003187C File Offset: 0x0002FA7C
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			this.context = context;
			int value = base.GetValue<int>("wait");
			this.timerIndex = TimerManager.Instance.StartTimer(value);
			TimerManager.Instance.OnTimerEnd += this.TimerEnd;
			return new ActionReturnContext
			{
				Wait = ScriptExecutor.WaitType.Event
			};
		}

		// Token: 0x06000795 RID: 1941 RVA: 0x000318D4 File Offset: 0x0002FAD4
		private void TimerEnd(int timerIndex)
		{
			if (this.timerIndex == timerIndex)
			{
				TimerManager.Instance.OnTimerEnd -= this.TimerEnd;
				this.context.Executor.Continue();
			}
		}

		// Token: 0x0400095C RID: 2396
		private ExecutionContext context;

		// Token: 0x0400095D RID: 2397
		private int timerIndex;
	}
}

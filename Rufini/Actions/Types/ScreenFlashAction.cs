using System;
using System.Collections.Generic;
using Carbine.Scenes;
using Carbine.Utility;
using Mother4.Scenes;
using Mother4.Scripts;
using Mother4.Scripts.Actions;
using Mother4.Utility;
using SFML.Graphics;

namespace Rufini.Actions.Types
{
	// Token: 0x02000155 RID: 341
	internal class ScreenFlashAction : RufiniAction
	{
		// Token: 0x06000768 RID: 1896 RVA: 0x00030598 File Offset: 0x0002E798
		public ScreenFlashAction()
		{
			this.timerId = -1;
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "col",
					Type = typeof(Color)
				},
				new ActionParam
				{
					Name = "tdur",
					Type = typeof(int)
				},
				new ActionParam
				{
					Name = "hdur",
					Type = typeof(int)
				},
				new ActionParam
				{
					Name = "blk",
					Type = typeof(bool)
				}
			};
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x00030664 File Offset: 0x0002E864
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			ActionReturnContext result = default(ActionReturnContext);
			Color value = base.GetValue<Color>("col");
			this.transDuration = base.GetValue<int>("tdur");
			this.holdDuration = base.GetValue<int>("hdur");
			this.blocking = base.GetValue<bool>("blk");
			Scene scene = SceneManager.Instance.Peek();
			if (scene is OverworldScene)
			{
				this.dimmer = ((OverworldScene)scene).Dimmer;
				this.originalFadeColor = this.dimmer.TargetColor;
				this.dimmer.ChangeColor(value, this.transDuration);
				if (this.blocking)
				{
					this.dimmer.OnFadeComplete += this.OnFadeComplete;
					result = new ActionReturnContext
					{
						Wait = ScriptExecutor.WaitType.Event
					};
					this.context = context;
				}
			}
			return result;
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x00030738 File Offset: 0x0002E938
		private void OnFadeComplete(ScreenDimmer sender)
		{
			if (this.timerId == -1)
			{
				this.timerId = TimerManager.Instance.StartTimer(this.holdDuration);
				TimerManager.Instance.OnTimerEnd += this.OnTimerEnd;
				return;
			}
			this.dimmer.OnFadeComplete -= this.OnFadeComplete;
			TimerManager.Instance.OnTimerEnd -= this.OnTimerEnd;
			this.context.Executor.Continue();
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x000307B8 File Offset: 0x0002E9B8
		private void OnTimerEnd(int timerIndex)
		{
			if (this.timerId == timerIndex)
			{
				this.dimmer.ChangeColor(this.originalFadeColor, this.transDuration);
			}
		}

		// Token: 0x0400094E RID: 2382
		private ExecutionContext context;

		// Token: 0x0400094F RID: 2383
		private Color originalFadeColor;

		// Token: 0x04000950 RID: 2384
		private int transDuration;

		// Token: 0x04000951 RID: 2385
		private int holdDuration;

		// Token: 0x04000952 RID: 2386
		private bool blocking;

		// Token: 0x04000953 RID: 2387
		private int timerId;

		// Token: 0x04000954 RID: 2388
		private ScreenDimmer dimmer;
	}
}

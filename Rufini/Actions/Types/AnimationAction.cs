using System;
using System.Collections.Generic;
using Carbine.Graphics;
using Carbine.Utility;
using Mother4.Data;
using Mother4.Scripts;
using Mother4.Scripts.Actions;
using SFML.System;

namespace Rufini.Actions.Types
{
	// Token: 0x02000122 RID: 290
	internal class AnimationAction : RufiniAction
	{
		// Token: 0x060006EA RID: 1770 RVA: 0x0002C160 File Offset: 0x0002A360
		public AnimationAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "spr",
					Type = typeof(string)
				},
				new ActionParam
				{
					Name = "sub",
					Type = typeof(string)
				},
				new ActionParam
				{
					Name = "x",
					Type = typeof(int)
				},
				new ActionParam
				{
					Name = "y",
					Type = typeof(int)
				},
				new ActionParam
				{
					Name = "depth",
					Type = typeof(int)
				},
				new ActionParam
				{
					Name = "blk",
					Type = typeof(bool)
				}
			};
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x0002C27C File Offset: 0x0002A47C
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			ActionReturnContext result = default(ActionReturnContext);
			this.context = context;
			string value = base.GetValue<string>("spr");
			string value2 = base.GetValue<string>("sub");
			int value3 = base.GetValue<int>("x");
			int value4 = base.GetValue<int>("y");
			int value5 = base.GetValue<int>("depth");
			this.blocking = base.GetValue<bool>("blk");
			this.graphic = new IndexedColorGraphic(Paths.GRAPHICS + value + ".dat", value2, new Vector2f((float)value3, (float)value4), (value5 < 0) ? value4 : value5);
			this.graphic.OnAnimationComplete += this.OnAnimationComplete;
			this.context.Pipeline.Add(this.graphic);
			if (this.blocking)
			{
				result.Wait = ScriptExecutor.WaitType.Event;
			}
			return result;
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x0002C358 File Offset: 0x0002A558
		private void OnAnimationComplete(AnimatedRenderable graphic)
		{
			this.context.Pipeline.Remove(graphic);
			this.graphic.OnAnimationComplete -= this.OnAnimationComplete;
			this.timerId = TimerManager.Instance.StartTimer(1);
			TimerManager.Instance.OnTimerEnd += this.OnTimerEnd;
			if (this.blocking)
			{
				this.context.Executor.Continue();
			}
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x0002C3CC File Offset: 0x0002A5CC
		private void OnTimerEnd(int timerIndex)
		{
			if (this.timerId == timerIndex)
			{
				this.graphic.Dispose();
				TimerManager.Instance.OnTimerEnd -= this.OnTimerEnd;
			}
		}

		// Token: 0x04000918 RID: 2328
		private Graphic graphic;

		// Token: 0x04000919 RID: 2329
		private ExecutionContext context;

		// Token: 0x0400091A RID: 2330
		private bool blocking;

		// Token: 0x0400091B RID: 2331
		private int timerId;
	}
}

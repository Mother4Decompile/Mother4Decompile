using System;
using System.Collections.Generic;
using Carbine.Utility;
using Mother4.Scripts.Actions.ParamTypes;

namespace Mother4.Scripts.Actions.Types
{
	// Token: 0x02000129 RID: 297
	internal class SetPlayerDirectionAction : RufiniAction
	{
		// Token: 0x060006FB RID: 1787 RVA: 0x0002C774 File Offset: 0x0002A974
		public SetPlayerDirectionAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "dir",
					Type = typeof(RufiniOption)
				},
				new ActionParam
				{
					Name = "spd",
					Type = typeof(float)
				}
			};
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x0002C7E4 File Offset: 0x0002A9E4
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			ActionReturnContext result = default(ActionReturnContext);
			this.context = context;
			this.directionTo = base.GetValue<RufiniOption>("dir").Option;
			this.speed = base.GetValue<float>("spd");
			if (context.Player != null)
			{
				context.Player.MovementLocked = true;
				int num = Math.Abs(this.directionTo - context.Player.Direction);
				int num2 = Math.Abs(this.directionTo + 8 - context.Player.Direction) % 8;
				this.increment = ((num < num2) ? -1 : 1);
				if (this.speed > 0f)
				{
					this.TurnStep(true);
					TimerManager.Instance.OnTimerEnd += this.OnTimer;
					result.Wait = ScriptExecutor.WaitType.Event;
				}
			}
			return result;
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x0002C8B4 File Offset: 0x0002AAB4
		private void TurnStep(bool isFirst)
		{
			int num = this.context.Player.Direction;
			if (!isFirst)
			{
				num += this.increment;
				if (num < 0)
				{
					num = 7;
				}
				if (num > 7)
				{
					num = 0;
				}
			}
			if (num != this.directionTo)
			{
				this.timerId = TimerManager.Instance.StartTimer((int)(1f / this.speed));
				return;
			}
			TimerManager.Instance.OnTimerEnd -= this.OnTimer;
			this.context.Player.MovementLocked = false;
			this.context.Executor.Continue();
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x0002C948 File Offset: 0x0002AB48
		private void OnTimer(int timerIndex)
		{
			if (this.timerId == timerIndex)
			{
				this.TurnStep(false);
			}
		}

		// Token: 0x0400091F RID: 2335
		private ExecutionContext context;

		// Token: 0x04000920 RID: 2336
		private int timerId;

		// Token: 0x04000921 RID: 2337
		private int directionTo;

		// Token: 0x04000922 RID: 2338
		private int increment;

		// Token: 0x04000923 RID: 2339
		private float speed;
	}
}

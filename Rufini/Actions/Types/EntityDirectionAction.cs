using System;
using System.Collections.Generic;
using Carbine.Actors;
using Carbine.Utility;
using Mother4.Actors.NPCs;
using Mother4.Scripts;
using Mother4.Scripts.Actions;
using Mother4.Scripts.Actions.ParamTypes;
using SFML.System;

namespace Rufini.Actions.Types
{
	// Token: 0x02000138 RID: 312
	internal class EntityDirectionAction : RufiniAction
	{
		// Token: 0x06000723 RID: 1827 RVA: 0x0002DE14 File Offset: 0x0002C014
		public EntityDirectionAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "name",
					Type = typeof(string)
				},
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

		// Token: 0x06000724 RID: 1828 RVA: 0x0002DED8 File Offset: 0x0002C0D8
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			ActionReturnContext result = default(ActionReturnContext);
			this.context = context;
			string name = base.GetValue<string>("name");
			this.directionTo = base.GetValue<RufiniOption>("dir").Option;
			this.speed = base.GetValue<float>("spd");
			this.npc = (NPC)context.ActorManager.Find((Actor n) => n is NPC && ((NPC)n).Name == name);
			if (this.npc != null)
			{
				this.npc.MovementLocked = true;
				if (this.directionTo >= 8)
				{
					Vector2f v = VectorMath.Normalize(context.Player.Position - this.npc.Position);
					this.directionTo = VectorMath.VectorToDirection(v);
				}
				int num = Math.Abs(this.directionTo - this.npc.Direction);
				int num2 = Math.Abs(this.directionTo + 8 - this.npc.Direction) % 8;
				this.increment = ((num < num2) ? -1 : 1);
				if (this.speed > 0f)
				{
					this.TurnStep(true);
					TimerManager.Instance.OnTimerEnd += this.OnTimer;
					result.Wait = ScriptExecutor.WaitType.Event;
				}
				else
				{
					this.npc.Direction = this.directionTo;
				}
			}
			return result;
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x0002E02C File Offset: 0x0002C22C
		private void TurnStep(bool isFirst)
		{
			int num = this.npc.Direction;
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
				this.npc.Direction = num;
			}
			if (num != this.directionTo)
			{
				this.timerId = TimerManager.Instance.StartTimer((int)(1f / this.speed));
				return;
			}
			TimerManager.Instance.OnTimerEnd -= this.OnTimer;
			this.npc.MovementLocked = false;
			this.context.Executor.Continue();
		}

		// Token: 0x06000726 RID: 1830 RVA: 0x0002E0C2 File Offset: 0x0002C2C2
		private void OnTimer(int timerIndex)
		{
			if (this.timerId == timerIndex)
			{
				this.TurnStep(false);
			}
		}

		// Token: 0x04000936 RID: 2358
		private ExecutionContext context;

		// Token: 0x04000937 RID: 2359
		private int timerId;

		// Token: 0x04000938 RID: 2360
		private int directionTo;

		// Token: 0x04000939 RID: 2361
		private int increment;

		// Token: 0x0400093A RID: 2362
		private float speed;

		// Token: 0x0400093B RID: 2363
		private NPC npc;
	}
}

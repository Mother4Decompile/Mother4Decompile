using System;
using System.Collections.Generic;
using Carbine.Graphics;
using Mother4.Scripts;
using Mother4.Scripts.Actions;
using Mother4.Scripts.Actions.ParamTypes;

namespace Rufini.Actions.Types
{
	// Token: 0x0200012C RID: 300
	internal class CameraMoveAction : RufiniAction
	{
		// Token: 0x06000703 RID: 1795 RVA: 0x0002CB48 File Offset: 0x0002AD48
		public CameraMoveAction()
		{
			this.paramList = new List<ActionParam>
			{
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
					Name = "spd",
					Type = typeof(float)
				},
				new ActionParam
				{
					Name = "mod",
					Type = typeof(RufiniOption)
				},
				new ActionParam
				{
					Name = "blk",
					Type = typeof(bool)
				}
			};
		}

		// Token: 0x06000704 RID: 1796 RVA: 0x0002CC38 File Offset: 0x0002AE38
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			ActionReturnContext result = default(ActionReturnContext);
			int value = base.GetValue<int>("x");
			int value2 = base.GetValue<int>("y");
			float value3 = base.GetValue<float>("spd");
			int option = base.GetValue<RufiniOption>("mod").Option;
			bool value4 = base.GetValue<bool>("blk");
			ViewManager.MoveMode moveToMode;
			switch (option)
			{
			case 1:
				moveToMode = ViewManager.MoveMode.Smoothed;
				break;
			case 2:
				moveToMode = ViewManager.MoveMode.ExpIn;
				break;
			case 3:
				moveToMode = ViewManager.MoveMode.ExpOut;
				break;
			default:
				moveToMode = ViewManager.MoveMode.Linear;
				break;
			}
			ViewManager.Instance.MoveToMode = moveToMode;
			ViewManager.Instance.FollowActor = null;
			ViewManager.Instance.MoveTo((float)value, (float)value2, value3);
			if (value4)
			{
				this.context = context;
				ViewManager.Instance.OnMoveToComplete += this.OnMoveToComplete;
				result.Wait = ScriptExecutor.WaitType.Event;
			}
			return result;
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x0002CD0D File Offset: 0x0002AF0D
		private void OnMoveToComplete(ViewManager sender)
		{
			ViewManager.Instance.OnMoveToComplete -= this.OnMoveToComplete;
			this.context.Executor.Continue();
		}

		// Token: 0x0400092B RID: 2347
		private ExecutionContext context;
	}
}

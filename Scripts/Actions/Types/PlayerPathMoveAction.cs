using System;
using System.Collections.Generic;
using System.Linq;
using Carbine.Maps;
using Mother4.Actors.NPCs.Movement;

namespace Mother4.Scripts.Actions.Types
{
	// Token: 0x0200014C RID: 332
	internal class PlayerPathMoveAction : RufiniAction
	{
		// Token: 0x06000751 RID: 1873 RVA: 0x0002FA68 File Offset: 0x0002DC68
		public PlayerPathMoveAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "cnstr",
					Type = typeof(string)
				},
				new ActionParam
				{
					Name = "spd",
					Type = typeof(int)
				},
				new ActionParam
				{
					Name = "snp",
					Type = typeof(bool)
				},
				new ActionParam
				{
					Name = "ext",
					Type = typeof(bool)
				},
				new ActionParam
				{
					Name = "blk",
					Type = typeof(bool)
				}
			};
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x0002FB74 File Offset: 0x0002DD74
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			this.context = context;
			string pathName = base.GetValue<string>("cnstr");
			int value = base.GetValue<int>("spd");
			bool value2 = base.GetValue<bool>("snp");
			this.blocking = base.GetValue<bool>("blk");
			bool value3 = base.GetValue<bool>("ext");
			Map.Path path = this.context.Paths.FirstOrDefault((Map.Path x) => x.Name == pathName);
			ActionReturnContext result;
			if (path.Points != null && path.Points.Count > 0)
			{
				if (value2)
				{
					this.context.Player.SetPosition(path.Points[0], value3);
				}
				this.mover = new PathMover((float)value, 0, false, path.Points);
				this.mover.OnPathComplete += this.PathComplete;
				this.context.Player.SetMover(this.mover);
				result = new ActionReturnContext
				{
					Wait = (this.blocking ? ScriptExecutor.WaitType.Event : ScriptExecutor.WaitType.Frame)
				};
			}
			else
			{
				Console.WriteLine("No path with name \"{0}\" exists.", pathName);
				result = default(ActionReturnContext);
			}
			return result;
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x0002FCB4 File Offset: 0x0002DEB4
		private void PathComplete()
		{
			this.mover.OnPathComplete -= this.PathComplete;
			this.context.Player.ClearMover();
			if (this.blocking)
			{
				this.context.Executor.Continue();
			}
		}

		// Token: 0x04000946 RID: 2374
		private ExecutionContext context;

		// Token: 0x04000947 RID: 2375
		private PathMover mover;

		// Token: 0x04000948 RID: 2376
		private bool blocking;
	}
}

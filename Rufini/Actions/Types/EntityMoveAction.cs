using System;
using System.Collections.Generic;
using Carbine.Actors;
using Mother4.Actors.NPCs;
using Mother4.Actors.NPCs.Movement;
using Mother4.Scripts;
using Mother4.Scripts.Actions;
using SFML.System;

namespace Rufini.Actions.Types
{
	// Token: 0x0200013B RID: 315
	internal class EntityMoveAction : RufiniAction
	{
		// Token: 0x0600072C RID: 1836 RVA: 0x0002E574 File Offset: 0x0002C774
		public EntityMoveAction()
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
					Name = "rel",
					Type = typeof(bool)
				},
				new ActionParam
				{
					Name = "spd",
					Type = typeof(int)
				},
				new ActionParam
				{
					Name = "sub",
					Type = typeof(string)
				},
				new ActionParam
				{
					Name = "blk",
					Type = typeof(bool)
				}
			};
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x0002E6E8 File Offset: 0x0002C8E8
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			ActionReturnContext result = default(ActionReturnContext);
			string name = base.GetValue<string>("name");
			int value = base.GetValue<int>("x");
			int value2 = base.GetValue<int>("y");
			bool value3 = base.GetValue<bool>("rel");
			int value4 = base.GetValue<int>("spd");
			string value5 = base.GetValue<string>("sub");
			this.blocking = base.GetValue<bool>("blk");
			NPC npc = (NPC)context.ActorManager.Find((Actor n) => n is NPC && ((NPC)n).Name == name);
			if (npc != null)
			{
				Vector2f vector2f = new Vector2f((float)value, (float)value2);
				if (value3)
				{
					vector2f += npc.Position;
				}
				if (value4 > 0)
				{
					PointMover pointMover = new PointMover(vector2f, (float)value4);
					pointMover.OnMoveComplete += this.OnMoveComplete;
					npc.SetMover(pointMover);
					this.context = context;
					if (this.blocking)
					{
						result.Wait = ScriptExecutor.WaitType.Event;
					}
				}
				else
				{
					npc.Position = vector2f;
				}
				if (value5.Length > 0)
				{
					npc.OverrideSubsprite(value5);
				}
				else
				{
					npc.ClearOverrideSubsprite();
				}
			}
			else
			{
				Console.WriteLine("Count not find NPC named \"{0}\"", name);
			}
			return result;
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x0002E828 File Offset: 0x0002CA28
		private void OnMoveComplete(PointMover sender)
		{
			sender.OnMoveComplete -= this.OnMoveComplete;
			if (this.blocking)
			{
				this.context.Executor.Continue();
			}
		}

		// Token: 0x04000940 RID: 2368
		private ExecutionContext context;

		// Token: 0x04000941 RID: 2369
		private bool blocking;
	}
}

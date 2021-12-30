using System;
using System.Collections.Generic;
using Mother4.Scripts.Actions.ParamTypes;
using SFML.System;

namespace Mother4.Scripts.Actions.Types
{
	// Token: 0x0200014D RID: 333
	internal class PlayerPositionAction : RufiniAction
	{
		// Token: 0x06000754 RID: 1876 RVA: 0x0002FD00 File Offset: 0x0002DF00
		public PlayerPositionAction()
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
					Name = "dir",
					Type = typeof(RufiniOption)
				}
			};
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x0002FD98 File Offset: 0x0002DF98
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			ActionReturnContext result = default(ActionReturnContext);
			int value = base.GetValue<int>("x");
			int value2 = base.GetValue<int>("y");
			int option = base.GetValue<RufiniOption>("dir").Option;
			context.Player.SetPosition(new Vector2f((float)value, (float)value2));
			context.Player.SetDirection(option);
			return result;
		}
	}
}

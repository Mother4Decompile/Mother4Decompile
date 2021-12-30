using System;
using System.Collections.Generic;
using Mother4.Scripts;
using Mother4.Scripts.Actions;

namespace Rufini.Actions.Types
{
	// Token: 0x02000149 RID: 329
	internal class MapMarkSetAction : RufiniAction
	{
		// Token: 0x0600074A RID: 1866 RVA: 0x0002F524 File Offset: 0x0002D724
		public MapMarkSetAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "map",
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
				}
			};
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x0002F5BC File Offset: 0x0002D7BC
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			Console.WriteLine("NOT IMPLEMENTED - BUG DAVE");
			return default(ActionReturnContext);
		}
	}
}

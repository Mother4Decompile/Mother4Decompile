using System;
using System.Collections.Generic;
using Mother4.Scripts;
using Mother4.Scripts.Actions;

namespace Rufini.Actions.Types
{
	// Token: 0x02000148 RID: 328
	internal class MapMarkClearAction : RufiniAction
	{
		// Token: 0x06000748 RID: 1864 RVA: 0x0002F4BC File Offset: 0x0002D6BC
		public MapMarkClearAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "map",
					Type = typeof(string)
				}
			};
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x0002F504 File Offset: 0x0002D704
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			Console.WriteLine("NOT IMPLEMENTED - BUG DAVE");
			return default(ActionReturnContext);
		}
	}
}

using System;
using System.Collections.Generic;
using Mother4.Scripts;
using Mother4.Scripts.Actions;
using Mother4.Scripts.Actions.ParamTypes;

namespace Rufini.Actions.Types
{
	// Token: 0x02000169 RID: 361
	internal class WeatherAction : RufiniAction
	{
		// Token: 0x06000796 RID: 1942 RVA: 0x00031908 File Offset: 0x0002FB08
		public WeatherAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "eff",
					Type = typeof(RufiniOption)
				}
			};
		}

		// Token: 0x06000797 RID: 1943 RVA: 0x00031950 File Offset: 0x0002FB50
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			Console.WriteLine("NOT IMPLEMENTED - BUG DAVE");
			return default(ActionReturnContext);
		}
	}
}

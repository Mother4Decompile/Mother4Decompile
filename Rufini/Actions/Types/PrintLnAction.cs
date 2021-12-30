using System;
using System.Collections.Generic;
using Mother4.Scripts;
using Mother4.Scripts.Actions;

namespace Rufini.Actions.Types
{
	// Token: 0x02000150 RID: 336
	internal class PrintLnAction : RufiniAction
	{
		// Token: 0x0600075B RID: 1883 RVA: 0x0002FF84 File Offset: 0x0002E184
		public PrintLnAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "text",
					Type = typeof(string)
				}
			};
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x0002FFCC File Offset: 0x0002E1CC
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			string value = base.GetValue<string>("text");
			Console.WriteLine(value);
			return default(ActionReturnContext);
		}
	}
}

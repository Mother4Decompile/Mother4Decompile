using System;
using System.Collections.Generic;
using Mother4.Scripts;
using Mother4.Scripts.Actions;

namespace Rufini.Actions.Types
{
	// Token: 0x0200012B RID: 299
	internal class CallAction : RufiniAction
	{
		// Token: 0x06000701 RID: 1793 RVA: 0x0002CABC File Offset: 0x0002ACBC
		public CallAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "scr",
					Type = typeof(string)
				}
			};
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x0002CB04 File Offset: 0x0002AD04
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			string value = base.GetValue<string>("scr");
			Script? script = ScriptLoader.Load(value);
			if (script != null)
			{
				context.Executor.PushScript(script.Value);
			}
			return default(ActionReturnContext);
		}
	}
}

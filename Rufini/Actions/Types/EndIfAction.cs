using System;
using System.Collections.Generic;
using Mother4.Scripts;
using Mother4.Scripts.Actions;

namespace Rufini.Actions.Types
{
	// Token: 0x02000141 RID: 321
	internal class EndIfAction : RufiniAction
	{
		// Token: 0x06000739 RID: 1849 RVA: 0x0002EEC7 File Offset: 0x0002D0C7
		public EndIfAction()
		{
			this.paramList = new List<ActionParam>();
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x0002EEDC File Offset: 0x0002D0DC
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			return default(ActionReturnContext);
		}
	}
}

using System;
using System.Collections.Generic;
using Carbine.Flags;
using Mother4.Scripts;
using Mother4.Scripts.Actions;

namespace Rufini.Actions.Types
{
	// Token: 0x02000158 RID: 344
	internal class SetFlagAction : RufiniAction
	{
		// Token: 0x06000770 RID: 1904 RVA: 0x000309EC File Offset: 0x0002EBEC
		public SetFlagAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "flg",
					Type = typeof(int)
				},
				new ActionParam
				{
					Name = "val",
					Type = typeof(bool)
				}
			};
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x00030A5C File Offset: 0x0002EC5C
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			int value = base.GetValue<int>("flg");
			bool value2 = base.GetValue<bool>("val");
			FlagManager.Instance[value] = value2;
			return default(ActionReturnContext);
		}
	}
}

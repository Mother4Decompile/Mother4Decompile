using System;
using System.Collections.Generic;
using Mother4.Battle;
using Mother4.Data;
using Mother4.Scripts;
using Mother4.Scripts.Actions;
using Mother4.Scripts.Actions.ParamTypes;

namespace Rufini.Actions.Types
{
	// Token: 0x0200011F RID: 287
	internal class AddExpAction : RufiniAction
	{
		// Token: 0x060006E3 RID: 1763 RVA: 0x0002BCA4 File Offset: 0x00029EA4
		public AddExpAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "char",
					Type = typeof(RufiniOption)
				},
				new ActionParam
				{
					Name = "val",
					Type = typeof(int)
				},
				new ActionParam
				{
					Name = "msg",
					Type = typeof(bool)
				},
				new ActionParam
				{
					Name = "sup",
					Type = typeof(bool)
				}
			};
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x0002BD68 File Offset: 0x00029F68
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			CharacterType byOptionInt = CharacterType.GetByOptionInt(base.GetValue<RufiniOption>("char").Option);
			int value = base.GetValue<int>("val");
			base.GetValue<bool>("msg");
			base.GetValue<bool>("sup");
			StatSet stats = CharacterStats.GetStats(byOptionInt);
			stats.Experience += value;
			CharacterStats.SetStats(byOptionInt, stats);
			Console.WriteLine("SORT OF IMPLEMENTED - BUG DAVE");
			return default(ActionReturnContext);
		}
	}
}

using System;
using System.Collections.Generic;
using Mother4.Battle;
using Mother4.Data;
using Mother4.Scripts;
using Mother4.Scripts.Actions;
using Mother4.Scripts.Actions.ParamTypes;

namespace Rufini.Actions.Types
{
	// Token: 0x0200014A RID: 330
	internal class StatModifyAction : RufiniAction
	{
		// Token: 0x0600074C RID: 1868 RVA: 0x0002F5DC File Offset: 0x0002D7DC
		public StatModifyAction()
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
					Name = "stat",
					Type = typeof(RufiniOption)
				},
				new ActionParam
				{
					Name = "val",
					Type = typeof(int)
				}
			};
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x0002F674 File Offset: 0x0002D874
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			CharacterType byOptionInt = CharacterType.GetByOptionInt(base.GetValue<RufiniOption>("char").Option);
			int option = base.GetValue<RufiniOption>("stat").Option;
			int value = base.GetValue<int>("val");
			StatSet stats = CharacterStats.GetStats(byOptionInt);
			switch (option)
			{
			case 0:
				stats.HP += value;
				break;
			case 1:
				stats.PP += value;
				break;
			case 2:
				stats.MaxHP += value;
				break;
			case 3:
				stats.MaxPP += value;
				break;
			case 4:
				stats.Offense += value;
				break;
			case 5:
				stats.Defense += value;
				break;
			case 6:
				stats.Speed += value;
				break;
			case 7:
				stats.Guts += value;
				break;
			case 8:
				stats.IQ += value;
				break;
			case 9:
				stats.Luck += value;
				break;
			case 10:
				stats.Meter += (float)value;
				break;
			case 11:
				stats.Level += value;
				break;
			}
			CharacterStats.SetStats(byOptionInt, stats);
			return default(ActionReturnContext);
		}
	}
}

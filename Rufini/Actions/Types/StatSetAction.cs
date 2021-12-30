using System;
using System.Collections.Generic;
using Mother4.Battle;
using Mother4.Data;
using Mother4.Scripts;
using Mother4.Scripts.Actions;
using Mother4.Scripts.Actions.ParamTypes;

namespace Rufini.Actions.Types
{
	// Token: 0x0200015E RID: 350
	internal class StatSetAction : RufiniAction
	{
		// Token: 0x0600077C RID: 1916 RVA: 0x00030F64 File Offset: 0x0002F164
		public StatSetAction()
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

		// Token: 0x0600077D RID: 1917 RVA: 0x00030FFC File Offset: 0x0002F1FC
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			CharacterType byOptionInt = CharacterType.GetByOptionInt(base.GetValue<RufiniOption>("char").Option);
			int option = base.GetValue<RufiniOption>("stat").Option;
			int value = base.GetValue<int>("val");
			StatSet stats = CharacterStats.GetStats(byOptionInt);
			switch (option)
			{
			case 0:
				stats.HP = value;
				break;
			case 1:
				stats.PP = value;
				break;
			case 2:
				stats.MaxHP = value;
				break;
			case 3:
				stats.MaxPP = value;
				break;
			case 4:
				stats.Offense = value;
				break;
			case 5:
				stats.Defense = value;
				break;
			case 6:
				stats.Speed = value;
				break;
			case 7:
				stats.Guts = value;
				break;
			case 8:
				stats.IQ = value;
				break;
			case 9:
				stats.Luck = value;
				break;
			case 10:
				stats.Meter = (float)value;
				break;
			case 11:
				stats.Level = value;
				break;
			}
			CharacterStats.SetStats(byOptionInt, stats);
			return default(ActionReturnContext);
		}
	}
}

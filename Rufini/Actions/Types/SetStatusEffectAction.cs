using System;
using System.Collections.Generic;
using Mother4.Battle;
using Mother4.Data;
using Mother4.Scripts;
using Mother4.Scripts.Actions;
using Mother4.Scripts.Actions.ParamTypes;

namespace Rufini.Actions.Types
{
	// Token: 0x0200012A RID: 298
	internal class SetStatusEffectAction : RufiniAction
	{
		// Token: 0x060006FF RID: 1791 RVA: 0x0002C95C File Offset: 0x0002AB5C
		public SetStatusEffectAction()
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
					Name = "eff",
					Type = typeof(RufiniOption)
				},
				new ActionParam
				{
					Name = "en",
					Type = typeof(bool)
				}
			};
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x0002C9F4 File Offset: 0x0002ABF4
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			RufiniOption value = base.GetValue<RufiniOption>("char");
			RufiniOption value2 = base.GetValue<RufiniOption>("eff");
			bool value3 = base.GetValue<bool>("en");
			StatusEffect statusEffect;
			switch (value2.Option)
			{
			case 0:
				statusEffect = StatusEffect.Diamondized;
				break;
			case 1:
				statusEffect = StatusEffect.Mushroomized;
				break;
			case 2:
				statusEffect = StatusEffect.Nausea;
				break;
			case 3:
				statusEffect = StatusEffect.Paralyzed;
				break;
			case 4:
				statusEffect = StatusEffect.Poisoned;
				break;
			case 5:
				statusEffect = StatusEffect.Possessed;
				break;
			case 6:
				statusEffect = StatusEffect.Sunstroke;
				break;
			default:
				statusEffect = StatusEffect.Invalid;
				break;
			}
			CharacterType byOptionInt = CharacterType.GetByOptionInt(value.Option);
			if (statusEffect != StatusEffect.Invalid)
			{
				if (value3)
				{
					CharacterStatusEffects.AddStatusEffect(byOptionInt, statusEffect);
				}
				else
				{
					CharacterStatusEffects.RemoveStatusEffect(byOptionInt, statusEffect);
				}
				if (context.Player != null)
				{
					context.Player.UpdateStatusEffects();
				}
			}
			return default(ActionReturnContext);
		}

		// Token: 0x04000924 RID: 2340
		private const int DIAMONDIZED = 0;

		// Token: 0x04000925 RID: 2341
		private const int MUSHROOMIZED = 1;

		// Token: 0x04000926 RID: 2342
		private const int NAUSEA = 2;

		// Token: 0x04000927 RID: 2343
		private const int PARALYZED = 3;

		// Token: 0x04000928 RID: 2344
		private const int POISONED = 4;

		// Token: 0x04000929 RID: 2345
		private const int POSSESSED = 5;

		// Token: 0x0400092A RID: 2346
		private const int SUNSTROKE = 6;
	}
}

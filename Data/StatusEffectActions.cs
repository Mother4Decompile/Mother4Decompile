using System;
using System.Collections.Generic;
using Mother4.Battle;
using Mother4.Battle.Actions;

namespace Mother4.Data
{
	// Token: 0x0200008B RID: 139
	internal class StatusEffectActions
	{
		// Token: 0x060002D4 RID: 724 RVA: 0x00012528 File Offset: 0x00010728
		public static Type Get(StatusEffect effect)
		{
			Type result = null;
			if (StatusEffectActions.types.ContainsKey(effect))
			{
				result = StatusEffectActions.types[effect];
			}
			return result;
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x00012554 File Offset: 0x00010754
		public static bool TryGet(StatusEffect effect, out Type type)
		{
			bool result = false;
			if (StatusEffectActions.types.ContainsKey(effect))
			{
				type = StatusEffectActions.types[effect];
				result = true;
			}
			else
			{
				type = null;
			}
			return result;
		}

		// Token: 0x0400042E RID: 1070
		private static Dictionary<StatusEffect, Type> types = new Dictionary<StatusEffect, Type>
		{
			{
				StatusEffect.Talking,
				typeof(TalkStatusEffectAction)
			}
		};
	}
}

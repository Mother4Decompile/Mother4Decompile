using System;
using System.Collections.Generic;
using Mother4.Battle;

namespace Mother4.Data
{
	// Token: 0x02000021 RID: 33
	internal static class CharacterStatusEffects
	{
		// Token: 0x06000075 RID: 117 RVA: 0x00004E54 File Offset: 0x00003054
		public static void AddPersistentEffects(CharacterType character, IEnumerable<StatusEffect> effects)
		{
			foreach (StatusEffect statusEffect in effects)
			{
				if (CharacterStatusEffects.PERSISTENT_EFFECTS.Contains(statusEffect))
				{
					CharacterStatusEffects.AddStatusEffect(character, statusEffect);
				}
			}
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00004EAC File Offset: 0x000030AC
		public static void AddStatusEffect(CharacterType character, StatusEffect effect)
		{
			ISet<StatusEffect> set;
			if (!CharacterStatusEffects.EFFECT_DICT.TryGetValue(character, out set))
			{
				set = new HashSet<StatusEffect>();
				CharacterStatusEffects.EFFECT_DICT.Add(character, set);
			}
			if (!set.Contains(effect))
			{
				set.Add(effect);
			}
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00004EEC File Offset: 0x000030EC
		public static void RemoveStatusEffect(CharacterType character, StatusEffect effect)
		{
			ISet<StatusEffect> set;
			if (CharacterStatusEffects.EFFECT_DICT.TryGetValue(character, out set) && set.Contains(effect))
			{
				set.Remove(effect);
			}
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00004F1C File Offset: 0x0000311C
		public static bool HasStatusEffect(CharacterType character, StatusEffect effect)
		{
			bool result = false;
			ISet<StatusEffect> set;
			if (CharacterStatusEffects.EFFECT_DICT.TryGetValue(character, out set))
			{
				foreach (StatusEffect statusEffect in set)
				{
					if (statusEffect == effect)
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x04000137 RID: 311
		private static readonly ISet<StatusEffect> PERSISTENT_EFFECTS = new HashSet<StatusEffect>
		{
			StatusEffect.Diamondized,
			StatusEffect.Mushroomized,
			StatusEffect.Nausea,
			StatusEffect.Paralyzed,
			StatusEffect.Poisoned,
			StatusEffect.Possessed,
			StatusEffect.Sunstroke,
			StatusEffect.Unconscious
		};

		// Token: 0x04000138 RID: 312
		private static readonly Dictionary<CharacterType, ISet<StatusEffect>> EFFECT_DICT = new Dictionary<CharacterType, ISet<StatusEffect>>();
	}
}

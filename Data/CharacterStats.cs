using System;
using System.Collections.Generic;
using Mother4.Battle;
using Mother4.Data.Character;

namespace Mother4.Data
{
	// Token: 0x020000EB RID: 235
	internal static class CharacterStats
	{
		// Token: 0x0600056B RID: 1387 RVA: 0x00021094 File Offset: 0x0001F294
		public static StatSet GetStats(CharacterType character)
		{
			StatSet initialStatSet;
			if (!CharacterStats.STATS_DICT.TryGetValue(character, out initialStatSet))
			{
				CharacterData data = CharacterFile.Instance.GetData(character);
				initialStatSet = data.InitialStatSet;
				CharacterStats.STATS_DICT.Add(character, initialStatSet);
			}
			return initialStatSet;
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x000210D0 File Offset: 0x0001F2D0
		public static void SetStats(CharacterType character, StatSet statset)
		{
			if (CharacterStats.STATS_DICT.ContainsKey(character))
			{
				CharacterStats.STATS_DICT[character] = statset;
			}
		}

		// Token: 0x0400073E RID: 1854
		private static Dictionary<CharacterType, StatSet> STATS_DICT = new Dictionary<CharacterType, StatSet>();
	}
}

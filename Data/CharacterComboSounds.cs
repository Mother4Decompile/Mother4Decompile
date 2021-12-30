using System;
using System.Collections.Generic;

namespace Mother4.Data
{
	// Token: 0x02000076 RID: 118
	internal static class CharacterComboSounds
	{
		// Token: 0x06000286 RID: 646 RVA: 0x0000FDE4 File Offset: 0x0000DFE4
		public static string Get(CharacterType character, int type, int index, int bpm)
		{
			CharacterType key = character;
			if (!CharacterComboSounds.prefixes.ContainsKey(key))
			{
				key = CharacterType.Travis;
			}
			int num = 0;
			int num2 = int.MaxValue;
			for (int i = 0; i < CharacterComboSounds.BPMS.Length; i++)
			{
				int num3 = Math.Abs(bpm - CharacterComboSounds.BPMS[i]);
				if (num3 < num2)
				{
					num2 = num3;
					num = i;
					if (num3 == 0)
					{
						break;
					}
				}
			}
			return string.Format("{0}{1}{2}-{3}.{4}", new object[]
			{
				CharacterComboSounds.prefixes[key],
				CharacterComboSounds.TYPES[type % CharacterComboSounds.TYPES.Length],
				index % 3 + 1,
				CharacterComboSounds.BPMS[num],
				"wav"
			});
		}

		// Token: 0x040003EC RID: 1004
		private const string EXTENSION = "wav";

		// Token: 0x040003ED RID: 1005
		private const int INDEXES = 3;

		// Token: 0x040003EE RID: 1006
		private static char[] TYPES = new char[]
		{
			'A',
			'B',
			'C'
		};

		// Token: 0x040003EF RID: 1007
		private static int[] BPMS = new int[]
		{
			120
		};

		// Token: 0x040003F0 RID: 1008
		private static Dictionary<CharacterType, string> prefixes = new Dictionary<CharacterType, string>
		{
			{
				CharacterType.Travis,
				"TravisCombo"
			},
			{
				CharacterType.Floyd,
				"FloydCombo"
			}
		};
	}
}

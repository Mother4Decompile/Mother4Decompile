using System;
using System.Collections.Generic;
using fNbt;
using Mother4.Battle;
using Mother4.Data.Psi;

namespace Mother4.Data.Character
{
	// Token: 0x02000018 RID: 24
	internal class CharacterData
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000042 RID: 66 RVA: 0x00003E7A File Offset: 0x0000207A
		public string QualifiedName
		{
			get
			{
				return this.qualifiedName;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000043 RID: 67 RVA: 0x00003E82 File Offset: 0x00002082
		public StatSet InitialStatSet
		{
			get
			{
				return this.initialStatSet;
			}
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00003E8C File Offset: 0x0000208C
		public CharacterData(NbtCompound tag)
		{
			if (tag.Name == null || tag.Name.Length == 0)
			{
				throw new ArgumentException("Cannot load character data from an unnamed tag.");
			}
			this.qualifiedName = tag.Path;
			this.LoadStatSet(tag);
			this.LoadPsiLearn(tag);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00003EDC File Offset: 0x000020DC
		private void LoadStatSet(NbtCompound tag)
		{
			NbtCompound nbtCompound;
			if (tag.TryGet<NbtCompound>("stats", out nbtCompound))
			{
				NbtTag nbtTag;
				this.initialStatSet.HP = (nbtCompound.TryGet("hp", out nbtTag) ? nbtTag.IntValue : 1);
				this.initialStatSet.PP = (nbtCompound.TryGet("pp", out nbtTag) ? nbtTag.IntValue : 0);
				this.initialStatSet.Offense = (nbtCompound.TryGet("offense", out nbtTag) ? nbtTag.IntValue : 0);
				this.initialStatSet.Defense = (nbtCompound.TryGet("defense", out nbtTag) ? nbtTag.IntValue : 0);
				this.initialStatSet.Speed = (nbtCompound.TryGet("speed", out nbtTag) ? nbtTag.IntValue : 0);
				this.initialStatSet.Guts = (nbtCompound.TryGet("guts", out nbtTag) ? nbtTag.IntValue : 0);
				this.initialStatSet.IQ = (nbtCompound.TryGet("iq", out nbtTag) ? nbtTag.IntValue : 0);
				this.initialStatSet.Luck = (nbtCompound.TryGet("luck", out nbtTag) ? nbtTag.IntValue : 0);
				this.initialStatSet.Level = (nbtCompound.TryGet("level", out nbtTag) ? nbtTag.IntValue : 0);
			}
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00004038 File Offset: 0x00002238
		private void LoadPsiLearn(NbtCompound tag)
		{
			this.psiLearnDict = new Dictionary<PsiType, CharacterData.PsiLearn[]>();
			NbtList nbtList;
			if (tag.TryGet<NbtList>("psi", out nbtList))
			{
				foreach (NbtTag nbtTag in nbtList)
				{
					if (nbtTag is NbtCompound)
					{
						NbtCompound nbtCompound = (NbtCompound)nbtTag;
						NbtTag nbtTag2;
						string text = nbtCompound.TryGet("psi", out nbtTag2) ? nbtTag2.StringValue : string.Empty;
						int[] array = nbtCompound.TryGet("learn", out nbtTag2) ? nbtTag2.IntArrayValue : null;
						if (array != null)
						{
							PsiType psiType = PsiFile.Instance.GetPsiType(text);
							CharacterData.PsiLearn[] array2 = new CharacterData.PsiLearn[array.Length / 2];
							for (int i = 0; i < array.Length; i += 2)
							{
								array2[i / 2] = default(CharacterData.PsiLearn);
								array2[i / 2].PsiLevel = array[i];
								array2[i / 2].LearnLevel = array[i + 1];
							}
							this.psiLearnDict.Add(psiType, array2);
						}
					}
				}
			}
		}

		// Token: 0x06000047 RID: 71 RVA: 0x0000416C File Offset: 0x0000236C
		public List<PsiLevel> GetKnownPsi(int characterLevel)
		{
			List<PsiLevel> list = new List<PsiLevel>();
			foreach (KeyValuePair<PsiType, CharacterData.PsiLearn[]> keyValuePair in this.psiLearnDict)
			{
				PsiType key = keyValuePair.Key;
				CharacterData.PsiLearn[] value = keyValuePair.Value;
				for (int i = 0; i < value.Length; i++)
				{
					if (value[i].LearnLevel <= characterLevel)
					{
						list.Add(new PsiLevel(key, value[i].PsiLevel));
					}
				}
			}
			return list;
		}

		// Token: 0x06000048 RID: 72 RVA: 0x0000420C File Offset: 0x0000240C
		public int GetPsiLearnLevel(PsiLevel psiLevel)
		{
			return this.GetPsiLearnLevel(psiLevel.PsiType, psiLevel.Level);
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00004224 File Offset: 0x00002424
		public int GetPsiLearnLevel(PsiType type, int psiLevel)
		{
			int result = int.MaxValue;
			CharacterData.PsiLearn[] array;
			if (this.psiLearnDict.TryGetValue(type, out array))
			{
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i].PsiLevel == psiLevel)
					{
						result = array[i].LearnLevel;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00004274 File Offset: 0x00002474
		public bool HasPsi(int characterLevel)
		{
			bool result = false;
			foreach (KeyValuePair<PsiType, CharacterData.PsiLearn[]> keyValuePair in this.psiLearnDict)
			{
				PsiType key = keyValuePair.Key;
				CharacterData.PsiLearn[] value = keyValuePair.Value;
				for (int i = 0; i < value.Length; i++)
				{
					if (value[i].LearnLevel <= characterLevel)
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x040000FA RID: 250
		private const string PSI_TAG = "psi";

		// Token: 0x040000FB RID: 251
		private const string PSI_LEARN_TAG = "learn";

		// Token: 0x040000FC RID: 252
		private const string PSI_LEVEL_TAG = "level";

		// Token: 0x040000FD RID: 253
		private const string STATS_TAG = "stats";

		// Token: 0x040000FE RID: 254
		private const string STAT_HP_TAG = "hp";

		// Token: 0x040000FF RID: 255
		private const string STAT_PP_TAG = "pp";

		// Token: 0x04000100 RID: 256
		private const string STAT_OFFENSE_TAG = "offense";

		// Token: 0x04000101 RID: 257
		private const string STAT_DEFENSE_TAG = "defense";

		// Token: 0x04000102 RID: 258
		private const string STAT_SPEED_TAG = "speed";

		// Token: 0x04000103 RID: 259
		private const string STAT_GUTS_TAG = "guts";

		// Token: 0x04000104 RID: 260
		private const string STAT_IQ_TAG = "iq";

		// Token: 0x04000105 RID: 261
		private const string STAT_LUCK_TAG = "luck";

		// Token: 0x04000106 RID: 262
		private const string STAT_LEVEL_TAG = "level";

		// Token: 0x04000107 RID: 263
		private string qualifiedName;

		// Token: 0x04000108 RID: 264
		private Dictionary<PsiType, CharacterData.PsiLearn[]> psiLearnDict;

		// Token: 0x04000109 RID: 265
		private StatSet initialStatSet;

		// Token: 0x02000019 RID: 25
		public struct KnownPsi
		{
			// Token: 0x17000008 RID: 8
			// (get) Token: 0x0600004B RID: 75 RVA: 0x000042F8 File Offset: 0x000024F8
			public PsiType Type
			{
				get
				{
					return this.psiType;
				}
			}

			// Token: 0x17000009 RID: 9
			// (get) Token: 0x0600004C RID: 76 RVA: 0x00004300 File Offset: 0x00002500
			public int Level
			{
				get
				{
					return this.psiLevel;
				}
			}

			// Token: 0x0600004D RID: 77 RVA: 0x00004308 File Offset: 0x00002508
			public KnownPsi(PsiType type, int level)
			{
				this.psiType = type;
				this.psiLevel = level;
			}

			// Token: 0x0400010A RID: 266
			private PsiType psiType;

			// Token: 0x0400010B RID: 267
			private int psiLevel;
		}

		// Token: 0x0200001A RID: 26
		private struct PsiLearn
		{
			// Token: 0x0400010C RID: 268
			public int PsiLevel;

			// Token: 0x0400010D RID: 269
			public int LearnLevel;
		}
	}
}

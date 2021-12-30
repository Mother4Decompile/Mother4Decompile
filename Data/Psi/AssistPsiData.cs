using System;
using fNbt;
using Mother4.Battle;

namespace Mother4.Data.Psi
{
	// Token: 0x02000029 RID: 41
	internal class AssistPsiData : PsiData
	{
		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x00005EFD File Offset: 0x000040FD
		public int[] Recovery
		{
			get
			{
				return this.recover;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x00005F05 File Offset: 0x00004105
		public StatusEffect[][] ClearEffects
		{
			get
			{
				return this.clear;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000C9 RID: 201 RVA: 0x00005F0D File Offset: 0x0000410D
		public AssistPsiData.DrainPair[][] Drain
		{
			get
			{
				return this.drain;
			}
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00005F15 File Offset: 0x00004115
		public AssistPsiData(NbtCompound tag)
		{
			this.Load(tag);
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00005F24 File Offset: 0x00004124
		private StatusEffect[][] ReadStatusEffects(NbtTag tag)
		{
			StatusEffect[][] array = null;
			if (tag is NbtList)
			{
				NbtList nbtList = (NbtList)tag;
				array = new StatusEffect[nbtList.Count][];
				for (int i = 0; i < array.Length; i++)
				{
					NbtByteArray nbtByteArray = nbtList.Get<NbtByteArray>(i);
					byte[] value = nbtByteArray.Value;
					array[i] = new StatusEffect[value.Length];
					for (int j = 0; j < value.Length; j++)
					{
						array[i][j] = (StatusEffect)value[j];
					}
				}
			}
			return array;
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00005F98 File Offset: 0x00004198
		private AssistPsiData.DrainPair[][] ReadDrain(NbtTag tag)
		{
			AssistPsiData.DrainPair[][] array = null;
			if (tag is NbtList)
			{
				NbtList nbtList = (NbtList)tag;
				array = new AssistPsiData.DrainPair[nbtList.Count][];
				for (int i = 0; i < array.Length; i++)
				{
					NbtByteArray nbtByteArray = nbtList.Get<NbtByteArray>(i);
					byte[] value = nbtByteArray.Value;
					array[i] = new AssistPsiData.DrainPair[value.Length];
					for (int j = 0; j < value.Length; j += 2)
					{
						byte min = value[j];
						byte max = value[j + 1];
						array[i][j] = new AssistPsiData.DrainPair((int)min, (int)max);
					}
				}
			}
			return array;
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00006028 File Offset: 0x00004228
		protected override void Load(NbtCompound tag)
		{
			base.Load(tag);
			NbtTag nbtTag;
			this.recover = (tag.TryGet("recover", out nbtTag) ? nbtTag.IntArrayValue : null);
			this.clear = (tag.TryGet("clear", out nbtTag) ? this.ReadStatusEffects(nbtTag) : null);
			this.drain = (tag.TryGet("drain", out nbtTag) ? this.ReadDrain(nbtTag) : null);
		}

		// Token: 0x040001BF RID: 447
		private const string RECOVER_TAG = "recover";

		// Token: 0x040001C0 RID: 448
		private const string CLEAR_TAG = "clear";

		// Token: 0x040001C1 RID: 449
		private const string DRAIN_TAG = "drain";

		// Token: 0x040001C2 RID: 450
		private int[] recover;

		// Token: 0x040001C3 RID: 451
		private StatusEffect[][] clear;

		// Token: 0x040001C4 RID: 452
		private AssistPsiData.DrainPair[][] drain;

		// Token: 0x0200002A RID: 42
		public struct DrainPair
		{
			// Token: 0x1700003D RID: 61
			// (get) Token: 0x060000CE RID: 206 RVA: 0x00006098 File Offset: 0x00004298
			public int Min
			{
				get
				{
					return this.min;
				}
			}

			// Token: 0x1700003E RID: 62
			// (get) Token: 0x060000CF RID: 207 RVA: 0x000060A0 File Offset: 0x000042A0
			public int Max
			{
				get
				{
					return this.max;
				}
			}

			// Token: 0x060000D0 RID: 208 RVA: 0x000060A8 File Offset: 0x000042A8
			public DrainPair(int min, int max)
			{
				if (min < max)
				{
					this.min = min;
					this.max = max;
					return;
				}
				this.min = max;
				this.max = min;
			}

			// Token: 0x040001C5 RID: 453
			private int min;

			// Token: 0x040001C6 RID: 454
			private int max;
		}
	}
}

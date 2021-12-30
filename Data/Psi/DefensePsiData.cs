using System;
using fNbt;
using Mother4.Battle;

namespace Mother4.Data.Psi
{
	// Token: 0x0200002B RID: 43
	internal class DefensePsiData : PsiData
	{
		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x000060CB File Offset: 0x000042CB
		public DefensePsiData.StatusEffectPair[][] StatusEffects
		{
			get
			{
				return this.statusEffects;
			}
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x000060D3 File Offset: 0x000042D3
		public DefensePsiData(NbtCompound tag)
		{
			this.Load(tag);
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x000060E4 File Offset: 0x000042E4
		private DefensePsiData.StatusEffectPair[][] ReadStatusEffects(NbtTag tag)
		{
			DefensePsiData.StatusEffectPair[][] array = null;
			if (tag is NbtList)
			{
				NbtList nbtList = (NbtList)tag;
				array = new DefensePsiData.StatusEffectPair[nbtList.Count][];
				for (int i = 0; i < array.Length; i++)
				{
					NbtByteArray nbtByteArray = nbtList.Get<NbtByteArray>(i);
					byte[] value = nbtByteArray.Value;
					array[i] = new DefensePsiData.StatusEffectPair[value.Length];
					for (int j = 0; j < value.Length; j += 2)
					{
						StatusEffect effect = (StatusEffect)value[j];
						byte chance = value[j + 1];
						array[i][j] = new DefensePsiData.StatusEffectPair(effect, chance);
					}
				}
			}
			return array;
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00006174 File Offset: 0x00004374
		protected override void Load(NbtCompound tag)
		{
			base.Load(tag);
			NbtTag tag2;
			this.statusEffects = (tag.TryGet("effect", out tag2) ? this.ReadStatusEffects(tag2) : null);
		}

		// Token: 0x040001C7 RID: 455
		private const string STATUS_EFFECT_TAG = "effect";

		// Token: 0x040001C8 RID: 456
		private DefensePsiData.StatusEffectPair[][] statusEffects;

		// Token: 0x0200002C RID: 44
		public struct StatusEffectPair
		{
			// Token: 0x17000040 RID: 64
			// (get) Token: 0x060000D5 RID: 213 RVA: 0x000061A7 File Offset: 0x000043A7
			public StatusEffect StatusEffect
			{
				get
				{
					return this.effect;
				}
			}

			// Token: 0x17000041 RID: 65
			// (get) Token: 0x060000D6 RID: 214 RVA: 0x000061AF File Offset: 0x000043AF
			public byte Chance
			{
				get
				{
					return this.chance;
				}
			}

			// Token: 0x060000D7 RID: 215 RVA: 0x000061B7 File Offset: 0x000043B7
			public StatusEffectPair(StatusEffect effect, byte chance)
			{
				this.effect = effect;
				this.chance = chance;
			}

			// Token: 0x040001C9 RID: 457
			private StatusEffect effect;

			// Token: 0x040001CA RID: 458
			private byte chance;
		}
	}
}

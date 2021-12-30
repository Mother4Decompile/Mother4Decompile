using System;
using fNbt;
using Mother4.Battle;

namespace Mother4.Data.Psi
{
	// Token: 0x0200002D RID: 45
	internal class OffensePsiData : PsiData
	{
		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x000061C7 File Offset: 0x000043C7
		public int Element
		{
			get
			{
				return this.element;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000D9 RID: 217 RVA: 0x000061CF File Offset: 0x000043CF
		public OffensePsiData.DamagePair[] Damage
		{
			get
			{
				return this.damage;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000DA RID: 218 RVA: 0x000061D7 File Offset: 0x000043D7
		public OffensePsiData.StatusEffectPair[][] StatusEffects
		{
			get
			{
				return this.statusEffects;
			}
		}

		// Token: 0x060000DB RID: 219 RVA: 0x000061DF File Offset: 0x000043DF
		public OffensePsiData(NbtCompound tag)
		{
			this.Load(tag);
		}

		// Token: 0x060000DC RID: 220 RVA: 0x000061F0 File Offset: 0x000043F0
		private OffensePsiData.DamagePair[] ReadDamage(NbtTag tag)
		{
			OffensePsiData.DamagePair[] array = null;
			if (tag is NbtList)
			{
				NbtList nbtList = (NbtList)tag;
				array = new OffensePsiData.DamagePair[nbtList.Count];
				for (int i = 0; i < array.Length; i++)
				{
					NbtIntArray nbtIntArray = nbtList.Get<NbtIntArray>(i);
					if (nbtIntArray.Value != null && nbtIntArray.Value.Length == 2)
					{
						array[i] = new OffensePsiData.DamagePair(nbtIntArray.Value[0], nbtIntArray.Value[1]);
					}
					else
					{
						array[i] = default(OffensePsiData.DamagePair);
					}
				}
			}
			return array;
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00006274 File Offset: 0x00004474
		private OffensePsiData.StatusEffectPair[][] ReadStatusEffects(NbtTag tag)
		{
			OffensePsiData.StatusEffectPair[][] array = null;
			if (tag is NbtList)
			{
				NbtList nbtList = (NbtList)tag;
				array = new OffensePsiData.StatusEffectPair[nbtList.Count][];
				for (int i = 0; i < array.Length; i++)
				{
					NbtByteArray nbtByteArray = nbtList.Get<NbtByteArray>(i);
					byte[] value = nbtByteArray.Value;
					array[i] = new OffensePsiData.StatusEffectPair[value.Length];
					for (int j = 0; j < value.Length; j += 2)
					{
						StatusEffect effect = (StatusEffect)value[j];
						byte chance = value[j + 1];
						array[i][j] = new OffensePsiData.StatusEffectPair(effect, chance);
					}
				}
			}
			return array;
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00006304 File Offset: 0x00004504
		protected override void Load(NbtCompound tag)
		{
			base.Load(tag);
			NbtTag nbtTag;
			this.element = (tag.TryGet("element", out nbtTag) ? nbtTag.IntValue : 0);
			this.damage = (tag.TryGet("damage", out nbtTag) ? this.ReadDamage(nbtTag) : null);
			this.statusEffects = (tag.TryGet("effect", out nbtTag) ? this.ReadStatusEffects(nbtTag) : null);
		}

		// Token: 0x040001CB RID: 459
		private const string ELEMENT_TAG = "element";

		// Token: 0x040001CC RID: 460
		private const string DAMAGE_TAG = "damage";

		// Token: 0x040001CD RID: 461
		private const string STATUS_EFFECT_TAG = "effect";

		// Token: 0x040001CE RID: 462
		private int element;

		// Token: 0x040001CF RID: 463
		private OffensePsiData.DamagePair[] damage;

		// Token: 0x040001D0 RID: 464
		private OffensePsiData.StatusEffectPair[][] statusEffects;

		// Token: 0x0200002E RID: 46
		public struct DamagePair
		{
			// Token: 0x17000045 RID: 69
			// (get) Token: 0x060000DF RID: 223 RVA: 0x00006374 File Offset: 0x00004574
			public int Min
			{
				get
				{
					return this.min;
				}
			}

			// Token: 0x17000046 RID: 70
			// (get) Token: 0x060000E0 RID: 224 RVA: 0x0000637C File Offset: 0x0000457C
			public int Max
			{
				get
				{
					return this.max;
				}
			}

			// Token: 0x060000E1 RID: 225 RVA: 0x00006384 File Offset: 0x00004584
			public DamagePair(int min, int max)
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

			// Token: 0x040001D1 RID: 465
			private int min;

			// Token: 0x040001D2 RID: 466
			private int max;
		}

		// Token: 0x0200002F RID: 47
		public struct StatusEffectPair
		{
			// Token: 0x17000047 RID: 71
			// (get) Token: 0x060000E2 RID: 226 RVA: 0x000063A7 File Offset: 0x000045A7
			public StatusEffect StatusEffect
			{
				get
				{
					return this.effect;
				}
			}

			// Token: 0x17000048 RID: 72
			// (get) Token: 0x060000E3 RID: 227 RVA: 0x000063AF File Offset: 0x000045AF
			public byte Chance
			{
				get
				{
					return this.chance;
				}
			}

			// Token: 0x060000E4 RID: 228 RVA: 0x000063B7 File Offset: 0x000045B7
			public StatusEffectPair(StatusEffect effect, byte chance)
			{
				this.effect = effect;
				this.chance = chance;
			}

			// Token: 0x040001D3 RID: 467
			private StatusEffect effect;

			// Token: 0x040001D4 RID: 468
			private byte chance;
		}
	}
}

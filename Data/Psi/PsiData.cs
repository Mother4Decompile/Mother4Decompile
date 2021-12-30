using System;
using fNbt;

namespace Mother4.Data.Psi
{
	// Token: 0x02000028 RID: 40
	internal abstract class PsiData
	{
		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000BC RID: 188 RVA: 0x00005D90 File Offset: 0x00003F90
		public string QualifiedName
		{
			get
			{
				return this.qualifiedName;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000BD RID: 189 RVA: 0x00005D98 File Offset: 0x00003F98
		public string Key
		{
			get
			{
				return this.key;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000BE RID: 190 RVA: 0x00005DA0 File Offset: 0x00003FA0
		public int Order
		{
			get
			{
				return this.order;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000BF RID: 191 RVA: 0x00005DA8 File Offset: 0x00003FA8
		public byte[] PP
		{
			get
			{
				return this.pp;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x00005DB0 File Offset: 0x00003FB0
		public byte[] Symbols
		{
			get
			{
				return this.symbols;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x00005DB8 File Offset: 0x00003FB8
		public byte[] Animations
		{
			get
			{
				return this.anim;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x00005DC0 File Offset: 0x00003FC0
		public byte[] Targets
		{
			get
			{
				return this.targets;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x00005DC8 File Offset: 0x00003FC8
		public int MaxLevel
		{
			get
			{
				return this.pp.Length - 1;
			}
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00005DD4 File Offset: 0x00003FD4
		protected virtual void Load(NbtCompound tag)
		{
			if (tag.Name == null || tag.Name.Length == 0)
			{
				throw new ArgumentException("Cannot load psi data from an unnamed tag.");
			}
			this.qualifiedName = tag.Path;
			NbtTag nbtTag;
			this.key = (tag.TryGet("key", out nbtTag) ? nbtTag.StringValue : string.Empty);
			this.order = (tag.TryGet("order", out nbtTag) ? nbtTag.IntValue : 0);
			this.pp = (tag.TryGet("pp", out nbtTag) ? nbtTag.ByteArrayValue : null);
			this.symbols = (tag.TryGet("symbols", out nbtTag) ? nbtTag.ByteArrayValue : null);
			this.anim = (tag.TryGet("anim", out nbtTag) ? nbtTag.ByteArrayValue : null);
			this.targets = (tag.TryGet("targets", out nbtTag) ? nbtTag.ByteArrayValue : null);
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00005EC8 File Offset: 0x000040C8
		public string GetSymbol(int level)
		{
			int num = Math.Max(0, Math.Min(this.MaxLevel, level));
			return PsiLetters.Get((int)this.symbols[num]);
		}

		// Token: 0x040001B2 RID: 434
		private const string KEY_TAG = "key";

		// Token: 0x040001B3 RID: 435
		private const string ORDER_TAG = "order";

		// Token: 0x040001B4 RID: 436
		private const string PP_TAG = "pp";

		// Token: 0x040001B5 RID: 437
		private const string SYMBOLS_TAG = "symbols";

		// Token: 0x040001B6 RID: 438
		private const string ANIM_TAG = "anim";

		// Token: 0x040001B7 RID: 439
		private const string TARGETS_TAG = "targets";

		// Token: 0x040001B8 RID: 440
		private string qualifiedName;

		// Token: 0x040001B9 RID: 441
		private string key;

		// Token: 0x040001BA RID: 442
		private int order;

		// Token: 0x040001BB RID: 443
		private byte[] pp;

		// Token: 0x040001BC RID: 444
		private byte[] symbols;

		// Token: 0x040001BD RID: 445
		private byte[] anim;

		// Token: 0x040001BE RID: 446
		private byte[] targets;
	}
}

using System;
using fNbt;

namespace Mother4.Data.Psi
{
	// Token: 0x02000030 RID: 48
	internal class OtherPsiData : PsiData
	{
		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x000063C7 File Offset: 0x000045C7
		public bool IsBattleAction
		{
			get
			{
				return this.isBattleAction;
			}
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x000063CF File Offset: 0x000045CF
		public OtherPsiData(NbtCompound tag)
		{
			this.Load(tag);
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x000063E0 File Offset: 0x000045E0
		protected override void Load(NbtCompound tag)
		{
			base.Load(tag);
			NbtTag nbtTag;
			this.isBattleAction = (tag.TryGet("battle", out nbtTag) && nbtTag.ByteValue > 0);
		}

		// Token: 0x040001D5 RID: 469
		private const string BATTLE_TAG = "battle";

		// Token: 0x040001D6 RID: 470
		private bool isBattleAction;
	}
}

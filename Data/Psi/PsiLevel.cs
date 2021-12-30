using System;

namespace Mother4.Data.Psi
{
	// Token: 0x02000032 RID: 50
	internal struct PsiLevel
	{
		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x000066E9 File Offset: 0x000048E9
		public PsiType PsiType
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x000066F1 File Offset: 0x000048F1
		public int Level
		{
			get
			{
				return this.level;
			}
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x000066F9 File Offset: 0x000048F9
		public PsiLevel(PsiType type, int level)
		{
			this.type = type;
			this.level = level;
		}

		// Token: 0x040001E2 RID: 482
		private PsiType type;

		// Token: 0x040001E3 RID: 483
		private int level;
	}
}

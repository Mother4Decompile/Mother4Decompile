using System;

namespace Mother4.Data.Psi
{
	// Token: 0x02000033 RID: 51
	internal struct PsiType
	{
		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x00006709 File Offset: 0x00004909
		public int Identifier
		{
			get
			{
				return this.id;
			}
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00006711 File Offset: 0x00004911
		public PsiType(int id)
		{
			this.id = id;
		}

		// Token: 0x060000FB RID: 251 RVA: 0x0000671A File Offset: 0x0000491A
		public override int GetHashCode()
		{
			return this.id;
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00006722 File Offset: 0x00004922
		public override bool Equals(object obj)
		{
			return obj is PsiType && this.GetHashCode() == obj.GetHashCode();
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00006742 File Offset: 0x00004942
		public static bool operator ==(PsiType a, PsiType b)
		{
			return a.id == b.id;
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00006754 File Offset: 0x00004954
		public static bool operator !=(PsiType a, PsiType b)
		{
			return !(a == b);
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00006760 File Offset: 0x00004960
		public static explicit operator int(PsiType type)
		{
			return type.id;
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00006769 File Offset: 0x00004969
		public static explicit operator PsiType(int id)
		{
			return new PsiType(id);
		}

		// Token: 0x040001E4 RID: 484
		private int id;
	}
}

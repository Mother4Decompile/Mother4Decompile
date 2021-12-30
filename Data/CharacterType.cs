using System;
using Carbine.Utility;

namespace Mother4.Data
{
	// Token: 0x020000F2 RID: 242
	internal struct CharacterType
	{
		// Token: 0x0600058A RID: 1418 RVA: 0x00021870 File Offset: 0x0001FA70
		public static CharacterType GetByOptionInt(int option)
		{
			switch (option)
			{
			case 0:
				return CharacterType.Travis;
			case 1:
				return CharacterType.Floyd;
			case 2:
				return CharacterType.Meryl;
			case 3:
				return CharacterType.Leo;
			case 4:
				return CharacterType.Zack;
			case 5:
				return CharacterType.Renee;
			case 6:
				return CharacterType.Dog;
			default:
				throw new ArgumentException("Unknown character option int.");
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x0600058B RID: 1419 RVA: 0x000218D7 File Offset: 0x0001FAD7
		public int Identifier
		{
			get
			{
				return this.id;
			}
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x000218DF File Offset: 0x0001FADF
		public CharacterType(int id)
		{
			this.id = id;
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x000218E8 File Offset: 0x0001FAE8
		public override int GetHashCode()
		{
			return this.id;
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x000218F0 File Offset: 0x0001FAF0
		public override bool Equals(object obj)
		{
			return obj is CharacterType && this.GetHashCode() == obj.GetHashCode();
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x00021910 File Offset: 0x0001FB10
		public static bool operator ==(CharacterType a, CharacterType b)
		{
			return a.id == b.id;
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x00021922 File Offset: 0x0001FB22
		public static bool operator !=(CharacterType a, CharacterType b)
		{
			return !(a == b);
		}

		// Token: 0x04000758 RID: 1880
		public static readonly CharacterType Travis = new CharacterType(Hash.Get("character.party.travis"));

		// Token: 0x04000759 RID: 1881
		public static readonly CharacterType Floyd = new CharacterType(Hash.Get("character.party.floyd"));

		// Token: 0x0400075A RID: 1882
		public static readonly CharacterType Meryl = new CharacterType(Hash.Get("character.party.meryl"));

		// Token: 0x0400075B RID: 1883
		public static readonly CharacterType Leo = new CharacterType(Hash.Get("character.party.leo"));

		// Token: 0x0400075C RID: 1884
		public static readonly CharacterType Zack = new CharacterType(Hash.Get("character.party.zack"));

		// Token: 0x0400075D RID: 1885
		public static readonly CharacterType Renee = new CharacterType(Hash.Get("character.party.renee"));

		// Token: 0x0400075E RID: 1886
		public static readonly CharacterType Dog = new CharacterType(Hash.Get("character.party.dog"));

		// Token: 0x0400075F RID: 1887
		private int id;
	}
}

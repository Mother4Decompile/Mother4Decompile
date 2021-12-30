using System;
using SFML.System;

namespace Mother4.Data
{
	// Token: 0x02000034 RID: 52
	internal struct SaveProfile
	{
		// Token: 0x040001E5 RID: 485
		public bool IsValid;

		// Token: 0x040001E6 RID: 486
		public int Index;

		// Token: 0x040001E7 RID: 487
		public CharacterType[] Party;

		// Token: 0x040001E8 RID: 488
		public string MapName;

		// Token: 0x040001E9 RID: 489
		public Vector2f Position;

		// Token: 0x040001EA RID: 490
		public int Time;

		// Token: 0x040001EB RID: 491
		public int Flavor;
	}
}

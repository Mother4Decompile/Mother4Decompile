using System;
using SFML.System;

namespace Carbine.Collision
{
	// Token: 0x0200001B RID: 27
	internal struct Triangle
	{
		// Token: 0x060000DF RID: 223 RVA: 0x0000528C File Offset: 0x0000348C
		public Triangle(Vector2f v1, Vector2f v2, Vector2f v3)
		{
			this.V1 = v1;
			this.V2 = v2;
			this.V3 = v3;
		}

		// Token: 0x0400006A RID: 106
		public Vector2f V1;

		// Token: 0x0400006B RID: 107
		public Vector2f V2;

		// Token: 0x0400006C RID: 108
		public Vector2f V3;
	}
}

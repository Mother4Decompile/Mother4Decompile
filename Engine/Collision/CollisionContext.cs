using System;
using SFML.System;

namespace Carbine.Collision
{
	// Token: 0x02000013 RID: 19
	public class CollisionContext
	{
		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600008E RID: 142 RVA: 0x0000372D File Offset: 0x0000192D
		// (set) Token: 0x0600008F RID: 143 RVA: 0x00003735 File Offset: 0x00001935
		public ICollidable Other { get; private set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000090 RID: 144 RVA: 0x0000373E File Offset: 0x0000193E
		// (set) Token: 0x06000091 RID: 145 RVA: 0x00003746 File Offset: 0x00001946
		public bool Colliding { get; private set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000092 RID: 146 RVA: 0x0000374F File Offset: 0x0000194F
		// (set) Token: 0x06000093 RID: 147 RVA: 0x00003757 File Offset: 0x00001957
		public bool WillCollide { get; private set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000094 RID: 148 RVA: 0x00003760 File Offset: 0x00001960
		// (set) Token: 0x06000095 RID: 149 RVA: 0x00003768 File Offset: 0x00001968
		public Vector2f MinimumTranslation { get; private set; }

		// Token: 0x06000096 RID: 150 RVA: 0x00003771 File Offset: 0x00001971
		public CollisionContext(ICollidable other, bool colliding, bool willCollide, Vector2f minTranslation)
		{
			this.Other = other;
			this.Colliding = colliding;
			this.WillCollide = willCollide;
			this.MinimumTranslation = minTranslation;
		}
	}
}

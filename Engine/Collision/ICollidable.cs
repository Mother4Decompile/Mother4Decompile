using System;
using SFML.Graphics;
using SFML.System;

namespace Carbine.Collision
{
	// Token: 0x02000004 RID: 4
	public interface ICollidable
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000016 RID: 22
		// (set) Token: 0x06000017 RID: 23
		Vector2f Position { get; set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000018 RID: 24
		Vector2f Velocity { get; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000019 RID: 25
		AABB AABB { get; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600001A RID: 26
		Mesh Mesh { get; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600001B RID: 27
		// (set) Token: 0x0600001C RID: 28
		bool Solid { get; set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600001D RID: 29
		VertexArray DebugVerts { get; }

		// Token: 0x0600001E RID: 30
		void Collision(CollisionContext context);
	}
}

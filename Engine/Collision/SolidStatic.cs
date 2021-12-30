using System;
using Carbine.Utility;
using SFML.Graphics;
using SFML.System;

namespace Carbine.Collision
{
	// Token: 0x02000019 RID: 25
	public class SolidStatic : ICollidable
	{
		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x000046F8 File Offset: 0x000028F8
		// (set) Token: 0x060000C5 RID: 197 RVA: 0x00004700 File Offset: 0x00002900
		public Vector2f Position { get; set; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x00004709 File Offset: 0x00002909
		public Vector2f Velocity
		{
			get
			{
				return VectorMath.ZERO_VECTOR;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x00004710 File Offset: 0x00002910
		// (set) Token: 0x060000C8 RID: 200 RVA: 0x00004718 File Offset: 0x00002918
		public AABB AABB { get; private set; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000C9 RID: 201 RVA: 0x00004721 File Offset: 0x00002921
		// (set) Token: 0x060000CA RID: 202 RVA: 0x00004729 File Offset: 0x00002929
		public Mesh Mesh { get; private set; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000CB RID: 203 RVA: 0x00004732 File Offset: 0x00002932
		// (set) Token: 0x060000CC RID: 204 RVA: 0x0000473A File Offset: 0x0000293A
		public bool Solid { get; set; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000CD RID: 205 RVA: 0x00004743 File Offset: 0x00002943
		// (set) Token: 0x060000CE RID: 206 RVA: 0x0000474B File Offset: 0x0000294B
		public VertexArray DebugVerts { get; private set; }

		// Token: 0x060000CF RID: 207 RVA: 0x00004754 File Offset: 0x00002954
		public SolidStatic(Mesh mesh)
		{
			this.Mesh = mesh;
			this.AABB = mesh.AABB;
			this.Position = new Vector2f(0f, 0f);
			this.Solid = true;
			VertexArray vertexArray = new VertexArray(PrimitiveType.LinesStrip, (uint)(mesh.Vertices.Count + 1));
			for (int i = 0; i < mesh.Vertices.Count; i++)
			{
				vertexArray[(uint)i] = new Vertex(mesh.Vertices[i], Color.Red);
			}
			vertexArray[(uint)mesh.Vertices.Count] = new Vertex(mesh.Vertices[0], Color.Red);
			this.DebugVerts = vertexArray;
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x0000480B File Offset: 0x00002A0B
		public void Collision(CollisionContext context)
		{
		}
	}
}

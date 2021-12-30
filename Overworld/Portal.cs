using System;
using Carbine.Collision;
using Carbine.Utility;
using SFML.Graphics;
using SFML.System;

namespace Mother4.Overworld
{
	// Token: 0x02000100 RID: 256
	internal class Portal : ICollidable
	{
		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060005E4 RID: 1508 RVA: 0x000230CD File Offset: 0x000212CD
		// (set) Token: 0x060005E5 RID: 1509 RVA: 0x000230D5 File Offset: 0x000212D5
		public Vector2f Position
		{
			get
			{
				return this.position;
			}
			set
			{
				this.position = value;
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060005E6 RID: 1510 RVA: 0x000230DE File Offset: 0x000212DE
		public Vector2f Velocity
		{
			get
			{
				return VectorMath.ZERO_VECTOR;
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060005E7 RID: 1511 RVA: 0x000230E5 File Offset: 0x000212E5
		public AABB AABB
		{
			get
			{
				return this.mesh.AABB;
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060005E8 RID: 1512 RVA: 0x000230F2 File Offset: 0x000212F2
		public Mesh Mesh
		{
			get
			{
				return this.mesh;
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060005E9 RID: 1513 RVA: 0x000230FA File Offset: 0x000212FA
		// (set) Token: 0x060005EA RID: 1514 RVA: 0x00023102 File Offset: 0x00021302
		public bool Solid
		{
			get
			{
				return this.solid;
			}
			set
			{
				this.solid = value;
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x060005EB RID: 1515 RVA: 0x0002310B File Offset: 0x0002130B
		public string Map
		{
			get
			{
				return this.map;
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060005EC RID: 1516 RVA: 0x00023113 File Offset: 0x00021313
		public Vector2f PositionTo
		{
			get
			{
				return this.positionTo;
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060005ED RID: 1517 RVA: 0x0002311B File Offset: 0x0002131B
		public int DirectionTo
		{
			get
			{
				return this.directionTo;
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060005EE RID: 1518 RVA: 0x00023123 File Offset: 0x00021323
		// (set) Token: 0x060005EF RID: 1519 RVA: 0x0002312B File Offset: 0x0002132B
		public VertexArray DebugVerts { get; private set; }

		// Token: 0x060005F0 RID: 1520 RVA: 0x00023134 File Offset: 0x00021334
		public Portal(int x, int y, int width, int height, int xTo, int yTo, int dirTo, string map)
		{
			this.position = new Vector2f((float)x, (float)y);
			this.positionTo = new Vector2f((float)xTo, (float)yTo);
			this.directionTo = dirTo;
			this.mesh = new Mesh(new FloatRect(VectorMath.ZERO_VECTOR, new Vector2f((float)width, (float)height)));
			this.map = map;
			this.solid = true;
			VertexArray vertexArray = new VertexArray(PrimitiveType.LinesStrip, (uint)(this.mesh.Vertices.Count + 1));
			for (int i = 0; i < this.mesh.Vertices.Count; i++)
			{
				vertexArray[(uint)i] = new Vertex(this.mesh.Vertices[i], Color.Blue);
			}
			vertexArray[(uint)this.mesh.Vertices.Count] = new Vertex(this.mesh.Vertices[0], Color.Blue);
			this.DebugVerts = vertexArray;
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x0002322B File Offset: 0x0002142B
		public void Collision(CollisionContext context)
		{
		}

		// Token: 0x040007AC RID: 1964
		private Vector2f position;

		// Token: 0x040007AD RID: 1965
		private Vector2f positionTo;

		// Token: 0x040007AE RID: 1966
		private int directionTo;

		// Token: 0x040007AF RID: 1967
		private Mesh mesh;

		// Token: 0x040007B0 RID: 1968
		private bool solid;

		// Token: 0x040007B1 RID: 1969
		private string map;
	}
}

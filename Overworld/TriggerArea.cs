using System;
using System.Collections.Generic;
using Carbine.Collision;
using Carbine.Utility;
using SFML.Graphics;
using SFML.System;

namespace Mother4.Overworld
{
	// Token: 0x02000105 RID: 261
	internal class TriggerArea : ICollidable
	{
		// Token: 0x170000FC RID: 252
		// (get) Token: 0x06000603 RID: 1539 RVA: 0x0002385C File Offset: 0x00021A5C
		// (set) Token: 0x06000604 RID: 1540 RVA: 0x00023864 File Offset: 0x00021A64
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

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x06000605 RID: 1541 RVA: 0x0002386D File Offset: 0x00021A6D
		public Vector2f Velocity
		{
			get
			{
				return VectorMath.ZERO_VECTOR;
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x06000606 RID: 1542 RVA: 0x00023874 File Offset: 0x00021A74
		public AABB AABB
		{
			get
			{
				return this.mesh.AABB;
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x06000607 RID: 1543 RVA: 0x00023881 File Offset: 0x00021A81
		public Mesh Mesh
		{
			get
			{
				return this.mesh;
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000608 RID: 1544 RVA: 0x00023889 File Offset: 0x00021A89
		// (set) Token: 0x06000609 RID: 1545 RVA: 0x00023891 File Offset: 0x00021A91
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

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x0600060A RID: 1546 RVA: 0x0002389A File Offset: 0x00021A9A
		public int Flag
		{
			get
			{
				return this.flag;
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x0600060B RID: 1547 RVA: 0x000238A2 File Offset: 0x00021AA2
		public string Script
		{
			get
			{
				return this.script;
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x0600060C RID: 1548 RVA: 0x000238AA File Offset: 0x00021AAA
		// (set) Token: 0x0600060D RID: 1549 RVA: 0x000238B2 File Offset: 0x00021AB2
		public VertexArray DebugVerts { get; private set; }

		// Token: 0x0600060E RID: 1550 RVA: 0x000238BC File Offset: 0x00021ABC
		public TriggerArea(Vector2f position, List<Vector2f> points, int flag, string script)
		{
			this.position = position;
			this.mesh = new Mesh(points);
			this.flag = flag;
			this.script = script;
			this.solid = true;
			VertexArray vertexArray = new VertexArray(PrimitiveType.LinesStrip, (uint)(this.mesh.Vertices.Count + 1));
			for (int i = 0; i < this.mesh.Vertices.Count; i++)
			{
				vertexArray[(uint)i] = new Vertex(this.mesh.Vertices[i], Color.Magenta);
			}
			vertexArray[(uint)this.mesh.Vertices.Count] = new Vertex(this.mesh.Vertices[0], Color.Magenta);
			this.DebugVerts = vertexArray;
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x00023986 File Offset: 0x00021B86
		public void Collision(CollisionContext context)
		{
		}

		// Token: 0x040007D9 RID: 2009
		private Vector2f position;

		// Token: 0x040007DA RID: 2010
		private Mesh mesh;

		// Token: 0x040007DB RID: 2011
		private bool solid;

		// Token: 0x040007DC RID: 2012
		private int flag;

		// Token: 0x040007DD RID: 2013
		private string script;
	}
}

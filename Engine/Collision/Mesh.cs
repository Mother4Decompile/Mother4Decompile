using System;
using System.Collections.Generic;
using Carbine.Utility;
using SFML.Graphics;
using SFML.System;

namespace Carbine.Collision
{
	// Token: 0x02000017 RID: 23
	public class Mesh
	{
		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x00004440 File Offset: 0x00002640
		public List<Vector2f> Vertices
		{
			get
			{
				return this.vertices;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000BA RID: 186 RVA: 0x00004448 File Offset: 0x00002648
		public List<Vector2f> Edges
		{
			get
			{
				return this.edges;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000BB RID: 187 RVA: 0x00004450 File Offset: 0x00002650
		public List<Vector2f> Normals
		{
			get
			{
				return this.normals;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000BC RID: 188 RVA: 0x00004458 File Offset: 0x00002658
		public AABB AABB
		{
			get
			{
				return this.aabb;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000BD RID: 189 RVA: 0x00004460 File Offset: 0x00002660
		public Vector2f Center
		{
			get
			{
				return new Vector2f(this.aabb.Size.X / 2f, this.aabb.Size.Y / 2f);
			}
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00004493 File Offset: 0x00002693
		public Mesh(List<Vector2f> points)
		{
			this.AddPoints(points);
		}

		// Token: 0x060000BF RID: 191 RVA: 0x000044A2 File Offset: 0x000026A2
		public Mesh(FloatRect rectangle)
		{
			this.AddRectangle(rectangle.Left, rectangle.Top, rectangle.Width, rectangle.Height);
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x000044CC File Offset: 0x000026CC
		public Mesh(IntRect rectangle)
		{
			this.AddRectangle((float)rectangle.Left, (float)rectangle.Top, (float)rectangle.Width, (float)rectangle.Height);
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x000044FC File Offset: 0x000026FC
		private void AddPoints(List<Vector2f> points)
		{
			this.vertices = new List<Vector2f>();
			this.edges = new List<Vector2f>();
			this.normals = new List<Vector2f>();
			for (int i = 0; i < points.Count; i++)
			{
				this.vertices.Add(points[i]);
				int index = (i + 1) % points.Count;
				float x = points[index].X - points[i].X;
				float y = points[index].Y - points[i].Y;
				Vector2f vector2f = new Vector2f(x, y);
				this.edges.Add(vector2f);
				Vector2f item = VectorMath.RightNormal(vector2f);
				this.normals.Add(item);
			}
			this.aabb = this.GetAABB();
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x000045CC File Offset: 0x000027CC
		private void AddRectangle(float x, float y, float width, float height)
		{
			this.AddPoints(new List<Vector2f>
			{
				new Vector2f(x, y),
				new Vector2f(x + width, y),
				new Vector2f(x + width, y + height),
				new Vector2f(x, y + height)
			});
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00004624 File Offset: 0x00002824
		private AABB GetAABB()
		{
			float num = float.MinValue;
			float num2 = float.MinValue;
			float num3 = float.MaxValue;
			float num4 = float.MaxValue;
			foreach (Vector2f vector2f in this.vertices)
			{
				num3 = ((vector2f.X < num3) ? vector2f.X : num3);
				num = ((vector2f.X > num) ? vector2f.X : num);
				num4 = ((vector2f.Y < num4) ? vector2f.Y : num4);
				num2 = ((vector2f.Y > num2) ? vector2f.Y : num2);
			}
			return new AABB(new Vector2f(num3, num4), new Vector2f(num - num3, num2 - num4));
		}

		// Token: 0x04000055 RID: 85
		private List<Vector2f> vertices;

		// Token: 0x04000056 RID: 86
		private List<Vector2f> edges;

		// Token: 0x04000057 RID: 87
		private List<Vector2f> normals;

		// Token: 0x04000058 RID: 88
		private AABB aabb;
	}
}

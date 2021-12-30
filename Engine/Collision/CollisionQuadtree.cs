using System;
using Carbine.Utility;
using SFML.Graphics;

namespace Carbine.Collision
{
	// Token: 0x02000016 RID: 22
	internal class CollisionQuadtree : Quadtree<ICollidable>
	{
		// Token: 0x060000B5 RID: 181 RVA: 0x000040D8 File Offset: 0x000022D8
		public CollisionQuadtree(int level, Rectangle bounds) : base(level, bounds)
		{
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x000040E4 File Offset: 0x000022E4
		protected override void Split()
		{
			int num = (int)(this.bounds.Width / 2f);
			int num2 = (int)(this.bounds.Height / 2f);
			int num3 = (int)this.bounds.X;
			int num4 = (int)this.bounds.Y;
			int level = this.level + 1;
			this.nodes[0] = new CollisionQuadtree(level, new Rectangle((float)(num3 + num), (float)num4, (float)num, (float)num2));
			this.nodes[1] = new CollisionQuadtree(level, new Rectangle((float)num3, (float)num4, (float)num, (float)num2));
			this.nodes[2] = new CollisionQuadtree(level, new Rectangle((float)num3, (float)(num4 + num2), (float)num, (float)num2));
			this.nodes[3] = new CollisionQuadtree(level, new Rectangle((float)(num3 + num), (float)(num4 + num2), (float)num, (float)num2));
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x000041B4 File Offset: 0x000023B4
		protected override int FindIndex(ICollidable obj)
		{
			int result = -1;
			float num = this.bounds.X + this.bounds.Width / 2f;
			float num2 = this.bounds.Y + this.bounds.Height / 2f;
			bool flag = obj.Position.Y + obj.AABB.Position.Y < num2 && obj.Position.Y + obj.AABB.Position.Y + obj.AABB.Size.Y < num2;
			bool flag2 = obj.Position.Y + obj.AABB.Position.Y > num2;
			bool flag3 = obj.Position.X + obj.AABB.Position.X < num && obj.Position.X + obj.AABB.Position.X + obj.AABB.Size.X < num;
			bool flag4 = obj.Position.X + obj.AABB.Position.X > num;
			if (flag3)
			{
				if (flag)
				{
					result = 1;
				}
				else if (flag2)
				{
					result = 2;
				}
			}
			else if (flag4)
			{
				if (flag)
				{
					result = 0;
				}
				else if (flag2)
				{
					result = 3;
				}
			}
			return result;
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00004310 File Offset: 0x00002510
		public override void DebugDraw(RenderTarget target)
		{
			base.DebugDraw(target);
			foreach (ICollidable collidable in this.objects)
			{
				Vertex[] array = new Vertex[collidable.Mesh.Vertices.Count + 1];
				for (int i = 0; i < collidable.Mesh.Vertices.Count; i++)
				{
					array[i] = new Vertex(collidable.Mesh.Vertices[i] + collidable.Position);
					array[i].Color = Color.Red;
				}
				array[collidable.Mesh.Vertices.Count] = new Vertex(collidable.Mesh.Vertices[0] + collidable.Position);
				array[collidable.Mesh.Vertices.Count].Color = Color.Red;
				target.Draw(array, PrimitiveType.LinesStrip);
			}
		}
	}
}

using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace Carbine.Utility
{
	// Token: 0x02000015 RID: 21
	internal class Quadtree<T> where T : class
	{
		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000A6 RID: 166 RVA: 0x00003C86 File Offset: 0x00001E86
		// (set) Token: 0x060000A7 RID: 167 RVA: 0x00003C8E File Offset: 0x00001E8E
		public IComparer<T> Comparer { get; set; }

		// Token: 0x060000A8 RID: 168 RVA: 0x00003C97 File Offset: 0x00001E97
		public Quadtree(int level, Rectangle bounds)
		{
			this.level = level;
			this.bounds = bounds;
			this.objects = new List<T>();
			this.nodes = new Quadtree<T>[4];
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00003CC4 File Offset: 0x00001EC4
		protected Quadtree()
		{
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00003CCC File Offset: 0x00001ECC
		public virtual void Clear()
		{
			this.objects.Clear();
			for (int i = 0; i < this.nodes.Length; i++)
			{
				if (this.nodes[i] != null)
				{
					this.nodes[i].Clear();
					this.nodes[i] = null;
				}
			}
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00003D18 File Offset: 0x00001F18
		protected virtual void Split()
		{
			int num = (int)(this.bounds.Width / 2f);
			int num2 = (int)(this.bounds.Height / 2f);
			int num3 = (int)this.bounds.X;
			int num4 = (int)this.bounds.Y;
			int num5 = this.level + 1;
			this.nodes[0] = new Quadtree<T>(num5, new Rectangle((float)(num3 + num), (float)num4, (float)num, (float)num2));
			this.nodes[1] = new Quadtree<T>(num5, new Rectangle((float)num3, (float)num4, (float)num, (float)num2));
			this.nodes[2] = new Quadtree<T>(num5, new Rectangle((float)num3, (float)(num4 + num2), (float)num, (float)num2));
			this.nodes[3] = new Quadtree<T>(num5, new Rectangle((float)(num3 + num), (float)(num4 + num2), (float)num, (float)num2));
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00003DE7 File Offset: 0x00001FE7
		protected virtual int FindIndex(T obj)
		{
			throw new NotImplementedException("FindIndex must be overridden for this type.");
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00003DF4 File Offset: 0x00001FF4
		protected virtual int FindIndex(Vector2f point)
		{
			int result = -1;
			float num = this.bounds.X + this.bounds.Width / 2f;
			float num2 = this.bounds.Y + this.bounds.Height / 2f;
			bool flag = point.X < num2;
			bool flag2 = point.Y >= num2;
			bool flag3 = point.X < num;
			bool flag4 = point.X >= num;
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

		// Token: 0x060000AE RID: 174 RVA: 0x00003E9C File Offset: 0x0000209C
		public virtual void Insert(T obj)
		{
			if (this.nodes[0] != null)
			{
				int num = this.FindIndex(obj);
				if (num != -1)
				{
					this.nodes[num].Insert(obj);
					return;
				}
			}
			this.objects.Add(obj);
			if (this.objects.Count > 10 && this.level < 5)
			{
				if (this.nodes[0] == null)
				{
					this.Split();
				}
				int i = 0;
				while (i < this.objects.Count)
				{
					int num2 = this.FindIndex(this.objects[i]);
					if (num2 != -1)
					{
						this.nodes[num2].Insert(this.objects[i]);
						this.objects.RemoveAt(i);
					}
					else
					{
						i++;
					}
				}
			}
			if (this.Comparer != null)
			{
				this.objects.Sort(this.Comparer);
			}
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00003F70 File Offset: 0x00002170
		public virtual void Remove(T obj)
		{
			if (this.nodes[0] != null)
			{
				int num = this.FindIndex(obj);
				if (num != -1)
				{
					this.nodes[num].Remove(obj);
				}
			}
			this.objects.Remove(obj);
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00003FB0 File Offset: 0x000021B0
		public virtual List<T> Retrieve(T obj)
		{
			List<T> returnList = new List<T>();
			return this.Retrieve(returnList, obj);
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00003FCC File Offset: 0x000021CC
		protected List<T> Retrieve(List<T> returnList, T obj)
		{
			int num = this.FindIndex(obj);
			if (this.nodes[0] != null)
			{
				if (num != -1)
				{
					this.nodes[num].Retrieve(returnList, obj);
				}
				else
				{
					for (int i = 0; i < 4; i++)
					{
						this.nodes[i].Retrieve(returnList, obj);
					}
				}
			}
			returnList.AddRange(this.objects);
			return returnList;
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x0000402C File Offset: 0x0000222C
		public virtual List<T> Retrieve(Vector2f point)
		{
			List<T> returnList = new List<T>();
			return this.Retrieve(returnList, point);
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00004048 File Offset: 0x00002248
		protected List<T> Retrieve(List<T> returnList, Vector2f point)
		{
			int num = this.FindIndex(point);
			if (this.nodes[0] != null)
			{
				if (num != -1)
				{
					this.nodes[num].Retrieve(returnList, point);
				}
				else
				{
					for (int i = 0; i < 4; i++)
					{
						this.nodes[i].Retrieve(returnList, point);
					}
				}
			}
			returnList.AddRange(this.objects);
			return returnList;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x000040A8 File Offset: 0x000022A8
		public virtual void DebugDraw(RenderTarget target)
		{
			foreach (Quadtree<T> quadtree in this.nodes)
			{
				if (quadtree != null)
				{
					quadtree.DebugDraw(target);
				}
			}
		}

		// Token: 0x0400004E RID: 78
		private const int MAX_OBJECTS = 10;

		// Token: 0x0400004F RID: 79
		private const int MAX_LEVELS = 5;

		// Token: 0x04000050 RID: 80
		protected int level;

		// Token: 0x04000051 RID: 81
		protected List<T> objects;

		// Token: 0x04000052 RID: 82
		protected Rectangle bounds;

		// Token: 0x04000053 RID: 83
		protected Quadtree<T>[] nodes;
	}
}

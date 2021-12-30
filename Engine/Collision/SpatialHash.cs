using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace Carbine.Collision
{
	// Token: 0x0200001A RID: 26
	internal class SpatialHash
	{
		// Token: 0x060000D1 RID: 209 RVA: 0x00004810 File Offset: 0x00002A10
		public SpatialHash(int width, int height)
		{
			this.widthInPixels = width;
			this.heightInPixels = height;
			this.widthInCells = (this.widthInPixels - 1) / 256 + 1;
			this.heightInCells = (this.heightInPixels - 1) / 256 + 1;
			int num = this.widthInCells * this.heightInCells;
			this.buckets = new ICollidable[num][];
			this.touches = new bool[num];
			this.InitializeDebugGrid();
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x0000488C File Offset: 0x00002A8C
		private void InitializeDebugGrid()
		{
			uint vertexCount = (uint)((this.widthInCells + this.heightInCells + 2) * 2);
			this.debugGridVerts = new VertexArray(PrimitiveType.Lines, vertexCount);
			int num = this.widthInCells * 256;
			int num2 = this.heightInCells * 256;
			uint num3 = 0U;
			while ((ulong)num3 <= (ulong)((long)this.widthInCells))
			{
				this.debugGridVerts[num3 * 2U] = new Vertex(new Vector2f(num3 * 256U, 0f), Color.Blue);
				this.debugGridVerts[num3 * 2U + 1U] = new Vertex(new Vector2f(num3 * 256U, (float)num2), Color.Blue);
				num3 += 1U;
			}
			uint num4 = (uint)((this.widthInCells + 1) * 2);
			uint num5 = 0U;
			while ((ulong)num5 <= (ulong)((long)this.heightInCells))
			{
				this.debugGridVerts[num4 + num5 * 2U] = new Vertex(new Vector2f(0f, num5 * 256U), Color.Blue);
				this.debugGridVerts[num4 + num5 * 2U + 1U] = new Vertex(new Vector2f((float)num, num5 * 256U), Color.Blue);
				num5 += 1U;
			}
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x000049BC File Offset: 0x00002BBC
		private void ClearTouches()
		{
			Array.Clear(this.touches, 0, this.touches.Length);
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x000049D4 File Offset: 0x00002BD4
		private int GetPositionHash(int x, int y)
		{
			int num = x / 256;
			int num2 = y / 256;
			return num + num2 * this.widthInCells;
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x000049FC File Offset: 0x00002BFC
		private void BucketInsert(int hash, ICollidable collidable)
		{
			int num = -1;
			ICollidable[] array = this.buckets[hash];
			if (array == null)
			{
				this.buckets[hash] = new ICollidable[4];
				array = this.buckets[hash];
			}
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == collidable)
				{
					return;
				}
				if (num < 0 && array[i] == null)
				{
					num = i;
				}
			}
			if (num >= 0)
			{
				array[num] = collidable;
				return;
			}
			int num2 = array.Length;
			if (num2 * 2 <= 512)
			{
				Array.Resize<ICollidable>(ref array, num2 * 2);
				array[num2] = collidable;
				this.buckets[hash] = array;
				return;
			}
			string message = string.Format("Cannot to insert more than {0} collidables into a single bucket.", 512);
			throw new InvalidOperationException(message);
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00004A9C File Offset: 0x00002C9C
		private void BucketRemove(int hash, ICollidable collidable)
		{
			ICollidable[] array = this.buckets[hash];
			if (array != null)
			{
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] == collidable)
					{
						array[i] = null;
						return;
					}
				}
			}
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00004AD0 File Offset: 0x00002CD0
		public void Insert(ICollidable collidable)
		{
			this.ClearTouches();
			AABB aabb = collidable.AABB;
			int num = ((int)aabb.Size.X - 1) / 256 + 1;
			int num2 = ((int)aabb.Size.Y - 1) / 256 + 1;
			for (int i = 0; i <= num2; i++)
			{
				int y = (i == num2) ? ((int)(collidable.Position.Y + aabb.Position.Y) + (int)aabb.Size.Y) : ((int)(collidable.Position.Y + aabb.Position.Y) + 256 * i);
				for (int j = 0; j <= num; j++)
				{
					int x = (j == num) ? ((int)(collidable.Position.X + aabb.Position.X) + (int)aabb.Size.X) : ((int)(collidable.Position.X + aabb.Position.X) + 256 * j);
					int positionHash = this.GetPositionHash(x, y);
					if (positionHash >= 0 && positionHash < this.buckets.Length && !this.touches[positionHash])
					{
						this.touches[positionHash] = true;
						this.BucketInsert(positionHash, collidable);
					}
				}
			}
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00004C20 File Offset: 0x00002E20
		public void Update(ICollidable collidable, Vector2f oldPosition, Vector2f newPosition)
		{
			this.ClearTouches();
			AABB aabb = collidable.AABB;
			int num = ((int)aabb.Size.X - 1) / 256 + 1;
			int num2 = ((int)aabb.Size.Y - 1) / 256 + 1;
			for (int i = 0; i <= num2; i++)
			{
				int y = (i == num2) ? ((int)(oldPosition.Y + aabb.Position.Y) + (int)aabb.Size.Y) : ((int)(oldPosition.Y + aabb.Position.Y) + 256 * i);
				int y2 = (i == num2) ? ((int)(newPosition.Y + aabb.Position.Y) + (int)aabb.Size.Y) : ((int)(newPosition.Y + aabb.Position.Y) + 256 * i);
				for (int j = 0; j <= num; j++)
				{
					int x = (j == num) ? ((int)(oldPosition.X + aabb.Position.X) + (int)aabb.Size.X) : ((int)(oldPosition.X + aabb.Position.X) + 256 * j);
					int x2 = (j == num) ? ((int)(newPosition.X + aabb.Position.X) + (int)aabb.Size.X) : ((int)(newPosition.X + aabb.Position.X) + 256 * j);
					int positionHash = this.GetPositionHash(x, y);
					int positionHash2 = this.GetPositionHash(x2, y2);
					bool flag = positionHash >= 0 && positionHash < this.buckets.Length;
					bool flag2 = positionHash2 >= 0 && positionHash2 < this.buckets.Length;
					if ((flag && !this.touches[positionHash]) || (flag2 && !this.touches[positionHash2]))
					{
						if (flag && positionHash != positionHash2)
						{
							this.BucketRemove(positionHash, collidable);
						}
						if (flag2 && positionHash != positionHash2)
						{
							this.BucketInsert(positionHash2, collidable);
						}
						if (flag)
						{
							this.touches[positionHash] = true;
						}
						if (flag2)
						{
							this.touches[positionHash2] = true;
						}
					}
				}
			}
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00004E5C File Offset: 0x0000305C
		public void Remove(ICollidable collidable)
		{
			this.ClearTouches();
			AABB aabb = collidable.AABB;
			int num = ((int)aabb.Size.X - 1) / 256 + 1;
			int num2 = ((int)aabb.Size.Y - 1) / 256 + 1;
			for (int i = 0; i <= num2; i++)
			{
				int y = (i == num2) ? ((int)(collidable.Position.Y + aabb.Position.Y) + (int)aabb.Size.Y) : ((int)(collidable.Position.Y + aabb.Position.Y) + 256 * i);
				for (int j = 0; j <= num; j++)
				{
					int x = (j == num) ? ((int)(collidable.Position.X + aabb.Position.X) + (int)aabb.Size.X) : ((int)(collidable.Position.X + aabb.Position.X) + 256 * j);
					int positionHash = this.GetPositionHash(x, y);
					if (positionHash >= 0 && positionHash < this.buckets.Length && !this.touches[positionHash])
					{
						this.touches[positionHash] = true;
						this.BucketRemove(positionHash, collidable);
					}
				}
			}
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00004FAC File Offset: 0x000031AC
		public void Query(Vector2f point, Stack<ICollidable> resultStack)
		{
			int positionHash = this.GetPositionHash((int)point.X, (int)point.Y);
			if (positionHash < 0 || positionHash >= this.buckets.Length || this.touches[positionHash])
			{
				return;
			}
			ICollidable[] array = this.buckets[positionHash];
			if (array != null)
			{
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] != null)
					{
						resultStack.Push(array[i]);
					}
				}
			}
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00005012 File Offset: 0x00003212
		public void Query(ICollidable collidable, Stack<ICollidable> resultStack)
		{
			this.Query(collidable, new Vector2f(0f, 0f), resultStack);
		}

		// Token: 0x060000DC RID: 220 RVA: 0x0000502C File Offset: 0x0000322C
		public void Query(ICollidable collidable, Vector2f offset, Stack<ICollidable> resultStack)
		{
			this.ClearTouches();
			Vector2f vector2f = collidable.Position + offset;
			AABB aabb = collidable.AABB;
			int num = ((int)aabb.Size.X - 1) / 256 + 1;
			int num2 = ((int)aabb.Size.Y - 1) / 256 + 1;
			for (int i = 0; i <= num2; i++)
			{
				int y = (i == num2) ? ((int)(vector2f.Y + aabb.Position.Y) + (int)aabb.Size.Y) : ((int)(vector2f.Y + aabb.Position.Y) + 256 * i);
				for (int j = 0; j <= num; j++)
				{
					int x = (j == num) ? ((int)(vector2f.X + aabb.Position.X) + (int)aabb.Size.X) : ((int)(vector2f.X + aabb.Position.X) + 256 * j);
					int positionHash = this.GetPositionHash(x, y);
					if (positionHash >= 0 && positionHash < this.buckets.Length && !this.touches[positionHash])
					{
						this.touches[positionHash] = true;
						ICollidable[] array = this.buckets[positionHash];
						if (array != null)
						{
							for (int k = 0; k < array.Length; k++)
							{
								if (array[k] != null && array[k] != collidable)
								{
									resultStack.Push(array[k]);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060000DD RID: 221 RVA: 0x000051B4 File Offset: 0x000033B4
		public void Clear()
		{
			for (int i = 0; i < this.buckets.Length; i++)
			{
				ICollidable[] array = this.buckets[i];
				if (array != null)
				{
					for (int j = 0; j < array.Length; j++)
					{
						array[j] = null;
					}
				}
			}
		}

		// Token: 0x060000DE RID: 222 RVA: 0x000051F4 File Offset: 0x000033F4
		public void DebugDraw(RenderTarget target)
		{
			RenderStates states = new RenderStates(BlendMode.Alpha, Transform.Identity, null, null);
			for (int i = 0; i < this.buckets.Length; i++)
			{
				ICollidable[] array = this.buckets[i];
				if (array != null)
				{
					foreach (ICollidable collidable in array)
					{
						if (collidable != null && collidable.DebugVerts != null)
						{
							states.Transform = Transform.Identity;
							states.Transform.Translate(collidable.Position);
							target.Draw(collidable.DebugVerts, states);
						}
					}
				}
			}
			target.Draw(this.debugGridVerts);
		}

		// Token: 0x04000060 RID: 96
		internal const int CELL_SIZE = 256;

		// Token: 0x04000061 RID: 97
		internal const int INITIAL_BUCKET_SIZE = 4;

		// Token: 0x04000062 RID: 98
		internal const int MAX_BUCKET_SIZE = 512;

		// Token: 0x04000063 RID: 99
		private int widthInPixels;

		// Token: 0x04000064 RID: 100
		private int heightInPixels;

		// Token: 0x04000065 RID: 101
		private int widthInCells;

		// Token: 0x04000066 RID: 102
		private int heightInCells;

		// Token: 0x04000067 RID: 103
		private ICollidable[][] buckets;

		// Token: 0x04000068 RID: 104
		private bool[] touches;

		// Token: 0x04000069 RID: 105
		private VertexArray debugGridVerts;
	}
}

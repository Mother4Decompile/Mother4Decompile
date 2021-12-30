using System;
using System.Collections.Generic;
using Carbine.Utility;
using SFML.Graphics;
using SFML.System;

namespace Carbine.Collision
{
	// Token: 0x02000014 RID: 20
	public class CollisionManager
	{
		// Token: 0x06000097 RID: 151 RVA: 0x00003796 File Offset: 0x00001996
		public CollisionManager(int width, int height)
		{
			this.spatialHash = new SpatialHash(width, height);
			this.resultStack = new Stack<ICollidable>(512);
			this.resultList = new List<ICollidable>(4);
		}

		// Token: 0x06000098 RID: 152 RVA: 0x000037C7 File Offset: 0x000019C7
		public void Add(ICollidable collidable)
		{
			this.spatialHash.Insert(collidable);
		}

		// Token: 0x06000099 RID: 153 RVA: 0x000037D8 File Offset: 0x000019D8
		public void AddAll<T>(ICollection<T> collidables) where T : ICollidable
		{
			foreach (T t in collidables)
			{
				ICollidable collidable = t;
				this.Add(collidable);
			}
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00003828 File Offset: 0x00001A28
		public void Remove(ICollidable collidable)
		{
			this.spatialHash.Remove(collidable);
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00003836 File Offset: 0x00001A36
		public void Update(ICollidable collidable, Vector2f oldPosition, Vector2f newPosition)
		{
			this.spatialHash.Update(collidable, oldPosition, newPosition);
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00003846 File Offset: 0x00001A46
		public bool PlaceFree(ICollidable obj, Vector2f position)
		{
			return this.PlaceFree(obj, position, null);
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00003851 File Offset: 0x00001A51
		public bool PlaceFree(ICollidable obj, Vector2f position, ICollidable[] collisionResults)
		{
			return this.PlaceFree(obj, position, collisionResults, null);
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00003860 File Offset: 0x00001A60
		public bool PlaceFree(ICollidable obj, Vector2f position, ICollidable[] collisionResults, Type[] ignoreTypes)
		{
			if (collisionResults != null)
			{
				Array.Clear(collisionResults, 0, collisionResults.Length);
			}
			bool flag = false;
			Vector2f offset = obj.Position - position;
			this.resultList.Clear();
			this.spatialHash.Query(obj, offset, this.resultStack);
			int num = 0;
			while (this.resultStack.Count > 0)
			{
				ICollidable collidable = this.resultStack.Pop();
				if (this.PlaceFreeBroadPhase(obj, position, collidable))
				{
					bool flag2 = this.CheckPositionCollision(obj, position, collidable);
					if (flag2)
					{
						bool flag3 = false;
						if (ignoreTypes != null)
						{
							for (int i = 0; i < ignoreTypes.Length; i++)
							{
								if (ignoreTypes[i] == collidable.GetType())
								{
									flag3 = true;
									break;
								}
							}
						}
						if (!flag3)
						{
							flag = true;
							if (collisionResults == null || num >= collisionResults.Length)
							{
								break;
							}
							collisionResults[num] = collidable;
							num++;
						}
					}
				}
			}
			this.resultStack.Clear();
			return !flag;
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00003938 File Offset: 0x00001B38
		public IEnumerable<ICollidable> ObjectsAtPosition(Vector2f position)
		{
			this.resultList.Clear();
			this.spatialHash.Query(position, this.resultStack);
			while (this.resultStack.Count > 0)
			{
				ICollidable collidable = this.resultStack.Pop();
				if (position.X >= collidable.Position.X + collidable.AABB.Position.X && position.X < collidable.Position.X + collidable.AABB.Position.X + collidable.AABB.Size.X && position.Y >= collidable.Position.Y + collidable.AABB.Position.Y && position.Y < collidable.Position.Y + collidable.AABB.Position.Y + collidable.AABB.Size.Y)
				{
					this.resultList.Add(collidable);
				}
			}
			return this.resultList;
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00003A50 File Offset: 0x00001C50
		private bool PlaceFreeBroadPhase(ICollidable objA, Vector2f position, ICollidable objB)
		{
			if (objA == objB)
			{
				return false;
			}
			if (objA.AABB.OnlyPlayer && !objB.AABB.IsPlayer)
			{
				return false;
			}
			if (!objA.Solid || !objB.Solid)
			{
				return false;
			}
			FloatRect floatRect = objA.AABB.GetFloatRect();
			floatRect.Left += position.X;
			floatRect.Top += position.Y;
			FloatRect floatRect2 = objB.AABB.GetFloatRect();
			floatRect2.Left += objB.Position.X;
			floatRect2.Top += objB.Position.Y;
			return floatRect.Intersects(floatRect2);
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00003B14 File Offset: 0x00001D14
		private bool CheckPositionCollision(ICollidable objA, Vector2f position, ICollidable objB)
		{
			int count = objA.Mesh.Edges.Count;
			int count2 = objB.Mesh.Edges.Count;
			for (int i = 0; i < count + count2; i++)
			{
				Vector2f vector2f;
				if (i < count)
				{
					vector2f = objA.Mesh.Normals[i];
				}
				else
				{
					vector2f = objB.Mesh.Normals[i - count];
				}
				vector2f = VectorMath.Normalize(vector2f);
				float minA = 0f;
				float minB = 0f;
				float maxA = 0f;
				float maxB = 0f;
				this.ProjectPolygon(vector2f, objA.Mesh, position, ref minA, ref maxA);
				this.ProjectPolygon(vector2f, objB.Mesh, objB.Position, ref minB, ref maxB);
				if (this.IntervalDistance(minA, maxA, minB, maxB) > 0f)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00003BE7 File Offset: 0x00001DE7
		private float IntervalDistance(float minA, float maxA, float minB, float maxB)
		{
			if (minA < minB)
			{
				return minB - maxA;
			}
			return minA - maxB;
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00003BF8 File Offset: 0x00001DF8
		private void ProjectPolygon(Vector2f normal, Mesh mesh, Vector2f offset, ref float min, ref float max)
		{
			float num = VectorMath.DotProduct(normal, mesh.Vertices[0] + offset);
			min = num;
			max = num;
			for (int i = 0; i < mesh.Vertices.Count; i++)
			{
				num = VectorMath.DotProduct(mesh.Vertices[i] + offset, normal);
				if (num < min)
				{
					min = num;
				}
				else if (num > max)
				{
					max = num;
				}
			}
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00003C6B File Offset: 0x00001E6B
		public void Clear()
		{
			this.spatialHash.Clear();
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00003C78 File Offset: 0x00001E78
		public void Draw(RenderTarget target)
		{
			this.spatialHash.DebugDraw(target);
		}

		// Token: 0x0400004B RID: 75
		private SpatialHash spatialHash;

		// Token: 0x0400004C RID: 76
		private Stack<ICollidable> resultStack;

		// Token: 0x0400004D RID: 77
		private List<ICollidable> resultList;
	}
}

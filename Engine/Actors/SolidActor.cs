using System;
using Carbine.Collision;
using SFML.Graphics;
using SFML.System;

namespace Carbine.Actors
{
	// Token: 0x02000005 RID: 5
	public abstract class SolidActor : Actor, ICollidable
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600001F RID: 31 RVA: 0x000023C5 File Offset: 0x000005C5
		public Vector2f LastPosition
		{
			get
			{
				return this.lastPosition;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000020 RID: 32 RVA: 0x000023CD File Offset: 0x000005CD
		public AABB AABB
		{
			get
			{
				return this.aabb;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000021 RID: 33 RVA: 0x000023D5 File Offset: 0x000005D5
		public Mesh Mesh
		{
			get
			{
				return this.mesh;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000022 RID: 34 RVA: 0x000023DD File Offset: 0x000005DD
		// (set) Token: 0x06000023 RID: 35 RVA: 0x000023E5 File Offset: 0x000005E5
		public bool Solid
		{
			get
			{
				return this.isSolid;
			}
			set
			{
				this.isSolid = value;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000024 RID: 36 RVA: 0x000023EE File Offset: 0x000005EE
		public VertexArray DebugVerts
		{
			get
			{
				return this.GetDebugVerts();
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000025 RID: 37 RVA: 0x000023F6 File Offset: 0x000005F6
		// (set) Token: 0x06000026 RID: 38 RVA: 0x000023FE File Offset: 0x000005FE
		public virtual bool MovementLocked
		{
			get
			{
				return this.isMovementLocked;
			}
			set
			{
				this.isMovementLocked = value;
			}
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002407 File Offset: 0x00000607
		public SolidActor(CollisionManager colman)
		{
			this.collisionManager = colman;
			this.ignoreCollisionTypes = null;
			this.isSolid = true;
			this.collisionResults = new ICollidable[8];
			if (this.collisionManager != null)
			{
				this.collisionManager.Add(this);
			}
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002444 File Offset: 0x00000644
		private VertexArray GetDebugVerts()
		{
			if (this.debugVerts == null)
			{
				VertexArray vertexArray = new VertexArray(PrimitiveType.LinesStrip, (uint)(this.mesh.Vertices.Count + 1));
				for (int i = 0; i < this.mesh.Vertices.Count; i++)
				{
					vertexArray[(uint)i] = new Vertex(this.mesh.Vertices[i], Color.Green);
				}
				vertexArray[(uint)this.mesh.Vertices.Count] = new Vertex(this.mesh.Vertices[0], Color.Green);
				this.debugVerts = vertexArray;
			}
			return this.debugVerts;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000024F0 File Offset: 0x000006F0
		protected virtual void HandleCollision(ICollidable[] collisionObjects)
		{
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000024F4 File Offset: 0x000006F4
		public override void Update()
		{
			this.lastPosition = this.position;
			if (this.collisionManager != null && !this.collisionManager.PlaceFree(this, this.position, this.collisionResults, this.ignoreCollisionTypes))
			{
				this.HandleCollision(this.collisionResults);
			}
			else
			{
				if (this.velocity.X != 0f && !this.isMovementLocked)
				{
					this.moveTemp = new Vector2f(this.position.X + this.velocity.X, this.position.Y);
					bool flag = this.collisionManager == null || this.collisionManager.PlaceFree(this, this.moveTemp, this.collisionResults, this.ignoreCollisionTypes);
					if (flag)
					{
						this.position = this.moveTemp;
					}
					else
					{
						this.velocity.X = 0f;
						this.HandleCollision(this.collisionResults);
					}
				}
				if (this.Velocity.Y != 0f && !this.isMovementLocked)
				{
					this.moveTemp = new Vector2f(this.position.X, this.position.Y + this.velocity.Y);
					bool flag = this.collisionManager == null || this.collisionManager.PlaceFree(this, this.moveTemp, this.collisionResults, this.ignoreCollisionTypes);
					if (flag)
					{
						this.position = this.moveTemp;
					}
					else
					{
						this.velocity.Y = 0f;
						this.HandleCollision(this.collisionResults);
					}
				}
			}
			if (!this.lastPosition.Equals(this.position) && this.collisionManager != null)
			{
				this.collisionManager.Update(this, this.lastPosition, this.position);
			}
		}

		// Token: 0x0600002B RID: 43 RVA: 0x000026C6 File Offset: 0x000008C6
		public virtual void Collision(CollisionContext context)
		{
		}

		// Token: 0x0400000A RID: 10
		private const int COLLISION_RESULTS_SIZE = 8;

		// Token: 0x0400000B RID: 11
		private VertexArray debugVerts;

		// Token: 0x0400000C RID: 12
		protected bool isMovementLocked;

		// Token: 0x0400000D RID: 13
		protected CollisionManager collisionManager;

		// Token: 0x0400000E RID: 14
		protected bool isSolid;

		// Token: 0x0400000F RID: 15
		protected AABB aabb;

		// Token: 0x04000010 RID: 16
		protected Mesh mesh;

		// Token: 0x04000011 RID: 17
		private Vector2f moveTemp;

		// Token: 0x04000012 RID: 18
		protected Vector2f lastPosition;

		// Token: 0x04000013 RID: 19
		protected Type[] ignoreCollisionTypes;

		// Token: 0x04000014 RID: 20
		private ICollidable[] collisionResults;
	}
}

using System;
using Carbine.Collision;
using Carbine.Graphics;
using Carbine.Utility;
using Mother4.Actors.Animation;
using Mother4.Data;
using Mother4.Overworld;
using SFML.Graphics;
using SFML.System;

namespace Mother4.Actors
{
	// Token: 0x020000A9 RID: 169
	internal class PartyFollower : IDisposable, ICollidable
	{
		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000388 RID: 904 RVA: 0x00016AEC File Offset: 0x00014CEC
		// (set) Token: 0x06000389 RID: 905 RVA: 0x00016AF4 File Offset: 0x00014CF4
		public int Place
		{
			get
			{
				return this.place;
			}
			set
			{
				this.place = value;
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x0600038A RID: 906 RVA: 0x00016AFD File Offset: 0x00014CFD
		public float Width
		{
			get
			{
				return this.followerGraphic.Size.X;
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x0600038B RID: 907 RVA: 0x00016B0F File Offset: 0x00014D0F
		public CharacterType Character
		{
			get
			{
				return this.character;
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x0600038C RID: 908 RVA: 0x00016B17 File Offset: 0x00014D17
		public int Direction
		{
			get
			{
				return this.direction;
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x0600038D RID: 909 RVA: 0x00016B1F File Offset: 0x00014D1F
		// (set) Token: 0x0600038E RID: 910 RVA: 0x00016B27 File Offset: 0x00014D27
		public Vector2f Position
		{
			get
			{
				return this.position;
			}
			set
			{
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x0600038F RID: 911 RVA: 0x00016B29 File Offset: 0x00014D29
		public Vector2f Velocity
		{
			get
			{
				return this.velocity;
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000390 RID: 912 RVA: 0x00016B31 File Offset: 0x00014D31
		public AABB AABB
		{
			get
			{
				return this.aabb;
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000391 RID: 913 RVA: 0x00016B39 File Offset: 0x00014D39
		public Mesh Mesh
		{
			get
			{
				return this.mesh;
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000392 RID: 914 RVA: 0x00016B41 File Offset: 0x00014D41
		// (set) Token: 0x06000393 RID: 915 RVA: 0x00016B49 File Offset: 0x00014D49
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

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000394 RID: 916 RVA: 0x00016B52 File Offset: 0x00014D52
		public VertexArray DebugVerts
		{
			get
			{
				return this.GetDebugVerts();
			}
		}

		// Token: 0x06000395 RID: 917 RVA: 0x00016B5C File Offset: 0x00014D5C
		public PartyFollower(RenderPipeline pipeline, CollisionManager colman, PartyTrain recorder, CharacterType character, Vector2f position, int direction, bool useShadow)
		{
			this.pipeline = pipeline;
			this.recorder = recorder;
			this.character = character;
			this.place = 0;
			this.isDead = (CharacterStats.GetStats(this.character).HP <= 0);
			this.useShadow = (useShadow && !this.isDead);
			this.position = position;
			this.velocity = VectorMath.ZERO_VECTOR;
			this.direction = direction;
			string file = CharacterGraphics.GetFile(character);
			this.followerGraphic = new IndexedColorGraphic(file, "walk south", this.position, (int)this.position.Y - 1);
			this.followerGraphic.SpeedModifier = 0f;
			this.followerGraphic.Frame = 0f;
			this.pipeline.Add(this.followerGraphic);
			if (this.useShadow)
			{
				this.shadowGraphic = new IndexedColorGraphic(Paths.GRAPHICS + "shadow.dat", ShadowSize.GetSubsprite(this.followerGraphic.Size), this.Position, (int)this.position.Y - 2);
				this.pipeline.Add(this.shadowGraphic);
			}
			this.animator = new AnimationControl(this.followerGraphic, this.direction);
			this.animator.UpdateSubsprite(this.GetAnimationContext());
			int width = this.followerGraphic.TextureRect.Width;
			int height = this.followerGraphic.TextureRect.Height;
			this.mesh = new Mesh(new FloatRect((float)(-(float)(width / 2)), -3f, (float)width, 6f));
			this.aabb = this.mesh.AABB;
			this.solid = true;
			this.collisionManager = colman;
		}

		// Token: 0x06000396 RID: 918 RVA: 0x00016D18 File Offset: 0x00014F18
		~PartyFollower()
		{
			this.Dispose(false);
		}

		// Token: 0x06000397 RID: 919 RVA: 0x00016D48 File Offset: 0x00014F48
		private VertexArray GetDebugVerts()
		{
			if (this.debugVerts == null)
			{
				Color color = new Color(61, 129, 166);
				VertexArray vertexArray = new VertexArray(PrimitiveType.LinesStrip, (uint)(this.mesh.Vertices.Count + 1));
				for (int i = 0; i < this.mesh.Vertices.Count; i++)
				{
					vertexArray[(uint)i] = new Vertex(this.mesh.Vertices[i], color);
				}
				vertexArray[(uint)this.mesh.Vertices.Count] = new Vertex(this.mesh.Vertices[0], color);
				this.debugVerts = vertexArray;
			}
			return this.debugVerts;
		}

		// Token: 0x06000398 RID: 920 RVA: 0x00016DFF File Offset: 0x00014FFF
		private void RecorderReset(Vector2f position, int direction)
		{
			this.position = position;
			this.direction = direction;
		}

		// Token: 0x06000399 RID: 921 RVA: 0x00016E10 File Offset: 0x00015010
		private AnimationContext GetAnimationContext()
		{
			return new AnimationContext
			{
				Velocity = this.velocity * (this.isRunning ? 2f : 1f) * (this.moving ? 1f : 0f),
				SuggestedDirection = this.direction,
				TerrainType = this.terrain,
				IsDead = this.isDead,
				IsCrouch = this.isCrouch,
				IsNauseous = false,
				IsTalk = false
			};
		}

		// Token: 0x0600039A RID: 922 RVA: 0x00016EAC File Offset: 0x000150AC
		public void Update(Vector2f newPosition, Vector2f newVelocity, TerrainType newTerrain)
		{
			this.lastPosition = this.position;
			this.position = newPosition;
			this.velocity = newVelocity;
			this.terrain = newTerrain;
			this.lastRunning = this.isRunning;
			this.isRunning = this.recorder.Running;
			this.isCrouch = this.recorder.Crouching;
			this.direction = VectorMath.VectorToDirection(this.velocity);
			this.lastMoving = this.moving;
			if ((int)this.lastPosition.X != (int)this.position.X || (int)this.lastPosition.Y != (int)this.position.Y)
			{
				this.followerGraphic.Position = new Vector2f((float)((int)this.position.X), (float)((int)this.position.Y));
				this.followerGraphic.Depth = (int)this.Position.Y;
				this.pipeline.Update(this.followerGraphic);
				if (this.useShadow)
				{
					this.shadowGraphic.Position = this.followerGraphic.Position;
					this.shadowGraphic.Depth = (int)this.Position.Y - 1;
					this.pipeline.Update(this.shadowGraphic);
				}
				this.collisionManager.Update(this, this.lastPosition, this.position);
				this.moving = true;
			}
			else
			{
				this.moving = false;
			}
			this.animator.UpdateSubsprite(this.GetAnimationContext());
		}

		// Token: 0x0600039B RID: 923 RVA: 0x0001702F File Offset: 0x0001522F
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600039C RID: 924 RVA: 0x00017040 File Offset: 0x00015240
		protected void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					this.pipeline.Remove(this.followerGraphic);
					this.pipeline.Remove(this.shadowGraphic);
					this.followerGraphic.Dispose();
					this.shadowGraphic.Dispose();
				}
				this.disposed = true;
			}
		}

		// Token: 0x0600039D RID: 925 RVA: 0x00017097 File Offset: 0x00015297
		public void Collision(CollisionContext context)
		{
		}

		// Token: 0x04000537 RID: 1335
		private bool disposed;

		// Token: 0x04000538 RID: 1336
		private RenderPipeline pipeline;

		// Token: 0x04000539 RID: 1337
		private PartyTrain recorder;

		// Token: 0x0400053A RID: 1338
		private CharacterType character;

		// Token: 0x0400053B RID: 1339
		private int place;

		// Token: 0x0400053C RID: 1340
		private int direction;

		// Token: 0x0400053D RID: 1341
		private Vector2f velocity;

		// Token: 0x0400053E RID: 1342
		private Vector2f position;

		// Token: 0x0400053F RID: 1343
		private Vector2f lastPosition;

		// Token: 0x04000540 RID: 1344
		private TerrainType terrain;

		// Token: 0x04000541 RID: 1345
		private IndexedColorGraphic followerGraphic;

		// Token: 0x04000542 RID: 1346
		private Graphic shadowGraphic;

		// Token: 0x04000543 RID: 1347
		private bool isRunning;

		// Token: 0x04000544 RID: 1348
		private bool lastRunning;

		// Token: 0x04000545 RID: 1349
		private bool moving;

		// Token: 0x04000546 RID: 1350
		private bool lastMoving;

		// Token: 0x04000547 RID: 1351
		private bool useShadow;

		// Token: 0x04000548 RID: 1352
		private bool isDead;

		// Token: 0x04000549 RID: 1353
		private bool isCrouch;

		// Token: 0x0400054A RID: 1354
		private AABB aabb;

		// Token: 0x0400054B RID: 1355
		private Mesh mesh;

		// Token: 0x0400054C RID: 1356
		private bool solid;

		// Token: 0x0400054D RID: 1357
		private VertexArray debugVerts;

		// Token: 0x0400054E RID: 1358
		private CollisionManager collisionManager;

		// Token: 0x0400054F RID: 1359
		private AnimationControl animator;
	}
}

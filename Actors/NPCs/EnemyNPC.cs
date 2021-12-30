using System;
using Carbine.Actors;
using Carbine.Collision;
using Carbine.Graphics;
using Carbine.Utility;
using Mother4.Actors.Animation;
using Mother4.Actors.NPCs.Movement;
using Mother4.Data;
using Mother4.Overworld;
using SFML.Graphics;
using SFML.System;

namespace Mother4.Actors.NPCs
{
	// Token: 0x02000052 RID: 82
	internal class EnemyNPC : SolidActor
	{
		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000207 RID: 519 RVA: 0x0000C675 File Offset: 0x0000A875
		public EnemyType Type
		{
			get
			{
				return this.enemyType;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000208 RID: 520 RVA: 0x0000C67D File Offset: 0x0000A87D
		public Graphic Graphic
		{
			get
			{
				return this.npcGraphic;
			}
		}

		// Token: 0x06000209 RID: 521 RVA: 0x0000C688 File Offset: 0x0000A888
		public EnemyNPC(RenderPipeline pipeline, CollisionManager colman, EnemyType enemyType, Vector2f position, FloatRect spawnArea) : base(colman)
		{
			this.pipeline = pipeline;
			this.position = position;
			this.enemyType = enemyType;
			this.mover = new MushroomMover(this, 100f, 2f);
			this.npcGraphic = new IndexedColorGraphic(EnemyGraphics.GetFilename(enemyType), "walk south", this.Position, (int)this.Position.Y);
			this.pipeline.Add(this.npcGraphic);
			this.hasDirection = new bool[8];
			this.hasDirection[0] = (this.npcGraphic.GetSpriteDefinition("walk east") != null);
			this.hasDirection[1] = (this.npcGraphic.GetSpriteDefinition("walk northeast") != null);
			this.hasDirection[2] = (this.npcGraphic.GetSpriteDefinition("walk north") != null);
			this.hasDirection[3] = (this.npcGraphic.GetSpriteDefinition("walk northwest") != null);
			this.hasDirection[4] = (this.npcGraphic.GetSpriteDefinition("walk west") != null);
			this.hasDirection[5] = (this.npcGraphic.GetSpriteDefinition("walk southwest") != null);
			this.hasDirection[6] = (this.npcGraphic.GetSpriteDefinition("walk south") != null);
			this.hasDirection[7] = (this.npcGraphic.GetSpriteDefinition("walk southeast") != null);
			this.shadowGraphic = new IndexedColorGraphic(Paths.GRAPHICS + "shadow.dat", ShadowSize.GetSubsprite(this.npcGraphic.Size), this.Position, (int)(this.Position.Y - 1f));
			this.pipeline.Add(this.shadowGraphic);
			int width = this.npcGraphic.TextureRect.Width;
			int height = this.npcGraphic.TextureRect.Height;
			this.mesh = new Mesh(new FloatRect((float)(-(float)(width / 2)), -3f, (float)width, 6f));
			this.aabb = this.mesh.AABB;
			this.animator = new AnimationControl(this.npcGraphic, this.direction);
			this.animator.UpdateSubsprite(this.GetAnimationContext());
		}

		// Token: 0x0600020A RID: 522 RVA: 0x0000C8CC File Offset: 0x0000AACC
		private AnimationContext GetAnimationContext()
		{
			return new AnimationContext
			{
				Velocity = this.velocity,
				SuggestedDirection = this.direction,
				TerrainType = TerrainType.None,
				IsDead = false,
				IsCrouch = false,
				IsNauseous = false,
				IsTalk = false
			};
		}

		// Token: 0x0600020B RID: 523 RVA: 0x0000C924 File Offset: 0x0000AB24
		public void OverrideSubsprite(string subsprite)
		{
			this.animator.OverrideSubsprite(subsprite);
		}

		// Token: 0x0600020C RID: 524 RVA: 0x0000C932 File Offset: 0x0000AB32
		public void ClearOverrideSubsprite()
		{
			this.animator.ClearOverride();
		}

		// Token: 0x0600020D RID: 525 RVA: 0x0000C93F File Offset: 0x0000AB3F
		public void FreezeSpriteForever()
		{
			this.npcGraphic.SpeedModifier = 0f;
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0000C954 File Offset: 0x0000AB54
		public override void Update()
		{
			this.lastVelocity = this.velocity;
			if (!this.isMovementLocked)
			{
				this.changed = this.mover.GetNextMove(ref this.position, ref this.velocity, ref this.direction);
			}
			if (this.changed)
			{
				this.animator.UpdateSubsprite(this.GetAnimationContext());
				this.npcGraphic.Position = VectorMath.Truncate(this.position);
				this.npcGraphic.Depth = (int)this.position.Y;
				this.pipeline.Update(this.npcGraphic);
				this.shadowGraphic.Position = VectorMath.Truncate(this.position);
				this.shadowGraphic.Depth = (int)this.position.Y - 1;
				this.pipeline.Update(this.shadowGraphic);
				Vector2f v = new Vector2f(this.velocity.X, 0f);
				Vector2f v2 = new Vector2f(0f, this.velocity.Y);
				this.lastPosition = this.position;
				if (this.collisionManager.PlaceFree(this, this.position + v))
				{
					this.position += v;
				}
				if (this.collisionManager.PlaceFree(this, this.position + v2))
				{
					this.position += v2;
				}
				this.collisionManager.Update(this, this.lastPosition, this.position);
				this.changed = false;
			}
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0000CAE4 File Offset: 0x0000ACE4
		protected override void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				try
				{
					if (disposing)
					{
						this.pipeline.Remove(this.npcGraphic);
						this.pipeline.Remove(this.shadowGraphic);
						this.npcGraphic.Dispose();
						this.shadowGraphic.Dispose();
					}
					this.disposed = true;
				}
				finally
				{
					base.Dispose(disposing);
				}
			}
		}

		// Token: 0x040002DE RID: 734
		private static readonly Vector2f HALO_OFFSET = new Vector2f(0f, -32f);

		// Token: 0x040002DF RID: 735
		private RenderPipeline pipeline;

		// Token: 0x040002E0 RID: 736
		private IndexedColorGraphic npcGraphic;

		// Token: 0x040002E1 RID: 737
		private IndexedColorGraphic haloGraphic;

		// Token: 0x040002E2 RID: 738
		private Graphic shadowGraphic;

		// Token: 0x040002E3 RID: 739
		private Mover mover;

		// Token: 0x040002E4 RID: 740
		private bool[] hasDirection;

		// Token: 0x040002E5 RID: 741
		private Vector2f lastVelocity;

		// Token: 0x040002E6 RID: 742
		private int direction;

		// Token: 0x040002E7 RID: 743
		private bool changed;

		// Token: 0x040002E8 RID: 744
		private EnemyType enemyType;

		// Token: 0x040002E9 RID: 745
		private AnimationControl animator;
	}
}

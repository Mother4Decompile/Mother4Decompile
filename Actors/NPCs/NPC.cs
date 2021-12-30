using System;
using System.Collections.Generic;
using Carbine;
using Carbine.Actors;
using Carbine.Collision;
using Carbine.Graphics;
using Carbine.Maps;
using Carbine.Utility;
using Mother4.Actors.Animation;
using Mother4.Actors.NPCs.Movement;
using Mother4.Data;
using Mother4.Overworld;
using SFML.Graphics;
using SFML.System;

namespace Mother4.Actors.NPCs
{
	// Token: 0x020000A6 RID: 166
	internal class NPC : SolidActor
	{
		// Token: 0x17000094 RID: 148
		// (get) Token: 0x0600036B RID: 875 RVA: 0x0001608D File Offset: 0x0001428D
		public List<Map.NPCtext> Text
		{
			get
			{
				return this.text;
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x0600036C RID: 876 RVA: 0x00016095 File Offset: 0x00014295
		public List<Map.NPCtext> TeleText
		{
			get
			{
				return this.teleText;
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x0600036D RID: 877 RVA: 0x0001609D File Offset: 0x0001429D
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x0600036E RID: 878 RVA: 0x000160A5 File Offset: 0x000142A5
		// (set) Token: 0x0600036F RID: 879 RVA: 0x000160AD File Offset: 0x000142AD
		public int Direction
		{
			get
			{
				return this.direction;
			}
			set
			{
				this.direction = value;
				this.forceSpriteUpdate = true;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000370 RID: 880 RVA: 0x000160BD File Offset: 0x000142BD
		// (set) Token: 0x06000371 RID: 881 RVA: 0x000160C5 File Offset: 0x000142C5
		public float HopFactor
		{
			get
			{
				return this.hopFactor;
			}
			set
			{
				this.hopFactor = value;
				this.hopFrame = Engine.Frame;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000372 RID: 882 RVA: 0x000160D9 File Offset: 0x000142D9
		public int Depth
		{
			get
			{
				return this.depth;
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000373 RID: 883 RVA: 0x000160E1 File Offset: 0x000142E1
		public Vector2f EmoticonPoint
		{
			get
			{
				return new Vector2f(this.position.X, this.position.Y - this.npcGraphic.Origin.Y);
			}
		}

		// Token: 0x06000374 RID: 884 RVA: 0x00016110 File Offset: 0x00014310
		public NPC(RenderPipeline pipeline, CollisionManager colman, Map.NPC npcData, object moverData) : base(null)
		{
			this.pipeline = pipeline;
			this.name = npcData.Name;
			this.direction = (int)npcData.Direction;
			this.text = npcData.Text;
			this.teleText = npcData.TeleText;
			this.shadow = npcData.Shadow;
			this.sticky = npcData.Sticky;
			NPC.MoveMode mode = (NPC.MoveMode)npcData.Mode;
			this.speed = npcData.Speed;
			this.delay = (int)npcData.Delay;
			this.distance = (int)npcData.Distance;
			this.startPosition.X = (float)npcData.X;
			this.startPosition.Y = (float)npcData.Y;
			this.SetMoveMode(mode, moverData);
			this.position = this.startPosition;
			this.depthOverride = (npcData.DepthOverride > int.MinValue);
			this.depth = (this.depthOverride ? npcData.DepthOverride : ((int)this.position.Y));
			this.pipeline = pipeline;
			if (npcData.Sprite != null && npcData.Sprite.Length > 0)
			{
				this.hasSprite = true;
				this.ChangeSprite(Paths.GRAPHICS + npcData.Sprite + ".dat", "stand south");
				if (this.shadow)
				{
					this.shadowGraphic = new IndexedColorGraphic(Paths.GRAPHICS + "shadow.dat", ShadowSize.GetSubsprite(this.npcGraphic.Size), this.position, this.depth - 1);
					this.pipeline.Add(this.shadowGraphic);
				}
				int width = this.npcGraphic.TextureRect.Width;
				int height = this.npcGraphic.TextureRect.Height;
				this.mesh = new Mesh(new FloatRect((float)(-(float)(width / 2)), -3f, (float)width, 6f));
			}
			else
			{
				this.mesh = new Mesh(new FloatRect(0f, 0f, (float)npcData.Width, (float)npcData.Height));
			}
			this.aabb = this.mesh.AABB;
			this.isSolid = npcData.Solid;
			this.collisionManager = colman;
			this.collisionManager.Add(this);
			this.lastVelocity = VectorMath.ZERO_VECTOR;
			this.state = NPC.State.Idle;
			this.ChangeState(this.state);
		}

		// Token: 0x06000375 RID: 885 RVA: 0x00016378 File Offset: 0x00014578
		public void SetMoveMode(NPC.MoveMode moveMode, object moverData)
		{
			NPC.MoveMode moveMode2 = moveMode;
			if (moverData is Map.Path)
			{
				moveMode2 = NPC.MoveMode.Path;
			}
			if (moverData is Map.Area)
			{
				moveMode2 = NPC.MoveMode.Area;
			}
			switch (moveMode2)
			{
			case NPC.MoveMode.RandomTurn:
				this.mover = new RandomTurnMover(60);
				return;
			case NPC.MoveMode.FacePlayer:
				this.mover = new FacePlayerMover();
				return;
			case NPC.MoveMode.Random:
				this.mover = new RandomMover(this.speed, (float)this.distance, this.delay);
				return;
			case NPC.MoveMode.Path:
			{
				Map.Path path = (Map.Path)moverData;
				bool loop = moveMode > NPC.MoveMode.None;
				this.mover = new PathMover(this.speed, this.delay, loop, path.Points);
				this.startPosition.X = (float)((int)path.Points[0].X);
				this.startPosition.Y = (float)((int)path.Points[0].Y);
				return;
			}
			case NPC.MoveMode.Area:
			{
				Map.Area area = (Map.Area)moverData;
				this.mover = new AreaMover(this.speed, this.delay, (float)this.distance, (float)area.Rectangle.Left, (float)area.Rectangle.Top, (float)area.Rectangle.Width, (float)area.Rectangle.Height);
				return;
			}
			default:
				this.mover = new NoneMover();
				return;
			}
		}

		// Token: 0x06000376 RID: 886 RVA: 0x000164C8 File Offset: 0x000146C8
		public void SetMover(Mover mover)
		{
			this.mover = mover;
		}

		// Token: 0x06000377 RID: 887 RVA: 0x000164D4 File Offset: 0x000146D4
		public void Telepathize()
		{
			this.effectGraphic = new IndexedColorGraphic(Paths.GRAPHICS + "telepathy.dat", "pulse", VectorMath.Truncate(this.position - new Vector2f(0f, this.npcGraphic.Origin.Y - this.npcGraphic.Size.Y / 4f)), 2147450881);
			this.pipeline.Add(this.effectGraphic);
		}

		// Token: 0x06000378 RID: 888 RVA: 0x00016557 File Offset: 0x00014757
		public void Untelepathize()
		{
			this.pipeline.Remove(this.effectGraphic);
			this.effectGraphic.Dispose();
			this.effectGraphic = null;
		}

		// Token: 0x06000379 RID: 889 RVA: 0x0001657C File Offset: 0x0001477C
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
				IsTalk = (this.state == NPC.State.Talking)
			};
		}

		// Token: 0x0600037A RID: 890 RVA: 0x000165DC File Offset: 0x000147DC
		public void ChangeSprite(string resource, string subsprite)
		{
			if (this.npcGraphic != null)
			{
				this.pipeline.Remove(this.npcGraphic);
			}
			this.npcGraphic = new IndexedColorGraphic(resource, subsprite, this.position, this.depth);
			this.pipeline.Add(this.npcGraphic);
			if (this.animator == null)
			{
				this.animator = new AnimationControl(this.npcGraphic, this.direction);
			}
			this.animator.ChangeGraphic(this.npcGraphic);
			this.animator.UpdateSubsprite(this.GetAnimationContext());
			this.hasSprite = true;
		}

		// Token: 0x0600037B RID: 891 RVA: 0x00016674 File Offset: 0x00014874
		public void OverrideSubsprite(string subsprite)
		{
			if (this.hasSprite)
			{
				this.animator.OverrideSubsprite(subsprite);
			}
		}

		// Token: 0x0600037C RID: 892 RVA: 0x0001668C File Offset: 0x0001488C
		public void ClearOverrideSubsprite()
		{
			if (this.hasSprite)
			{
				this.animator.ClearOverride();
				this.animator.UpdateSubsprite(this.GetAnimationContext());
				if (this.animationLoopCountTarget > 0)
				{
					this.animationLoopCount = 0;
					this.animationLoopCountTarget = 0;
					this.npcGraphic.OnAnimationComplete -= this.npcGraphic_OnAnimationComplete;
				}
			}
		}

		// Token: 0x0600037D RID: 893 RVA: 0x000166EC File Offset: 0x000148EC
		public void SetAnimationLoopCount(int loopCount)
		{
			if (this.hasSprite && this.animator.Overriden)
			{
				this.animationLoopCount = 0;
				this.animationLoopCountTarget = Math.Max(1, loopCount);
				this.npcGraphic.OnAnimationComplete += this.npcGraphic_OnAnimationComplete;
			}
		}

		// Token: 0x0600037E RID: 894 RVA: 0x0001673C File Offset: 0x0001493C
		private void npcGraphic_OnAnimationComplete(AnimatedRenderable renderable)
		{
			this.animationLoopCount++;
			if (this.animationLoopCount >= this.animationLoopCountTarget)
			{
				this.npcGraphic.SpeedModifier = 0f;
				this.npcGraphic.OnAnimationComplete -= this.npcGraphic_OnAnimationComplete;
			}
		}

		// Token: 0x0600037F RID: 895 RVA: 0x0001678C File Offset: 0x0001498C
		private void ChangeState(NPC.State newState)
		{
			if (newState != this.state)
			{
				this.lastState = this.state;
				this.state = newState;
			}
		}

		// Token: 0x06000380 RID: 896 RVA: 0x000167AC File Offset: 0x000149AC
		public void StartTalking()
		{
			if (!this.talkPause)
			{
				this.velocity.X = 0f;
				this.velocity.Y = 0f;
				this.stateMemory = this.state;
			}
			this.ChangeState(NPC.State.Talking);
			this.talkPause = false;
		}

		// Token: 0x06000381 RID: 897 RVA: 0x000167FB File Offset: 0x000149FB
		public void PauseTalking()
		{
			if (this.state == NPC.State.Talking)
			{
				this.ChangeState(NPC.State.Idle);
				this.talkPause = true;
			}
		}

		// Token: 0x06000382 RID: 898 RVA: 0x00016814 File Offset: 0x00014A14
		public void StopTalking()
		{
			this.ChangeState(this.stateMemory);
			this.isMovementLocked = false;
			this.talkPause = false;
			this.animator.ClearOverride();
		}

		// Token: 0x06000383 RID: 899 RVA: 0x0001683B File Offset: 0x00014A3B
		public void StartMoving()
		{
			this.ChangeState(NPC.State.Moving);
		}

		// Token: 0x06000384 RID: 900 RVA: 0x00016844 File Offset: 0x00014A44
		public void ForceDepth(int newDepth)
		{
			this.depthOverride = true;
			this.depth = newDepth;
			this.forceSpriteUpdate = true;
		}

		// Token: 0x06000385 RID: 901 RVA: 0x0001685B File Offset: 0x00014A5B
		public void ResetDepth()
		{
			this.depthOverride = false;
		}

		// Token: 0x06000386 RID: 902 RVA: 0x00016864 File Offset: 0x00014A64
		public override void Update()
		{
			this.lastVelocity = this.velocity;
			if (this.state != NPC.State.Talking)
			{
				if (!this.MovementLocked)
				{
					this.changed = this.mover.GetNextMove(ref this.position, ref this.velocity, ref this.direction);
				}
				base.Update();
				if (this.hopFactor >= 1f)
				{
					this.lastZOffset = this.zOffset;
					this.zOffset = (float)Math.Sin((double)((float)(Engine.Frame - this.hopFrame) / (this.hopFactor * 0.3f))) * this.hopFactor;
					if (this.zOffset < 0f)
					{
						this.zOffset = 0f;
						this.hopFactor = 0f;
					}
				}
				if ((int)this.lastPosition.X != (int)this.position.X || (int)this.lastPosition.Y != (int)this.position.Y || (int)this.lastZOffset != (int)this.zOffset || this.forceSpriteUpdate)
				{
					if (this.state != NPC.State.Moving)
					{
						this.ChangeState(NPC.State.Moving);
					}
					if (!this.depthOverride)
					{
						this.depth = (int)this.position.Y;
					}
					if (this.hasSprite)
					{
						this.graphicOffset.Y = -this.zOffset;
						this.npcGraphic.Position = VectorMath.Truncate(this.position + this.graphicOffset);
						this.npcGraphic.Depth = this.depth;
						this.pipeline.Update(this.npcGraphic);
						if (this.shadow)
						{
							this.shadowGraphic.Position = VectorMath.Truncate(this.position);
							this.shadowGraphic.Depth = this.depth - 1;
							this.pipeline.Update(this.shadowGraphic);
						}
					}
					this.forceSpriteUpdate = false;
				}
				else if (this.state != NPC.State.Idle)
				{
					this.ChangeState(NPC.State.Idle);
				}
			}
			if (this.hasSprite)
			{
				this.animator.UpdateSubsprite(this.GetAnimationContext());
			}
		}

		// Token: 0x06000387 RID: 903 RVA: 0x00016A70 File Offset: 0x00014C70
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
						if (this.shadow)
						{
							this.shadowGraphic.Dispose();
						}
					}
					this.disposed = true;
				}
				finally
				{
					base.Dispose(disposing);
				}
			}
		}

		// Token: 0x0400050C RID: 1292
		private RenderPipeline pipeline;

		// Token: 0x0400050D RID: 1293
		private IndexedColorGraphic npcGraphic;

		// Token: 0x0400050E RID: 1294
		private Graphic shadowGraphic;

		// Token: 0x0400050F RID: 1295
		private IndexedColorGraphic effectGraphic;

		// Token: 0x04000510 RID: 1296
		private string name;

		// Token: 0x04000511 RID: 1297
		private int direction;

		// Token: 0x04000512 RID: 1298
		private bool talkPause;

		// Token: 0x04000513 RID: 1299
		private NPC.State state;

		// Token: 0x04000514 RID: 1300
		private NPC.State lastState;

		// Token: 0x04000515 RID: 1301
		private NPC.State stateMemory;

		// Token: 0x04000516 RID: 1302
		private List<Map.NPCtext> text;

		// Token: 0x04000517 RID: 1303
		private List<Map.NPCtext> teleText;

		// Token: 0x04000518 RID: 1304
		private Vector2f lastVelocity;

		// Token: 0x04000519 RID: 1305
		private Vector2f startPosition;

		// Token: 0x0400051A RID: 1306
		private Vector2f graphicOffset;

		// Token: 0x0400051B RID: 1307
		private float lastZOffset;

		// Token: 0x0400051C RID: 1308
		private float speed;

		// Token: 0x0400051D RID: 1309
		private int delay;

		// Token: 0x0400051E RID: 1310
		private int distance;

		// Token: 0x0400051F RID: 1311
		private float hopFactor;

		// Token: 0x04000520 RID: 1312
		private long hopFrame;

		// Token: 0x04000521 RID: 1313
		private Mover mover;

		// Token: 0x04000522 RID: 1314
		private bool changed;

		// Token: 0x04000523 RID: 1315
		private bool shadow;

		// Token: 0x04000524 RID: 1316
		private bool sticky;

		// Token: 0x04000525 RID: 1317
		private int depth;

		// Token: 0x04000526 RID: 1318
		private bool depthOverride;

		// Token: 0x04000527 RID: 1319
		private bool hasSprite;

		// Token: 0x04000528 RID: 1320
		private bool forceSpriteUpdate;

		// Token: 0x04000529 RID: 1321
		private AnimationControl animator;

		// Token: 0x0400052A RID: 1322
		private int animationLoopCount;

		// Token: 0x0400052B RID: 1323
		private int animationLoopCountTarget;

		// Token: 0x020000A7 RID: 167
		public enum State
		{
			// Token: 0x0400052D RID: 1325
			Idle,
			// Token: 0x0400052E RID: 1326
			Talking,
			// Token: 0x0400052F RID: 1327
			Moving
		}

		// Token: 0x020000A8 RID: 168
		public enum MoveMode
		{
			// Token: 0x04000531 RID: 1329
			None,
			// Token: 0x04000532 RID: 1330
			RandomTurn,
			// Token: 0x04000533 RID: 1331
			FacePlayer,
			// Token: 0x04000534 RID: 1332
			Random,
			// Token: 0x04000535 RID: 1333
			Path,
			// Token: 0x04000536 RID: 1334
			Area
		}
	}
}

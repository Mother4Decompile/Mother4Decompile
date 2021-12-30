using System;
using Carbine;
using Carbine.Actors;
using Carbine.Collision;
using Carbine.Graphics;
using Carbine.Input;
using Carbine.Utility;
using Mother4.Actors.Animation;
using Mother4.Actors.NPCs.Movement;
using Mother4.Battle;
using Mother4.Data;
using Mother4.Overworld;
using SFML.Graphics;
using SFML.System;

namespace Mother4.Actors
{
	// Token: 0x020000AA RID: 170
	internal class Player : SolidActor
	{
		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x0600039E RID: 926 RVA: 0x00017099 File Offset: 0x00015299
		public Vector2f CheckVector
		{
			get
			{
				return this.checkVector;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x0600039F RID: 927 RVA: 0x000170A1 File Offset: 0x000152A1
		public int Direction
		{
			get
			{
				return this.direction;
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060003A0 RID: 928 RVA: 0x000170A9 File Offset: 0x000152A9
		public bool Running
		{
			get
			{
				return this.isRunning;
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060003A1 RID: 929 RVA: 0x000170B1 File Offset: 0x000152B1
		// (set) Token: 0x060003A2 RID: 930 RVA: 0x000170B9 File Offset: 0x000152B9
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

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060003A3 RID: 931 RVA: 0x000170CD File Offset: 0x000152CD
		public int Depth
		{
			get
			{
				return this.playerGraphic.Depth;
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060003A4 RID: 932 RVA: 0x000170DA File Offset: 0x000152DA
		public Vector2f EmoticonPoint
		{
			get
			{
				return new Vector2f(this.position.X, this.position.Y - this.playerGraphic.Origin.Y);
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060003A5 RID: 933 RVA: 0x00017108 File Offset: 0x00015308
		// (set) Token: 0x060003A6 RID: 934 RVA: 0x00017110 File Offset: 0x00015310
		public bool InputLocked
		{
			get
			{
				return this.isInputLocked;
			}
			set
			{
				this.isInputLocked = value;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060003A7 RID: 935 RVA: 0x00017119 File Offset: 0x00015319
		// (set) Token: 0x060003A8 RID: 936 RVA: 0x00017121 File Offset: 0x00015321
		public override bool MovementLocked
		{
			get
			{
				return this.isMovementLocked;
			}
			set
			{
				this.isMovementLocked = value;
				this.playerGraphic.SpeedModifier = (float)(this.isMovementLocked ? 0 : 1);
				this.recorder.MovementLocked = this.isMovementLocked;
			}
		}

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x060003A9 RID: 937 RVA: 0x00017154 File Offset: 0x00015354
		// (remove) Token: 0x060003AA RID: 938 RVA: 0x0001718C File Offset: 0x0001538C
		public event Player.OnCollisionHanlder OnCollision;

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x060003AB RID: 939 RVA: 0x000171C4 File Offset: 0x000153C4
		// (remove) Token: 0x060003AC RID: 940 RVA: 0x000171FC File Offset: 0x000153FC
		public event Player.OnTelepathyAnimationCompleteHanlder OnTelepathyAnimationComplete;

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x060003AD RID: 941 RVA: 0x00017234 File Offset: 0x00015434
		// (remove) Token: 0x060003AE RID: 942 RVA: 0x0001726C File Offset: 0x0001546C
		public event Player.OnRunningChangeHandler OnRunningChange;

		// Token: 0x060003AF RID: 943 RVA: 0x000172A4 File Offset: 0x000154A4
		public Player(RenderPipeline pipeline, CollisionManager colman, PartyTrain recorder, Vector2f position, int direction, CharacterType character, bool useShadow, bool isOcean, bool isRunning) : base(colman)
		{
			this.position = position;
			this.moveVector = new Vector2f(0f, 0f);
			this.pipeline = pipeline;
			this.mesh = new Mesh(new FloatRect(-8f, -3f, 15f, 6f));
			this.aabb = base.Mesh.AABB;
			this.ignoreCollisionTypes = new Type[]
			{
				typeof(PartyFollower)
			};
			this.checkVector = new Vector2f(0f, 10f);
			this.direction = direction;
			this.character = character;
			this.terrainType = (isOcean ? TerrainType.Ocean : TerrainType.Tile);
			this.recorder = recorder;
			this.recorder.Reset(this.position, this.direction, this.terrainType);
			this.speed = 0f;
			this.isRunning = isRunning;
			this.runVector = VectorMath.DirectionToVector(this.direction);
			this.UpdateStatusEffects();
			string file = CharacterGraphics.GetFile(character);
			this.ChangeSprite(file, "walk south");
			this.isShadowEnabled = (useShadow && !isOcean);
			this.shadowGraphic = new IndexedColorGraphic(Paths.GRAPHICS + "shadow.dat", ShadowSize.GetSubsprite(this.playerGraphic.Size), position, (int)position.Y - 1);
			this.shadowGraphic.Visible = this.isShadowEnabled;
			pipeline.Add(this.shadowGraphic);
			InputManager.Instance.ButtonPressed += this.ButtonPressed;
			InputManager.Instance.ButtonReleased += this.ButtonReleased;
			TimerManager.Instance.OnTimerEnd += this.CrouchTimerEnd;
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x0001746C File Offset: 0x0001566C
		public void UpdateStatusEffects()
		{
			this.isDead = (CharacterStatusEffects.HasStatusEffect(this.character, StatusEffect.Unconscious) || CharacterStats.GetStats(this.character).HP <= 0);
			this.isNauseous = CharacterStatusEffects.HasStatusEffect(this.character, StatusEffect.Nausea);
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x000174BC File Offset: 0x000156BC
		public void Telepathize()
		{
			this.effectGraphic = new IndexedColorGraphic(Paths.GRAPHICS + "telepathy.dat", "telepathy", VectorMath.Truncate(this.position - new Vector2f(0f, this.playerGraphic.Origin.Y - this.playerGraphic.Size.Y / 4f)), 2147450881);
			this.effectGraphic.OnAnimationComplete += this.effectGraphic_OnAnimationComplete;
			this.pipeline.Add(this.effectGraphic);
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x00017558 File Offset: 0x00015758
		private void effectGraphic_OnAnimationComplete(AnimatedRenderable graphic)
		{
			this.effectGraphic.OnAnimationComplete -= this.effectGraphic_OnAnimationComplete;
			this.pipeline.Remove(this.effectGraphic);
			this.effectGraphic.Dispose();
			this.effectGraphic = null;
			if (this.OnTelepathyAnimationComplete != null)
			{
				this.OnTelepathyAnimationComplete(this);
			}
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x000175B3 File Offset: 0x000157B3
		private void ButtonPressed(InputManager sender, Button b)
		{
			if (!this.isMovementLocked && !this.isInputLocked && b == Button.B)
			{
				this.isRunButtonPressed = true;
			}
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x000175D0 File Offset: 0x000157D0
		private void ButtonReleased(InputManager sender, Button b)
		{
			if (!this.isMovementLocked && !this.isInputLocked && b == Button.B)
			{
				this.isRunButtonReleased = true;
			}
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x000175ED File Offset: 0x000157ED
		private void CrouchTimerEnd(int timerIndex)
		{
			if (this.crouchTimerIndex == timerIndex)
			{
				this.isRunTimerComplete = true;
			}
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x000175FF File Offset: 0x000157FF
		public void SetPosition(Vector2f position)
		{
			this.SetPosition(position, false);
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x00017609 File Offset: 0x00015809
		public void SetPosition(Vector2f position, bool extend)
		{
			this.position = position;
			this.lastPosition = position;
			this.recorder.Reset(this.position, this.direction, this.terrainType, extend);
			this.UpdateGraphics();
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x0001763D File Offset: 0x0001583D
		public void SetDirection(int dir)
		{
			while (dir < 0)
			{
				dir += 8;
			}
			this.direction = dir % 8;
			this.animator.UpdateSubsprite(this.GetAnimationContext());
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x00017664 File Offset: 0x00015864
		public void SetShadow(bool isVisible)
		{
			this.isShadowEnabled = isVisible;
			this.shadowGraphic.Visible = this.isShadowEnabled;
		}

		// Token: 0x060003BA RID: 954 RVA: 0x0001767E File Offset: 0x0001587E
		public void SetMover(Mover mover)
		{
			this.mover = mover;
		}

		// Token: 0x060003BB RID: 955 RVA: 0x00017687 File Offset: 0x00015887
		public void ClearMover()
		{
			this.mover = null;
		}

		// Token: 0x060003BC RID: 956 RVA: 0x00017690 File Offset: 0x00015890
		public void OverrideSubsprite(string subsprite)
		{
			this.animator.OverrideSubsprite(subsprite);
			this.animator.UpdateSubsprite(this.GetAnimationContext());
		}

		// Token: 0x060003BD RID: 957 RVA: 0x000176B0 File Offset: 0x000158B0
		public void ClearOverrideSubsprite()
		{
			this.animator.ClearOverride();
			this.animator.UpdateSubsprite(this.GetAnimationContext());
			if (this.animationLoopCountTarget > 0)
			{
				this.animationLoopCount = 0;
				this.animationLoopCountTarget = 0;
				this.playerGraphic.OnAnimationComplete -= this.playerGraphic_OnAnimationComplete;
			}
		}

		// Token: 0x060003BE RID: 958 RVA: 0x00017707 File Offset: 0x00015907
		public void SetAnimationLoopCount(int loopCount)
		{
			if (this.animator.Overriden)
			{
				this.animationLoopCount = 0;
				this.animationLoopCountTarget = Math.Max(1, loopCount);
				this.playerGraphic.OnAnimationComplete += this.playerGraphic_OnAnimationComplete;
			}
		}

		// Token: 0x060003BF RID: 959 RVA: 0x00017744 File Offset: 0x00015944
		private void playerGraphic_OnAnimationComplete(AnimatedRenderable renderable)
		{
			this.animationLoopCount++;
			if (this.animationLoopCount >= this.animationLoopCountTarget)
			{
				this.playerGraphic.SpeedModifier = 0f;
				this.playerGraphic.OnAnimationComplete -= this.playerGraphic_OnAnimationComplete;
			}
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x00017794 File Offset: 0x00015994
		private void SetRunning(bool isRunning)
		{
			this.isRunning = isRunning;
			if (this.OnRunningChange != null)
			{
				this.OnRunningChange(this);
			}
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x000177B4 File Offset: 0x000159B4
		private AnimationContext GetAnimationContext()
		{
			return new AnimationContext
			{
				Velocity = this.velocity * this.speed,
				SuggestedDirection = this.direction,
				TerrainType = this.terrainType,
				IsDead = this.isDead,
				IsCrouch = this.isCrouch,
				IsNauseous = this.isNauseous,
				IsTalk = false
			};
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x0001782C File Offset: 0x00015A2C
		public void ChangeSprite(string resource, string subsprite)
		{
			if (this.playerGraphic != null)
			{
				this.pipeline.Remove(this.playerGraphic);
			}
			this.playerGraphic = new IndexedColorGraphic(resource, subsprite, this.position, (int)this.position.Y);
			this.pipeline.Add(this.playerGraphic);
			if (this.animator == null)
			{
				this.animator = new AnimationControl(this.playerGraphic, this.direction);
			}
			this.animator.ChangeGraphic(this.playerGraphic);
			this.animator.UpdateSubsprite(this.GetAnimationContext());
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x000178C4 File Offset: 0x00015AC4
		private void HandleRunFlags()
		{
			if (this.isNauseous)
			{
				return;
			}
			if (this.isRunButtonPressed)
			{
				if (!this.isRunning)
				{
					this.isCrouch = true;
					this.crouchTimerIndex = TimerManager.Instance.StartTimer(10);
					this.runVector = VectorMath.DirectionToVector(this.direction);
					this.moveVector = VectorMath.ZERO_VECTOR;
				}
				else
				{
					this.SetRunning(false);
				}
				this.isRunButtonPressed = false;
			}
			if (this.isRunButtonReleased)
			{
				if (this.isRunReady)
				{
					this.isRunReady = false;
					this.SetRunning(true);
				}
				else
				{
					TimerManager.Instance.Cancel(this.crouchTimerIndex);
				}
				this.animator.ClearOverride();
				this.isCrouch = false;
				this.isRunButtonReleased = false;
			}
			if (this.isRunTimerComplete)
			{
				this.isRunReady = true;
				this.isRunTimerComplete = false;
			}
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x00017990 File Offset: 0x00015B90
		public override void Input()
		{
			if (this.mover == null)
			{
				this.HandleRunFlags();
				if (!this.isInputLocked)
				{
					if (this.moveVector.X != 0f || this.moveVector.Y != 0f)
					{
						this.lastMoveVector = this.moveVector;
						this.lastSpeed = this.speed;
					}
					Vector2f v = VectorMath.Truncate(InputManager.Instance.Axis);
					this.speed = (this.isRunning ? 3f : 1f);
					this.recorder.Running = this.isRunning;
					this.recorder.Crouching = this.isCrouch;
					if (!this.isCrouch && !this.isRunning)
					{
						this.moveVector = v;
						if (v.X != 0f || v.Y != 0f)
						{
							this.direction = VectorMath.VectorToDirection(this.moveVector);
						}
					}
					else if (this.isCrouch)
					{
						if (v.X != 0f || v.Y != 0f)
						{
							this.runVector = v;
							this.direction = VectorMath.VectorToDirection(v);
							this.animator.UpdateSubsprite(this.GetAnimationContext());
						}
					}
					else if (this.isRunning)
					{
						if (v.X != 0f || v.Y != 0f)
						{
							this.runVector = v;
						}
						this.moveVector = this.runVector;
						this.direction = VectorMath.VectorToDirection(this.moveVector);
					}
				}
				else
				{
					this.moveVector.X = 0f;
					this.moveVector.Y = 0f;
				}
				if (((this.lastMoveVector.X != this.moveVector.X || this.lastMoveVector.Y != this.moveVector.Y) && (this.moveVector.X != 0f || this.moveVector.Y != 0f)) || this.speed != this.lastSpeed)
				{
					this.checkVector = VectorMath.Truncate(VectorMath.Normalize(this.moveVector) * 11f);
				}
				this.velocity = this.moveVector;
				this.animator.UpdateSubsprite(this.GetAnimationContext());
				this.isSolid = !InputManager.Instance.State[Button.L];
			}
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x00017BFC File Offset: 0x00015DFC
		private void HandleCornerSliding()
		{
			if (this.direction % 2 == 0)
			{
				Vector2f vector2f = VectorMath.DirectionToVector(this.direction);
				Vector2f vector2f2 = VectorMath.LeftNormal(vector2f);
				int num = (this.direction == 0 || this.direction == 4) ? 8 : 10;
				int num2 = -1;
				for (int i = num; i > 0; i--)
				{
					bool flag = this.collisionManager.PlaceFree(this, this.position + vector2f + vector2f2 * (float)i, null, this.ignoreCollisionTypes);
					if (flag)
					{
						num2 = i;
						break;
					}
				}
				int num3 = -1;
				for (int j = num; j > 0; j--)
				{
					bool flag2 = this.collisionManager.PlaceFree(this, this.position + vector2f - vector2f2 * (float)j, null, this.ignoreCollisionTypes);
					if (flag2)
					{
						num3 = j;
						break;
					}
				}
				if (num2 >= 0 || num3 >= 0)
				{
					Vector2f position = this.position + ((num2 > num3) ? vector2f2 : (-vector2f2));
					bool flag3 = this.collisionManager.PlaceFree(this, position, null, this.ignoreCollisionTypes);
					if (flag3)
					{
						this.lastPosition = this.position;
						this.position = position;
						this.collisionManager.Update(this, this.lastPosition, this.position);
						position = this.position + vector2f;
						flag3 = this.collisionManager.PlaceFree(this, position, null, this.ignoreCollisionTypes);
						if (flag3)
						{
							this.lastPosition = this.position;
							this.position = position;
							this.collisionManager.Update(this, this.lastPosition, this.position);
						}
					}
				}
			}
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x00017D9C File Offset: 0x00015F9C
		protected override void HandleCollision(ICollidable[] collisionObjects)
		{
			if (this.OnCollision != null)
			{
				this.OnCollision(this, collisionObjects);
			}
			for (int i = 0; i < collisionObjects.Length; i++)
			{
				if (collisionObjects[i] != null && !(collisionObjects[i] is Portal) && !(collisionObjects[i] is TriggerArea))
				{
					if (this.isRunning)
					{
						this.SetRunning(false);
						return;
					}
					if (collisionObjects[i] is SolidStatic)
					{
						this.HandleCornerSliding();
						return;
					}
				}
			}
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x00017E08 File Offset: 0x00016008
		private void UpdateGraphics()
		{
			this.graphicOffset.Y = -this.zOffset;
			this.playerGraphic.Position = VectorMath.Truncate(this.position + this.graphicOffset);
			this.playerGraphic.Depth = (int)this.Position.Y;
			this.pipeline.Update(this.playerGraphic);
			if (this.isShadowEnabled)
			{
				this.shadowGraphic.Position = VectorMath.Truncate(this.position);
				this.shadowGraphic.Depth = this.playerGraphic.Depth - 1;
				this.pipeline.Update(this.shadowGraphic);
			}
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x00017EB8 File Offset: 0x000160B8
		public override void Update()
		{
			if (this.mover != null)
			{
				this.mover.GetNextMove(ref this.position, ref this.velocity, ref this.direction);
				this.speed = (float)((int)VectorMath.Magnitude(this.velocity));
				this.animator.UpdateSubsprite(this.GetAnimationContext());
				this.velocity.X = Math.Max(-1f, Math.Min(1f, this.velocity.X));
				this.velocity.Y = Math.Max(-1f, Math.Min(1f, this.velocity.Y));
				this.moveVector = this.velocity;
			}
			int num = 0;
			while ((float)num < this.speed)
			{
				base.Update();
				if ((int)this.lastPosition.X != (int)this.position.X || (int)this.lastPosition.Y != (int)this.position.Y)
				{
					this.recorder.Record(this.position, this.moveVector, this.terrainType);
				}
				num++;
			}
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
			if ((int)this.lastPosition.X != (int)this.position.X || (int)this.lastPosition.Y != (int)this.position.Y || (int)this.lastZOffset != (int)this.zOffset)
			{
				this.UpdateGraphics();
			}
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x0001808F File Offset: 0x0001628F
		public override void Collision(CollisionContext context)
		{
			if (!(context.Other is TriggerArea))
			{
				base.Collision(context);
				this.playerGraphic.Position = this.Position;
				this.playerGraphic.Depth = (int)this.Position.Y;
			}
		}

		// Token: 0x060003CA RID: 970 RVA: 0x000180D0 File Offset: 0x000162D0
		protected override void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					this.pipeline.Remove(this.playerGraphic);
					this.pipeline.Remove(this.shadowGraphic);
					this.playerGraphic.Dispose();
					this.shadowGraphic.Dispose();
				}
				InputManager.Instance.ButtonPressed -= this.ButtonPressed;
				InputManager.Instance.ButtonReleased -= this.ButtonReleased;
				TimerManager.Instance.OnTimerEnd -= this.CrouchTimerEnd;
				this.disposed = true;
			}
			base.Dispose(disposing);
		}

		// Token: 0x04000550 RID: 1360
		private const float HOT_POINT_LENGTH = 11f;

		// Token: 0x04000551 RID: 1361
		private const int SLIDE_DISTANCE_X = 10;

		// Token: 0x04000552 RID: 1362
		private const int SLIDE_DISTANCE_Y = 8;

		// Token: 0x04000553 RID: 1363
		private const float SPEED_WALK = 1f;

		// Token: 0x04000554 RID: 1364
		private const float SPEED_RUN = 3f;

		// Token: 0x04000555 RID: 1365
		private const float SPEED_CYCLE = 4f;

		// Token: 0x04000556 RID: 1366
		private const int RUN_TIMER_DURATION = 10;

		// Token: 0x04000557 RID: 1367
		private Vector2f moveVector;

		// Token: 0x04000558 RID: 1368
		private Vector2f runVector;

		// Token: 0x04000559 RID: 1369
		private Vector2f lastMoveVector;

		// Token: 0x0400055A RID: 1370
		private RenderPipeline pipeline;

		// Token: 0x0400055B RID: 1371
		private PartyTrain recorder;

		// Token: 0x0400055C RID: 1372
		private Graphic shadowGraphic;

		// Token: 0x0400055D RID: 1373
		private IndexedColorGraphic playerGraphic;

		// Token: 0x0400055E RID: 1374
		private int direction;

		// Token: 0x0400055F RID: 1375
		private float speed;

		// Token: 0x04000560 RID: 1376
		private float lastSpeed;

		// Token: 0x04000561 RID: 1377
		private CharacterType character;

		// Token: 0x04000562 RID: 1378
		private Mover mover;

		// Token: 0x04000563 RID: 1379
		private bool isShadowEnabled;

		// Token: 0x04000564 RID: 1380
		private bool isDead;

		// Token: 0x04000565 RID: 1381
		private bool isNauseous;

		// Token: 0x04000566 RID: 1382
		private bool isCrouch;

		// Token: 0x04000567 RID: 1383
		private bool isRunning;

		// Token: 0x04000568 RID: 1384
		private bool isRunReady;

		// Token: 0x04000569 RID: 1385
		private int crouchTimerIndex;

		// Token: 0x0400056A RID: 1386
		private float hopFactor;

		// Token: 0x0400056B RID: 1387
		private long hopFrame;

		// Token: 0x0400056C RID: 1388
		private float lastZOffset;

		// Token: 0x0400056D RID: 1389
		private Vector2f graphicOffset;

		// Token: 0x0400056E RID: 1390
		private bool isRunButtonPressed;

		// Token: 0x0400056F RID: 1391
		private bool isRunButtonReleased;

		// Token: 0x04000570 RID: 1392
		private bool isRunTimerComplete;

		// Token: 0x04000571 RID: 1393
		private bool isInputLocked;

		// Token: 0x04000572 RID: 1394
		private IndexedColorGraphic effectGraphic;

		// Token: 0x04000573 RID: 1395
		private AnimationControl animator;

		// Token: 0x04000574 RID: 1396
		private int animationLoopCount;

		// Token: 0x04000575 RID: 1397
		private int animationLoopCountTarget;

		// Token: 0x04000576 RID: 1398
		private TerrainType terrainType;

		// Token: 0x04000577 RID: 1399
		private Vector2f checkVector;

		// Token: 0x020000AB RID: 171
		// (Invoke) Token: 0x060003CC RID: 972
		public delegate void OnCollisionHanlder(Player sender, ICollidable[] collisionObjects);

		// Token: 0x020000AC RID: 172
		// (Invoke) Token: 0x060003D0 RID: 976
		public delegate void OnTelepathyAnimationCompleteHanlder(Player sender);

		// Token: 0x020000AD RID: 173
		// (Invoke) Token: 0x060003D4 RID: 980
		public delegate void OnRunningChangeHandler(Player sender);
	}
}

using System;
using Carbine.Actors;
using Carbine.Utility;
using SFML.Graphics;
using SFML.System;

namespace Carbine.Graphics
{
	// Token: 0x02000030 RID: 48
	public class ViewManager
	{
		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060001B5 RID: 437 RVA: 0x00008873 File Offset: 0x00006A73
		public static ViewManager Instance
		{
			get
			{
				if (ViewManager.instance == null)
				{
					ViewManager.instance = new ViewManager();
				}
				return ViewManager.instance;
			}
		}

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060001B6 RID: 438 RVA: 0x0000888C File Offset: 0x00006A8C
		// (remove) Token: 0x060001B7 RID: 439 RVA: 0x000088C4 File Offset: 0x00006AC4
		public event ViewManager.OnMoveHandler OnMove;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x060001B8 RID: 440 RVA: 0x000088FC File Offset: 0x00006AFC
		// (remove) Token: 0x060001B9 RID: 441 RVA: 0x00008934 File Offset: 0x00006B34
		public event ViewManager.OnMoveToCompleteHandler OnMoveToComplete;

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060001BA RID: 442 RVA: 0x00008969 File Offset: 0x00006B69
		public View View
		{
			get
			{
				return this.view;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060001BB RID: 443 RVA: 0x00008971 File Offset: 0x00006B71
		public FloatRect Viewrect
		{
			get
			{
				this.SetViewRect();
				return this.viewRect;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060001BC RID: 444 RVA: 0x0000897F File Offset: 0x00006B7F
		public Vector2f FinalCenter
		{
			get
			{
				return this.GetCenter();
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060001BD RID: 445 RVA: 0x00008987 File Offset: 0x00006B87
		public Vector2f FinalTopLeft
		{
			get
			{
				return this.GetCenter() - Engine.HALF_SCREEN_SIZE;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060001BE RID: 446 RVA: 0x00008999 File Offset: 0x00006B99
		// (set) Token: 0x060001BF RID: 447 RVA: 0x000089A4 File Offset: 0x00006BA4
		public Vector2f Center
		{
			get
			{
				return this.viewCenter;
			}
			set
			{
				this.previousViewCenter = this.viewCenter;
				this.viewCenter = value;
				if (this.previousViewCenter != this.viewCenter && this.OnMove != null)
				{
					this.OnMove(this, this.view.Center);
				}
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060001C0 RID: 448 RVA: 0x000089F6 File Offset: 0x00006BF6
		public Vector2f TopLeft
		{
			get
			{
				return this.viewCenter - Engine.HALF_SCREEN_SIZE;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060001C1 RID: 449 RVA: 0x00008A08 File Offset: 0x00006C08
		// (set) Token: 0x060001C2 RID: 450 RVA: 0x00008A10 File Offset: 0x00006C10
		public Actor FollowActor
		{
			get
			{
				return this.followActor;
			}
			set
			{
				this.followActor = value;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x00008A19 File Offset: 0x00006C19
		// (set) Token: 0x060001C4 RID: 452 RVA: 0x00008A21 File Offset: 0x00006C21
		public Vector2f Offset
		{
			get
			{
				return this.offset;
			}
			set
			{
				this.offset = value;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060001C5 RID: 453 RVA: 0x00008A2A File Offset: 0x00006C2A
		// (set) Token: 0x060001C6 RID: 454 RVA: 0x00008A32 File Offset: 0x00006C32
		public ViewManager.MoveMode MoveToMode
		{
			get
			{
				return this.moveToMode;
			}
			set
			{
				this.moveToMode = value;
			}
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x00008A3C File Offset: 0x00006C3C
		private ViewManager()
		{
			this.window = Engine.FrameBuffer;
			this.view = new View(new Vector2f(0f, 0f), new Vector2f(320f, 180f));
			this.window.SetView(this.view);
			this.viewCenter = VectorMath.ZERO_VECTOR;
			this.offset = VectorMath.ZERO_VECTOR;
			this.shakeOffset = VectorMath.ZERO_VECTOR;
			this.viewRect = default(FloatRect);
			this.SetViewRect();
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x00008AC7 File Offset: 0x00006CC7
		private Vector2f GetCenter()
		{
			return VectorMath.Truncate(this.viewCenter + this.offset + this.shakeOffset);
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x00008AEC File Offset: 0x00006CEC
		private void SetViewRect()
		{
			this.viewRect.Left = this.view.Center.X - this.view.Size.X / 2f;
			this.viewRect.Top = this.view.Center.Y - this.view.Size.Y / 2f;
			this.viewRect.Width = this.view.Size.X;
			this.viewRect.Height = this.view.Size.Y;
		}

		// Token: 0x060001CA RID: 458 RVA: 0x00008B94 File Offset: 0x00006D94
		private float CalculateSmoothMovement(float progress)
		{
			return (float)(1.0 - (Math.Cos((double)progress * 2.0 * 3.141592653589793) / 2.0 + 0.5)) * (this.moveToSpeed - 0.5f) + 0.5f;
		}

		// Token: 0x060001CB RID: 459 RVA: 0x00008BF0 File Offset: 0x00006DF0
		public void Update()
		{
			if (this.shakeProgress < this.shakeDuration)
			{
				float num = this.shakeIntensity.X * (1f - (float)this.shakeProgress / (float)this.shakeDuration);
				float num2 = this.shakeIntensity.Y * (1f - (float)this.shakeProgress / (float)this.shakeDuration);
				this.shakeOffset.X = -num + (float)Engine.Random.NextDouble() * num * 2f;
				this.shakeOffset.Y = -num2 + (float)Engine.Random.NextDouble() * num2 * 2f;
				this.shakeProgress++;
			}
			if (this.followActor != null)
			{
				if (!this.isMovingTo)
				{
					this.previousViewCenter = this.viewCenter;
					this.viewCenter = this.followActor.Position;
				}
				else
				{
					this.moveToPosition = this.followActor.Position;
				}
			}
			if (this.isMovingTo)
			{
				float num3 = VectorMath.Magnitude(this.moveFromPosition - this.moveToPosition);
				float num4 = VectorMath.Magnitude(this.viewCenter - this.moveToPosition);
				float num5 = (num3 > 0f) ? (1f - num4 / num3) : 1f;
				float num6 = 1f;
				switch (this.moveToMode)
				{
				case ViewManager.MoveMode.Linear:
					num6 = this.moveToSpeed;
					break;
				case ViewManager.MoveMode.Smoothed:
					num6 = this.CalculateSmoothMovement(num5);
					break;
				case ViewManager.MoveMode.ExpIn:
					num6 = (1f - num5) * (this.moveToSpeed - 0.5f) + 0.5f;
					break;
				case ViewManager.MoveMode.ExpOut:
					num6 = num5 * (this.moveToSpeed - 0.5f) + 0.5f;
					break;
				}
				this.previousViewCenter = this.viewCenter;
				this.viewCenter += VectorMath.Normalize(this.moveToPosition - this.moveFromPosition) * Math.Max(0.1f, num6);
				if (num4 - num6 <= 0.5f)
				{
					this.viewCenter = this.moveToPosition;
					this.isMovingTo = false;
					if (this.OnMoveToComplete != null)
					{
						this.OnMoveToComplete(this);
					}
				}
			}
			this.view.Center = VectorMath.Truncate(this.viewCenter + this.offset + this.shakeOffset);
			if (this.previousViewCenter != this.viewCenter && this.OnMove != null)
			{
				this.OnMove(this, this.view.Center);
			}
		}

		// Token: 0x060001CC RID: 460 RVA: 0x00008E7D File Offset: 0x0000707D
		public void MoveTo(Actor actor, float speed)
		{
			if (actor != null)
			{
				this.followActor = actor;
				this.MoveTo(actor.Position, speed);
			}
		}

		// Token: 0x060001CD RID: 461 RVA: 0x00008E98 File Offset: 0x00007098
		public void MoveTo(Vector2f position, float speed)
		{
			if (speed > 0f)
			{
				this.moveFromPosition = this.viewCenter;
				this.moveToPosition = position;
				this.moveToSpeed = speed;
				this.isMovingTo = true;
				return;
			}
			this.moveFromPosition = this.viewCenter;
			this.moveToPosition = position;
			this.viewCenter = this.moveToPosition;
			this.moveToSpeed = 0f;
			this.isMovingTo = false;
		}

		// Token: 0x060001CE RID: 462 RVA: 0x00008F00 File Offset: 0x00007100
		public void MoveTo(float x, float y, float speed)
		{
			this.MoveTo(new Vector2f(x, y), speed);
		}

		// Token: 0x060001CF RID: 463 RVA: 0x00008F10 File Offset: 0x00007110
		public void CancelMoveTo()
		{
			this.isMovingTo = false;
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x00008F19 File Offset: 0x00007119
		public void Move(float x, float y)
		{
			this.Move(new Vector2f(x, y));
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x00008F28 File Offset: 0x00007128
		public void Move(Vector2f offset)
		{
			this.view.Move(offset);
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x00008F36 File Offset: 0x00007136
		public void UseView()
		{
			this.window.SetView(this.view);
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x00008F49 File Offset: 0x00007149
		public void UseDefault()
		{
			this.window.SetView(this.window.DefaultView);
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x00008F64 File Offset: 0x00007164
		public void Reset()
		{
			this.view.Reset(new FloatRect(0f, 0f, 320f, 180f));
			this.viewCenter = this.view.Center;
			this.shakeOffset = VectorMath.ZERO_VECTOR;
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x00008FB1 File Offset: 0x000071B1
		public void Shake(Vector2f intensity, float duration)
		{
			this.shakeIntensity = intensity;
			this.shakeDuration = (int)(duration * 60f);
			this.shakeProgress = 0;
			this.shakeOffset = VectorMath.ZERO_VECTOR;
		}

		// Token: 0x040000FA RID: 250
		private static ViewManager instance;

		// Token: 0x040000FB RID: 251
		private View view;

		// Token: 0x040000FC RID: 252
		private RenderTarget window;

		// Token: 0x040000FD RID: 253
		private Actor followActor;

		// Token: 0x040000FE RID: 254
		private FloatRect viewRect;

		// Token: 0x040000FF RID: 255
		private Vector2f previousViewCenter;

		// Token: 0x04000100 RID: 256
		private Vector2f viewCenter;

		// Token: 0x04000101 RID: 257
		private Vector2f offset;

		// Token: 0x04000102 RID: 258
		private Vector2f shakeOffset;

		// Token: 0x04000103 RID: 259
		private Vector2f shakeIntensity;

		// Token: 0x04000104 RID: 260
		private int shakeDuration;

		// Token: 0x04000105 RID: 261
		private int shakeProgress;

		// Token: 0x04000106 RID: 262
		private bool isMovingTo;

		// Token: 0x04000107 RID: 263
		private Vector2f moveFromPosition;

		// Token: 0x04000108 RID: 264
		private Vector2f moveToPosition;

		// Token: 0x04000109 RID: 265
		private float moveToSpeed;

		// Token: 0x0400010A RID: 266
		private ViewManager.MoveMode moveToMode;

		// Token: 0x02000031 RID: 49
		public enum MoveMode
		{
			// Token: 0x0400010E RID: 270
			Linear,
			// Token: 0x0400010F RID: 271
			Smoothed,
			// Token: 0x04000110 RID: 272
			ExpIn,
			// Token: 0x04000111 RID: 273
			ExpOut
		}

		// Token: 0x02000032 RID: 50
		// (Invoke) Token: 0x060001D8 RID: 472
		public delegate void OnMoveHandler(ViewManager sender, Vector2f newCenter);

		// Token: 0x02000033 RID: 51
		// (Invoke) Token: 0x060001DC RID: 476
		public delegate void OnMoveToCompleteHandler(ViewManager sender);
	}
}

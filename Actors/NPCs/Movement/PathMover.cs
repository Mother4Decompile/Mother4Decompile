using System;
using System.Collections.Generic;
using Carbine.Utility;
using SFML.System;

namespace Mother4.Actors.NPCs.Movement
{
	// Token: 0x020000A3 RID: 163
	internal class PathMover : Mover
	{
		// Token: 0x1400000C RID: 12
		// (add) Token: 0x0600035E RID: 862 RVA: 0x00015CCC File Offset: 0x00013ECC
		// (remove) Token: 0x0600035F RID: 863 RVA: 0x00015D04 File Offset: 0x00013F04
		public event PathMover.OnPathCompleteHandler OnPathComplete;

		// Token: 0x06000360 RID: 864 RVA: 0x00015D3C File Offset: 0x00013F3C
		public PathMover(float speed, int timeOut, bool loop, List<Vector2f> path)
		{
			this.speed = speed;
			this.timeOut = timeOut;
			this.loop = loop;
			this.path = new List<Vector2f>(path);
			this.nodeIndex = 0;
			this.target = path[this.nodeIndex];
			this.incr = 1;
		}

		// Token: 0x06000361 RID: 865 RVA: 0x00015D94 File Offset: 0x00013F94
		private bool IsAtTarget(ref Vector2f position)
		{
			return Math.Abs(this.target.X - position.X) <= 1f && Math.Abs(this.target.Y - position.Y) <= 1f;
		}

		// Token: 0x06000362 RID: 866 RVA: 0x00015DE2 File Offset: 0x00013FE2
		private void DoStep(ref Vector2f position, ref Vector2f velocity, ref int direction)
		{
			base.MovementStep(this.speed, ref position, ref this.target, ref velocity);
			this.changed = true;
			if (this.IsAtTarget(ref position))
			{
				this.targetReachedFlag = true;
				velocity = VectorMath.ZERO_VECTOR;
			}
		}

		// Token: 0x06000363 RID: 867 RVA: 0x00015E1C File Offset: 0x0001401C
		public override bool GetNextMove(ref Vector2f position, ref Vector2f velocity, ref int direction)
		{
			this.changed = false;
			if (!this.targetReachedFlag)
			{
				this.DoStep(ref position, ref velocity, ref direction);
			}
			else if (this.timer < this.timeOut)
			{
				this.timer++;
			}
			else
			{
				if (this.nodeIndex + this.incr >= this.path.Count)
				{
					if (this.loop)
					{
						this.nodeIndex = 0;
					}
					else
					{
						this.incr = -this.incr;
						if (this.OnPathComplete != null)
						{
							this.OnPathComplete();
						}
					}
				}
				else if (this.nodeIndex + this.incr < 0)
				{
					if (this.loop)
					{
						this.nodeIndex = this.path.Count - 1;
					}
					else
					{
						this.incr = -this.incr;
						if (this.OnPathComplete != null)
						{
							this.OnPathComplete();
						}
					}
				}
				this.nodeIndex += this.incr;
				this.target = this.path[this.nodeIndex];
				this.timer = 0;
				this.targetReachedFlag = false;
			}
			return this.changed;
		}

		// Token: 0x040004FB RID: 1275
		private List<Vector2f> path;

		// Token: 0x040004FC RID: 1276
		private Vector2f target;

		// Token: 0x040004FD RID: 1277
		private int nodeIndex;

		// Token: 0x040004FE RID: 1278
		private int incr;

		// Token: 0x040004FF RID: 1279
		private int timer;

		// Token: 0x04000500 RID: 1280
		private int timeOut;

		// Token: 0x04000501 RID: 1281
		private float speed;

		// Token: 0x04000502 RID: 1282
		private bool loop;

		// Token: 0x04000503 RID: 1283
		private bool changed;

		// Token: 0x04000504 RID: 1284
		private bool targetReachedFlag;

		// Token: 0x020000A4 RID: 164
		// (Invoke) Token: 0x06000365 RID: 869
		public delegate void OnPathCompleteHandler();
	}
}

using System;
using Carbine;
using SFML.Graphics;
using SFML.System;

namespace Mother4.Actors.NPCs.Movement
{
	// Token: 0x0200009F RID: 159
	internal class AreaMover : Mover
	{
		// Token: 0x06000354 RID: 852 RVA: 0x00015A39 File Offset: 0x00013C39
		public AreaMover(float speed, int timeOut, float distance, float left, float top, float width, float height)
		{
			this.speed = speed;
			this.timeOut = 60;
			this.distance = distance;
			this.bounds = new FloatRect(left, top, width, height);
			this.Randomize();
		}

		// Token: 0x06000355 RID: 853 RVA: 0x00015A78 File Offset: 0x00013C78
		private void Randomize()
		{
			this.target.X = this.bounds.Left + (float)Engine.Random.Next((int)this.bounds.Width);
			this.target.Y = this.bounds.Top + (float)Engine.Random.Next((int)this.bounds.Height);
		}

		// Token: 0x06000356 RID: 854 RVA: 0x00015AE4 File Offset: 0x00013CE4
		private bool IsAtTarget(ref Vector2f position)
		{
			return Math.Abs(this.target.X - position.X) <= 1f && Math.Abs(this.target.Y - position.Y) <= 1f;
		}

		// Token: 0x06000357 RID: 855 RVA: 0x00015B34 File Offset: 0x00013D34
		public override bool GetNextMove(ref Vector2f position, ref Vector2f velocity, ref int direction)
		{
			this.changed = false;
			if (!this.targetReachedFlag)
			{
				base.MovementStep(this.speed, ref position, ref this.target, ref velocity);
				this.changed = true;
				if (this.IsAtTarget(ref position) || this.abortTimer > this.timeToAbort)
				{
					this.targetReachedFlag = true;
					velocity = new Vector2f(0f, 0f);
				}
			}
			else if (this.timer < this.timeOut)
			{
				this.timer++;
			}
			else
			{
				this.Randomize();
				this.timer = 0;
				this.abortTimer = 0;
				this.targetReachedFlag = false;
			}
			this.abortTimer++;
			return this.changed;
		}

		// Token: 0x040004EC RID: 1260
		private FloatRect bounds;

		// Token: 0x040004ED RID: 1261
		private float distance;

		// Token: 0x040004EE RID: 1262
		private Vector2f target;

		// Token: 0x040004EF RID: 1263
		private int timer;

		// Token: 0x040004F0 RID: 1264
		private int timeOut;

		// Token: 0x040004F1 RID: 1265
		private int abortTimer;

		// Token: 0x040004F2 RID: 1266
		private int timeToAbort = 120;

		// Token: 0x040004F3 RID: 1267
		private float speed;

		// Token: 0x040004F4 RID: 1268
		private bool changed;

		// Token: 0x040004F5 RID: 1269
		private bool targetReachedFlag;
	}
}

using System;
using Carbine;
using SFML.System;

namespace Mother4.Actors.NPCs.Movement
{
	// Token: 0x020000A5 RID: 165
	internal class RandomMover : Mover
	{
		// Token: 0x06000368 RID: 872 RVA: 0x00015F3F File Offset: 0x0001413F
		public RandomMover(float speed, float distance, int timeOut)
		{
			this.speed = speed;
			this.distance = distance;
			this.timeOut = timeOut;
			this.target = default(Vector2f);
		}

		// Token: 0x06000369 RID: 873 RVA: 0x00015F68 File Offset: 0x00014168
		private void Randomize(ref Vector2f position)
		{
			this.target.X = position.X + (float)Math.Sign(Engine.Random.Next(3) - 1) * this.distance;
			this.target.Y = position.Y + (float)Math.Sign(Engine.Random.Next(3) - 1) * this.distance;
		}

		// Token: 0x0600036A RID: 874 RVA: 0x00015FD0 File Offset: 0x000141D0
		public override bool GetNextMove(ref Vector2f position, ref Vector2f velocity, ref int direction)
		{
			this.changed = false;
			bool flag = Math.Abs(this.target.X - position.X) <= 1f && Math.Abs(this.target.Y - position.Y) <= 1f;
			bool flag2 = this.timer <= 0;
			bool flag3 = this.timer > this.timeOut;
			if ((flag && flag3) || flag2 || flag3)
			{
				this.Randomize(ref position);
				this.timer = 0;
			}
			else
			{
				base.MovementStep(1f, ref position, ref this.target, ref velocity);
				this.changed = true;
			}
			this.timer++;
			return this.changed;
		}

		// Token: 0x04000506 RID: 1286
		private float distance;

		// Token: 0x04000507 RID: 1287
		private Vector2f target;

		// Token: 0x04000508 RID: 1288
		private int timer;

		// Token: 0x04000509 RID: 1289
		private int timeOut;

		// Token: 0x0400050A RID: 1290
		private float speed;

		// Token: 0x0400050B RID: 1291
		private bool changed;
	}
}

using System;
using Carbine.Utility;
using SFML.System;

namespace Mother4.Actors.NPCs.Movement
{
	// Token: 0x0200000E RID: 14
	internal class PointMover : Mover
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000012 RID: 18 RVA: 0x000032A8 File Offset: 0x000014A8
		// (remove) Token: 0x06000013 RID: 19 RVA: 0x000032E0 File Offset: 0x000014E0
		public event PointMover.OnMoveCompleteHandler OnMoveComplete;

		// Token: 0x06000014 RID: 20 RVA: 0x00003315 File Offset: 0x00001515
		public PointMover(Vector2f position, float speed)
		{
			this.target = position;
			this.speed = speed;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x0000332C File Offset: 0x0000152C
		public override bool GetNextMove(ref Vector2f position, ref Vector2f velocity, ref int direction)
		{
			bool result = false;
			if (!this.done)
			{
				if (Math.Abs(this.target.X - position.X) > this.speed || Math.Abs(this.target.Y - position.Y) > this.speed)
				{
					base.MovementStep(this.speed, ref position, ref this.target, ref velocity);
					direction = VectorMath.VectorToDirection(velocity);
					result = true;
				}
				else
				{
					position = this.target;
					velocity = VectorMath.ZERO_VECTOR;
					this.done = true;
					result = true;
					if (this.OnMoveComplete != null)
					{
						this.OnMoveComplete(this);
					}
				}
			}
			return result;
		}

		// Token: 0x040000BF RID: 191
		private Vector2f target;

		// Token: 0x040000C0 RID: 192
		private float speed;

		// Token: 0x040000C1 RID: 193
		private bool done;

		// Token: 0x0200000F RID: 15
		// (Invoke) Token: 0x06000017 RID: 23
		public delegate void OnMoveCompleteHandler(PointMover sender);
	}
}

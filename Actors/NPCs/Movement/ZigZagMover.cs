using System;
using Carbine;
using Carbine.Collision;
using Carbine.Graphics;
using Carbine.Utility;
using SFML.Graphics;
using SFML.System;

namespace Mother4.Actors.NPCs.Movement
{
	// Token: 0x02000010 RID: 16
	internal class ZigZagMover : Mover
	{
		// Token: 0x0600001A RID: 26 RVA: 0x000033E8 File Offset: 0x000015E8
		public ZigZagMover(CollisionManager collisionManager, ICollidable collidable, FloatRect area, float amplitude, float speed, float chaseThreshold)
		{
			this.collisionManager = collisionManager;
			this.collidable = collidable;
			this.amplitude = amplitude;
			this.speed = speed;
			this.chaseThreshold = chaseThreshold;
			this.area = area;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x0000341D File Offset: 0x0000161D
		private void resetTimer()
		{
			this.timerEnd = Engine.Frame + 180L;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00003434 File Offset: 0x00001634
		public override bool GetNextMove(ref Vector2f position, ref Vector2f velocity, ref int direction)
		{
			this.changed = false;
			if (Engine.Frame > this.timerEnd)
			{
				this.mode = ZigZagMover.Mode.Move;
				float num = 2.1474836E+09f;
				if (ViewManager.Instance.FollowActor != null)
				{
					num = VectorMath.Magnitude(ViewManager.Instance.FollowActor.Position - position);
				}
				if (num > this.chaseThreshold)
				{
					do
					{
						int num2 = (int)this.area.Left + Engine.Random.Next((int)this.area.Width);
						int num3 = (int)this.area.Top + Engine.Random.Next((int)this.area.Height);
						this.toPosition = new Vector2f((float)num2, (float)num3);
						if (VectorMath.Magnitude(position - this.toPosition) <= 30f)
						{
							break;
						}
					}
					while (!this.collisionManager.PlaceFree(this.collidable, this.toPosition));
				}
				else
				{
					this.toPosition = ViewManager.Instance.FollowActor.Position;
				}
				this.fromPosition = position;
				this.directionVector = VectorMath.Normalize(this.toPosition - position);
				this.normalVector = VectorMath.Normalize(VectorMath.LeftNormal(this.directionVector));
				this.resetTimer();
				velocity = VectorMath.ZERO_VECTOR;
			}
			if (this.mode == ZigZagMover.Mode.Move)
			{
				if (VectorMath.Magnitude(position - this.toPosition) > 1f)
				{
					float num4 = VectorMath.Magnitude(this.toPosition - this.fromPosition);
					float num5 = VectorMath.Magnitude(position - this.fromPosition) / num4;
					float x = (float)Math.Sin((double)num5 * 3.141592653589793 * 3.0) * (1f - num5) * (this.amplitude / 4f);
					velocity = this.directionVector * this.speed + this.normalVector * x;
				}
				else
				{
					this.mode = ZigZagMover.Mode.Wait;
				}
				this.changed = true;
			}
			return this.changed;
		}

		// Token: 0x040000C3 RID: 195
		private const int TIMER_LENGTH = 180;

		// Token: 0x040000C4 RID: 196
		private bool changed;

		// Token: 0x040000C5 RID: 197
		private ZigZagMover.Mode mode;

		// Token: 0x040000C6 RID: 198
		private long timerEnd;

		// Token: 0x040000C7 RID: 199
		private Vector2f fromPosition;

		// Token: 0x040000C8 RID: 200
		private Vector2f toPosition;

		// Token: 0x040000C9 RID: 201
		private Vector2f directionVector;

		// Token: 0x040000CA RID: 202
		private Vector2f normalVector;

		// Token: 0x040000CB RID: 203
		private CollisionManager collisionManager;

		// Token: 0x040000CC RID: 204
		private ICollidable collidable;

		// Token: 0x040000CD RID: 205
		private float amplitude;

		// Token: 0x040000CE RID: 206
		private float chaseThreshold;

		// Token: 0x040000CF RID: 207
		private float speed;

		// Token: 0x040000D0 RID: 208
		private FloatRect area;

		// Token: 0x02000011 RID: 17
		private enum Mode
		{
			// Token: 0x040000D2 RID: 210
			Wait,
			// Token: 0x040000D3 RID: 211
			Move
		}
	}
}

using System;
using Carbine;
using Carbine.Collision;
using Carbine.Graphics;
using Carbine.Utility;
using SFML.Graphics;
using SFML.System;

namespace Mother4.Actors.NPCs.Movement
{
	// Token: 0x0200000A RID: 10
	internal class TeleportMover : Mover
	{
		// Token: 0x0600000D RID: 13 RVA: 0x00002FCF File Offset: 0x000011CF
		public TeleportMover(CollisionManager collisionManager, ICollidable collidable, FloatRect area, float maxDistance, float chaseThreshold)
		{
			this.collisionManager = collisionManager;
			this.collidable = collidable;
			this.area = area;
			this.maxDistance = maxDistance;
			this.chaseThreshold = chaseThreshold;
			this.resetTimer();
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00003002 File Offset: 0x00001202
		private void resetTimer()
		{
			this.timerEnd = Engine.Frame + 33L;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00003014 File Offset: 0x00001214
		public override bool GetNextMove(ref Vector2f position, ref Vector2f velocity, ref int direction)
		{
			this.changed = false;
			if (this.mode == TeleportMover.Mode.Wait)
			{
				if (Engine.Frame > this.timerEnd)
				{
					this.mode = TeleportMover.Mode.Teleport;
					this.resetTimer();
				}
				velocity = VectorMath.ZERO_VECTOR;
			}
			if (this.mode == TeleportMover.Mode.Teleport)
			{
				float num = 2.1474836E+09f;
				if (ViewManager.Instance.FollowActor != null)
				{
					num = VectorMath.Magnitude(ViewManager.Instance.FollowActor.Position - position);
				}
				if (num < this.chaseThreshold)
				{
					if (ViewManager.Instance.FollowActor is Player)
					{
						Player player = (Player)ViewManager.Instance.FollowActor;
						position = ViewManager.Instance.FollowActor.Position - VectorMath.DirectionToVector(player.Direction) * 4f;
					}
					else
					{
						position = ViewManager.Instance.FollowActor.Position;
					}
					direction = VectorMath.VectorToDirection(ViewManager.Instance.FollowActor.Position - position);
				}
				else
				{
					Vector2f vector2f;
					do
					{
						int num2 = (int)this.area.Left + Engine.Random.Next((int)this.area.Width);
						int num3 = (int)this.area.Top + Engine.Random.Next((int)this.area.Height);
						vector2f = new Vector2f((float)num2, (float)num3);
					}
					while (!this.collisionManager.PlaceFree(this.collidable, vector2f));
					position = vector2f;
					direction = 6;
				}
				velocity = VectorMath.ZERO_VECTOR;
				this.changed = true;
				this.mode = TeleportMover.Mode.Wait;
			}
			return this.changed;
		}

		// Token: 0x040000AB RID: 171
		private const int TIMER_LENGTH = 33;

		// Token: 0x040000AC RID: 172
		private CollisionManager collisionManager;

		// Token: 0x040000AD RID: 173
		private ICollidable collidable;

		// Token: 0x040000AE RID: 174
		private FloatRect area;

		// Token: 0x040000AF RID: 175
		private float maxDistance;

		// Token: 0x040000B0 RID: 176
		private float chaseThreshold;

		// Token: 0x040000B1 RID: 177
		private long timerEnd;

		// Token: 0x040000B2 RID: 178
		private TeleportMover.Mode mode;

		// Token: 0x040000B3 RID: 179
		private bool changed;

		// Token: 0x0200000B RID: 11
		private enum Mode
		{
			// Token: 0x040000B5 RID: 181
			Wait,
			// Token: 0x040000B6 RID: 182
			Teleport
		}
	}
}

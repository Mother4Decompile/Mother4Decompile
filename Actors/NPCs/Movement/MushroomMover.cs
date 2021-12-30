using System;
using Carbine.Graphics;
using Carbine.Utility;
using SFML.System;

namespace Mother4.Actors.NPCs.Movement
{
	// Token: 0x02000008 RID: 8
	internal class MushroomMover : Mover
	{
		// Token: 0x0600000A RID: 10 RVA: 0x00002E77 File Offset: 0x00001077
		public MushroomMover(EnemyNPC enemy, float chaseThreshold, float speed)
		{
			this.enemy = enemy;
			this.chaseThreshold = chaseThreshold;
			this.speed = speed;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002E94 File Offset: 0x00001094
		public override bool GetNextMove(ref Vector2f position, ref Vector2f velocity, ref int direction)
		{
			this.changed = false;
			if (this.mode == MushroomMover.Mode.Wait)
			{
				if (ViewManager.Instance.FollowActor != null)
				{
					float num = VectorMath.Magnitude(ViewManager.Instance.FollowActor.Position - position);
					this.mode = ((num < this.chaseThreshold) ? MushroomMover.Mode.Pop : MushroomMover.Mode.Wait);
				}
				velocity = VectorMath.ZERO_VECTOR;
			}
			else if (this.mode == MushroomMover.Mode.Pop)
			{
				this.enemy.OverrideSubsprite("pop");
				this.enemy.Graphic.OnAnimationComplete += this.OnAnimationComplete;
				this.mode = MushroomMover.Mode.PopWait;
				this.changed = true;
			}
			else if (this.mode == MushroomMover.Mode.Chase && ViewManager.Instance.FollowActor != null)
			{
				direction = VectorMath.VectorToDirection(ViewManager.Instance.FollowActor.Position - position);
				velocity = VectorMath.DirectionToVector(direction) * this.speed;
				this.changed = true;
			}
			return this.changed;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002F9F File Offset: 0x0000119F
		private void OnAnimationComplete(AnimatedRenderable graphic)
		{
			this.mode = MushroomMover.Mode.Chase;
			this.enemy.ClearOverrideSubsprite();
			this.enemy.Graphic.OnAnimationComplete -= this.OnAnimationComplete;
		}

		// Token: 0x040000A1 RID: 161
		private MushroomMover.Mode mode;

		// Token: 0x040000A2 RID: 162
		private bool changed;

		// Token: 0x040000A3 RID: 163
		private EnemyNPC enemy;

		// Token: 0x040000A4 RID: 164
		private float chaseThreshold;

		// Token: 0x040000A5 RID: 165
		private float speed;

		// Token: 0x02000009 RID: 9
		private enum Mode
		{
			// Token: 0x040000A7 RID: 167
			Wait,
			// Token: 0x040000A8 RID: 168
			Pop,
			// Token: 0x040000A9 RID: 169
			PopWait,
			// Token: 0x040000AA RID: 170
			Chase
		}
	}
}

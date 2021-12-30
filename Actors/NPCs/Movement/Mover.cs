using System;
using SFML.System;

namespace Mother4.Actors.NPCs.Movement
{
	// Token: 0x02000005 RID: 5
	internal abstract class Mover
	{
		// Token: 0x06000004 RID: 4 RVA: 0x00002CBC File Offset: 0x00000EBC
		protected void MovementStep(float stepSize, ref Vector2f position, ref Vector2f target, ref Vector2f velocity)
		{
			velocity.X = (float)Math.Sign(target.X - position.X) * Math.Min(stepSize, Math.Abs(target.X - position.X));
			velocity.Y = (float)Math.Sign(target.Y - position.Y) * Math.Min(stepSize, Math.Abs(target.Y - position.Y));
		}

		// Token: 0x06000005 RID: 5
		public abstract bool GetNextMove(ref Vector2f position, ref Vector2f velocity, ref int direction);

		// Token: 0x04000095 RID: 149
		protected const float TOLERANCE = 1f;
	}
}

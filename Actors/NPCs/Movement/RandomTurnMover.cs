using System;
using Carbine;
using SFML.System;

namespace Mother4.Actors.NPCs.Movement
{
	// Token: 0x020000A1 RID: 161
	internal class RandomTurnMover : Mover
	{
		// Token: 0x0600035A RID: 858 RVA: 0x00015C57 File Offset: 0x00013E57
		public RandomTurnMover(int time)
		{
			this.time = time;
			this.timer = 0;
		}

		// Token: 0x0600035B RID: 859 RVA: 0x00015C70 File Offset: 0x00013E70
		public override bool GetNextMove(ref Vector2f position, ref Vector2f velocity, ref int direction)
		{
			this.changed = false;
			if (this.timer >= this.time)
			{
				this.timer = 0;
				direction = Engine.Random.Next(8);
				this.changed = true;
			}
			this.timer++;
			return this.changed;
		}

		// Token: 0x040004F8 RID: 1272
		private int time;

		// Token: 0x040004F9 RID: 1273
		private int timer;

		// Token: 0x040004FA RID: 1274
		private bool changed;
	}
}

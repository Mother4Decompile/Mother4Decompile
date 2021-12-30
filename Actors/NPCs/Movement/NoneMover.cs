using System;
using SFML.System;

namespace Mother4.Actors.NPCs.Movement
{
	// Token: 0x020000A2 RID: 162
	internal class NoneMover : Mover
	{
		// Token: 0x0600035D RID: 861 RVA: 0x00015CC9 File Offset: 0x00013EC9
		public override bool GetNextMove(ref Vector2f position, ref Vector2f velocity, ref int direction)
		{
			return false;
		}
	}
}

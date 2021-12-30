using System;
using SFML.Graphics;
using SFML.System;

namespace Carbine.Collision
{
	// Token: 0x02000012 RID: 18
	public struct AABB
	{
		// Token: 0x0600008B RID: 139 RVA: 0x00003664 File Offset: 0x00001864
		public AABB(Vector2f position, Vector2f size)
		{
			this.Position = position;
			this.Size = size;
			this.IsPlayer = false;
			this.OnlyPlayer = false;
			this.floatRect = new FloatRect(this.Position.X, this.Position.Y, this.Size.X, this.Size.Y);
		}

		// Token: 0x0600008C RID: 140 RVA: 0x000036C4 File Offset: 0x000018C4
		public AABB(Vector2f position, Vector2f size, bool isPlayer, bool onlyPlayer)
		{
			this.Position = position;
			this.Size = size;
			this.IsPlayer = isPlayer;
			this.OnlyPlayer = onlyPlayer;
			this.floatRect = new FloatRect(this.Position.X, this.Position.Y, this.Size.X, this.Size.Y);
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00003725 File Offset: 0x00001925
		public FloatRect GetFloatRect()
		{
			return this.floatRect;
		}

		// Token: 0x04000042 RID: 66
		private FloatRect floatRect;

		// Token: 0x04000043 RID: 67
		public readonly Vector2f Position;

		// Token: 0x04000044 RID: 68
		public readonly Vector2f Size;

		// Token: 0x04000045 RID: 69
		public readonly bool IsPlayer;

		// Token: 0x04000046 RID: 70
		public readonly bool OnlyPlayer;
	}
}

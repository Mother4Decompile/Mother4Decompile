using System;
using SFML.System;

namespace Carbine.Tiles
{
	// Token: 0x0200005A RID: 90
	public struct Tile
	{
		// Token: 0x06000291 RID: 657 RVA: 0x0000DD83 File Offset: 0x0000BF83
		public Tile(uint tileID, Vector2f position, bool flipHoriz, bool flipVert, bool flipDiag, ushort animId)
		{
			this.ID = tileID;
			this.Position = position;
			this.FlipHorizontal = flipHoriz;
			this.FlipVertical = flipVert;
			this.FlipDiagonal = flipDiag;
			this.AnimationId = animId;
		}

		// Token: 0x040001F1 RID: 497
		public const uint SIZE = 8U;

		// Token: 0x040001F2 RID: 498
		public readonly uint ID;

		// Token: 0x040001F3 RID: 499
		public readonly Vector2f Position;

		// Token: 0x040001F4 RID: 500
		public readonly bool FlipHorizontal;

		// Token: 0x040001F5 RID: 501
		public readonly bool FlipVertical;

		// Token: 0x040001F6 RID: 502
		public readonly bool FlipDiagonal;

		// Token: 0x040001F7 RID: 503
		public readonly ushort AnimationId;
	}
}

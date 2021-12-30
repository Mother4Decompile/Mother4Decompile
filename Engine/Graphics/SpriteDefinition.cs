using System;
using SFML.System;

namespace Carbine.Graphics
{
	// Token: 0x0200002D RID: 45
	public class SpriteDefinition
	{
		// Token: 0x17000065 RID: 101
		// (get) Token: 0x0600018F RID: 399 RVA: 0x00007A22 File Offset: 0x00005C22
		// (set) Token: 0x06000190 RID: 400 RVA: 0x00007A2A File Offset: 0x00005C2A
		public string Name { get; private set; }

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000191 RID: 401 RVA: 0x00007A33 File Offset: 0x00005C33
		// (set) Token: 0x06000192 RID: 402 RVA: 0x00007A3B File Offset: 0x00005C3B
		public Vector2i Coords { get; private set; }

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000193 RID: 403 RVA: 0x00007A44 File Offset: 0x00005C44
		// (set) Token: 0x06000194 RID: 404 RVA: 0x00007A4C File Offset: 0x00005C4C
		public Vector2i Bounds { get; private set; }

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000195 RID: 405 RVA: 0x00007A55 File Offset: 0x00005C55
		// (set) Token: 0x06000196 RID: 406 RVA: 0x00007A5D File Offset: 0x00005C5D
		public Vector2f Origin { get; private set; }

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000197 RID: 407 RVA: 0x00007A66 File Offset: 0x00005C66
		// (set) Token: 0x06000198 RID: 408 RVA: 0x00007A6E File Offset: 0x00005C6E
		public int Frames { get; private set; }

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000199 RID: 409 RVA: 0x00007A77 File Offset: 0x00005C77
		// (set) Token: 0x0600019A RID: 410 RVA: 0x00007A7F File Offset: 0x00005C7F
		public float[] Speeds { get; private set; }

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x0600019B RID: 411 RVA: 0x00007A88 File Offset: 0x00005C88
		// (set) Token: 0x0600019C RID: 412 RVA: 0x00007A90 File Offset: 0x00005C90
		public bool FlipX { get; private set; }

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x0600019D RID: 413 RVA: 0x00007A99 File Offset: 0x00005C99
		// (set) Token: 0x0600019E RID: 414 RVA: 0x00007AA1 File Offset: 0x00005CA1
		public bool FlipY { get; private set; }

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600019F RID: 415 RVA: 0x00007AAA File Offset: 0x00005CAA
		// (set) Token: 0x060001A0 RID: 416 RVA: 0x00007AB2 File Offset: 0x00005CB2
		public int Mode { get; private set; }

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060001A1 RID: 417 RVA: 0x00007ABB File Offset: 0x00005CBB
		// (set) Token: 0x060001A2 RID: 418 RVA: 0x00007AC3 File Offset: 0x00005CC3
		public int[] Data { get; private set; }

		// Token: 0x060001A3 RID: 419 RVA: 0x00007ACC File Offset: 0x00005CCC
		public SpriteDefinition(string name, Vector2i coords, Vector2i bounds, Vector2f origin, int frames, float[] speeds, bool flipX, bool flipY, int mode, int[] data)
		{
			this.Name = name;
			this.Coords = coords;
			this.Bounds = bounds;
			this.Origin = origin;
			this.Frames = frames;
			this.Speeds = speeds;
			this.FlipX = flipX;
			this.FlipY = flipY;
			this.Mode = mode;
			this.Data = data;
		}
	}
}
